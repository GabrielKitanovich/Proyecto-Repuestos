using System.ComponentModel.DataAnnotations;

namespace ProyectoRepuestos.Models.Dtos
{
    public class ApplicationUserDTO
    {
        [Required]
        public required string UserName { get; set; }
        [Required]
        public required string Email { get; set; }
        
        [Required]
        public required string PasswordHash { get; set; }
    }
}