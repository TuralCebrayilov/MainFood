using Food.DAL;
using Food.Models;
using Food.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Food.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;

        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM
            {
                HomeSlider = await _db.HomeSliders.Where(x => x.IsDeactive == false).ToListAsync(),
                Comment = await _db.Comments.ToListAsync(),
                MenuCategories = await _db.MenuCategories.Where(x => x.IsDeactive == false).ToListAsync(),
                Drinks = await _db.Drinks.ToListAsync(),
                MenuProducts = await _db.MenuProducts.Where(x => x.IsDeactive == false).ToListAsync(),
                Chef=await _db.Chefs.Where(x=>x.IsDeactive==false).ToListAsync(),
            };
            return View(homeVM);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            MenuProduct menuProduct = await _db.MenuProducts.Where(x => x.IsDeactive == false).Where(x=> x.Id==id).FirstOrDefaultAsync();

            ViewBag.Image = menuProduct.Image;

            return Json(menuProduct);
        }
     

        //public   IActionResult ByCategory(int id)
        //{
        //  MenuCategory category = _db.MenuCategories.Include(c => c.Products).FirstOrDefault(c => c.Id == id);
        //    if (category == null)
        //    {
        //        return NotFound();
        //    }
        //    var product=category.Products;
        //    return View(product);
        //}
        public IActionResult Error()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Comment comment)
        {
            if (ModelState.IsValid)
            {
                await _db.Comments.AddAsync(comment);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(comment);
        }

    }
}