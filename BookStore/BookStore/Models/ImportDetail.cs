using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class ImportDetail
    {
        [Key]
        [Required]
        public int ImportDetailId { get; set; }

        [Display(Name = "Mã phiếu nhập")]
        public int? ImportId { get; set; }

        [Required(ErrorMessage = "Chọn sản phẩm cần nhập thêm.")]
        [Display(Name = "Mã sản phẩm")]
        public int? BookId { get; set; }

        [Display(Name = "Số lượng")]
        [Required(ErrorMessage = "Nhập số lượng.")]

        public int? Quantity { get; set; }

        [Display(Name = "Giá nhập")]
        [Required(ErrorMessage = "Nhập giá nhập.")]
        public decimal? InputPrice { get; set; }

        [Display(Name = "Giá bán")]
        [Required(ErrorMessage = "Nhập giá bán.")]
        public decimal? OutputPrice { get; set; }

        [Display(Name = "Tổng tiền")]
        public decimal? TotalPrice { get; set; }

        public virtual Book? Book { get; set; }

        public virtual Import? Import { get; set; }


    }
}
