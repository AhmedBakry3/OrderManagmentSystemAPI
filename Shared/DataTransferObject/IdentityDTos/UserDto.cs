


namespace Shared.DataTransferObject.IdentityDTos
{
    public class UserDto
    {
        [EmailAddress]
        public string Email { get; set; } = default!;
        public string Token { get; set; } = default!;
        public string Username { get; set; } = default!;
    }
}
