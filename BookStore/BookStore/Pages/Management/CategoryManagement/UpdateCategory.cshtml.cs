using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Pages.Management.CategoryManagement
{
    public class UpdateCategoryModel : PageModel
    {
        private readonly BookStoreDbContext _context;

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public Category UpdateCategory { get; set; }

        public UpdateCategoryModel(BookStoreDbContext context)
        {
            _context = context;
        }

        public void OnGet(int id)
        {
            UpdateCategory = _context.Categories.FirstOrDefault(x => x.CategoryId == id);
        }

        public IActionResult OnPost()
        {
            Category category = _context.Categories.FirstOrDefault(x => x.CategoryId == UpdateCategory.CategoryId);
            if (ModelState.IsValid)
            {
                try
                {
                    category.CategoryName = UpdateCategory.CategoryName;
                    category.Active = UpdateCategory.Active;

                    List<Book> books = _context.Books.Where(x => x.CategoryId == UpdateCategory.CategoryId).ToList();

                    foreach (var book in books)
                    {
                        if (UpdateCategory.Active)
                        {
                            book.Active = true;
                        }
                        else
                        {
                            book.Active = false;
                        }
                        _context.Books.Update(book);
                        _context.SaveChanges();
                    }

                    _context.Categories.Update(category);
                    _context.SaveChanges();
                    StatusMessage = "Cập nhật thể loại thành công.";
                    return RedirectToPage("./CategoryManager");
                }
                catch (Exception ex)
                {
                    StatusMessage = "Cập nhật thể loại thất bại!";
                    return Page();
                }
            }
            else
            {
                StatusMessage = "Cập nhật thể loại thất bại!";
                return Page();
            }
        }
    }
}

