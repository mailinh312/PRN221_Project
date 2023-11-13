using BookStore.Helpers;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Pages.Store
{
    [Authorize(Roles = "Administrator, Stock manager, Order staff, Customer")]
    public class CheckOutModel : PageModel
    {
        private readonly BookStoreDbContext _context;

        private readonly UserManager<AppUser> _userManager;

        [BindProperty]
        public Order NewOrder { get; set; }

        public List<OrderDetail> cart { get; set; }

        public decimal Total { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public CheckOutModel(BookStoreDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public AppUser currentUser { get; set; }

        public async Task<IActionResult> OnGet()
        {
            currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToPage("/login");
            }

            cart = HttpContext.Session.GetObjectFromJson<List<OrderDetail>>("cart");
            if (cart == null || cart.Count == 0)
            {
                StatusMessage = "Bạn chưa có sản phẩm nào để thanh toán.";
                return RedirectToPage("Cart");
            }
            Total = GetTotalPrice(cart);
            return Page();
        }

        public async Task<IActionResult> OnPostAddOrder()
        {
            cart = HttpContext.Session.GetObjectFromJson<List<OrderDetail>>("cart");
            Order order = new Order();
            order.UserId = (await _userManager.GetUserAsync(User)).Id;
            order.OrderDate = DateTime.Now;
            order.Receiver = NewOrder.Receiver;
            order.Address = NewOrder.Address;
            order.Phone = NewOrder.Phone;
            order.Note = NewOrder.Note;
            order.TotalPrice = GetTotalPrice(cart);
            order.StatusId = 1;

            _context.Orders.Add(order);
            _context.SaveChanges();
            AddOrderDetail(cart, order.OrderId);

            cart.Clear();
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            StatusMessage = "Đặt hàng thành công.";
            return RedirectToPage("Cart");
        }

        private decimal GetTotalPrice(List<OrderDetail> orders)
        {
            decimal total = 0;
            foreach (OrderDetail order in orders)
            {
                total += Convert.ToDecimal(order.Price);
            }
            return total;
        }

        private void AddOrderDetail(List<OrderDetail> orderDetails, int orderId)
        {
            foreach (OrderDetail od in orderDetails)
            {
                od.OrderId = orderId;
                od.Price = od.Book.Price * od.Quantity;
                od.Book = null;
                Book book = _context.Books.FirstOrDefault(x => x.BookId == od.BookId);
                book.StockQuantity -= od.Quantity;
                _context.Books.Update(book);
                _context.SaveChanges(true);
                _context.OrderDetails.Add(od);
                _context.SaveChanges();
            }
        }
    }
}
