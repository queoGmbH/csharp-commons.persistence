using System;

using Queo.Commons.Persistence.Generic;

namespace Queo.Commons.Persistence
{
    /// <summary>
    ///     Eine beispielhafte Implementierung für ein Entity mit einem Integer als ID.
    ///     Diese kann auch als Grundlage für konkrete Implementierungen von Domainobjekten
    ///     verwendet werden.
    /// </summary>
    public class Entity : Entity<int>
    {
        /// <summary>
        /// Ctor. Die Business ID wird mit initialisiert.
        /// </summary>
        public Entity()
        {
        }

        /// <summary>
        /// Ctor. Setzt die ID und initialisiert intern das Objekt mit einer Business ID.
        /// </summary>
        /// <param name="id">Die zu setzende ID</param>
        public Entity(int id) : base(id)
        {
        }

        /// <summary>
        /// Ctor. Hauptsächlich für Tests gedacht. Die Business ID kann hier explizit gesetzt werden.
        /// </summary>
        /// <param name="businessId"></param>
        public Entity(Guid businessId) : base(businessId)
        {
        }

        /// <summary>
        /// Ctor. Hauptsächlich für Tests gedacht.
        /// </summary>
        /// <param name="id">Die ID</param>
        /// <param name="businessId">Die Business ID</param>
        public Entity(int id, Guid businessId) : base(id, businessId)
        {
        }
    }
}