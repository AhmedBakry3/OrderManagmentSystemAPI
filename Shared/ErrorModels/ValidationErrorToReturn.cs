


namespace Shared.ErrorModels
{
    public class ValidationErrorToReturn
    {
        public int StatusCode { get; set; } = (int)HttpStatusCode.BadRequest;
        public string ErrorMessage { get; set; } = "Validation Error";
        public IEnumerable<ValidationError> ValidationErrors { get; set; } = [];
    }
}
