using Food.DAL;
using Food.Migrations;
using Food.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Food.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DrinksController : Controller
    {
        private readonly AppDbContext _db;
        public DrinksController(AppDbContext db)
        {
            
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            List<Drink> drinks= await _db.Drinks.ToListAsync();
            return View(drinks);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Drink drink)
        {
            await _db.Drinks.AddAsync(drink);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Drink _DbMenuProduct = await _db.Drinks.FirstOrDefaultAsync(x => x.Id == id);
            if (_DbMenuProduct == null)
            {
                return BadRequest();
            }
            if (_DbMenuProduct.IsDeactive)
            {
                _DbMenuProduct.IsDeactive = false;
            }
            else
            {
                _DbMenuProduct.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Drink _dbdrink = await _db.Drinks.FirstOrDefaultAsync(x => x.Id == id);
            if (_dbdrink == null)
            {
                return BadRequest();
            }
           
            return View(_dbdrink);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Drink drink, int CatId)
        {
            if (id == null)
            {
                return NotFound();
            }
            Drink _dbdrink = await _db.Drinks.FirstOrDefaultAsync(x => x.Id == id);
            if (_dbdrink == null)
            {
                return BadRequest();
            }
         

            
            _dbdrink.Name = drink.Name;
            _dbdrink.Price = drink.Price;
           

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detail(int id)
        {
            Drink menuProduct = await _db.Drinks.FindAsync(id);
            return View(menuProduct);
        }

    }
}
