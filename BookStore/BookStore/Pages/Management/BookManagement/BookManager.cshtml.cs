using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace BookStore.Pages.Management.BookManagement
{
    [Authorize(Roles = "Administrator, Order staff, Stock manager")]
    public class BookManagerModel : PageModel
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

        public const int ITEMS_PER_PAGE = 5;

        [BindProperty(SupportsGet = true, Name = "p")]
        public int CurrentPage { get; set; }
        public int CountPages { get; set; }

        [BindProperty]
        public Book NewBook { get; set; }


        public BookManagerModel(BookStoreDbContext context)
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
            if((Books.Count() / ITEMS_PER_PAGE) % 5 != 0)
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

            Books = Books.Skip((CurrentPage - 1) * 5).Take(ITEMS_PER_PAGE).ToList();
        }
    }
}
