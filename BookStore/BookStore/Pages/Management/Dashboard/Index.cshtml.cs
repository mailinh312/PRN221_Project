using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Pages.Management.Dashboard
{
    [Authorize(Roles = "Administrator")]
    public class IndexModel : PageModel
    {

        private readonly BookStoreDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public List<OrderDetail> OrderDetails { get; set; }

        //Số người dùng
        public int numberOfAccounts { get; set; }

        //Số sản phẩm đã bán
        public int numberOfSold { get; set; }

        //Vốn
        public double totalInvestment { get; set; }

        //Tổng lãi
        public double totalEarning { get; set; }

        //Top 5 sản phấm bán chạy
        public List<BestSellerProduct> Top5BestSeller { get; set; }

        public IndexModel(BookStoreDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task OnGet()
        {
            numberOfAccounts = await _userManager.Users.CountAsync();
            numberOfSold = Convert.ToInt32(_context.OrderDetails.Sum(o => o.Quantity));
            totalInvestment = CalculateInvestment();

            OrderDetails = _context.OrderDetails.ToList();

            foreach (OrderDetail detail in OrderDetails)
            {
                totalEarning += CalculateInterest(detail);
            }

            Top5BestSeller = _context.OrderDetails.GroupBy(od => od.BookId).Select(x => new BestSellerProduct
            {
                Id = x.Key,
                SoldQuantity = x.Sum(od => od.Quantity),
                Price = x.First().Price // lấy giá trị từ một bản ghi trong nhóm

            }).OrderByDescending(p => p.SoldQuantity).Take(5).ToList();
            foreach (BestSellerProduct bestSellerProduct in Top5BestSeller)
            {
                bestSellerProduct.Title = _context.Books.FirstOrDefault(x => x.BookId == bestSellerProduct.Id).Title;
            }
        }

        private double CalculateInterest(OrderDetail orderDetail)
        {
            double interest = 0;
            Book book = _context.Books.FirstOrDefault(x => x.BookId == orderDetail.BookId);
            if (book != null)
            {
                interest = Convert.ToDouble(book.Price * orderDetail.Quantity - book.OriginPrice * orderDetail.Quantity);
            }
            return interest;
        }

        private double CalculateInvestment()
        {
            return Convert.ToDouble(_context.Imports.Sum(x => x.TotalPrice));
        }
    }
}
