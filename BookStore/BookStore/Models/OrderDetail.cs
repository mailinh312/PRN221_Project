using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class OrderDetail
    {
        [Key]
        [Display(Name = "Mã chi tiết")]
        public int OrderDetailId { get; set; }

        [Required]
        [Display(Name = "Mã đơn hàng")]
        public int? OrderId { get; set; }

        [Required]
        [Display(Name = "Mã sản phẩm")]
        public int? BookId { get; set; }

        [Display(Name = "Số lượng")]
        public int? Quantity { get; set; }

        [Display(Name = "Giá tiền")]
        public decimal? Price { get; set; }

        public virtual Book? Book { get; set; }
        public virtual Order? Order { get; set; }
    }
}
