using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniAccount.Models;

namespace mini_account_app.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly RoleManager<AppIdentityRole> _roleManager;
        public RoleController(RoleManager<AppIdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        // GET: Role
        public async Task<IActionResult> Index()
        {
            var data = await _roleManager.Roles.AsNoTracking().ToListAsync();

            return View(data);
        }

        // GET: Role/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appIdentityRole = await _roleManager.Roles.AsNoTracking().Where(_ => _.Id == id).FirstOrDefaultAsync();
            if (appIdentityRole == null)
            {
                return NotFound();
            }



            return View(appIdentityRole);
        }

        // GET: Role/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Role/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,NormalizedName,ConcurrencyStamp")] AppIdentityRole appIdentityRole)
        {
            if (ModelState.IsValid)
            {
                await _roleManager.CreateAsync(appIdentityRole);
                return RedirectToAction(nameof(Index));
            }
            return View(appIdentityRole);
        }

        // GET: Role/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appIdentityRole = await _roleManager.Roles.AsNoTracking().Where(_ => _.Id == id).FirstOrDefaultAsync();

            if (appIdentityRole == null)
            {
                return NotFound();
            }
            return View(appIdentityRole);
        }

        // POST: Role/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,NormalizedName,ConcurrencyStamp")] AppIdentityRole appIdentityRole)
        {
            if (id != appIdentityRole.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var newRole = await _roleManager.Roles.FirstOrDefaultAsync(_=>_.Id==id);
                    newRole.Name = appIdentityRole.Name;
                    newRole.NormalizedName = appIdentityRole.NormalizedName;
                    await _roleManager.UpdateAsync(newRole);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppIdentityRoleExists(appIdentityRole.Id))
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
            return View(appIdentityRole);
        }

        // GET: Role/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var appIdentityRole = await _roleManager.Roles.AsNoTracking().Where(_ => _.Id == id).FirstOrDefaultAsync();

            if (appIdentityRole == null)
            {
                return NotFound();
            }

            return View(appIdentityRole);
        }

        // POST: Role/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var appIdentityRole = await _roleManager.Roles.AsNoTracking().Where(_ => _.Id == id).FirstOrDefaultAsync();

            if (appIdentityRole == null)
            {
                return NotFound();
            }

            await _roleManager.DeleteAsync(appIdentityRole);

            return RedirectToAction(nameof(Index));
        }

        private bool AppIdentityRoleExists(string id)
        {
            return _roleManager.FindByIdAsync(id) != null ? true : false;
        }
    }
}
