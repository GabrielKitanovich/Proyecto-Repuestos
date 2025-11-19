using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

        private readonly TokenValidationParameters _tokenValidationParameters;

        public AuthenticationController(
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            ApplicationDbContext context, 
            IConfiguration configuration, 
            IMapper mapper, 
            TokenValidationParameters tokenValidationParameters)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _configuration = configuration;
            _tokenValidationParameters = tokenValidationParameters;
            _mapper = mapper;
        }

        [HttpPost("register-user")]
        public async Task<IActionResult> Register(ApplicationUserDTO payload)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid registration data.");
            }

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

            switch (payload.Role)
            {
                case "Admin":
                    await _userManager.AddToRoleAsync(user, Enums.UserRoles.Admin.ToString());
                    break;
                case "User":
                    await _userManager.AddToRoleAsync(user, Enums.UserRoles.User.ToString());
                    break;
                case "SignedOff":
                    await _userManager.AddToRoleAsync(user, Enums.UserRoles.SignedOff.ToString());
                    break;
                default:
                    await _userManager.AddToRoleAsync(user, Enums.UserRoles.User.ToString());
                    break;
            }

            return Created(nameof(Register), $"User {payload.Email} created successfully!");
        }

        [HttpPost("login-user")]
        public async Task<IActionResult> Login(LoginDto payload)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid login data.");
            }

            var user = await _userManager.FindByEmailAsync(payload.Email);
            if (user != null || await _userManager.CheckPasswordAsync(user!, payload.PasswordHash))
            {
                var token = await GenerateJwtTokenAsync(user!, "");
                return Ok(token);
            }
            return Unauthorized("Invalid email or password.");
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequestDto payload)
        {
            try
            {
                var result = await VerifyAndGenerateToken(payload);
                if (result == null)
                {
                    return BadRequest($"Invalid refresh token.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        private async Task<AuthResultDto?> VerifyAndGenerateToken(TokenRequestDto payload)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                 // Validate the token format
                var tokenInVerification = jwtTokenHandler.ValidateToken(payload.Token, _tokenValidationParameters, out var validatedToken);
                // Validate encryption algorithm
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                    if (result == false)
                    {
                        Console.WriteLine("Invalid token algorithm");
                        return null;
                    }
                }
                // Check if token is expired
                var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp)!.Value);
                var expiryDate = DateTimeOffset.FromUnixTimeSeconds(utcExpiryDate).UtcDateTime;
                if (expiryDate > DateTime.UtcNow)
                {
                    throw new Exception("Token has not yet expired.");
                }
                // Check if refresh token exists
                var dbRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == payload.RefreshToken);
                if (dbRefreshToken == null)
                {
                    throw new Exception("Refresh token does not exist.");
                }
                else
                {
                    //validate refresh token
                    var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)!.Value;
                    if (dbRefreshToken.JwtId != jti)
                    {
                        throw new Exception("Refresh token does not match JWT.");
                    }
                    // Check if refresh token is expired
                    if (dbRefreshToken.DateExpire < DateTime.UtcNow)
                    {
                        throw new Exception("Refresh token has expired.");
                    }
                    // Check if refresh token is revoked
                    if (dbRefreshToken.IsRevoked)
                    {
                        throw new Exception("Refresh token has been revoked.");
                    }
                    var dbUserData = await _userManager.FindByIdAsync(dbRefreshToken.UserId);
                    var newTokenResponse = await GenerateJwtTokenAsync(dbUserData!, payload.RefreshToken);
                    return newTokenResponse;

                }
            }
            catch (SecurityTokenExpiredException)
            {
                var dbRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == payload.RefreshToken) ?? throw new Exception("Refresh token does not exist.");
                // Generate new token
                var dbUserData = await _userManager.FindByIdAsync(dbRefreshToken.UserId);
                    var newTokenResponse = await GenerateJwtTokenAsync(dbUserData!, payload.RefreshToken);

                    return newTokenResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Something went wrong: {ex.Message}");
            }

           

        }

        private async Task<AuthResultDto> GenerateJwtTokenAsync(ApplicationUser user, string existingRefreshToken)
        {
            var authClaims = new List<Claim>()
            {
                new(ClaimTypes.Name, user.UserName!),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(JwtRegisteredClaimNames.Email, user.Email!),
                new(JwtRegisteredClaimNames.Sub, user.Email!),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            // Add user roles to claims
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]!));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.UtcNow.AddMinutes(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = new RefreshToken();

            if (string.IsNullOrEmpty(existingRefreshToken))
            {
                refreshToken = new RefreshToken()
                {
                    Token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString(),
                    JwtId = token.Id,
                    IsRevoked = false,
                    UserId = user.Id,
                    DateAdded = DateTime.UtcNow,
                    DateExpire = DateTime.UtcNow.AddMonths(6)
                };
                await _context.RefreshTokens.AddAsync(refreshToken);
                await _context.SaveChangesAsync();
            }


            var response = new AuthResultDto
            {
                Token = jwtToken,
                RefreshToken = string.IsNullOrEmpty(existingRefreshToken) ? refreshToken.Token : existingRefreshToken,
                ExpiresAt = token.ValidTo
            };
            return response;
        }
    }


}