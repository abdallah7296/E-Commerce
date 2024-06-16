using E_come.DTO.AccountDTO;
using E_come.DTO.AdressDTO;
using E_come.DTO.DTOAccount;
using E_come.DTO.JWTDTO;
using E_come.DTO.ReviewDTO;
using E_come.Migrations;
using E_come.Model;
using E_come.services.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;

namespace E_come.services
{
    public class AuthServices : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;
        private readonly RoleManager<IdentityRole> rolemanger;
        private readonly DBContext context;
        private readonly JWT jwt;

        public AuthServices(UserManager<ApplicationUser> userManager , IOptions<JWT> jwt , IConfiguration config , 
            RoleManager<IdentityRole>  rolemanger , DBContext context )
        {
            this.userManager = userManager;
            this.config = config;
            this.rolemanger = rolemanger;
            this.context = context;
            this.jwt = jwt.Value;
        }

        public async Task<string> AddRoleAsync(AddRoleDTO model)
        {
           var user = await userManager.FindByNameAsync(model.UserName);
            if (user == null || !await rolemanger.RoleExistsAsync(model.RoleName))
                return "Invalid UserName or RoleName";

            if (await userManager.IsInRoleAsync(user, model.RoleName))
                return "User already is assigned to this role";

            var result = await userManager.AddToRoleAsync(user , model.RoleName);
            return result.Succeeded ? string.Empty : "something went ROng";
          
            
        }

        public async Task<AuthModel> RegisterAsync(RegisterUserDTO model)
        {
            if (await userManager.FindByEmailAsync(model.Email) is not null)
                return new AuthModel { Message = "Email is already registered!" };

            if (await userManager.FindByNameAsync(model.UserName) is not null)
                return new AuthModel { Message = "Username is already registered!" };

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                City   = model.City,
                Phone = model.Phone,
                Street = model.Street,
                
            };
            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                    errors += $"{error.Description},";

                return new AuthModel { Message = errors };
            }
            await userManager.AddToRoleAsync(user, "User");

            var jwtSecurityToken = await CreateJwtToken(user);
            return new AuthModel
            {
                Email = user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = user.UserName,
            };
        }

        public async Task<ShowUserDto> GetUser(string userId)
        {
            
            if (userId == null)
            return new ShowUserDto { message = "User ID is null. Please provide a valid user ID." };

            var user = await userManager.FindByIdAsync(userId);
           
            if (user == null)
                return new ShowUserDto { message = "User not found." };
           
            ShowUserDto showUser = new ShowUserDto();
                showUser.UserName = user.UserName;
                showUser.FirstName = user.FirstName;
                showUser.LastName = user.LastName;
                showUser.Email = user.Email;
                showUser.Phone = user.Phone;
                showUser.City = user.City;
                showUser.Street = user.Street;          
                showUser.IsAuthenticated = true;

            var roles = await userManager.GetRolesAsync(user);
            foreach (var itemRole in roles)
            {
                showUser.Roles.Add(itemRole);
            }
            

         

            var addresses = await context.addresses.Where(a => a.UserName == userId).ToListAsync();
            List<AddressDTO> addressDTOs = new List<AddressDTO>();
            foreach (var item in addresses)
            {
                addressDTOs.Add(new AddressDTO
                {
                    city = item.city,
                    country = item.country,
                    zipCode = item.zipCode,
                    street_number = item.street_number,
                    region = item.region,
                    phone_number = item.Unit_number
                });
                
            }
            showUser.addresses = addressDTOs;
            return showUser;
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            /*  var roleClaims = new List<Claim>();
              foreach (var role in roles)
                  roleClaims.Add(new Claim("roles", role));

              var claims = new[]
              {

                  new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                  new Claim(JwtRegisteredClaimNames.Email, user.Email),
                  new Claim("uid", user.Id)
              }
              .Union(userClaims)
              .Union(roleClaims);
             */

            //claims Token
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

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


            return mytoken;
        }
       

    }
}
