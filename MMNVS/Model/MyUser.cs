using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MMNVS.Model
{
    public class MyUser : IdentityUser
    {
        public string? Notes { get; set; }
    }
}
