using Microsoft.AspNetCore.Identity;

namespace BookStore.Models
{
    public class AppUser:IdentityUser
    {
        public string? Address {  get; set; }
        public string? Fullname { get; set; }

    }
}
