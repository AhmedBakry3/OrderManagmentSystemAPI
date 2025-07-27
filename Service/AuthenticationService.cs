



using DomainLayer.Models;

namespace Service
{
    public class AuthenticationService(UserManager<IdentityUser> _userManager, IConfiguration _configuration, IMapper _mapper) : IAuthenticationService
    {
        public IJwtGenerator JwtGenerator { get; set; }  

        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            var User = await _userManager.FindByEmailAsync(loginDto.Email) ?? throw new UserNotFoundException(loginDto.Email);

            var IsPasswordValid = await _userManager.CheckPasswordAsync(User, loginDto.Password);
            if (IsPasswordValid)
                return new UserDto
                {
                    Email = User.Email,
                    Username = User.UserName,
                    Token = await CreateTokenAsync(User),
                };
            else
                throw new UnauthorizedException();
        }
        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            if (await _userManager.FindByEmailAsync(registerDto.Email) is not null)
                throw new BadRequestException(new List<string> { "Email is already registered" });

            if (await _userManager.FindByNameAsync(registerDto.UserName) is not null)
                throw new BadRequestException(new List<string> { "Username is already taken" });

            var identityUser = new IdentityUser
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(identityUser, registerDto.Password);
            if (result.Succeeded)
            {

                var customUser = new User
                {
                    Id = Guid.NewGuid(),
                    Username = identityUser.UserName,
                    PasswordHash = identityUser.PasswordHash,
                    Role = "Customer"
                };
                await _userManager.AddToRoleAsync(identityUser, "Customer");

                return new UserDto
                {
                    Email = identityUser.Email,
                    Token = await CreateTokenAsync(identityUser),
                    Username = customUser.Username
                };
            }
            else
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new BadRequestException(errors);
            }
        }




        // JWT Authentication
        public async Task<string> CreateTokenAsync(IdentityUser User)
        {
            var Claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, User.Email!),
                new Claim(ClaimTypes.NameIdentifier, User.Id),
                new Claim(ClaimTypes.Name, User.UserName!)
            };
            var Roles = await _userManager.GetRolesAsync(User);
            foreach (var role in Roles)
            {
                Claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var SecretKey = _configuration.GetSection("JWTOptions")["SecretKey"];
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var Credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            var Token = new JwtSecurityToken(
                issuer: _configuration["JWTOptions:Issuer"],
                audience: _configuration["JWTOptions:Audience"],
                claims: Claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: Credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}
