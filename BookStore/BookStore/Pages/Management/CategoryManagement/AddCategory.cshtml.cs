using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Pages.Management.CategoryManagement
{
    [Authorize(Roles = "Administrator, Order staff, Stock manager")]
    public class AddCategoryModel : PageModel
    {
        private readonly BookStoreDbContext _context;

        [TempData]
        public string StatusMessage { get; set; }

        public List<Category> Categories { get; set; }

        [BindProperty]
        public Category NewCategory { get; set; }

        public AddCategoryModel(BookStoreDbContext context, IHostEnvironment hostEnvironment)
        {
            _context = context;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {

            Categories = _context.Categories.ToList();
            if (ModelState.IsValid)
            {
                NewCategory.Active = true;
                try
                {
                    _context.Categories.Add(NewCategory);
                    _context.SaveChanges();

                    StatusMessage = "Thêm mới thể loại thành công.";
                    return RedirectToPage("./CategoryManager");
                }
                catch (Exception ex)
                {
                    StatusMessage = "Thêm thể loại thất bại!";
                    return Page();
                }
            }
            else
            {
                StatusMessage = "Thêm sản phẩm thất bại!";
                return Page();
            }
        }
    }
}