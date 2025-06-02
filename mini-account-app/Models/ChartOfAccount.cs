using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mini_account_app.Models
{
    [Table("ChartOfAccounts")]
    public class ChartOfAccount
    {
        public int Id { get; set; }
        [Required]
        public string? AccountType { get; set; }
        [Required]
        public string? AccountName { get; set; }
    }

    public class ChartOfAccountsType
    {
        public static List<string> lstAccountType
        {
            get
            {
                return new List<string>()
                {
                    "Cash","Bank","Receivable"
                };
            }
        }
    }

}
