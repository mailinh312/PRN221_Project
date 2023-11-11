using MessagePack;
using System.ComponentModel.DataAnnotations;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;

namespace BookStore.Models
{
    public class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        [Key]
        [Display(Name = "Mã đơn hàng")]
        public int OrderId { get; set; }
        public String? UserId { get; set; }

        [Display(Name = "Ngày đặt hàng")]
        public DateTime? OrderDate { get; set; }

        [Display(Name = "Người nhận")]
        public string? Receiver { get; set; }

        [Display(Name = "Địa chỉ")]
        public string? Address { get; set; }

        [Display(Name = "Tổng thanh toán")]
        public decimal? TotalPrice { get; set; }
        public int? StatusId { get; set; }

        [Display(Name = "Người đặt hàng")]
        public virtual AppUser? User { get; set; }
        public virtual Status? Status { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
