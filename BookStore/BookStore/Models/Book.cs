﻿using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "Giá sản phẩm không được để trống!")]
        public decimal? Price { get; set; }

        public int? AuthorId { get; set; }

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Display(Name = "Nhà xuất bản")]
        [Required(ErrorMessage = "Nhà xuất bản không được để trống!")]
        public string? Publisher { get; set; }

        [Required(ErrorMessage = "Ngày xuất bản không được để trống!")]
        [Display(Name = "Ngày xuất bản")]
        public DateTime? PublishDate { get; set; }

        [Display(Name = "Số lượng")]
        [Required(ErrorMessage = "Số lượng trong kho không được để trống!")]
        public int? StockQuantity { get; set; }

        [Display(Name = "Link ảnh")]
        [Required(ErrorMessage = "Chọn ảnh sản phẩm!")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Hoạt động")]
        public bool Active { get; set; }

        [Display(Name = "Tác giả")]
        public virtual Author? Author { get; set; }

        [Display(Name = "Thể loại")]
        public virtual Category? Category { get; set; }
    }
}
