using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Pages.Management.AuthorManagement
{

    [Authorize (Roles = "Administrator, Order staff, Stock manager")]
    public class AuthorManagerModel : PageModel
    {
        private readonly BookStoreDbContext _context;

        [TempData]
        public string StatusMessage { get; set; }

        public List<Author> Authors { get; set; }

        [BindProperty]
        public string searchTitle { get; set; }

        public const int ITEMS_PER_PAGE = 5;

        [BindProperty(SupportsGet = true, Name = "p")]
        public int CurrentPage { get; set; }
        public int CountPages { get; set; }


        public AuthorManagerModel(BookStoreDbContext context)
        {
            _context = context;
        }

        public void OnGet(string search)
        {
            searchTitle = search;
            Authors = _context.Authors.ToList();

            if (searchTitle != null)
            {
                Authors = Authors.Where(x => x.AuthorName.ToUpper().Contains(searchTitle.ToUpper().Trim())).ToList();
            }

            CountPages = Authors.Count() / ITEMS_PER_PAGE;
            if ((Authors.Count() / ITEMS_PER_PAGE) % 5 != 0)
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

            Authors = Authors.Skip((CurrentPage - 1) * 5).Take(ITEMS_PER_PAGE).ToList();
        }
    }
}

