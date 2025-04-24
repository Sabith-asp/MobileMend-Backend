using System.Security.Cryptography;
using Application.Interfaces.Services;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MobileMend.Application.DTOs;
using MobileMend.Application.Interfaces.Repositories;
using MobileMend.Application.Interfaces.Services;
using MobileMend.Common.Helpers;
using MobileMend.Domain.Entities;
//using Org.BouncyCastle.Crypto.Generators;
//using Sprache;

namespace MobileMend.Application.Services
{

    public class AuthService:IAuthService
    {
        private readonly IAuthRepository authrepo;
        private readonly JWTGenerator jwtgenerator;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IEmailService emailService;
        private readonly IConfiguration config;
        public AuthService(IAuthRepository _authrepo,JWTGenerator _jwtgenerator, IHttpContextAccessor _httpContextAccessor,IEmailService _emailService,IConfiguration _config) { 
        authrepo = _authrepo;
            jwtgenerator = _jwtgenerator;
            httpContextAccessor = _httpContextAccessor;
            emailService = _emailService;
            config = _config;
        }
        public async Task<ResponseDTO<object>> Register(RegisterDTO regdata)
        {
            try {
                var usernameExisting = await authrepo.GetByUserName(regdata.UserName);
                if (usernameExisting != null) {
                    return new ResponseDTO<object> { StatusCode = 409, Message = "Username already exist" };
                }
                var emailExisting = await authrepo.GetByUserName(regdata.Email);
                if (emailExisting != null)
                {
                    return new ResponseDTO<object> { StatusCode = 409, Message = "Email already exist" };
                }

                regdata.Password = Hashpassword(regdata.Password);
                var userId = Guid.NewGuid();
                await authrepo.Register(regdata, userId);


                var token = Guid.NewGuid().ToString(); // Can use better token gen
                await authrepo.updateEmailVerifyToken(userId, token);
                var domainURL = config["AppSettings:BackendUrl"];
                var verificationLink = $"{domainURL}/api/EmailService/confirm?email={regdata.Email}&token={token}";
                await emailService.SendEmailAsync(regdata.Email, "Verify your email", $"Click to verify: {verificationLink}");

                return new ResponseDTO<object> { StatusCode = 201, Message = "Registration Successful. Verify your email to login" };
            
            } catch (Exception ex) {
                return new ResponseDTO<object> { StatusCode = 500,Error=ex.Message};
            }
        
        }

        public async Task<ResponseDTO<object>> Login(LoginDTO logindata)
        {
            try {

                var user = await authrepo.GetByUserName(logindata.UserName);
                if (user == null) {
                    return new ResponseDTO<object> { StatusCode = 404,Message="User not found" };
                }
                if (!user.EmailConfirmed) {
                    return new ResponseDTO<object> { StatusCode = 400, Message = "Verify your email to login" };

                }
                if (user.IsBlocked) {
                    return new ResponseDTO<object> { StatusCode = 400, Message = "You are blocked by admin" };
                }
                var verifypass = Verifypassword(logindata.Password, user.PasswordHash);
                if (!verifypass) {

                    return new ResponseDTO<object> { StatusCode = 401, Message = "Invalid Password" };
                
                }
                var token = await jwtgenerator.GenerateToken(user);
                var context = httpContextAccessor.HttpContext;
                
                if (context != null)
                {
                    context.Response.Cookies.Append("accessToken", token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = DateTime.Now.AddDays(7)
                    });
                }
                var refreshtoken = GenerateRefreshToken();
                await authrepo.UpdateRefreshToken(user.UserID,refreshtoken);
                
                return new ResponseDTO<object> { StatusCode = 200, Message = "Login success", Data = new { Token = token,role=user.Role } };



            } catch (Exception ex) {
                return new ResponseDTO<object> { StatusCode = 500, Error = ex.Message };
            }
        
        }


        public async Task<ResponseDTO<object>> RefreshAccessToken(Guid userid)
        {
            try
            {

                var user = await authrepo.CheckRefreshToken(userid);
                if (user == null || user.RefreshTokenExpiry < DateTime.Now)
                {
                    return new ResponseDTO<object> { StatusCode = 401, Message = "Invalid or expired refresh token" };
                }
                var context=httpContextAccessor.HttpContext;

                    var accessToken = await jwtgenerator.GenerateToken(user);
                    var newRefreshToken = GenerateRefreshToken();
                if (context != null)
                {
                    context.Response.Cookies.Append("accessToken", accessToken, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = DateTime.Now.AddDays(7)
                    });
                }
                await authrepo.UpdateRefreshToken(user.UserID, newRefreshToken);
                return new ResponseDTO<object>
                {
                    StatusCode = 200,
                    Message = "Token Refreshed Successfully",
                    
                };

            
            
            } catch (Exception ex) {
                return new ResponseDTO<object> { StatusCode = 500, Error = ex.Message };
            }
        
        }

        public async Task<Guid> GetTechnicianIdByUserId(string userid)
        {
            try {
                var technicianid = await authrepo.GetTechnicianIdByUserId(userid);
                return technicianid;
            } catch (Exception ex) { 
            throw new Exception(ex.Message);
            }
        }

        public async Task<string> GetTechnicianStatus(Guid technicianId)
        {
            try
            {
                var technicianStatus = await authrepo.GetTechnicianStatus(technicianId);

                return technicianStatus;
            }
            catch (Exception ex)
            {
                return ex.Message;

            }
        }





        public string Hashpassword(string password)
        {
            string hasshed_password = BCrypt.Net.BCrypt.HashPassword(password);
            return hasshed_password;
        }

        public bool Verifypassword(string password, string hashedpassword)
        {
            bool verifiedpassword = BCrypt.Net.BCrypt.Verify(password, hashedpassword);
            return verifiedpassword;
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }
    }
}
