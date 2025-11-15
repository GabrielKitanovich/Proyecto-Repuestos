using System.Globalization;
using Microsoft.AspNetCore.Identity;

namespace ProyectoRepuestos.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Custom { get; set; }
    }
}