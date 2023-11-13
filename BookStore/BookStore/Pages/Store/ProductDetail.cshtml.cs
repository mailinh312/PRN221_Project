using BookStore.Helpers;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookStore.Pages.Store
{
	public class ProductDetailModel : PageModel
	{
		public readonly BookStoreDbContext _context;

		[TempData]
		public string StatusMessage { get; set; }

		public List<OrderDetail> cart { get; set; }
		public ProductDetailModel(BookStoreDbContext context)
		{
			_context = context;
		}

		public Book detailBook { get; set; }
		public void OnGet(int id)
		{

			detailBook = _context.Books.Include(x => x.Category).Include(x => x.Author).FirstOrDefault(x => x.BookId == id);

		}

		public IActionResult OnPostAddToCart(int productId, int number)
		{
			Book book = new Book();

			cart = HttpContext.Session.GetObjectFromJson<List<OrderDetail>>("cart");

			if (productId != 0)
			{
				book = _context.Books.FirstOrDefault(x => x.BookId == productId);

				//Nếu cart null thì khởi tạo cart rồi add sản phẩm
				if (cart == null)
				{
					cart = new List<OrderDetail>();
					OrderDetail od = new OrderDetail();
					od.BookId = Convert.ToInt32(productId);
					od.Book = book;
					od.Quantity = (number != 0) ? number : 1;
					od.Price = od.Book.Price * od.Quantity;
					cart.Add(od);
					HttpContext.Session.SetObjectAsJson("cart", cart);
					StatusMessage = "Đã thêm sản phẩm vào giỏ hàng.";
				}
				else
				{
					//Nếu sản phẩm đã có trong cart
					if (Exists(cart, Convert.ToInt32(productId)) != -1)
					{
						OrderDetail od = cart[Exists(cart, Convert.ToInt32(productId))];
						if (number != 0)
						{
							od.Quantity += number;
						}
						else
						{
							od.Quantity += 1;
						}
						od.Book = book;
						od.Price = od.Book.Price * od.Quantity;
						HttpContext.Session.SetObjectAsJson("cart", cart);
						StatusMessage = "Sản phẩm đã có trong giỏ hàng.";
					}
					else
					{
						OrderDetail od = new OrderDetail();
						od.BookId = Convert.ToInt32(productId);
						od.Book = book;
						od.Quantity = number;
						od.Price = od.Book.Price * od.Quantity;
						cart.Add(od);
						HttpContext.Session.SetObjectAsJson("cart", cart);
						StatusMessage = "Đã thêm sản phẩm vào giỏ hàng.";
					}

				}
			}
			detailBook = detailBook = _context.Books.Include(x => x.Category).Include(x => x.Author).FirstOrDefault(x => x.BookId == productId); ;
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
	}
}
