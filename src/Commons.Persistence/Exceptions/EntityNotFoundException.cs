using Queo.Commons.Persistence.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Queo.Commons.Persistence.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public Type? EntityType { get; }
        public string Id { get; } = "unknown";

        public EntityNotFoundException() : base(ExceptionMessages.msg_EntityNotFoundException)
        {
        }

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
