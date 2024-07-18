using Mappers.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApplicationServices.Extensions
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(e => e.Value!.Errors.Count > 0)
                    .SelectMany(e => e.Value!.Errors.Select(er => er.ErrorMessage))
                    .ToList();

                context.Result = new BadRequestObjectResult(new ResponseDTO { IsSuccessful = false, Errors = errors });
            }
        }
    }
}
