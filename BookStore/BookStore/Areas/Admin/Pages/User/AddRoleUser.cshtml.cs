using BookStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System;

namespace BookStore.Areas.Admin.Pages.User
{
    public class AddRoleUserModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly BookStoreDbContext _context;
        public AddRoleUserModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            BookStoreDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public AppUser user { get; set; }

        // thuộc tính này dùng để lấy dữ liệu mà người dùng gửi về từ form ~ các role mà admin muốn gắn cho user
        [BindProperty]
        [DisplayName("Cấp quyền cho tài khoản")]
        public string[] RoleNamesOfUser { get; set; }

        public List<IdentityRoleClaim<string>> claimsInRole { get; set; }

        public List<IdentityUserClaim<string>> claimsInUserClaim { get; set; }

        public SelectList allRoles { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound($"Không có user");
            }
            user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Không thấy user với id = {id}");
            }


            // if user đc tìm thấy
            // phải lấy được danh sách các role đang có rồi đổ ra view để hiển thị

            RoleNamesOfUser = (await _userManager.GetRolesAsync(user)).ToArray<string>();

            List<string> roleNames = await _roleManager.Roles.Select(x => x.Name).ToListAsync();

            allRoles = new SelectList(roleNames);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound($"Không có user");
            }
            user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Không thấy user với id = {id}");
            }

            // lấy tất cả những role cũ của user đang có
            var oldRoleNames = (await _userManager.GetRolesAsync(user)).ToArray();
            // so sánh giữa role cũ và role mới chọn 

            // nếu có trong role cũ mà ko có trong role mới vừa chọn -> thì đấy là những cái role cần phải xóa
            var deleteRoles = oldRoleNames.Where(x => !RoleNamesOfUser.Contains(x));
            // những cái role có ở trong list role mới nhưng ko có trong role cũ -> thì đấy là những cái role cần thêm vào
            var addRoles = RoleNamesOfUser.Where(x => !oldRoleNames.Contains(x));

            List<string> roleNames = await _roleManager.Roles.Select(x => x.Name).ToListAsync();
            allRoles = new SelectList(roleNames);

            //delete role thừa
            var rsDelete = await _userManager.RemoveFromRolesAsync(user, deleteRoles);
            if (!rsDelete.Succeeded)
            {
                rsDelete.Errors.ToList().ForEach(err =>
                {
                    ModelState.AddModelError(string.Empty, err.Description);
                });
                return Page();
            }
            //thêm một mảng các role cho user
            var rsAdd = await _userManager.AddToRolesAsync(user, addRoles);
            if (!rsAdd.Succeeded)
            {
                rsAdd.Errors.ToList().ForEach(err =>
                {
                    ModelState.AddModelError(string.Empty, err.Description);
                });
                return Page();
            }

            StatusMessage = $"Cập nhật thành công role cho user: {user.UserName}";

            return RedirectToPage("./Index");
        }
    }
}