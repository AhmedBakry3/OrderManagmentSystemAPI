

namespace ServiceAbstraction
{
    public interface IMailService
    {
        Task SendAsync(Email email);
    }
}
