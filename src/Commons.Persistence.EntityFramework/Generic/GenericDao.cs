using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

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
        ///     Abstract property to specify the referenced entities to be explicitly included
        ///     in a query that should be loaded when an entity is loaded (Eager-Loading).
        ///     Must be overritten by more specific DAOs if required.
        /// </summary>
        protected virtual IEnumerable<Expression<Func<TEntity, object>>> PropertiesToInclude
        {
            get { return new Expression<Func<TEntity, object>>[0]; }
        }

        /// <summary>  
        ///     Empty the session.
        /// </summary>
        /// <remarks>
        ///     Die Methode sollte nur in Testfällen verwendet werden.
        ///
        ///     The method should only be used in test cases.
        /// </remarks>
        public virtual void Clear()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Deletes the passed entity.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(TEntity entity)
        {
            _dbContext.Remove(entity);
        }

        /// <summary>
        ///     Checks whether there is an entity with the primary key.
        /// </summary>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns>
        ///     <code>true</code>, if an entity with the specified primary key exists, otherwise <code>false</code>
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
        ///     Checks asynchronously whether there is an entity with the primary key.
        /// </summary>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns>
        ///     <code>true</code>, if an entity with the specified primary key exists, otherwise <code>false</code>
        /// </returns>
        public virtual async Task<bool> ExistsAsync(TKey primaryKey)
        {
            TEntity entity = await _dbContext.FindAsync<TEntity>(primaryKey);
            if (entity == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Searches for all <see cref="T" />
        /// </summary>
        /// <param name="pageable">Contains information for pagination</param>
        /// <returns></returns>
        public virtual IPage<TEntity> Find(IPageable pageable)
        {
            int totalCount = _dbContext.Set<TEntity>().Count();
            IList<TEntity> entities = _dbContext.Set<TEntity>().Skip(pageable.FirstItem).Take(pageable.PageSize).ToList();
            return new Page<TEntity>(entities, pageable, totalCount);
        }

        /// <summary>
        ///     Searches asynchronously for all <see cref="T" />
        /// </summary>
        /// <param name="pageable">Contains information for pagination</param>
        /// <returns></returns>
        public virtual async Task<IPage<TEntity>> FindAsync(IPageable pageable)
        {
            int totalCount = await _dbContext.Set<TEntity>().CountAsync();
            IList<TEntity> entities = await _dbContext.Set<TEntity>().Skip(pageable.FirstItem).Take(pageable.PageSize).ToListAsync();
            return new Page<TEntity>(entities, pageable, totalCount);
        }

        /// <summary>
        ///     Searches for <see cref="T" /> using a list of Ids.
        /// </summary>
        /// <param name="ids">
        ///     List of Ids in which the <see cref="Entity.Id" /> of a <see cref="T" /> must be contained in order to be found.
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
        ///     Searches asynchronously for <see cref="T" /> using a list of Ids.
        /// </summary>
        /// <param name="ids">
        ///     List of Ids in which the<see cref= "Entity.Id" /> of a<see cref="T" /> must be contained in order to be found.
        /// </param>
        /// <returns></returns>
        public virtual async Task<IList<TEntity>> FindByIdsAsync(TKey[] ids)
        {
            List<TEntity> entities = new List<TEntity>();
            foreach (TKey id in ids)
            {
                TEntity entity = await GetAsync(id);
                if (entity != null)
                {
                    entities.Add(entity);
                }
            }

            return entities;
        }

        /// <summary>
        ///     Transfers all open changes to the database.
        /// </summary>
        /// <remarks>
        ///     In general, this method does not need to be called, as the control
        ///     is implicitly via the session or the transaction and via the FlushMode. 
        ///     In certain cases, however, it is helpful, e.g. for test cases.
        /// </remarks>
        public void Flush()
        {
            _dbContext.SaveChanges();
        }

        /// <summary>
        ///     Transfers all open changes to the database asynchronously.
        /// </summary>
        /// <remarks>
        ///     In general, this method does not need to be called, as the control
        ///     is implicitly via the session or the transaction and via the FlushMode. 
        ///     In certain cases, however, it is helpful, e.g. for test cases.
        /// </remarks>
        public async Task FlushAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        ///     Returns the entity with the specified primary key.
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
        ///     Returns the entity with the specified primary key asynchronously.
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> GetAsync(TKey primaryKey)
        {
            IReadOnlyList<IProperty> keyProperties = GetKeyProperties(typeof(TEntity));
            ParameterExpression entityParameter = Expression.Parameter(typeof(TEntity), "e");
            Expression expressionBody = BuildPredicate(keyProperties, new ValueBuffer(new object[] { primaryKey }), entityParameter);

            Expression<Func<TEntity, bool>> keyExpression = Expression.Lambda<Func<TEntity, bool>>(expressionBody, entityParameter);
            TEntity entity = await DbSetWithIncludedProperties().FirstOrDefaultAsync(keyExpression);
            return entity;
        }

        /// <summary>
        ///     Returns a list with all entities.
        /// </summary>
        /// <returns>List with all entities.</returns>
        public virtual IList<TEntity> GetAll()
        {
            List<TEntity> entities = _dbContext.Set<TEntity>().ToList();
            return entities;
        }

        /// <summary>
        ///     Returns a list with all entities asynchronously.
        /// </summary>
        /// <returns>List with all entities.</returns>
        public virtual async Task<IList<TEntity>> GetAllAsync()
        {
            List<TEntity> entities = await _dbContext.Set<TEntity>().ToListAsync();
            return entities;
        }

        /// <summary>
        ///     Returns the number of all objects.
        /// </summary>
        /// <returns>Number of objects.</returns>
        public virtual long GetCount()
        {
            int count = _dbContext.Set<TEntity>().Count();
            return count;
        }

        /// <summary>
        ///     Returns the number of all objects asynchronously.
        /// </summary>
        /// <returns>Number of objects.</returns>
        public virtual async Task<long> GetCountAsync()
        {
            int count = await _dbContext.Set<TEntity>().CountAsync();
            return count;
        }

        /// <summary>
        ///     Saves the passed entity.
        /// </summary>
        /// <param name="entity">The entity to be saved</param>
        /// <returns>The stored entity</returns>
        public virtual TEntity Save(TEntity entity)
        {
            EntityEntry<TEntity> entityEntry = _dbContext.Add(entity);
            return entityEntry.Entity;
        }

        /// <summary>
        ///     Saves the passed entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to be saved</param>
        /// <returns>The stored entity</returns>
        public virtual async Task<TEntity> SaveAsync(TEntity entity)
        {
            EntityEntry<TEntity> entityEntry = await _dbContext.AddAsync(entity);
            return entityEntry.Entity;
        }

        /// <summary>
        ///     Saves all entities contained in the passed list.
        /// </summary>
        /// <param name="entities">List of entities to be saved.</param>
        /// <returns>List with stored entities</returns>
        public virtual IList<TEntity> Save(IList<TEntity> entities)
        {
            _dbContext.AddRange(entities);
            return entities;
        }

        /// <summary>
        ///     Saves asynchronously all entities that are contained in the passed list.
        /// </summary>
        /// <param name="entities">Liste mit zu speichernden Entities.</param>
        /// <returns>Liste mit gespeicherten Entities</returns>
        public virtual async Task<IList<TEntity>> SaveAsync(IList<TEntity> entities)
        {
            await _dbContext.AddRangeAsync(entities);
            return entities;
        }

        /// <summary>
        ///     Helper method for including all references specified by the property (<see cref="PropertiesToInclude" />)
        ///     that are to be loaded when entities are loaded (eager loading).
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
