using System.ComponentModel.DataAnnotations;

namespace GroupProjectDepositCatalogWebApp.Models
{
    public class CalculatorEntity
    {
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public float InvestmentSum { get; set; }
        public int DurrationInMonths { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}%")]
        public float EffectiveYearlyInterest { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public float Total { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public float InterestSum { get; set; }
        public int productId { get; set; }
    }
}
