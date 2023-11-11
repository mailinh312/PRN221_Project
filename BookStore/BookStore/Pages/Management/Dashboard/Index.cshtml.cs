using BookStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Pages.Management.Dashboard
{
    public class IndexModel : PageModel
    {

        private readonly BookStoreDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public List<OrderDetail> OrderDetails { get; set; }
        public int numberOfAccounts { get; set; }
        public int numberOfSold { get; set; }

        public double totalEarning { get; set; }

        public IndexModel(BookStoreDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task OnGet()
        {
            numberOfAccounts = await _userManager.Users.CountAsync();
            numberOfSold = Convert.ToInt32(_context.OrderDetails.Sum(o => o.Quantity));

            OrderDetails = _context.OrderDetails.ToList();
            foreach (OrderDetail detail in OrderDetails)
            {
                totalEarning += CalculateInterest(detail);
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
    }
}
