using Microsoft.AspNetCore.Identity;

namespace Property_Management_System.Models;
public class ApplicationUser : IdentityUser
{
    public int? BuildingId { get; set; }
}