


public class MailServiceTests
{
    private readonly Mock<IOptions<MailSettings>> _mockOptions;
    private readonly Mock<IMailService> _mockMailService;

    public MailServiceTests()
    {
        _mockOptions = new Mock<IOptions<MailSettings>>();
        _mockMailService = new Mock<IMailService>();
    }

    [Fact]
    public async Task SendAsync_ShouldHandleInvalidEmail_WhenInvalidEmailFormat()
    {
        var mailSettings = new MailSettings
        {
            Email = "ahmedelbakry221@gmail.com",
            DisplayName = "Ahmed Bakry",
            Host = "smtp.gmail.com",
            Port = 587,
            Password = "tvjzwtiauucmdxhf"
        };

        _mockOptions.Setup(opt => opt.Value).Returns(mailSettings);

        var mailService = new MailService(_mockOptions.Object);

        var email = new Email
        {
            To = "invalidemail.com",
            Subject = "Order #123 Status Updated",
            Body = "Dear Ahmed Bakry,\n\nYour order #123 status has been updated to 'Delivered'.\n\nThanks!"
        };

        await Assert.ThrowsAsync<MailKit.Net.Smtp.SmtpCommandException>(() => mailService.SendAsync(email));
    }

    [Fact]
    public async Task SendAsync_ShouldNotSendEmail_WhenMissingRecipientEmail()
    {
        var mailSettings = new MailSettings
        {
            Email = "ahmedelbakry221@gmail.com",
            DisplayName = "Ahmed Bakry",
            Host = "smtp.gmail.com",
            Port = 587,
            Password = "tvjzwtiauucmdxhf"
        };

        _mockOptions.Setup(opt => opt.Value).Returns(mailSettings);

        var mailService = new MailService(_mockOptions.Object);

        var email = new Email
        {
            To = "",
            Subject = "Order #123 Status Updated",
            Body = "Dear Ahmed Bakry,\n\nYour order #123 status has been updated to 'Delivered'.\n\nThanks!"
        };

        await Assert.ThrowsAsync<MimeKit.ParseException>(() => mailService.SendAsync(email));
    }

    [Fact]
    public async Task SendAsync_ShouldSendEmail_WhenValidEmailData()
    {
        var mailSettings = new MailSettings
        {
            Email = "ahmedelbakry221@gmail.com",
            DisplayName = "Ahmed Bakry",
            Host = "smtp.gmail.com",
            Port = 587,
            Password = "tvjzwtiauucmdxhf"
        };

        _mockOptions.Setup(opt => opt.Value).Returns(mailSettings);

        var mailService = new MailService(_mockOptions.Object);

        var email = new Email
        {
            To = "ahmed.elbakry.ab@gmail.com",
            Subject = "Order #123 Status Updated",
            Body = "Dear Ahmed Bakry,\n\nYour order #123 status has been updated to 'Delivered'.\n\nThanks!"
        };

        await mailService.SendAsync(email);  // This will now use real credentials (ensure you're using an app password for testing)
    }
}
