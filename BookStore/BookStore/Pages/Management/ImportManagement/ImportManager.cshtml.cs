using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Pages.Management.ImportManagement
{
    [Authorize(Roles = "Administrator, Stock manager")]
    public class ImportManagerModel : PageModel
    {
        private readonly BookStoreDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        [TempData]
        public string StatusMessage { get; set; }

        public List<Import> Imports { get; set; }

        public const int ITEMS_PER_PAGE = 10;

        [BindProperty(SupportsGet = true, Name = "p")]
        public int CurrentPage { get; set; }
        public int CountPages { get; set; }


        public ImportManagerModel(BookStoreDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task OnGet()
        {

            Imports = _context.Imports.Include(x => x.User).OrderByDescending(x => x.ImportDate).ToList();

            CountPages = Imports.Count() / ITEMS_PER_PAGE;
            if ((Imports.Count() / ITEMS_PER_PAGE) % 10 != 0)
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

            Imports = Imports.Skip((CurrentPage - 1) * 10).Take(ITEMS_PER_PAGE).ToList();
        }
    }
}
