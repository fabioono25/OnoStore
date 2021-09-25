using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace OnoStore.WebAPI.Core.Controllers
{
    [ApiController]
    public abstract class BaseController : Controller
    {
        protected ICollection<string> Errors = new List<string>();

        protected ActionResult CustomResponse(object result = null)
        {
            if (ValidOperation())
            {
                return Ok(result);
            }

            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Messages", Errors.ToArray() }
            }));
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(e => e.Errors);
            foreach (var error in errors)
            {
                AddErrorProcessing(error.ErrorMessage);
            }

            return CustomResponse();
        }

        protected ActionResult CustomResponse(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                AddErrorProcessing(error.ErrorMessage);
            }

            return CustomResponse();
        }

        protected bool ValidOperation()
        {
            return !Errors.Any();
        }

        protected void AddErrorProcessing(string error)
        {
            Errors.Add(error);
        }

        protected void CleanErrorProcessing()
        {
            Errors.Clear();
        }
    }
}