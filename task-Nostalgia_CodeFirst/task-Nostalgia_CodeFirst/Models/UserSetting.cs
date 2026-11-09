using System.ComponentModel.DataAnnotations;

namespace task_Nostalgia_CodeFirst.Models
{
    public class UserSetting
    {
        [Key]
        public int SettingsID { get; set; }

        public int UserID { get; set; }

        public bool OrderUpdates { get; set; } = true;

        public bool AuctionUpdates { get; set; } = true;

        public User User { get; set; }
    }
}