using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Pages.Management.ImportManagement
{
    [Authorize(Roles = "Administrator, Stock manager")]
    public class AddImportModel : PageModel
    {
        private readonly BookStoreDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        [TempData]
        public string StatusMessage { get; set; }

        public List<ImportDetail> ImportDetails { get; set; }

        public const int ITEMS_PER_PAGE = 5;

        [BindProperty(SupportsGet = true, Name = "p")]
        public int CurrentPage { get; set; }
        public int CountPages { get; set; }


        public AddImportModel(BookStoreDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public void OnGet()
        {
            ImportDetails = DetailImportList.ListImportDetail;
            if (ImportDetails != null)
            {
                foreach (var import in ImportDetails)
                {
                    Book book = _context.Books.FirstOrDefault(x => x.BookId == import.BookId);
                    import.Book = book;
                }
            }

            CountPages = ImportDetails.Count() / ITEMS_PER_PAGE;
            if ((ImportDetails.Count() / ITEMS_PER_PAGE) % 5 != 0)
            {
                CountPages = CountPages + 1;
            }

            if (CurrentPage < 1)
            {
                CurrentPage = 1;
            }
            if (CurrentPage > CountPages)
            {
                CurrentPage = CountPages;
            }

            ImportDetails = ImportDetails.Skip((CurrentPage - 1) * 5).Take(ITEMS_PER_PAGE).ToList();
        }

        public async Task<IActionResult> OnPost()
        {
            ImportDetails = DetailImportList.ListImportDetail;
            Decimal? total = 0;
            if (ImportDetails.Count < 1)
            {
                StatusMessage = "Chưa có sản phẩm nào được chọn để nhập hàng.";
                return Page();
            }

            foreach (var item in ImportDetails)
            {
                total += item.TotalPrice;
            }

            Import import = new Import();
            import.UserId = _userManager.GetUserId(User);
            import.ImportDate = DateTime.Now;
            import.TotalPrice = total;

            _context.Imports.Add(import);
            _context.SaveChanges();
            addImportDetail(DetailImportList.ListImportDetail, import.ImportId);
            StatusMessage = "Đã lưu phiếu nhập hàng.";
            return Page();
        }

        public void addImportDetail(List<ImportDetail> list, int importId)
        {
            foreach (var item in list)
            {
                item.ImportId = importId;
                Book book = _context.Books.FirstOrDefault(x => x.BookId == item.BookId);
                //update lại số lượng trong kho, giá nhập và giá bán của sản phẩm vừa được nhập thêm
                book.StockQuantity += item.Quantity;
                book.OriginPrice = item.InputPrice;
                book.Price = item.OutputPrice;
                _context.Books.Update(book);
                _context.SaveChanges();
                item.Book = book;
                _context.ImportDetails.Add(item);
                _context.SaveChanges();
            }
            list.Clear();
        }
    }
}