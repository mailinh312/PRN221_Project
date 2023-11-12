using BookStore.Areas.Admin.Pages.Role;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.Entity;

namespace Areas.Admin.Role
{
    [Authorize(Roles = "Administrator")]
    public class IndexModel : RolePageModel
    {
        public IndexModel(RoleManager<IdentityRole> roleManager, BookStoreDbContext bookStoreDbContext) : base(roleManager, bookStoreDbContext)
        {
        }

        public List<IdentityRole> Roles { get; set; }


        public void OnGet()
        {
            Roles =  _roleManager.Roles.ToList();
        }
    }
}
