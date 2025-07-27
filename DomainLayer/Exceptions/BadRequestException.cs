

namespace DomainLayer.Exceptions
{
    public sealed class BadRequestException(List<string> Errors) : Exception("Validation Errors")
    {
        public List<string> Errors { get; } = Errors;
    }
}
