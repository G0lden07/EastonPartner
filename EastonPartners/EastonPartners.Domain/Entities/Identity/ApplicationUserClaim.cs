using Microsoft.AspNetCore.Identity;

namespace EastonPartners.Domain.Entities.Identity
{
    public class ApplicationUserClaim : IdentityUserClaim<string>
    {
        public virtual ApplicationUser User { get; set; }
    }
}
