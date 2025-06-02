using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mini_account_app.Models;
using MiniAccount.Models;

namespace mini_account_app.Controllers
{
    [Authorize]
    public class UserToRoleMappingController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<AppIdentityRole> _roleManager;


        public UserToRoleMappingController(UserManager<IdentityUser> userManager, RoleManager<AppIdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: UserToRoleMapping
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();

            var userRolesViewModel = new List<UserToRoleMappingDto>();

            foreach (var _ in users)
            {
                var roles = await _userManager.GetRolesAsync(_);
                userRolesViewModel.Add(new UserToRoleMappingDto
                {
                    UserId = _.Id,
                    User = _.UserName,
                    Roles = string.Join(", ", roles.ToList())
                });
            }

            return View(userRolesViewModel);
        }

        // GET: UserToRoleMapping/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var userToRoleMapping = await _context.UserToRoleMapping
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (userToRoleMapping == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(userToRoleMapping);
        //}

        // GET: UserToRoleMapping/Create
        public async Task<IActionResult> Create()
        {
            var users = await _userManager.Users.ToListAsync();
            var userItems = new List<SelectListItem>();
            users.ForEach(_ =>
            userItems.Add(new SelectListItem()
            { Value = _.Id, Text = _.UserName }
            ));

            var roles = await _roleManager.Roles.ToListAsync();

            ViewBag.UserList = userItems;

            var data = new UserToRoleMappingDto_Create();
            data.Roles = roles.Select(_ => new RoleSelection() { RoleName = _.Name }).ToList();

            return View(data);
        }

        // POST: UserToRoleMapping/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserToRoleMappingDto_Create userToRoleMapping)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(userToRoleMapping.UserId);
                if (user == null)
                {
                    return NotFound();
                }

                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, roles);

                foreach (var item in userToRoleMapping.Roles)
                {
                    if (item.RoleName != "" && item.IsSelected)
                    {

                        await _userManager.AddToRoleAsync(user, item.RoleName);

                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View(userToRoleMapping);
        }

        // GET: UserToRoleMapping/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);

            var data = new UserToRoleMappingDto_Create()
            {
                UserId = user.Id,
                User = user.UserName,
                Roles = roles.Select(_ => new RoleSelection()
                {
                    IsSelected = userRoles.Contains(_.Name),
                    RoleName = _.Name
                })
                .ToList()
            };


            return View(data);
        }

        // POST: UserToRoleMapping/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserToRoleMappingDto_Create userToRoleMapping)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(userToRoleMapping.UserId);
                if (user == null)
                {
                    return NotFound();
                }

                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, roles);

                foreach (var item in userToRoleMapping.Roles)
                {
                    if (item.RoleName != "" && item.IsSelected)
                    {

                        await _userManager.AddToRoleAsync(user, item.RoleName);

                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View(userToRoleMapping);
        }

        // GET: UserToRoleMapping/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);

            var data = new UserToRoleMappingDto_Create()
            {
                UserId = user.Id,
                User = user.UserName,
                Roles = roles.Select(_ => new RoleSelection()
                {
                    IsSelected = userRoles.Contains(_.Name),
                    RoleName = _.Name
                })
                .ToList()
            };


            return View(data);
        }

        // POST: UserToRoleMapping/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);

            return RedirectToAction(nameof(Index));
        }

        //private bool UserToRoleMappingExists(int id)
        //{
        //    return _context.UserToRoleMapping.Any(e => e.Id == id);
        //}
    }
}
