using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Status
    {
        public Status()
        {
            Orders = new HashSet<Order>();
        }

        [Key]
        [Display (Name = "Mã trạng thái")] 
        public int StatusId { get; set; }

        [Display(Name = "Tên trạng thái")]
        public string? StatusName { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
