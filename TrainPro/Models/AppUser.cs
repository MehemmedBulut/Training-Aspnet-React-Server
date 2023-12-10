using Microsoft.AspNetCore.Identity;

namespace TrainPro.Models
{
    public class AppUser:IdentityUser
    {
        public string Name { get; set; }
    }
}
