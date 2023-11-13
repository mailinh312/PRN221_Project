using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Pages.Management.ImportManagement
{
    [Authorize(Roles = "Administrator, Stock manager")]
    public class AddImportDetailModel : PageModel
    {
        private readonly BookStoreDbContext _context;

        [TempData]
        public string StatusMessage { get; set; }

        public List<Book> Books { get; set; }

        [BindProperty]
        public ImportDetail NewImportDetail { get; set; }

        public AddImportDetailModel(BookStoreDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            Books = _context.Books.ToList();
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    NewImportDetail.TotalPrice = NewImportDetail.InputPrice * NewImportDetail.Quantity;
                    DetailImportList.ListImportDetail.Add(NewImportDetail);
                    return RedirectToPage("./AddImport");
                }
                catch (Exception ex)
                {
                    StatusMessage = "Không thể nhập thêm sản phẩm!";
                    Books = _context.Books.ToList();
                    return Page();
                }
            }
            else
            {
                StatusMessage = "Hãy điền đầy đủ thông tin!";
                Books = _context.Books.ToList();
                return Page();
            }
        }
    }
}
