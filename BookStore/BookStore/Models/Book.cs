using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Book
    {
        [Key]
        [Display(Name = "Mã sách")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Tiêu đề sách không được để trống!")]
        [Display(Name = "Tiêu đề")]
        public string? Title { get; set; }
        public int? CategoryId { get; set; }

        [Display(Name = "Giá nhập")]
        public decimal? OriginPrice { get; set; }

        [Display(Name = "Giá")]
        public decimal? Price { get; set; }

        public int? AuthorId { get; set; }

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Display(Name = "Nhà xuất bản")]
        public string? Publisher { get; set; }

        [Display(Name = "Ngày xuất bản")]
        public DateTime? PublishDate { get; set; }

        [Display(Name = "Số lượng")]
        public int? StockQuantity { get; set; }

        [Display(Name = "Link ảnh")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Hoạt động")]
        public bool Active { get; set; }

        [Display(Name = "Tác giả")]
        public virtual Author? Author { get; set; }

        [Display(Name = "Thể loại")]
        public virtual Category? Category { get; set; }
    }
}
