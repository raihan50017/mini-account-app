using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mini_account_app.Data;
using mini_account_app.Models;
using mini_account_app.Service.VoucherEntry;

namespace mini_account_app.Controllers
{
    [Authorize(Roles = "Admin,Accountant,Viewer")]
    public class VoucherEntryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IVoucherEntryService _voucherEntryService;

        public VoucherEntryController(ApplicationDbContext context, IVoucherEntryService voucherEntryService)
        {
            _context = context;
            _voucherEntryService = voucherEntryService;
        }

        // GET: VoucherEntryService
        public async Task<IActionResult> Index()
        {
            return View(await _context.VoucherEntry.ToListAsync());
        }

        // GET: VoucherEntryService/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voucherEntry = await _context.VoucherEntry
                .FirstOrDefaultAsync(m => m.Id == id);
            if (voucherEntry == null)
            {
                return NotFound();
            }

            return View(voucherEntry);
        }

        // GET: VoucherEntryService/Create
        public async Task<IActionResult> Create()
        {
            var nextVoucher = await _voucherEntryService.NextVoucherNumber();

            var accounts = await _context.ChartOfAccounts.ToListAsync();

            ViewBag.lstVoucherType = VoucherType.lstVoucherType.Select(_ => new SelectListItem()
            { Value = _, Text = _ });

            ViewBag.lstAccount = accounts.Select(_ => new SelectListItem()
            { Value = _.Id.ToString(), Text = _.AccountType + ":: " + _.AccountName });

            var data = new VoucherEntry();
            data.VoucherSerial = nextVoucher.serial;
            data.VoucherNo = nextVoucher.voucherNo;

            data.lstVoucherEntryDetails = new List<VoucherEntryDetails>()
            {
                new VoucherEntryDetails(),
                new VoucherEntryDetails()
            };

            return View(data);
        }

        // POST: VoucherEntryService/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VoucherEntry voucherEntry)
        {
            if (ModelState.IsValid)
            {
                var nextVoucher = await _voucherEntryService.NextVoucherNumber();
                voucherEntry.VoucherSerial = nextVoucher.serial;
                voucherEntry.VoucherNo = nextVoucher.voucherNo;

                _context.Add(voucherEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(voucherEntry);
        }

        // GET: VoucherEntryService/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accounts = await _context.ChartOfAccounts.ToListAsync();

            ViewBag.lstVoucherType = VoucherType.lstVoucherType.Select(_ => new SelectListItem()
            { Value = _, Text = _ });

            ViewBag.lstAccount = accounts.Select(_ => new SelectListItem()
            { Value = _.Id.ToString(), Text = _.AccountType + ":: " + _.AccountName });

            var voucherEntry = await _context
                .VoucherEntry
                .Include(_ => _.lstVoucherEntryDetails)
                .FirstOrDefaultAsync(_ => _.Id == id);

            if (voucherEntry == null)
            {
                return NotFound();
            }
            return View(voucherEntry);
        }

        // POST: VoucherEntryService/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VoucherEntry voucherEntry)
        {
            if (id != voucherEntry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var entity = await _context
                        .VoucherEntry
                        .Include(_ => _.lstVoucherEntryDetails)
                        .FirstOrDefaultAsync(_ => _.Id == id);

                    if (entity == null)
                    {
                        return NotFound();
                    }

                    entity.VoucherType = voucherEntry.VoucherType;
                    entity.VoucherDate = voucherEntry.VoucherDate;
                    entity.ReferenceNo = voucherEntry.ReferenceNo;
                    entity.lstVoucherEntryDetails = new List<VoucherEntryDetails>();

                    voucherEntry?.lstVoucherEntryDetails?.ForEach(_ =>
                    entity.lstVoucherEntryDetails.Add(new VoucherEntryDetails()
                    {
                        AccountTypeId = _.AccountTypeId,
                        Debit = _.Debit,
                        Credit = _.Credit
                    }));


                    //delete details
                    _context.VoucherEntryDetails.RemoveRange(
                        _context.VoucherEntryDetails.Where(_=>_.MasterId==id)
                        );
                    //end-delete details


                    _context.Update(entity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoucherEntryExists(voucherEntry.Id))
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
            return View(voucherEntry);
        }

        // GET: VoucherEntryService/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voucherEntry = await _context.VoucherEntry
                .FirstOrDefaultAsync(m => m.Id == id);
            if (voucherEntry == null)
            {
                return NotFound();
            }

            return View(voucherEntry);
        }

        // POST: VoucherEntryService/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var voucherEntry = await _context.VoucherEntry.FindAsync(id);
            if (voucherEntry != null)
            {
                _context.VoucherEntry.Remove(voucherEntry);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VoucherEntryExists(int id)
        {
            return _context.VoucherEntry.Any(e => e.Id == id);
        }
    }
}
