using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace BookStore.Pages.Management.BookManagement
{
    public class UpdateProductModel : PageModel
    {
        private readonly BookStoreDbContext _context;

        [TempData]
        public string StatusMessage { get; set; }

        private IHostEnvironment _environment;
        public List<Category> Categories { get; set; }
        public List<Book> Books { get; set; }
        public List<Author> Authors { get; set; }

        [BindProperty]
        public Book UpdateBook { get; set; }

        [Required(ErrorMessage = "Chọn ít nhất 1 tệp")]
        [DataType(DataType.Upload)]
        //[FileExtensions(Extensions = "png, jpg, jpeg, gif")]
        [Display(Name = "Chọn ảnh sản phẩm")]

        [BindProperty]
        public IFormFile[] FileUploads { get; set; }

        public UpdateProductModel(BookStoreDbContext context, IHostEnvironment hostEnvironment)
        {
            _context = context;
            _environment = hostEnvironment;
            FileUploads = new IFormFile[5];
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
                if (FileUploads != null)
                {
                    foreach (var FileUpload in FileUploads)
                    {
                        imgURL = FileUpload.FileName;
                        var file = Path.Combine(_environment.ContentRootPath, "E:\\Documents\\PRN221\\BookStore\\BookStore\\wwwroot\\Images\\", FileUpload.FileName);

                        using (var fileStrean = new FileStream(file, FileMode.Create))
                        {
                            FileUpload.CopyTo(fileStrean);
                        }
                    }
                }


                if (imgURL != null)
                {
                    UpdateBook.ImageUrl = imgURL;
                }
                try
                {
                    book.Title = UpdateBook.Title;
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
