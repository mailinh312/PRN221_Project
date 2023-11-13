using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Pages.Management.ImportManagement
{
    [Authorize(Roles = "Administrator, Stock manager")]
    public class DetailImportModel : PageModel
    {
        private readonly BookStoreDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        [TempData]
        public string StatusMessage { get; set; }

        public List<ImportDetail> ImportDetails { get; set; }

        [BindProperty]
        public string searchTitle { get; set; }

        public const int ITEMS_PER_PAGE = 10;

        [BindProperty(SupportsGet = true, Name = "p")]
        public int CurrentPage { get; set; }
        public int CountPages { get; set; }


        public DetailImportModel(BookStoreDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGet(string search, int importId)
        {
            searchTitle = search;
            if (importId == null)
            {
                return RedirectToPage("./ImportManager");
            }

            ImportDetails = _context.ImportDetails.Where(x => x.ImportId == importId).Include(x => x.Book).ToList();

            if (searchTitle != null)
            {

            }

            CountPages = ImportDetails.Count() / ITEMS_PER_PAGE;
            if ((ImportDetails.Count() / ITEMS_PER_PAGE) % 10 != 0)
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

            ImportDetails = ImportDetails.Skip((CurrentPage - 1) * 10).Take(ITEMS_PER_PAGE).ToList();

            return Page();
        }
    }
}