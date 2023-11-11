using BookStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Pages.Management.OrderManagement
{
    public class DetailOrderModel : PageModel
    {
        private readonly BookStoreDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        [TempData]
        public string StatusMessage { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

        [BindProperty]
        public string searchTitle { get; set; }

        public const int ITEMS_PER_PAGE = 5;

        [BindProperty(SupportsGet = true, Name = "p")]
        public int CurrentPage { get; set; }
        public int CountPages { get; set; }


        public DetailOrderModel(BookStoreDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGet(string search, int orderId)
        {
            searchTitle = search;
            if(orderId  == null)
            {
                return RedirectToPage("./OrderManager");
            }

            OrderDetails = _context.OrderDetails.Where(x => x.OrderId == orderId).Include(x => x.Book).ToList();

            if (searchTitle != null)
            {
               
            }

            CountPages = OrderDetails.Count() / ITEMS_PER_PAGE;
            if ((OrderDetails.Count() / ITEMS_PER_PAGE) % 5 != 0)
            {
                CountPages = CountPages + 1;
            }

            if (CurrentPage < 1)
            {
                CurrentPage = 1;
            }
            if (CurrentPage > CountPages)
            {
                CurrentPage = CountPages;
            }

            OrderDetails = OrderDetails.Skip((CurrentPage - 1) * 5).Take(ITEMS_PER_PAGE).ToList();

            return Page();
        }
    }
}