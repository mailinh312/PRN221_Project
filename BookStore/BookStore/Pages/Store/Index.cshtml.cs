using BookStore.Helpers;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Pages.Store
{
	public class IndexModel : PageModel
	{
		private readonly BookStoreDbContext _context;

		[TempData]
		public string StatusMessage { get; set; }
		public List<Category> Categories { get; set; }
		public List<Book> Books { get; set; }
		public List<Author> Authors { get; set; }

		[BindProperty]
		public int SelectedCid { get; set; }

		[BindProperty]
		public int SelectedAuthorId { get; set; }

		[BindProperty]
		public string searchTitle { get; set; }

		public const int ITEMS_PER_PAGE = 12;

		[BindProperty(SupportsGet = true, Name = "p")]
		public int CurrentPage { get; set; }
		public int CountPages { get; set; }

		[BindProperty]
		public Book NewBook { get; set; }

		public List<OrderDetail> cart { get; set; }

		public IndexModel(BookStoreDbContext context)
		{
			_context = context;
		}

		public void OnGet(int cid, string search, int aid)
		{
			searchTitle = search;
			Categories = _context.Categories.ToList();
			Authors = _context.Authors.ToList();

			if (cid == 0)
			{
				SelectedCid = 0;
				if (aid == 0)
				{
					SelectedAuthorId = 0;
					Books = _context.Books.Include(x => x.Category).Include(x => x.Author).ToList();
				}

				else
				{
					SelectedAuthorId = aid;
					Books = _context.Books.Where(x => x.AuthorId == aid).Include(x => x.Category).Include(x => x.Author).ToList();
				}

				if (search != null)
				{
					Books = Books.Where(x => x.Title.ToUpper().Contains(search.ToUpper())).ToList();
				}
			}
			else
			{
				SelectedCid = cid;

				if (aid == 0)
				{
					SelectedAuthorId = 0;
					Books = _context.Books.Where(x => x.CategoryId == SelectedCid).Include(x => x.Category).Include(x => x.Author).ToList();
				}

				else
				{
					SelectedAuthorId = aid;
					Books = _context.Books.Where(x => x.AuthorId == SelectedAuthorId && x.CategoryId == SelectedCid).Include(x => x.Category).Include(x => x.Author).ToList();
				}

				if (search != null)
				{
					searchTitle = search;
					Books = Books.Where(x => x.Title.ToUpper().Contains(search.ToUpper().Trim())).ToList();
				}
			}

			CountPages = Books.Count() / ITEMS_PER_PAGE;
			if ((Books.Count() / ITEMS_PER_PAGE) % 12 != 0)
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

			Books = Books.Skip((CurrentPage - 1) * 12).Take(ITEMS_PER_PAGE).ToList();
		}

		public IActionResult OnGetAddToCart(string productId, int number, string search, int currentPage)
		{
			CurrentPage = currentPage;

			Book book = new Book();

			cart = HttpContext.Session.GetObjectFromJson<List<OrderDetail>>("cart");

			if (Convert.ToString(productId) != null)
			{
				book = _context.Books.FirstOrDefault(x => x.BookId == Convert.ToInt32(productId));

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
						od.Quantity = 1;
						od.Price = od.Book.Price * od.Quantity;
						cart.Add(od);
						HttpContext.Session.SetObjectAsJson("cart", cart);
						StatusMessage = "Đã thêm sản phẩm vào giỏ hàng.";
					}

				}
			}

			searchTitle = search;
			Categories = _context.Categories.ToList();
			Authors = _context.Authors.ToList();
			Books = _context.Books.Include(x => x.Category).Include(x => x.Author).ToList();
			if (search != null)
			{
				Books = Books.Where(x => x.Title.ToUpper().Contains(search.ToUpper())).ToList();
			}

			CountPages = Books.Count() / ITEMS_PER_PAGE;
			if ((Books.Count() / ITEMS_PER_PAGE) % 12 != 0)
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

			Books = Books.Skip((CurrentPage - 1) * 12).Take(ITEMS_PER_PAGE).ToList();

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