﻿using Food.DAL;

using Food.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Food.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CostController : Controller
    {
        private readonly AppDbContext _Db;
        public CostController(AppDbContext Db)
        {
            _Db = Db;
        }
        public async Task<IActionResult> Index(/*int page=1*/)
        {
            //    decimal take = 3;
            //    ViewBag.PageCount = Math.Ceiling((await _Db.Costs.CountAsync()/take));
            List<Cost> costs = await _Db.Costs.OrderByDescending(x => x.Id).ToListAsync();




            return View(costs);
        }
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cost cost)
        {
            if (cost.Amount <= 0)
            {
                ModelState.AddModelError("Amount", "Məbləğ düzgün daxil edilməyib.");
                return View();
            }
            cost.By = User.Identity.Name;
            Budget budget = await _Db.Budgets.FirstOrDefaultAsync();
            budget.LastModifiedDescription = cost.Description;
            budget.LastModifiedDate = DateTime.UtcNow.AddHours(4);
            budget.LastModifiedAmount = cost.Amount;
            budget.LastModifiedBy = cost.By;
            budget.TotalBudget -= cost.Amount;
            await _Db.Costs.AddAsync(cost);
            await _Db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
