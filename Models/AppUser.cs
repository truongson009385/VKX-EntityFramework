using Microsoft.AspNetCore.Identity;
using System.Xml.Linq;

namespace api.Models
{
    public class AppUser : IdentityUser
    {
        public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
    }
}
