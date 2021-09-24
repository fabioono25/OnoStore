using System.Threading.Tasks;
using FluentValidation.Results;
using OnoStore.Core.Data;

namespace OnoStore.Core.Messages
{
    public abstract class CommandHandler
    {
        protected ValidationResult ValidationResult;
        //private IUnitOfWork UnitOfWork;

        protected CommandHandler()
        {
            ValidationResult = new ValidationResult();
        }

        protected void AddError(string message)
        {
            ValidationResult.Errors.Add(new ValidationFailure(string.Empty, message));
        }

        protected async Task<ValidationResult> PersistData(IUnitOfWork uow)
        {
            if (!await uow.Commit()) AddError("Error when persist data");

            return ValidationResult;
        }
    }
}