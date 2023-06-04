using Microsoft.AspNetCore.Identity;

namespace GroupProjectDepositCatalogWebApp.Models
{
    public class ApplicationUserModel : IdentityUser
    {
        //public int Id { get; set; }dotnet identity is a string Id model
        // additional stuff for the user dunno like image?

        // Navigation properties
        public ICollection<DepositModel>? MyDeposits { get; set; }
    }
}