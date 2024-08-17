using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoProgramacionAvanzada.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.SqlServer.Server;
using ProyectoProgramacionAvanzada.Data;

namespace ProyectoProgramacionAvanzada.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Product = await _context.Products.FirstOrDefaultAsync(m => m.id == id);

            if (Product == null)
            {
                return NotFound();
            }

            return View(Product);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product Product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(Product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Product = await _context.Products.FindAsync(id);
            if (Product == null)
            {
                return NotFound();
            }

            return View(Product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product Product)
        {
            if (id != Product.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(Product.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View(Product);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Product = await _context.Products.FirstOrDefaultAsync(m => m.id == id);

            if (Product == null)
            {
                return NotFound();
            }

            return View(Product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var Product_Image = await _context.ProductImages.FindAsync(id);
            _context.ProductImages.Remove(Product_Image);



            var Product = await _context.Products.FindAsync(id);
            _context.Products.Remove(Product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.id == id);
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
