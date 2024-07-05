using Microsoft.AspNetCore.Identity;
using NET8_Microservices_AuthAPI.Data;
using NET8_Microservices_AuthAPI.Models;
using NET8_Microservices_AuthAPI.Models.DTOs;

namespace NET8_Microservices_AuthAPI.Services.IServices
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext appDbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IJwtTokenGenerator jwtTokenGenerator;

        //DI for saving the user details in DB Identity tables. .NET has many helper classes.
        public AuthService(AppDbContext appDbContext, UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager,IJwtTokenGenerator jwtTokenGenerator)
        {
            this.appDbContext = appDbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            //assign roles like Admin, customer etc to the user
            var user = appDbContext.ApplicationUsers.FirstOrDefault(x => x.Email.ToLower()==email.ToLower());
            if (user != null)
            {
                if(! await roleManager.RoleExistsAsync(roleName))
                {
                    // if role does not exist, add to the Roles table
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
                //add to user_role table
                await userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            //From UI user enters username and password. Now we need to verify the user.
            //If yes, then return the token and UserDTO
            //try
            //{
                var user = appDbContext.ApplicationUsers.FirstOrDefault(x => x.UserName.ToLower() == loginRequestDTO.UserName.ToLower());
                
                bool isValid=await  userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

                if (user == null || isValid ==false )
                {
                    return new LoginResponseDTO() { User=null,Token=""};
                }
                else
                {
                //generate JWT token
                //once you get the token, just go to jwt.io and then paste it to decode.
                //It will give all the claim values. Just for checking
                var accessToken = jwtTokenGenerator.GenerateToken(user);

                UserDTO userDto = new()
                    {
                        Name = user.Name,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Id = user.Id
                    };

                return new LoginResponseDTO() { User = userDto, Token = accessToken };
                }
            //}
            //catch(Exception ex)
            //{

            //}
            //throw new NotImplementedException();
        }

        public async Task<string> Registration(RegistrationRequestDTO registrationRequestDTO)
        {
            //From the UI, user will fill the registration form and submit.
            // This is passsed here. So here we need to save this user data in the DB table.
            //Then return the UserDTO which contains the userID. Does not contain Password.
            // Use DI for managing Users and Roles. .NET have many helper classes

            //Before saving, we need to create ApplicationUser object. Bcz in AppDbContext it uses ApplicationUser
            ApplicationUser applicationUser = new()
            {
                UserName = registrationRequestDTO.Email,
                Email = registrationRequestDTO.Email,
                PhoneNumber = registrationRequestDTO.PhoneNumber,
                NormalizedEmail = registrationRequestDTO.Email.ToUpper(),
                Name = registrationRequestDTO.Name,
            };

            try
            {
                //saves user in Db . Also hash the password and do all security things.
                var result =await userManager.CreateAsync(applicationUser, registrationRequestDTO.Password);
                if (result.Succeeded)
                {
                    var userToReturn = appDbContext.ApplicationUsers.First(x => x.Email == registrationRequestDTO.Email);
                    UserDTO userDTO = new()
                    {
                        Id = userToReturn.Id,
                        Email = userToReturn.Email,
                        PhoneNumber = userToReturn.PhoneNumber,
                        Name = userToReturn.Name
                    };

                    return "";
                }
                else
                    return result.Errors.FirstOrDefault().Description;
            }
            catch(Exception ex)
            {
                return "Error Encountered";
            }
            
        }
    }
}
