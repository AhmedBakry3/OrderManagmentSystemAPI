

namespace OrderManagement.Web.Factories
{
    public static class ApiResponseFactory
    {
        public static IActionResult GenerateApiValidationErrorResponse(ActionContext context)
        {
            var Errors = context.ModelState.Where(M => M.Value.Errors.Any())
                .Select(M => new ValidationError()
                {
                    Fields = M.Key,
                    Error = M.Value.Errors.Select(E => E.ErrorMessage)
                });
            var Response = new ValidationErrorToReturn()
            {
                ValidationErrors = Errors
            };
            return new BadRequestObjectResult(Response);
        }
    }
}
