using BookStore.Models;

namespace BookStore.Pages.Management.Dashboard
{
    public class Top3Category
    {
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int? SoldQuantity { get; set; }
    }
}
