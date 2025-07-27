

namespace DomainLayer.Models
{
    public class User : BaseEntity<Guid> 
    {
        public string Username { get; set; }      
        public string PasswordHash { get; set; }
        public string Role { get; set; }
    }
}
