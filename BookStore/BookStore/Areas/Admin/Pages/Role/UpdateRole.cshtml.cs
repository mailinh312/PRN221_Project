using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace BookStore.Areas.Admin.Pages.Role
{
    [Authorize(Roles = "Administrator")]
    public class UpdateRoleModel : RolePageModel
    {
        public UpdateRoleModel(RoleManager<IdentityRole> roleManager, BookStoreDbContext bookStoreDbContext) : base(roleManager, bookStoreDbContext)
        {
        }

        public class RoleInput
        {
            [Required]
            [Display(Name = "Mã vai trò")]
            public string Id { get; set; }

            [Required(ErrorMessage = "Tên vai trò không được để trống!")]
            [Display(Name = "Tên vai trò")]
            [StringLength(50, MinimumLength = 3, ErrorMessage = "Tên phải dài từ {2} đến {1} kí tự")]
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
            if (ModelState.IsValid)
            {
                IdentityRole updateRole = await _roleManager.FindByIdAsync(Input.Id);
                if (updateRole != null)
                {
                    updateRole.Name = Input.Name;
                    var result = await _roleManager.UpdateAsync(updateRole);
                    if (result.Succeeded)
                    {
                        StatusMessage = "Cập nhật thành công.";
                        return RedirectToPage("./Index");
                    }
                    else
                    {
                        result.Errors.ToList().ForEach(error =>
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        });
                        StatusMessage = "Cập nhật thất bại!";
                        return Page();
                    }
                }
            }
            else
            {
                StatusMessage = "Cập nhật thất bại!";
                return Page();
            }
            return Page();
        }
    }
}

