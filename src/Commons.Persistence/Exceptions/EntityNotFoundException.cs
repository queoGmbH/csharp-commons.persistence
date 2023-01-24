using Queo.Commons.Persistence.Resources;
using System;
using System.Runtime.Serialization;

namespace Queo.Commons.Persistence.Exceptions
{
    /// <summary>
    /// Thrown to indicate that an entity was not found.
    /// </summary>
    [Serializable]
    public class EntityNotFoundException : Exception
    {
        /// <summary>
        /// Type of the entity that was not found or <code>null</code>
        /// </summary>
        public Type? EntityType { get; }

        /// <summary>
        /// ID of the entity that was not found or "unknown".
        /// </summary>
        public string Id { get; } = "unknown";

        public EntityNotFoundException() : base(ExceptionMessages.msg_EntityNotFoundException)
        {
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="entityType">Type of the entity that was not found.</param>
        /// <param name="id">ID of the entity that was not found.</param>
        public EntityNotFoundException(Type entityType, string id) : base(ExceptionMessages.msg_EntityNotFoundException)
        {
            EntityType = entityType;
            Id = id;
        }

        public EntityNotFoundException(string message) : base(message)
        {
        }

        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EntityNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override string Message
        {
            get
            {
                string msg = base.Message;
                if (EntityType != null)
                {
                    msg = msg + " " + string.Format(ExceptionMessages.msg_EntityNotFoundException_EntityData, EntityType.Name, Id);
                }
                return msg;
            }
        }
    }
}
