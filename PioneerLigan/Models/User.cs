using PioneerLigan.HelperClasses;

namespace PioneerLigan.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int PlayerId { get; set; } = 0;
        public string UserRole { get; set; } = string.Empty;
    }
}
