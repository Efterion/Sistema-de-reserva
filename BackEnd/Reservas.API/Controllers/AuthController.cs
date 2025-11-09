using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;

using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;

using System.Text;

using Reservas.Domain.Entities; 

using Reservas.API.Contracts; 

namespace Reservas.API.Controllers

{

    [ApiController]

    [Route("api/[controller]")] 

    public class AuthController : ControllerBase

    {

        private readonly UserManager<User> _userManager;

        private readonly IConfiguration _configuration;

        

        public AuthController(UserManager<User> userManager, IConfiguration configuration)

        {

            _userManager = userManager;

            _configuration = configuration;

        }

        [HttpPost("register")] 

        public async Task<IActionResult> Register([FromBody] RegisterRequest request)

        {

            var userExists = await _userManager.FindByEmailAsync(request.Email);

            if (userExists != null)

            {

                return BadRequest("Error: El email ya está en uso.");

            }

            var user = new User

            {

                Email = request.Email,

                SecurityStamp = Guid.NewGuid().ToString(),

                UserName = request.Username

            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)

            {

                return BadRequest("Error: No se pudo crear el usuario. " + string.Join(", ", result.Errors.Select(e => e.Description)));

            }

            return Ok(new { message = "Usuario creado exitosamente." });

        }

        [HttpPost("login")]

        public async Task<IActionResult> Login([FromBody] AuthRequest request)

        {

            var user = await _userManager.FindByEmailAsync(request.Email);

            // Validamos usuario y contraseña

            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))

            {

                return Unauthorized("Email o contraseña inválidos.");

            }

            // Si es válido, creamos el Token

            var authClaims = new List<Claim>

            {

                new Claim(ClaimTypes.Name, user.UserName),

                new Claim(ClaimTypes.Email, user.Email),

                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

            };

            var token = GenerateJwtToken(authClaims);

            // Devolvemos el token al cliente

            return Ok(new AuthResponse(

                user.Email,

                user.UserName,

                new JwtSecurityTokenHandler().WriteToken(token)

            ));

        }

        private JwtSecurityToken GenerateJwtToken(List<Claim> authClaims)

        {

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(

                issuer: _configuration["Jwt:Issuer"],

                audience: _configuration["Jwt:Audience"],

                expires: DateTime.Now.AddHours(3), // Duración del token

                claims: authClaims,

                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)

            );

            return token;

        }

    }

}
