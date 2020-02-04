using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WebApiVotacion.Entidades;

namespace WebApiVotacion.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _rolManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> rolManager,
            ApplicationDbContext context,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _rolManager = rolManager;
            _configuration = configuration;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> CreateUser()
        {
            try
            {

                string line;

                // Read the file and display it line by line.  
                System.IO.StreamReader file = new System.IO.StreamReader(@"C:\Users\sedic\Desktop\Libro1.csv");
                while ((line = file.ReadLine()) != null)
                {
                    var registro = line.Split(';');

                    var user = new ApplicationUser { UserName = registro[0], Nombre = registro[1], Email = "",  PhoneNumber = "", Cedula = registro[0], Activo = true };
                    var result = await _userManager.CreateAsync(user, "Infoseg.00");
                    if (!result.Succeeded)
                    {
                        
                        return Json(new { success = false, message = string.Join("-", registro) });
                    }                    
                }

                return Json(new { success = true, message = "Creacion terminada." });
            }
            catch (Exception exc)
            {
                string ErrorMsg = exc.GetBaseException().InnerException != null ? exc.GetBaseException().InnerException.Message : exc.GetBaseException().Message;
                return Json(new { success = false, message = "Error!. " + ErrorMsg });
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] string cedula)
        {
            try
            {
                var usuario = _userManager.FindByNameAsync(cedula).Result;

                if (usuario == null)
                    return Json(new { success = false, message = "Usuario no valido." });

                return BuildToken(usuario);
            }
            catch (Exception exc)
            {
                string ErrorMsg = exc.GetBaseException().InnerException != null ? exc.GetBaseException().InnerException.Message : exc.GetBaseException().Message;
                return Json(new { success = false, message = "Error! " + ErrorMsg });
            }
        }

        private IActionResult BuildToken(ApplicationUser Usuario)
        {
            try
            {           
                var roles = _userManager.GetRolesAsync(Usuario).Result;

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName, Usuario.Cedula),
                    new Claim(ClaimTypes.NameIdentifier, Usuario.Id),
                    new Claim(ClaimTypes.GivenName, Usuario.Nombre),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token");
                claimsIdentity.AddClaims(roles.Select(role => new Claim(ClaimTypes.Role, role)));

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetConnectionString("Key").ToString()));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var expiration = DateTime.UtcNow.AddDays(1);
                var codigos = claimsIdentity.Claims;

                JwtSecurityToken token = new JwtSecurityToken(
                   issuer: _configuration.GetConnectionString("serverDomain"),
                   audience: _configuration.GetConnectionString("serverDomain"),
                   claims: claimsIdentity.Claims,
                   expires: expiration,
                   signingCredentials: creds);

                return Ok(new
                {
                    success = true,
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = expiration,
                    nombre = Usuario.Nombre,
                    id = Usuario.Id
                });
            }
            catch (Exception exc)
            {
                string ErrorMsg = exc.GetBaseException().InnerException != null ? exc.GetBaseException().InnerException.Message : exc.GetBaseException().Message;
                return Json(new { success = false, message = "Error! " + ErrorMsg });
            }
        }

    }
}