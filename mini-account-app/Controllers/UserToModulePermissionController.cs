using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mini_account_app.Data;
using mini_account_app.Models;

namespace mini_account_app.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserToModulePermissionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserToModulePermissionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserToModulePermission
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.UserToModulePermission.Include(u => u.Users);
            return View(await applicationDbContext.OrderBy(_=>_.Users.UserName).ToListAsync());
        }

        // GET: UserToModulePermission/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userToModulePermission = await _context.UserToModulePermission
                .Include(u => u.Users)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userToModulePermission == null)
            {
                return NotFound();
            }

            return View(userToModulePermission);
        }

        // GET: UserToModulePermission/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: UserToModulePermission/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Module")] UserToModulePermission userToModulePermission)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userToModulePermission);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", userToModulePermission.UserId);
            return View(userToModulePermission);
        }

        // GET: UserToModulePermission/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userToModulePermission = await _context.UserToModulePermission.FindAsync(id);
            if (userToModulePermission == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", userToModulePermission.UserId);
            return View(userToModulePermission);
        }

        // POST: UserToModulePermission/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Module")] UserToModulePermission userToModulePermission)
        {
            if (id != userToModulePermission.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userToModulePermission);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserToModulePermissionExists(userToModulePermission.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", userToModulePermission.UserId);
            return View(userToModulePermission);
        }

        // GET: UserToModulePermission/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userToModulePermission = await _context.UserToModulePermission
                .Include(u => u.Users)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userToModulePermission == null)
            {
                return NotFound();
            }

            return View(userToModulePermission);
        }

        // POST: UserToModulePermission/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userToModulePermission = await _context.UserToModulePermission.FindAsync(id);
            if (userToModulePermission != null)
            {
                _context.UserToModulePermission.Remove(userToModulePermission);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserToModulePermissionExists(int id)
        {
            return _context.UserToModulePermission.Any(e => e.Id == id);
        }
    }
}
