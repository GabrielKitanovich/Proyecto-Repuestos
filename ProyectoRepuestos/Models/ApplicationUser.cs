using System.Globalization;
using Microsoft.AspNetCore.Identity;
using ProyectoRepuestos.Bases;

namespace ProyectoRepuestos.Models
{

    public class ApplicationUser : BaseIdentityUser
    {
        public string? Custom { get; set; }
    }
}