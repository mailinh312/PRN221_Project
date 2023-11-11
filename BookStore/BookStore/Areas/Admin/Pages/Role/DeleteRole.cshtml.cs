using BookStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Areas.Admin.Pages.Role
{
    public class DeleteRoleModel : RolePageModel
    {
        public DeleteRoleModel(RoleManager<IdentityRole> roleManager, BookStoreDbContext bookStoreDbContext) : base(roleManager, bookStoreDbContext)
        {
        }

        public class RoleInput
        {
            [Required]
            [Display(Name = "Mã vai trò")]
            public string Id { get; set; }

            [Display(Name = "Tên vai trò")]
            public string Name { get; set; }
        }

        [BindProperty]
        public RoleInput Input { get; set; }

        public IdentityRole role { get; set; }

        public async Task<IActionResult> OnGet(string id)
        {
            if (id == null) return NotFound("Không tìm thấy role");
            role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                Input = new RoleInput
                {
                    Id = role.Id,
                    Name = role.Name
                };

                return Page();
            }
            return NotFound("Không tìm thấy role");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            IdentityRole deleteRole = await _roleManager.FindByIdAsync(Input.Id);
            if (ModelState.IsValid)
            {
                if (deleteRole != null)
                {
                    deleteRole.Name = Input.Name;
                    var result = await _roleManager.DeleteAsync(deleteRole);
                    if (result.Succeeded)
                    {
                        StatusMessage = $"Xóa thành công {deleteRole.Name}.";
                        return RedirectToPage("./Index");
                    }
                    else
                    {
                        result.Errors.ToList().ForEach(error =>
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        });
                        StatusMessage = $"Không thể xóa {deleteRole.Name}!";
                        return Page();
                    }
                }
            }
            else
            {
                StatusMessage = $"Không thể xóa {deleteRole.Name}!";
                return Page();
            }
            return Page();
        }
    }
}