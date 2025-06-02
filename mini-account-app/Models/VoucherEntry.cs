using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mini_account_app.Models
{
    public class VoucherEntry
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Voucher No.")]
        public string? VoucherNo { get; set; }
        public int VoucherSerial { get; set; }

        [Required]
        [DisplayName("Voucher Type")]
        public string? VoucherType { get; set; }

        [Required]
        [DisplayName("Voucher Date")]
        public DateTime VoucherDate { get; set; } = DateTime.Now;

        [DisplayName("Reference No")]
        public string? ReferenceNo { get; set; }
        public List<VoucherEntryDetails>? lstVoucherEntryDetails { get; set; } = new();
    }

    public class VoucherEntryDetails
    {
        public int Id { get; set; }


        [ForeignKey(nameof(VoucherEntries))]
        public int MasterId { get; set; }
        public virtual VoucherEntry? VoucherEntries { get; set; }


        [ForeignKey(nameof(ChartOfAccounts))]
        public int AccountTypeId { get; set; }
        public virtual ChartOfAccount? ChartOfAccounts { get; set; }

        public double Debit { get; set; }
        public double Credit { get; set; }
    }

    public class VoucherType
    {
        public static List<string> lstVoucherType
        {
            get
            {
                return new List<string>()
                {
                    "Journal Vouchers","Payment Vouchers","Receipt Vouchers"
                };
            }
        }
    }

    public record NextVoucherNumber(string voucherNo, int serial);


}
