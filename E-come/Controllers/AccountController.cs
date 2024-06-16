using E_come.DTO.AccountDTO;
using E_come.DTO.DTOAccount;
using E_come.DTO.JWTDTO;
using E_come.Model;
using E_come.services;
using E_come.services.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_come.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;
        private readonly IAuthRepository authRepository;
        private readonly JWT jwt;

        public AccountController(IUserRepository _userRepository, UserManager<ApplicationUser> _userManager
            ,IConfiguration _config ,IAuthRepository authRepository, IOptions<JWT> jwt)
        {
            userRepository = _userRepository;
            userManager = _userManager;
            config = _config;
            this.authRepository = authRepository;
            this.jwt = jwt.Value;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(RegisterUserDTO userDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await authRepository.RegisterAsync(userDTO);

            if (!result.IsAuthenticated)
               return BadRequest(result.Message);

            return Ok(new { email = result.Email ,roles = result.Roles , token = result.Token , expiresOn = result.ExpiresOn });  
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (ModelState.IsValid)
            {
                AuthModel authModel = new AuthModel();
                ApplicationUser user = await userManager.FindByNameAsync(loginDTO.UserName);
              
                if (user != null)
                {
                    var userRoles = await userManager.GetRolesAsync(user);
                    bool result = await userManager.CheckPasswordAsync(user, loginDTO.Password);
                    if (result == true )
                    {
                        //claims Token
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        claims.Add(new Claim(ClaimTypes.Email, user.Email));

                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                        /*   var claims = new List<Claim>()
                             {
                              new Claim("UserName", user.UserName),
                              new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                              new Claim(JwtRegisteredClaimNames.Email, user.Email),
                              new Claim("Id", user.Id)
                             }; */

                        // get role
                        var roles = await userManager.GetRolesAsync(user);
                        foreach (var itemRole in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, itemRole));
                        }
                        SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));///config["JWT:Secret"]));

                        SigningCredentials signingCred =
                            new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                        //Creat Token
                        JwtSecurityToken mytoken = new JwtSecurityToken(

                            issuer: jwt.Issuer,  //config["JWT:ValidIssuer"], //serverurl, //  // url Web Api
                            audience: jwt.Audience, //config["JWT:ValidAudience"],//  serverurl,//url customer angular or React
                            claims: claims,
                            expires: DateTime.Now.AddHours(12),
                            signingCredentials: signingCred
                            );
      
                        return Ok(new
                        {
                            mytoken = new JwtSecurityTokenHandler().WriteToken(mytoken),
                            expiration = mytoken.ValidTo,
                            roles = userRoles
                        });

                    }
                }
                return Unauthorized();
            }
            return Unauthorized();

        }
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromForm] ForgetPassword _user)
        {
         ApplicationUser user = await userManager.FindByNameAsync(_user.UserName);
         var token = await userManager.GeneratePasswordResetTokenAsync(user); 
            if (user != null)
            {
                var result = await userManager.ResetPasswordAsync(user, token,_user.ChangePass);
                if (result.Succeeded)
                {
                    return Ok("New password Add Done");
                }
                else
                {
                    var Errors = string.Empty;
                    foreach (var error in result.Errors)
                    {
                        Errors += $"{error.Description}  +  ";
                    }
                    return BadRequest(Errors);
                }
            }
            return Unauthorized();
        }

        [Authorize(Roles = "User")]
        [HttpPost("changePassword")]
        public async Task<IActionResult> changePassword([FromForm] ChangePassword _user)
        {
            ApplicationUser user = await userManager.FindByNameAsync(_user.UserName);
            if (user != null)
            {

                var result = await userManager.ChangePasswordAsync(user, _user.OldPassword, _user.NewPassword);
                //ChangePasswordAsync(user, user.PasswordHash, NewPassword);
                //  GeneratePasswordResetTokenAsync(user);
                if (result.Succeeded)
                {

                    return Ok("Password  Change Succeeded");
                }
                else
                {
                    var Errors = string.Empty;
                    foreach (var error in result.Errors)
                    {
                        Errors += $"{error.Description}  +  ";
                    }
                    return BadRequest(Errors);
                }


            }
            return Unauthorized();
        }

        [HttpPost("AddRole")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRole(AddRoleDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await authRepository.AddRoleAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(model);
        }
        [HttpGet("ShowUser")]
        public async Task<IActionResult> getAccount()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await authRepository.GetUser(userId);
            if (!user.IsAuthenticated)
            return BadRequest(user.message);


            return Ok(new { user.FirstName, user.LastName, user.Email, user.UserName , 
                            user.Street , user.City , user.Phone , user.Roles,  user.addresses});     
        }


    }
}
