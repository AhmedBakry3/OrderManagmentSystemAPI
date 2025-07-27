


namespace Shared.DataTransferObject.IdentityDTos
{
    public class RegisterDto
    {
        [EmailAddress]
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string UserName { get; set; } = default!;
        [Phone]
        public string? PhoneNumber { get; set; } = default!;
    }
}
