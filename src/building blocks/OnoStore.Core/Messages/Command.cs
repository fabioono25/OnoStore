using System;
using FluentValidation.Results;

namespace OnoStore.Core.Messages
{
    public abstract class Command : Message //, IRequest<ValidationResult>
    {
        public DateTime Timestamp { get; private set; } // when this command generated this result
        public ValidationResult ValidationResult { get; set; }

        protected Command()
        {
            Timestamp = DateTime.Now;
        }

        public virtual bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}