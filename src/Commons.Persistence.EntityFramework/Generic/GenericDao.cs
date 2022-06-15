using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

using Queo.Commons.Persistence.Generic;

namespace Queo.Commons.Persistence.EntityFramework.Generic
{
    public class GenericDao<TEntity, TKey> : IGenericDao<TEntity, TKey> where TEntity : class
    {
        private readonly DbContext _dbContext;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public GenericDao(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected DbContext DbContext
        {
            get { return _dbContext; }
        }

        /// <summary>
        ///     Abstraktes Property zur Angabe der bei einer Abfrage explizit mit einzuschließenden
        ///     referenzierten Entitäten, die beim Laden einer Entität mitgeladen werden sollen (Eager-Loading).
        ///     Müssen bei Bedarf von spezifischeren DAOs entsprechend überschrieben werden.
        /// </summary>
        protected virtual IEnumerable<Expression<Func<TEntity, object>>> PropertiesToInclude
        {
            get { return new Expression<Func<TEntity, object>>[0]; }
        }

        /// <summary>
        ///     Leert die Session.
        /// </summary>
        /// <remarks>
        ///     Die Methode sollte nur in Testfällen verwendet werden.
        /// </remarks>
        public virtual void Clear()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Löscht die übergebene Entität
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(TEntity entity)
        {
            _dbContext.Remove(entity);
        }

        /// <summary>
        ///     Überprüft, ob es ein Entity mit dem Primärschlüssel gibt.
        /// </summary>
        /// <param name="primaryKey">Der Primärschlüssel.</param>
        /// <returns>
        ///     <code>true</code>, wenn ein Entity mit dem angegebenen Primärschlüssel existiert, sonst <code>false</code>
        /// </returns>
        public virtual bool Exists(TKey primaryKey)
        {
            TEntity entity = _dbContext.Find<TEntity>(primaryKey);
            if (entity == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Sucht nach allen <see cref="T" />
        /// </summary>
        /// <param name="pageable">Enthält Informationen für Pagination</param>
        /// <returns></returns>
        public virtual IPage<TEntity> Find(IPageable pageable)
        {
            int totalCount = _dbContext.Set<TEntity>().Count();
            IList<TEntity> entities = _dbContext.Set<TEntity>().Skip(pageable.FirstItem).Take(pageable.PageSize).ToList();
            return new Page<TEntity>(entities, pageable, totalCount);
        }

        /// <summary>
        ///     Sucht nach <see cref="T" /> anhand einer Liste mit Ids.
        /// </summary>
        /// <param name="ids">
        ///     Liste mit Ids in denen die <see cref="Entity.Id" /> einer <see cref="T" /> enthalten sein muss, damit
        ///     sie gefunden wird.
        /// </param>
        /// <returns></returns>
        public virtual IList<TEntity> FindByIds(TKey[] ids)
        {
            List<TEntity> entities = new List<TEntity>();
            foreach (TKey id in ids)
            {
                TEntity entity = Get(id);
                if (entity != null)
                {
                    entities.Add(entity);
                }
            }

            return entities;
        }

        /// <summary>
        ///     Übernimmt alle offenen Änderungen in die Datenbank.
        /// </summary>
        /// <remarks>
        ///     Im Allgemeinen braucht diese Methode nicht aufgerufen werden, da die Steuerung
        ///     implizit über die Session bzw. die Transaktion und über den FlushMode erfolgt.
        ///     In bestimmten Fällen ist es aber hilfreich, wie z.B. bei Testfällen.
        /// </remarks>
        public void Flush()
        {
            _dbContext.SaveChanges();
        }

        /// <summary>
        ///     Liefert das Entity mit dem angegebenen Primary Key.
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public virtual TEntity Get(TKey primaryKey)
        {
            IReadOnlyList<IProperty> keyProperties = GetKeyProperties(typeof(TEntity));
            ParameterExpression entityParameter = Expression.Parameter(typeof(TEntity), "e");
            Expression expressionBody = BuildPredicate(keyProperties, new ValueBuffer(new object[] { primaryKey }), entityParameter);

            Expression<Func<TEntity, bool>> keyExpression = Expression.Lambda<Func<TEntity, bool>>(expressionBody, entityParameter);
            TEntity entity = DbSetWithIncludedProperties().FirstOrDefault(keyExpression);
            return entity;
        }

        /// <summary>
        ///     Liefert eine Liste mit allen Entitäten.
        /// </summary>
        /// <returns>Liste mit allen Entities.</returns>
        public virtual IList<TEntity> GetAll()
        {
            List<TEntity> entities = _dbContext.Set<TEntity>().ToList();
            return entities;
        }

        /// <summary>
        ///     Liefert die Anzahl aller Objekte.
        /// </summary>
        /// <returns>Anzahl der Objekte.</returns>
        public virtual long GetCount()
        {
            int count = _dbContext.Set<TEntity>().Count();
            return count;
        }

        /// <summary>
        ///     Speichert die übergebene Entität
        /// </summary>
        /// <param name="entity">Das zu speichernde Entity</param>
        /// <returns>Das gespeicherte Entity</returns>
        public virtual TEntity Save(TEntity entity)
        {
            EntityEntry<TEntity> entityEntry = _dbContext.Add(entity);
            return entityEntry.Entity;
        }

        /// <summary>
        ///     Speichert alle Entitäten die in der übergebene Liste enthalten sind
        /// </summary>
        /// <param name="entities">Liste mit zu speichernden Entities.</param>
        /// <returns>Liste mit gespeicherten Entities</returns>
        public virtual IList<TEntity> Save(IList<TEntity> entities)
        {
            _dbContext.AddRange(entities);
            return entities;
        }

        /// <summary>
        ///     Hilfsmethode zur Inkludierung aller über die Eigenschaft (<see cref="PropertiesToInclude" />) angegebenen
        ///     Referenzen, die beim Laden von Entitäten mitgeladen werden sollen (Eager-Loading).
        /// </summary>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> DbSetWithIncludedProperties()
        {
            IQueryable<TEntity> dbSet = _dbContext.Set<TEntity>().AsQueryable();
            return PropertiesToInclude.Aggregate(dbSet, (current, propertyPath) => current.Include(propertyPath));
        }

        private static BinaryExpression BuildPredicate(
            IReadOnlyList<IProperty> keyProperties,
            ValueBuffer keyValues,
            ParameterExpression entityParameter)
        {
            ConstantExpression keyValuesConstant = Expression.Constant(keyValues);

            BinaryExpression predicate = GenerateEqualExpression(keyProperties[0], 0);

            for (int i = 1; i < keyProperties.Count; i++)
            {
                predicate = Expression.AndAlso(predicate, GenerateEqualExpression(keyProperties[i], i));
            }

            return predicate;

            BinaryExpression GenerateEqualExpression(IProperty property, int i)
            {
                MethodInfo propMethod = typeof(EF).GetTypeInfo().GetDeclaredMethod(nameof(EF.Property)).MakeGenericMethod(property.ClrType);
                MethodCallExpression leftExpression = Expression.Call(
                    propMethod,
                    entityParameter,
                    Expression.Constant(property.Name, typeof(string)));
                // MethodInfo um den Primärschlüsselwert aus dem Array abzurufen
                MethodInfo getKeyValueMethod =
                    typeof(ValueBuffer).GetRuntimeProperties().Single(p => p.GetIndexParameters().Length > 0).GetMethod;
                UnaryExpression rightExpression = Expression.Convert(
                    Expression.Call(
                        keyValuesConstant,
                        getKeyValueMethod,
                        Expression.Constant(i)),
                    property.ClrType);
                return Expression.Equal(
                    leftExpression,
                    rightExpression);
            }
        }

        private IReadOnlyList<IProperty> GetKeyProperties(Type entityType)
        {
            IEntityType entityModel = _dbContext.Model.FindEntityType(entityType);
            IReadOnlyList<IProperty> keyProperties = entityModel.FindPrimaryKey().Properties;
            return keyProperties;
        }
    }
}
