using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using mini_account_app.Data;
using mini_account_app.Models;
using mini_account_app.Service.VoucherEntry;
using System.Data;

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

                int newId;

                var parameters = new[]
                {
                    new SqlParameter("@VoucherNo", voucherEntry.VoucherNo),
                    new SqlParameter("@VoucherSerial", voucherEntry.VoucherSerial),
                    new SqlParameter("@VoucherType", voucherEntry.VoucherType),
                    new SqlParameter("@VoucherDate", voucherEntry.VoucherDate),
                    new SqlParameter("@ReferenceNo", (object?)voucherEntry.ReferenceNo ?? DBNull.Value),
                    new SqlParameter
                    {
                        ParameterName = "@NewId",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    }
                };

                await _context.Database.ExecuteSqlRawAsync("EXEC sp_InsertVoucherEntry @VoucherNo, @VoucherSerial, @VoucherType, @VoucherDate, @ReferenceNo, @NewId OUT", parameters);

                newId = (int)parameters[5].Value;

                foreach (var detail in voucherEntry.lstVoucherEntryDetails!)
                {
                    var detailParams = new[]
                    {
                        new SqlParameter("@MasterId", newId),
                        new SqlParameter("@AccountTypeId", detail.AccountTypeId),
                        new SqlParameter("@Debit", detail.Debit),
                        new SqlParameter("@Credit", detail.Credit)
                    };

                    await _context.Database.ExecuteSqlRawAsync("EXEC sp_InsertVoucherEntryDetail @MasterId, @AccountTypeId, @Debit, @Credit", detailParams);
                }
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
                    var updateParams = new[]
                    {
                        new SqlParameter("@Id", id),
                        new SqlParameter("@VoucherType", voucherEntry.VoucherType!),
                                        new SqlParameter("@VoucherDate", voucherEntry.VoucherDate),
                        new SqlParameter("@ReferenceNo", (object?)voucherEntry.ReferenceNo ?? DBNull.Value)
                    };

                    await _context.Database.ExecuteSqlRawAsync("EXEC sp_UpdateVoucherEntry @Id, @VoucherType, @VoucherDate, @ReferenceNo", updateParams);

                    var deleteParams = new[]
                    {
                            new SqlParameter("@MasterId", id)
                    };

                    await _context.Database.ExecuteSqlRawAsync("EXEC sp_DeleteVoucherEntryDetailsByMasterId @MasterId", deleteParams);

                    if (voucherEntry.lstVoucherEntryDetails != null)
                    {
                        foreach (var detail in voucherEntry.lstVoucherEntryDetails)
                        {
                            var detailParams = new[]
                            {
                                new SqlParameter("@MasterId", id),
                                new SqlParameter("@AccountTypeId", detail.AccountTypeId),
                                new SqlParameter("@Debit", detail.Debit),
                                new SqlParameter("@Credit", detail.Credit)
                            };

                            await _context.Database.ExecuteSqlRawAsync("EXEC sp_InsertVoucherEntryDetail @MasterId, @AccountTypeId, @Debit, @Credit", detailParams);
                        }
                    }
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
            var idParam = new SqlParameter("@Id", id);

            await _context.Database.ExecuteSqlRawAsync("EXEC sp_DeleteVoucherEntryWithDetails @Id", idParam);

            return RedirectToAction(nameof(Index));
        }

        private bool VoucherEntryExists(int id)
        {
            return _context.VoucherEntry.Any(e => e.Id == id);
        }
    }
}
