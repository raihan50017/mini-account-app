using mini_account_app.Models;

namespace mini_account_app.Service.VoucherEntry
{
    public interface IVoucherEntryService
    {
        Task<NextVoucherNumber> NextVoucherNumber();
    }
}
