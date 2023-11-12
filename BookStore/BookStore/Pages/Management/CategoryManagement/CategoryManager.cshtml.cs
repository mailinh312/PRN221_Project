using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Pages.Management.CategoryManagement
{
    [Authorize(Roles = "Administrator, Order staff, Stock manager")]
    public class CategoryManagerModel : PageModel
    {
        private readonly BookStoreDbContext _context;

        [TempData]
        public string StatusMessage { get; set; }
        public List<Category> Categories { get; set; }

        [BindProperty]
        public string searchTitle { get; set; }

        public const int ITEMS_PER_PAGE = 5;

        [BindProperty(SupportsGet = true, Name = "p")]
        public int CurrentPage { get; set; }
        public int CountPages { get; set; }


        public CategoryManagerModel(BookStoreDbContext context)
        {
            _context = context;
        }

        public void OnGet(string search)
        {
            searchTitle = search;
            Categories = _context.Categories.ToList();
            if (searchTitle != null)
            {
                Categories = _context.Categories.Where(x => x.CategoryName.ToUpper().Contains(searchTitle.ToUpper().Trim())).ToList();
            }

            CountPages = Categories.Count() / ITEMS_PER_PAGE + 1;

            if (CurrentPage < 1)
            {
                CurrentPage = 1;
            }
            if (CurrentPage > CountPages)
            {
                CurrentPage = CountPages;
            }

            Categories = Categories.Skip((CurrentPage - 1) * 5).Take(ITEMS_PER_PAGE).ToList();
        }
    }
}
