using AspNetCoreHero.ToastNotification.Abstractions;
using IngressBkpAutomation.Constants;
using IngressBkpAutomation.Models;
using IngressBkpAutomation.Utilities;
using IngressBkpAutomation.ViewModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace IngressBkpAutomation.Controllers
{
    public class HomeController : Controller
    {
        private readonly INotyfService _notyf;
        private readonly IngressSetupDbContext _context;
        private InputValidator _validateService = new InputValidator();

        public HomeController(IngressSetupDbContext context, INotyfService notyf)
        {
            _notyf = notyf;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Denied()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string userID, string password, string returnUrl)
        {
            var userInput = new List<Tuple<string, string, InputDataType>>
            {
                Tuple.Create("username", userID, InputDataType.Default),
                Tuple.Create("password", password, InputDataType.Password),
            };

            var validUserInputs = _validateService.Validate(userInput);
            if (!validUserInputs.Success)
            {
                _notyf.Error(validUserInputs.Message);
                return View();
            }

            var dbUser = await ValidateUser(userID, password);
            if (dbUser == null)
            {
                _notyf.Error("Sorry, Invalid user credentials");
                return View();
            }

            if (!dbUser.Status)
            {
                _notyf.Error("Sorry, Your account has been temporarily disabled. Kindly contact admin");
                return View();
            }

            // Create Claims 
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, dbUser.Names));
            claims.Add(new Claim(ClaimTypes.Role, dbUser.Role));
            claims.Add(new Claim(StrValues.UserId, dbUser.UserID));

            // Create Identity
            var claimsIdentity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            // Create Principal 
            
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTime.Now.AddMinutes(10),
                //IsPersistent = login.RememberLogin
            };
            // Sign In
            await HttpContext.SignInAsync(claimsPrincipal, authProperties);

            _notyf.Success("Logged in successfully");
            // Redirect
            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Backup");
        }

        public async Task<User> ValidateUser(string userID, string password)
        {
            password = Decryptor.Encrypt(password);
            var dbUser = await _context.Users
                .FirstOrDefaultAsync(u => u.UserID.ToUpper().Equals(userID.ToUpper()) && u.Password == password);

            return dbUser;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ListUsers(int page = 1)
        {
            page = page < 1 ? 1 : page;
            const int pageSize = 9;

            var users = await _context.Users.OrderByDescending(s => s.DateCreated).ToListAsync();
            int totalItems = users.Count();
            var pager = new Pager(totalItems, page, pageSize);
            int skip = (page - 1) * pageSize;
            var data = users.Skip(skip).Take(pager.PageSize).ToList();

            ViewBag.Pager = pager;
            return View(users);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult NewUser()
        {
            ViewBag.roles = new SelectList(ArrValues.Roles);
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult NewUser([Bind("UserID,Names,Password,ConfirmPassword,Email,Phone,Status,Role")] User user)
        {
            try
            {
                user.Role = user?.Role ?? StrValues.UserRole;
                var userInput = new List<Tuple<string, string, InputDataType>>
                {
                    Tuple.Create("User ID", user.UserID, InputDataType.Default),
                    Tuple.Create("Password", user.Password, InputDataType.Password),
                    Tuple.Create("Confirm Password", user.ConfirmPassword, InputDataType.Password),
                };

                var validUserInputs = _validateService.Validate(userInput);
                if (!validUserInputs.Success)
                {
                    _notyf.Error(validUserInputs.Message);
                    return View();
                }

                if (!user.Password.Equals(user.ConfirmPassword))
                {
                    _notyf.Error("Sorry, Password and Confirm password do not match");
                    return View(user);
                }

                if (_context.Users.Any(s => s.UserID.ToUpper().Equals(user.UserID.ToUpper())))
                {
                    _notyf.Error("Sorry, User already exist");
                    return View(user);
                }

                user.Password = Decryptor.Encrypt(user.Password);
                _context.Users.Add(user);
                _context.SaveChanges();
                _notyf.Success("User added successfully");
                return RedirectToAction("ListUsers");
            }
            catch (Exception)
            {
                _notyf.Error("Sorry, An error occurred");
                return View(user);
            }
        }
    }
}