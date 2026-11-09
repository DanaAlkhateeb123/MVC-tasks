using System.ComponentModel.DataAnnotations;

namespace task_Nostalgia_CodeFirst.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Role { get; set; } = "USER";

        public UserSetting UserSetting { get; set; }

        public ICollection<Listing> Listings { get; set; }
    }
}