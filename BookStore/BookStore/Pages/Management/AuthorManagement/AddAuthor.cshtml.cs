using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Pages.Management.AuthorManagement
{
    [Authorize(Roles = "Administrator, Order staff, Stock manager")]
    public class AddAuthorModel : PageModel
    {
        private readonly BookStoreDbContext _context;

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public Author NewAuthor { get; set; }

        public AddAuthorModel(BookStoreDbContext context, IHostEnvironment hostEnvironment)
        {
            _context = context;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {

            if (ModelState.IsValid)
            {
                NewAuthor.Active = true;
                try
                {
                    _context.Authors.Add(NewAuthor);
                    _context.SaveChanges();

                    StatusMessage = "Thêm mới tác giả thành công.";
                    return RedirectToPage("./AuthorManager");
                }
                catch (Exception ex)
                {
                    StatusMessage = "Thêm tác giả thất bại!";
                    return Page();
                }
            }
            else
            {
                StatusMessage = "Thêm tác giả thất bại!";
                return Page();
            }
        }
    }
}
