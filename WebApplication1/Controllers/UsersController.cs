using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;


namespace WebApplication1.Controllers
{



    public class UsersController : Controller
    {
        private readonly AppDbContext _db;

        [BindProperty]
        public Registr Registration { get; set; }
        public User Use { get; set; }


        public UsersController(AppDbContext db)
        {
            _db = db;
        }

        public int GetUserID()
        {

            string UserEmail = User.Identity.Name;
            User user = _db.Users.FirstOrDefault(u => u.Email == UserEmail);
  
            return user.ID;

        }
        public string Hash(string input)
        {

            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login model)
        {

            string pass = Hash(model.Password);
                User user = await _db.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == pass);
                if (user != null)
                {
                    await Authenticate(model.Email); 
                    
                    if (user.Role == "Admin")
                    {
                        return RedirectToAction("Index", "Exams");
                    }
                    else if (user.Role == "User")
                    {
                        return RedirectToAction("Index_us", "Exams");
                    }
                }
                return View(model);

        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Registr model)
        {
            Regex regex = new Regex("(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*])");


            MatchCollection matches = regex.Matches(model.Password);
            if (matches.Count < 4)
            {
                ModelState.AddModelError("Password", "password must contain at least 1 alpha, 1 number, 1 special character, 1 lower");
            }
            if (ModelState.IsValid)
            {
                User user = await _db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {

                    string pass = Hash(model.Password);
                    _db.Users.Add(new User { Email = model.Email, Password = pass, FirstName = model.FirstName, LastName = model.LastName, Role = model.Role });
                    await _db.SaveChangesAsync();
                    await Authenticate(model.Email);

                    return RedirectToAction("Login", controllerName: "Users");

                }
                else
                    ModelState.AddModelError("", "Something wrong, try again");
            }
            return View(model);
        }

        private async Task Authenticate(string userName)
        {
          
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
       
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
           
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Users");
        }
        public ActionResult Index()
        {
            return View();
        }
        public string GetRole()
        {
            string UserEmail = User.Identity.Name;
            User user = _db.Users.FirstOrDefault(u => u.Email == UserEmail);

            return user.Role;
        }

        public IActionResult Account()
        {
            Use = new User();

            Use = _db.Users.FirstOrDefault(u => u.ID == GetUserID());

            return View(Use);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Account(User User)
        {
            _db.Users.Update(User);
            _db.SaveChanges();

            if (GetRole() == "Admin")
            { return RedirectToAction("Index", controllerName: "Exams"); }
            else { return RedirectToAction("Index_us", controllerName: "Exams"); }

        }
        public IActionResult Account_ad()
        {
            Use = new User();
            Use = _db.Users.FirstOrDefault(u => u.ID == GetUserID());
            return View(Use);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Account_ad(User User)
        {
            Regex regex = new Regex("(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*])");


            MatchCollection matches = regex.Matches(User.Password);
            if (matches.Count < 4)
            {
                ModelState.AddModelError("Password", "password must contain at least 1 alpha, 1 number, 1 special character, 1 lower");
            }
            else
            {
                string passw = Hash(User.Password);
                User.Password = passw;
                _db.Users.Update(User);
                _db.SaveChanges();
                if (GetRole() == "Admin")
                { return RedirectToAction("Index", controllerName: "Exams"); }
                else { return RedirectToAction("Index_us", controllerName: "Exams"); }
            }
            return View(User);


        }
    }
 

}