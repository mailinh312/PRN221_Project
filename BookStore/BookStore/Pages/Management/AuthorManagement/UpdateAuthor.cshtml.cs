using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Pages.Management.AuthorManagement
{
    [Authorize(Roles = "Administrator, Order staff, Stock manager")]
    public class UpdateAuthorModel : PageModel
    {
        private readonly BookStoreDbContext _context;

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public Author UpdateAuthor { get; set; }

        public UpdateAuthorModel(BookStoreDbContext context)
        {
            _context = context;
        }

        public void OnGet(int id)
        {
            UpdateAuthor = _context.Authors.FirstOrDefault(x => x.AuthorId == id);
        }

        public IActionResult OnPost()
        {
            Author author = _context.Authors.FirstOrDefault(x => x.AuthorId == UpdateAuthor.AuthorId);
            if (ModelState.IsValid)
            {
                try
                {
                    author.AuthorName = UpdateAuthor.AuthorName;
                    author.Bio = UpdateAuthor.Bio;
                    author.Active = UpdateAuthor.Active;

                    List<Book> books = _context.Books.Where(x => x.AuthorId == UpdateAuthor.AuthorId).ToList();
                    foreach (var book in books)
                    {
                        if (UpdateAuthor.Active)
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

                    _context.Authors.Update(author);
                    _context.SaveChanges();
                    StatusMessage = "Cập nhật tác giả thành công.";
                    return RedirectToPage("./AuthorManager");
                }
                catch (Exception ex)
                {
                    StatusMessage = "Cập nhật tác giả thất bại!";
                    return Page();
                }
            }
            else
            {
                StatusMessage = "Cập nhật tác giả thất bại!";
                return Page();
            }
        }
    }
}
