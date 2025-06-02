using Microsoft.EntityFrameworkCore;
using mini_account_app.Data;
using mini_account_app.Models;

namespace mini_account_app.Service.VoucherEntry
{
    public class VoucherEntryService : IVoucherEntryService
    {
        private readonly ApplicationDbContext _context;
        public VoucherEntryService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<NextVoucherNumber> NextVoucherNumber()
        {
            int maxNo = 1;
            try
            {
                maxNo = await _context.VoucherEntry
                                           .Select(e => (int?)e.VoucherSerial) // Select first
                                           .MaxAsync() ?? 0;               // Then MaxAsync safely
                maxNo += 1;
            }
            catch (Exception err)
            {
                return new NextVoucherNumber(voucherNo: $"V-00{maxNo}", serial: maxNo);
            }

            return new NextVoucherNumber(voucherNo: $"V-00{maxNo}", serial: maxNo);
        }
    }
}
