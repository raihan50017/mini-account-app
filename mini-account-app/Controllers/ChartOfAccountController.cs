using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using mini_account_app.Data;
using mini_account_app.Models;
using Newtonsoft.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Collections.Generic;

namespace mini_account_app.Controllers
{
    [Authorize(Roles = "Admin,Accountant")]
    public class ChartOfAccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChartOfAccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ChartOfAccount
        public async Task<IActionResult> Index()
        {
            return View(await _context.ChartOfAccounts.ToListAsync());
        }

        // GET: ChartOfAccount/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chartOfAccounts = await _context.ChartOfAccounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chartOfAccounts == null)
            {
                return NotFound();
            }

            return View(chartOfAccounts);
        }

        // GET: ChartOfAccount/Create
        public IActionResult Create()
        {
            ViewBag.lstAccountType = ChartOfAccountsType.lstAccountType.Select(_ => new SelectListItem()
            { Value = _, Text = _ });

            return View();
        }

        // POST: ChartOfAccount/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AccountType,AccountName")] ChartOfAccount chartOfAccounts)
        {
            if (ModelState.IsValid)
            {

                if (string.IsNullOrEmpty(chartOfAccounts.AccountType) || string.IsNullOrEmpty(chartOfAccounts.AccountName))
                {
                    ModelState.AddModelError("", "Account Type and Account Name are required.");
                    return View(chartOfAccounts);
                }

                await _context.Database.ExecuteSqlRawAsync(
                 "EXEC sp_ManageChartOfAccounts @Action = {0}, @Id = {1}, @AccountType = {2}, @AccountName = {3}",
                "Create", null, chartOfAccounts.AccountType, chartOfAccounts.AccountName);

                return RedirectToAction(nameof(Index));
            }
            return View(chartOfAccounts);
        }

        // GET: ChartOfAccount/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chartOfAccounts = await _context.ChartOfAccounts.FindAsync(id);
            if (chartOfAccounts == null)
            {
                return NotFound();
            }

            var lstAccountType = ChartOfAccountsType.lstAccountType.Select(_ => new SelectListItem()
            { Value = _, Text = _ });

            ViewData["lstAccountType"] = new SelectList(lstAccountType, "Value", "Text", chartOfAccounts.AccountType);

            return View(chartOfAccounts);
        }

        // POST: ChartOfAccount/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountType,AccountName")] ChartOfAccount chartOfAccounts)
        {
            if (id != chartOfAccounts.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    if (string.IsNullOrEmpty(chartOfAccounts.AccountType) || string.IsNullOrEmpty(chartOfAccounts.AccountName))
                    {
                        ModelState.AddModelError("", "Account Type and Account Name are required.");
                        return View(chartOfAccounts);
                    }

                    await _context.Database.ExecuteSqlRawAsync(
                           "EXEC sp_ManageChartOfAccounts @Action = {0}, @Id = {1}, @AccountType = {2}, @AccountName = {3}",
                              "Update", chartOfAccounts.Id, chartOfAccounts.AccountType, chartOfAccounts.AccountName);

                    return RedirectToAction(nameof(Index));

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChartOfAccountsExists(chartOfAccounts.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(chartOfAccounts);
        }

        // GET: ChartOfAccount/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chartOfAccounts = await _context.ChartOfAccounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chartOfAccounts == null)
            {
                return NotFound();
            }

            return View(chartOfAccounts);
        }

        // POST: ChartOfAccount/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chartOfAccounts = await _context.ChartOfAccounts.FindAsync(id);
            if (chartOfAccounts != null)
            {
                await _context.Database.ExecuteSqlRawAsync(
                            "EXEC sp_ManageChartOfAccounts @Action = {0}, @Id = {1}, @AccountType = {2}, @AccountName = {3}",
                            "Delete", id, DBNull.Value, DBNull.Value);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChartOfAccountsExists(int id)
        {
            return _context.ChartOfAccounts.Any(e => e.Id == id);
        }
    }
}
