using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace BookStore.Pages.Management.BookManagement
{
    [Authorize(Roles = "Administrator, Order staff, Stock manager")]
    public class UpdateProductModel : PageModel
    {
        private readonly BookStoreDbContext _context;

        [TempData]
        public string StatusMessage { get; set; }

        private readonly IWebHostEnvironment _environment;
        public List<Category> Categories { get; set; }
        public List<Book> Books { get; set; }
        public List<Author> Authors { get; set; }

        [BindProperty]
        public Book UpdateBook { get; set; }

        [DataType(DataType.Upload)]
        //[FileExtensions(Extensions = "png, jpg, jpeg, gif")]
        [Display(Name = "Chọn ảnh sản phẩm")]

        [BindProperty]
        public IFormFile FileUpload { get; set; }

        public UpdateProductModel(BookStoreDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _environment = hostEnvironment;
        }

        public void OnGet(int id)
        {
            Categories = _context.Categories.ToList();
            Authors = _context.Authors.ToList();
            UpdateBook = _context.Books.FirstOrDefault(x => x.BookId == id);
        }

        public IActionResult OnPost()
        {
            Categories = _context.Categories.ToList();
            Authors = _context.Authors.ToList();
            Book book = _context.Books.FirstOrDefault(x => x.BookId == UpdateBook.BookId);
            if (ModelState.IsValid)
            {
                String imgURL = "";
                if (FileUpload != null)
                {

                    imgURL = FileUpload.FileName;
                    var file = Path.Combine(_environment.WebRootPath, "Images", FileUpload.FileName);

                    using (var fileStrean = new FileStream(file, FileMode.Create))
                    {
                        FileUpload.CopyTo(fileStrean);
                    }

                }

                if (imgURL != null)
                {
                    UpdateBook.ImageUrl = imgURL;
                }
                else
                {
                    imgURL = UpdateBook.ImageUrl;
                }

                try
                {
                    book.Title = UpdateBook.Title;
                    book.OriginPrice = UpdateBook.OriginPrice;
                    book.Price = UpdateBook.Price;
                    book.CategoryId = UpdateBook.CategoryId;
                    book.ImageUrl = UpdateBook.ImageUrl;
                    book.AuthorId = UpdateBook.AuthorId;
                    book.Description = UpdateBook.Description;
                    book.PublishDate = UpdateBook.PublishDate;
                    book.Publisher = UpdateBook.Publisher;
                    book.Active = UpdateBook.Active;
                    book.StockQuantity = UpdateBook.StockQuantity;


                    _context.Books.Update(book);
                    _context.SaveChanges();

                    StatusMessage = "Cập nhật sản phẩm thành công.";
                    return RedirectToPage("./BookManager");
                }
                catch (Exception ex)
                {
                    StatusMessage = "Cập nhật sản phẩm thất bại!";
                    return Page();
                }
            }
            else
            {
                StatusMessage = "Cập nhật sản phẩm thất bại!";
                return Page();
            }
        }
    }
}
