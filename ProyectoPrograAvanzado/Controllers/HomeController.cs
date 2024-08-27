using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProyectoProgramacionAvanzada.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using ProyectoProgramacionAvanzada.Data;
using ProyectoProgramacionAvanzada.ViewModel;

namespace ProyectoProgramacionAvanzada.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                                    .Include(p => p.Images)
                                    .ToListAsync();

            return View(products);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            
            var product = await _context.Products
                                        .Include(p => p.Images)
                                        .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
           
            var product = new Product
            {
                Name = model.Name,
                Price = model.Price,
                Details = model.Details,
                Images = new List<ProductImage>()
            };

            if (model.Images != null && model.Images.Count > 0) 
            {
                string folder = "products/";

                model.productImages = new List<ProductImageViewModel>();

                foreach (var file in model.Images)
                {
                    var imageUrl = await UploadImage(folder, file);

                    var productImage = new ProductImage
                    {
                        ImageUrl = imageUrl,
                        Name = file.FileName,
                        product = product
                    };

                    product.Images.Add(productImage);
                }
            }


              
            _context.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var product = await _context.Products
                               .Include(p => p.Images)
                               .FirstOrDefaultAsync(p => p.Id == id);

            if (id == null)
            {
                return NotFound();
            }

            if (product == null)
            {
                return NotFound();
            }

            var viewModel = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Details = product.Details,
                productImages = product.Images.Select(pi => new ProductImageViewModel
                {
                    Id = pi.Id,
                    Image_Url = pi.ImageUrl,
                    Name = pi.Name
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel viewModel, IFormFile newImage)
        {
            

            var product = await _context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == viewModel.Id);

            if (product == null)
            {
                return NotFound();
            }

            product.Name = viewModel.Name;
            product.Price = viewModel.Price;
            product.Details = viewModel.Details;

            if (newImage != null && newImage.Length > 0)
            {
                var imageFileName = $"{Guid.NewGuid()}_{newImage.FileName}";
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/products", imageFileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await newImage.CopyToAsync(stream);
                }

                var newProductImage = new ProductImage
                {
                    ImageUrl = $"/products/{imageFileName}",
                    Name = newImage.FileName,
                    ProductId = product.Id
                };

                _context.ProductImages.Add(newProductImage);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var productImage = await _context.ProductImages.FindAsync(id);
            _context.ProductImages.Remove(productImage);



            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Cart()
        {
            return View();
        }


        private async Task<string> UploadImage(string folderPath, IFormFile file)
        {
            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;
            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);
            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
            return "/" + folderPath;
                 

        }

        [HttpPost]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var image = await _context.ProductImages.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.ImageUrl.TrimStart('/'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            _context.ProductImages.Remove(image);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Edit), new { id = image.ProductId });
        }

        [Authorize(Roles = "admin")]
        public IActionResult RestrictedAction() {
        return View();
        }

        [NonAction]
        public string NonActionMethod()
        {
            return "This is a non-action method";
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
