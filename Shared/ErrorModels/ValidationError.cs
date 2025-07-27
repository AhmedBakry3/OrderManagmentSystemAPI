


namespace Shared.ErrorModels
{
    public class ValidationError
    {
        public string Fields { get; set; } = default!;
        public IEnumerable<string> Error { get; set; } = default!;
    }
}
