using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace BookStore.Pages.Management.BookManagement
{
    [Authorize(Roles = "Administrator, Order staff, Stock manager")]
    public class AddProductModel : PageModel
    {
        private readonly BookStoreDbContext _context;

        [TempData]
        public string StatusMessage { get; set; }

        private IHostEnvironment _environment;
        public List<Category> Categories { get; set; }
        public List<Book> Books { get; set; }
        public List<Author> Authors { get; set; }

        [BindProperty]
        public Book NewBook { get; set; }

        [Required(ErrorMessage = "Chọn ít nhất 1 tệp")]
        [DataType(DataType.Upload)]
        //[FileExtensions(Extensions = "png, jpg, jpeg, gif")]
        [Display(Name = "Chọn ảnh sản phẩm")]

        [BindProperty]
        public IFormFile[] FileUploads { get; set; }

        public AddProductModel(BookStoreDbContext context, IHostEnvironment hostEnvironment)
        {
            _context = context;
            _environment = hostEnvironment;
            FileUploads = new IFormFile[5];
        }

        public void OnGet(IFormFile formFile)
        {
            Categories = _context.Categories.ToList();
            Authors = _context.Authors.ToList();
        }



        public IActionResult OnPost()
        {

            Categories = _context.Categories.ToList();
            Authors = _context.Authors.ToList();
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
                    NewBook.ImageUrl = imgURL;
                }
                NewBook.Active = true;
                try
                {
                    _context.Books.Add(NewBook);
                    _context.SaveChanges();

                    StatusMessage = "Thêm mới sản phẩm thành công.";
                    return RedirectToPage("./BookManager");
                }
                catch (Exception ex)
                {
                    StatusMessage = "Thêm sản phẩm thất bại!";
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
