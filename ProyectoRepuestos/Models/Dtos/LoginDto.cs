using System.ComponentModel.DataAnnotations;

namespace ProyectoRepuestos.Models.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email is required")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public required string PasswordHash { get; set; }
    }
}