using Microsoft.AspNetCore.Identity;

namespace Resource.Domain.Entity
{
    public class ReservationHolder: IdentityUser
    {
        public virtual ICollection<Resource> Resources { get; set; }

        public ReservationHolder()
        {
            
        }
    }

    public static class ReservationHolderRoles
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }
}
