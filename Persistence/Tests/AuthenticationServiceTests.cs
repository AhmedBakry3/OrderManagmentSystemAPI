namespace ServiceLayer.Tests
{
    public class AuthenticationServiceTests
    {
        private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly AuthenticationService _authService;

        public AuthenticationServiceTests()
        {
            _mockUserManager = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null
            );
            _mockMapper = new Mock<IMapper>();
            _mockConfiguration = new Mock<IConfiguration>();

            _mockConfiguration.Setup(c => c["JWTOptions:SecretKey"]).Returns("supersecretkey");
            _mockConfiguration.Setup(c => c["JWTOptions:Issuer"]).Returns("issuer");
            _mockConfiguration.Setup(c => c["JWTOptions:Audience"]).Returns("audience");

            _authService = new AuthenticationService(_mockUserManager.Object, _mockConfiguration.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnUserDto_WhenValidCredentials()
        {
            var loginDto = new LoginDto { Email = "ahmed.bakry@example.com", Password = "Password123" };
            var identityUser = new IdentityUser { Email = "ahmed.bakry@example.com", UserName = "AhmedBakry", Id = "1" };

            _mockUserManager.Setup(u => u.FindByEmailAsync(loginDto.Email)).ReturnsAsync(identityUser);
            _mockUserManager.Setup(u => u.CheckPasswordAsync(identityUser, loginDto.Password)).ReturnsAsync(true);

            var result = await _authService.LoginAsync(loginDto);

            Assert.NotNull(result);
            Assert.Equal(identityUser.Email, result.Email);
            Assert.Equal(identityUser.UserName, result.Username);
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnUserDto_WhenRegisterSuccessful()
        {
            var registerDto = new RegisterDto { Email = "ahmed.bakry@example.com", UserName = "AhmedBakry", Password = "Password123", PhoneNumber = "1234567890" };
            var identityUser = new IdentityUser { Email = registerDto.Email, UserName = registerDto.UserName, PhoneNumber = registerDto.PhoneNumber };
            var resultIdentity = IdentityResult.Success;

            _mockUserManager.Setup(u => u.FindByEmailAsync(registerDto.Email)).ReturnsAsync((IdentityUser)null);
            _mockUserManager.Setup(u => u.FindByNameAsync(registerDto.UserName)).ReturnsAsync((IdentityUser)null);
            _mockUserManager.Setup(u => u.CreateAsync(identityUser, registerDto.Password)).ReturnsAsync(resultIdentity);
            _mockUserManager.Setup(u => u.AddToRoleAsync(identityUser, "Customer")).ReturnsAsync(resultIdentity);

            var result = await _authService.RegisterAsync(registerDto);

            Assert.NotNull(result);
            Assert.Equal(registerDto.Email, result.Email);
            Assert.Equal(registerDto.UserName, result.Username);
        }

        [Fact]
        public async Task RegisterAsync_ShouldThrowBadRequestException_WhenEmailAlreadyRegistered()
        {
            var registerDto = new RegisterDto { Email = "ahmed.bakry@example.com", UserName = "newuser", Password = "Password123", PhoneNumber = "1234567890" };
            var existingUser = new IdentityUser { Email = registerDto.Email, UserName = registerDto.UserName };

            _mockUserManager.Setup(u => u.FindByEmailAsync(registerDto.Email)).ReturnsAsync(existingUser);

            await Assert.ThrowsAsync<BadRequestException>(() => _authService.RegisterAsync(registerDto));
        }

        [Fact]
        public async Task RegisterAsync_ShouldThrowBadRequestException_WhenUsernameAlreadyTaken()
        {
            var registerDto = new RegisterDto { Email = "newuser@example.com", UserName = "AhmedBakry", Password = "Password123", PhoneNumber = "1234567890" };
            var existingUser = new IdentityUser { Email = registerDto.Email, UserName = registerDto.UserName };

            _mockUserManager.Setup(u => u.FindByEmailAsync(registerDto.Email)).ReturnsAsync((IdentityUser)null);
            _mockUserManager.Setup(u => u.FindByNameAsync(registerDto.UserName)).ReturnsAsync(existingUser);

            await Assert.ThrowsAsync<BadRequestException>(() => _authService.RegisterAsync(registerDto));
        }
    }
}
