using API_Vi_EnegryMeter.ViewModels.Account;
using DATA.Interfaces;
using DATA.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API_Vi_EnegryMeter.Controllers.Account
{
   // [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUser _user;
        private readonly string _jwtSecret;

        public AccountController(IUser user, IConfiguration configuration)
        {
            _user = user;
            _jwtSecret = configuration["Jwt:Secret"];
        }
       
        
        [HttpGet]
        [Route("get-user-data")]
        public IActionResult Index()
        {
            try
            {
                var data = _user.GetAll();
                var final = data.Select(x => new SignUpViewModel()
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Email = x.Email,
                    Mobile = x.Mobile,
                    Password = x.Password,
                    Isactive = x.Isactive
                });

                var model = new SignUpListViewModel()
                {
                    lstSingupUser = final
                };
                //ViewData["Heading"] = "ADD USER";
                return Ok(new { status=true, data=model });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }
        
      
        
        public IActionResult UserAlreadyExist(string UserName)
        {
            try
            {
                var data = _user.GetUserByName(UserName);
                if (data != null)
                {
                    return new JsonResult($" User {UserName} is already exist.");
                }
                else
                {
                    return new JsonResult(true);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        #region Login
        [AllowAnonymous]
        public IActionResult login()
        {
            return Ok();
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public IActionResult login(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = _user.GetUserByName(user.UserName.ToString().Trim());
                    if (data != null)
                    {
                        bool b = (data.UserName == user.UserName && DecryptPassword(data.Password) == user.Password);
                        if (b)
                        {
                            bool isAuthentication = false;
                            ClaimsIdentity identity = null;
                            if (data.UserName == "Admin")
                            {
                                identity = new ClaimsIdentity(new[] {
                                    new Claim(ClaimTypes.Name, user.UserName),//For Claim Identity
                                    new Claim(ClaimTypes.Role,"Admin"),
                                    new Claim(ClaimTypes.NameIdentifier,Convert.ToString(data.Id))//For Role Based Authentication
                                }, CookieAuthenticationDefaults.AuthenticationScheme);


                                isAuthentication = true;

                            }
                            else
                            {
                                identity = new ClaimsIdentity(new[] {
                                    new Claim(ClaimTypes.Name, user.UserName),//For Claim Identity
                                    new Claim(ClaimTypes.Role,"User"),//For Role Based Authentication
                                    new Claim(ClaimTypes.NameIdentifier,Convert.ToString(data.Id))
                                }, CookieAuthenticationDefaults.AuthenticationScheme);


                                isAuthentication = true;

                            }

                            if (isAuthentication)
                            {
                                var principle = new ClaimsPrincipal(identity);
                                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle);

                                var token = GenerateJwtToken(identity);
                                return Ok(new { status = true, data = "Login successfully", token });
                            }
                            else
                            {
                                return BadRequest(new { status = false,data = "Please Check Creadentials." });
                            }
                        }
                        else
                        {

                            return BadRequest(new { status = false, data = "Please Check Creadentials." });
                        }
                    }
                    else
                    {

                        return BadRequest(new { status = false, data = "Creadentials Not found / Inactive." });
                    }
                }
                else
                {

                    return BadRequest(new { status = false, data = "Enter Valid Details." });

                }
            }
            catch (Exception ex)
            {

               return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error:{ex.Message}");
            }
           
           
        } 
        #endregion

        private string GenerateJwtToken(ClaimsIdentity identity)
        {
            //var token = Guid.NewGuid().ToString();
            //return token;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                //  Expires = DateTime.UtcNow.AddDays(7), // Token expiry time
                Expires = DateTime.UtcNow.AddMinutes(480),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private static string DecryptPassword(string pwd)
        {
            if (string.IsNullOrEmpty(pwd))
            {
                return null;
            }
            else
            {
                byte[] StorePass = Convert.FromBase64String(pwd);
                string DecryptPass = ASCIIEncoding.ASCII.GetString(StorePass);
                return DecryptPass;
            }
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var allCookies = Request.Cookies.Keys;
            foreach (var cookie in allCookies)
            {
                Response.Cookies.Delete(cookie);
            }
            return RedirectToAction("login");
        }

        #region SingUpViewModel
        public IActionResult SingUp()
        {
            // ViewData["Heading"] = "Add User";
            return Ok();
        }
        [HttpPost]
        public IActionResult SingUp(SignUpViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User obj = new User()
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        Mobile = model.Mobile,
                        Password = Encrypted(model.Password),
                        Isactive = model.Isactive
                    };
                    _user.Add(obj);
                    // TempData["message"] = "User Registration Successfully, Please re-login.";
                    return RedirectToAction("index", "Dashboard");
                }
                ModelState.AddModelError("", "Enter Valid Sign up Details");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return Ok();
        } 
        #endregion

        #region Dotnet Create Login
        //private static string Encrypted(string Pwd)
        //{
        //    if (string.IsNullOrEmpty(Pwd))
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        byte[] storePass = ASCIIEncoding.ASCII.GetBytes(Pwd);
        //        string EncryptPass = Convert.ToBase64String(storePass);
        //        return EncryptPass;
        //    }
        //}

        //[HttpGet]
        //public IActionResult create()
        //{
        //    return Ok();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Route("create-user")]
        //public IActionResult create(SignUpViewModel model)
        //{
        //    try
        //    {
        //        ModelState.Remove("Id");
        //        if (ModelState.IsValid)
        //        {
        //            if (HttpContext.Session.GetString("Username") != null)
        //            {
        //                User obj = new User()
        //                {
        //                    UserName = model.UserName,
        //                    Email = model.Email,
        //                    Mobile = model.Mobile,
        //                    Password = Encrypted(model.Password),
        //                    Isactive = model.Isactive
        //                };
        //                _user.Add(obj);
        //                //  TempData["message"] = "User Registration Successfully, Please re-login.";
        //                return RedirectToAction("index", "Account");
        //            }

        //        }
        //        ModelState.AddModelError("", "Enter Valid Sign up Details");
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("", ex.ToString());
        //    }
        //    return Ok();
        //} 
        #endregion

        [HttpPost]
        [Route("create")]
        //[ValidateAntiForgeryToken]
        public IActionResult CreateUser(User model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var existingUser = _user.GetUserByName(model.UserName.Trim());
                    if (existingUser != null)
                    {
                        return Conflict(new { status = false, data = "Username already exists." }); // HTTP 409 Conflict
                    }

                    // Create the new user
                    User obj = new User()
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        Mobile = model.Mobile,
                        Password = Encrypted(model.Password),
                        Isactive = model.Isactive
                    };
                    _user.Add(obj);

                                     
                    return Ok(new { status = true, data = "User Registration Successfully" });
                }
                else
                {
                    return BadRequest("Invalid sign up details."); // HTTP 400 Bad Request
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error:{ex.Message}"); // HTTP 500 Internal Server Error
            }

        }
        private static string Encrypted(string Pwd)
        {
            if (string.IsNullOrEmpty(Pwd))
            {
                return null;
            }
            else
            {
                byte[] storePass = ASCIIEncoding.ASCII.GetBytes(Pwd);
                string EncryptPass = Convert.ToBase64String(storePass);
                return EncryptPass;
            }
        }



        #region Edit User
        [HttpGet]
        [Route("get-user-foredit")]
        public IActionResult Edit(int User_Id)
        {
            try
            {
                var data = _user.GetUserById(User_Id);
                if (data != null)
                {
                    Signup_Edit_ViewModel model = new Signup_Edit_ViewModel()
                    {
                        Id = data.Id,
                        UserName = data.UserName,
                        Email = data.Email,
                        Mobile = data.Mobile,
                        Password = DecryptPassword(data.Password),
                        ConfirmPassword = DecryptPassword(data.Password),
                        Isactive = data.Isactive
                    };
                    return Ok(new { status = true, data = model });
                }
                else
                {
                    return Ok(new { status = false, data = "User Not Found." });
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
           
        }
       
        [HttpPost]
       // [ValidateAntiForgeryToken]
        [Route("edit-user")]
        public IActionResult Edit(Signup_Edit_ViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = _user.GetUserById(model.Id);
                    if (data != null)
                    {

                        data.UserName = data.UserName;
                        data.Email = model.Email;
                        data.Mobile = model.Mobile;
                        data.Password = Encrypted(model.Password);
                        data.Isactive = model.Isactive;

                        _user.Update(data);

                       

                        return Ok(new { status = true, data = "User Updated Successfully" });

                    }
                    else
                    {
                        return Ok(new { status = false, data = "User Not Found." });

                    }
                }
                else
                {
                    return Ok(new { status = false, data = "Internal server error,ModelState InValid" });
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
           
        } 
        #endregion
    }
}
