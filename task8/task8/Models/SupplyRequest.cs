using Microsoft.AspNetCore.Identity;

namespace Task8.Models
{
    public class SupplyRequest
    {
        public enum OrderStatus
        {
            Pendding,
            Approved,
            Rejected
        }
        public int Id { get; set; }
        public DateOnly RequestDate { get; set; }
        public double TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        public ICollection<RequestItem> requestItems { get; set; }
    }
}