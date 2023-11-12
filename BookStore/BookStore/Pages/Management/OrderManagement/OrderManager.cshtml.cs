using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Pages.Management.OrderManagement
{
    [Authorize(Roles = "Administrator, Order staff")]
    public class OrderManagerModel : PageModel
    {
        private readonly BookStoreDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        [TempData]
        public string StatusMessage { get; set; }

        public List<Order> Orders { get; set; }

        public List<Status> Statuss { get; set; }

        [BindProperty]
        public string searchTitle { get; set; }

        public const int ITEMS_PER_PAGE = 10;

        [BindProperty(SupportsGet = true, Name = "p")]
        public int CurrentPage { get; set; }
        public int CountPages { get; set; }


        public OrderManagerModel(BookStoreDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task OnGet(string search)
        {
            searchTitle = search;

            Statuss = _context.Status.ToList(); 

            Orders = _context.Orders.Include(x => x.User).Include(x => x.Status).ToList();
            
            if (searchTitle != null)
            {
                Orders = _context.Orders.Where(x => x.OrderId == Convert.ToInt32(search.Trim())).Include(x => x.User).Include(x => x.Status).ToList();
            }

            CountPages = Orders.Count() / ITEMS_PER_PAGE;
            if ((Orders.Count() / ITEMS_PER_PAGE) % 10 != 0)
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

            Orders = Orders.Skip((CurrentPage - 1) * 10).Take(ITEMS_PER_PAGE).ToList();
        }

        public async Task<IActionResult> OnPost(int currentPage, String searchId, int sid, int id)
        {
            searchTitle = searchId;

            Statuss = _context.Status.ToList();

            Orders = _context.Orders.Include(x => x.User).Include(x => x.Status).ToList();

            if (searchTitle != null)
            {
                Orders = _context.Orders.Where(x => x.OrderId == Convert.ToInt32(searchId.Trim())).Include(x => x.User).Include(x => x.Status).ToList();
            }

            if (id != 0) {
                Order order = _context.Orders.FirstOrDefault(x => x.OrderId == id);
                if (order != null)
                {
                    order.StatusId = sid;
                    _context.Orders.Update(order);
                    _context.SaveChanges();
                    StatusMessage = $"Cập nhật trạng thái đơn hàng ID = {order.OrderId} thành công.";
                }
            }
            else
            {
                StatusMessage = "Cập nhật trạng thái đơn hàng thất bại.";
            }

            CountPages = Orders.Count() / ITEMS_PER_PAGE;
            if ((Orders.Count() / ITEMS_PER_PAGE) % 10 != 0)
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
            CurrentPage = currentPage;

            Orders = Orders.Skip((CurrentPage - 1) * 10).Take(ITEMS_PER_PAGE).ToList();

            return Page();
        }
    }
}

