using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Areas.Admin.Pages.Role
{
    [Authorize(Roles = "Administrator")]
    public class AddRoleModel : RolePageModel
    {
        [TempData]
        public string StatusMessage { get; set; }

        public AddRoleModel(RoleManager<IdentityRole> roleManager, BookStoreDbContext bookStoreDbContext) : base(roleManager, bookStoreDbContext)
        {
        }

        public class RoleInput
        {
            [Required(ErrorMessage = "Tên vai trò không được để trống!")]
            [Display(Name = "Tên vai trò")]
            [StringLength(50, MinimumLength = 3, ErrorMessage = "Tên phải dài từ {2} đến {1} kí tự")]
            public string Name { get; set; }
        }

        [BindProperty]
        public RoleInput Input { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var NewRole = new IdentityRole();
                NewRole.Name = Input.Name;
                var result = await _roleManager.CreateAsync(NewRole);
                if (result.Succeeded)
                {
                    StatusMessage = "Tạo mới thành công.";
                    return RedirectToPage("./Index");
                }
                else
                {
                    result.Errors.ToList().ForEach(error =>
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    });
                    StatusMessage = "Tạo mới thất bại!";
                    return Page();
                }
            }
            else
            {
                StatusMessage = "Tạo mới thất bại!";
                return Page();
            }

        }
    }
}
