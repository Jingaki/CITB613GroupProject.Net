using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace GroupProjectDepositCatalogWebApp.Models
{
    public class ShiftingInterestRateDataModel
    {
        public int Id { get; set; }
        public int DepositId { get; set; }
        //data
        public int Month { get; set; }
        public float YearlyEIR { get; set; }//effective interest rate
        //data

        // Navigation property
        public DepositModel Deposit { get; set; } = null!;
    }
}
