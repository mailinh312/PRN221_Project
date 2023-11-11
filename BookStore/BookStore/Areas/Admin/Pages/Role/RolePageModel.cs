using BookStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Areas.Admin.Pages.Role
{
    public class RolePageModel : PageModel
    {
        protected readonly RoleManager<IdentityRole> _roleManager;
        protected readonly BookStoreDbContext _context;

        [TempData]
        public String StatusMessage { get; set; }

        public RolePageModel(RoleManager<IdentityRole> roleManager, BookStoreDbContext bookStoreDbContext)
        {
            _roleManager = roleManager;
            _context = bookStoreDbContext;
        }
    }
}
