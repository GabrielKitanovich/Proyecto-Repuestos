using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProyectoRepuestos.Bases;
using ProyectoRepuestos.Models;
using ProyectoRepuestos.Models.Dtos;
using ProyectoRepuestos.Services;

namespace ProyectoRepuestos.Controllers
{
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        private readonly IMapper _mapper;

        public AuthenticationController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context, IConfiguration configuration, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost("register-user")]
        public async Task<IActionResult> Register(ApplicationUserDTO payload)
        {
            var userExists = await _userManager.FindByEmailAsync(payload.Email);
            if (userExists != null)
            {
                return BadRequest($"User {payload.Email} already exists");
            }
            ApplicationUser user = _mapper.Map<ApplicationUser>(payload);
            var result = await _userManager.CreateAsync(user, payload.PasswordHash);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest($"User creation failed: {errors}");
            }
            return Created(nameof(Register), $"User {payload.Email} created successfully!");
        }
    }
}