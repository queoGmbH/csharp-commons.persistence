using System.Collections.Generic;

namespace Queo.Commons.Persistence.Generic
{
    public interface IGenericDao<T, in TKey>
    {
        /// <summary>
        ///     Leert die Session.
        /// </summary>
        /// <remarks>
        ///     Die Methode sollte nur in Testfällen verwendet werden.
        /// </remarks>
        void Clear();

        /// <summary>
        ///     Löscht die übergebene Entität
        /// </summary>
        /// <param name="entity"></param>
        void Delete(T entity);

        /// <summary>
        ///     Überprüft, ob es ein Entity mit dem Primärschlüssel gibt.
        /// </summary>
        /// <param name="primaryKey">Der Primärschlüssel.</param>
        /// <returns>
        ///     <code>true</code>, wenn ein Entity mit dem angegebenen Primärschlüssel existiert, sonst <code>false</code>
        /// </returns>
        bool Exists(TKey primaryKey);

        /// <summary>
        ///     Übernimmt alle offenen Änderungen in die Datenbank.
        /// </summary>
        /// <remarks>
        ///     Im Allgemeinen braucht diese Methode nicht aufgerufen werden, da die Steuerung
        ///     implizit über die Session bzw. die Transaktion und über den FlushMode erfolgt.
        ///     In bestimmten Fällen ist es aber hilfreich, wie z.B. bei Testfällen.
        /// </remarks>
        void Flush();

        /// <summary>
        ///     Liefert das Entity mit dem angegebenen Primary Key.
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        T Get(TKey primaryKey);

        /// <summary>
        ///     Liefert eine Liste mit allen Entitäten.
        /// </summary>
        /// <returns>Liste mit allen Entities.</returns>
        IList<T> GetAll();

        /// <summary>
        ///     Liefert die Anzahl aller Objekte.
        /// </summary>
        /// <returns>Anzahl der Objekte.</returns>
        long GetCount();

        /// <summary>
        ///     Speichert die übergebene Entität
        /// </summary>
        /// <param name="entity">Das zu speichernde Entity</param>
        /// <returns>Das gespeicherte Entity</returns>
        T Save(T entity);

        /// <summary>
        ///     Speichert alle Entitäten die in der übergebene Liste enthalten sind
        /// </summary>
        /// <param name="entities">Liste mit zu speichernden Entities.</param>
        /// <returns>Liste mit gespeicherten Entities</returns>
        IList<T> Save(IList<T> entities);

        /// <summary>
        /// Sucht nach <see cref="T"/> anhand einer Liste mit Ids. 
        /// </summary>
        /// <param name="ids">Liste mit Ids in denen die <see cref="Entity.Id"/> einer <see cref="T"/> enthalten sein muss, damit sie gefunden wird.</param>
        /// <returns></returns>
        IList<T> FindByIds(TKey[] ids);
    }
}
