using BookStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography.Xml;

namespace BookStore.Areas.Admin.Pages.User
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly BookStoreDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public class UserAndRole : AppUser
        {
            public string RoleName { get; set; }
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public string searchTitle { get; set; }

        public List<UserAndRole> Users { get; set; }

        public List<IdentityRole> Roles { get; set; }

        [BindProperty]
        public IdentityRole SelectedRole { get; set; }

        public const int ITEMS_PER_PAGE = 10;

        [BindProperty(SupportsGet = true, Name = "p")]
        public int CurrentPage { get; set; }
        public int CountPages { get; set; }

        [BindProperty]
        public Book NewBook { get; set; }


        public IndexModel(BookStoreDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task OnGet(string search, string roleId)
        {
            var appUser = _userManager.Users.ToList();
            searchTitle = search;
            Roles = _roleManager.Roles.ToList();
            if (search != null)
            {
                appUser = _userManager.Users.Where(x => x.UserName.ToUpper().Contains(search.ToUpper().Trim())).ToList();
            }


            CountPages = appUser.Count() / ITEMS_PER_PAGE;
            if ((appUser.Count() / ITEMS_PER_PAGE) % 10 != 0)
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

            var userAndRole = appUser.Skip((CurrentPage - 1) * 10)
                .Take(ITEMS_PER_PAGE)
                .Select(x => new UserAndRole
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Email = x.Email
                }).ToList();

            Users = userAndRole;
            foreach (var user in Users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                user.RoleName = String.Join(",", roles);
            }
        }
    }
}
