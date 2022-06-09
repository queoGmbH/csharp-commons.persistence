using System;

namespace Queo.Commons.Persistence.Generic
{
    public class Entity<TKey>
    {
        private Guid _businessId;
        private TKey _id;

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="Entity{TKey}" />-Klasse.
        /// </summary>
        public Entity()
        {
            _businessId = Guid.NewGuid();
        }

        public Entity(TKey id) : this()
        {
            _id = id;
        }

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="Entity{TKey}" />-Klasse.
        ///     Hauptsächlich für Tests gedacht.
        /// </summary>
        public Entity(Guid businessId)
        {
            _businessId = businessId;
        }

        /// <summary>
        /// Initialisiert eine neue Instanz mit ID und BID.
        /// Hauptsächlich für Tests gedacht.
        /// </summary>
        /// <param name="id">Die ID</param>
        /// <param name="businessId">Die Business ID</param>
        public Entity(TKey id, Guid businessId)
        {
            _id = id;
            _businessId = businessId;
        }

        /// <summary>
        ///     Liefert die BusinessId des Objekts.
        /// </summary>
        public virtual Guid BusinessId
        {
            get { return _businessId; }
            protected set { _businessId = value; }
        }

        /// <summary>
        ///     Liefert die ID des Objekts.
        /// </summary>
        public virtual TKey Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        /// <summary>
        ///     Bestimmt, ob das angegebene Objekt mit dem aktuellen Objekt identisch ist.
        ///     Es wird auf Typgleichheit und die Gleichheit der BusinessId geprüft.
        /// </summary>
        /// <returns>
        ///     true, wenn das angegebene Objekt und das aktuelle Objekt gleich sind, andernfalls false.
        /// </returns>
        /// <param name="obj">Das Objekt, das mit dem aktuellen Objekt verglichen werden soll.</param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj == null)
            {
                return false;
            }

            if (!(obj is Entity<TKey>))
            {
                return false;
            }

            if (GetTypeUnproxied() != ((Entity<TKey>)obj).GetTypeUnproxied())
            {
                return false;
            }

            Entity<TKey> other = (Entity<TKey>)obj;
            if (!BusinessId.Equals(other.BusinessId))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Fungiert als Hashfunktion für einen bestimmten Typ.
        /// </summary>
        /// <returns>
        ///     Ein Hashcode für das aktuelle Objekt.
        /// </returns>
        public override int GetHashCode()
        {
            return GetType().GetHashCode() ^ BusinessId.GetHashCode();
        }

        public virtual Type GetTypeUnproxied()
        {
            return GetType();
        }

        /// <summary>
        ///     Gibt eine Zeichenfolge zurück, die das aktuelle Objekt darstellt.
        /// </summary>
        /// <returns>
        ///     Eine Zeichenfolge, die das aktuelle Objekt darstellt.
        /// </returns>
        public override string ToString()
        {
            string toString = $"Type: {GetType().Name}, Id: {Id}, Bid: {BusinessId}";
            return toString;
        }
    }
}