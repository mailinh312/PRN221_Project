using BookStore.Helpers;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;

namespace BookStore.Pages.Store
{
	public class Cart : PageModel
	{
		private readonly BookStoreDbContext _context;
		public List<OrderDetail> cart { get; set; }
		public decimal Total { get; set; }

		[TempData]
		public string StatusMessage { get; set; }

		public Cart(BookStoreDbContext context)
		{
			_context = context;
		}
		public IActionResult OnGet(string productId, int number)
		{

			cart = HttpContext.Session.GetObjectFromJson<List<OrderDetail>>("cart");
			if (cart != null)
			{
				Total = GetTotalPrice(cart);
			}
			else
			{
				Total = 0;
			}


			return Page();
		}

		public IActionResult OnGetDelete(int id)
		{
			cart = SessionHelper.GetObjectFromJson<List<OrderDetail>>(HttpContext.Session, "cart");
			int index = Exists(cart, id);
			cart.RemoveAt(index);
			SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
			Total = GetTotalPrice(cart);
			return Page();
		}

		public IActionResult OnPostUpdate(int[] quantities)
		{
			cart = SessionHelper.GetObjectFromJson<List<OrderDetail>>(HttpContext.Session, "cart");
			for (var i = 0; i < cart.Count; i++)
			{
				cart[i].Quantity = quantities[i];
				cart[i].Price = quantities[i] * cart[i].Book.Price;
			}
			SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
			SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
			Total = GetTotalPrice(cart);
			return Page();
		}

		private int Exists(List<OrderDetail> cart, int id)
		{
			for (int i = 0; i < cart.Count; i++)
			{
				if (cart[i].BookId == id)
				{
					return i;
				}
			}
			return -1;
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
	}
}
