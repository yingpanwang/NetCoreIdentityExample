using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JwtAuth.Cookie.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;

namespace JwtAuth.Cookie.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            string showName = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            ViewBag.ShowName = string.IsNullOrEmpty(showName) ? "请登录" : showName;
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(string name, string password)
        {
            // 模拟登陆
            if (name == "admin" && password == "123")
            {
                var claimIdentity = new ClaimsIdentity("Cookie");
                claimIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, ""));
                claimIdentity.AddClaim(new Claim(ClaimTypes.Name, name));
                claimIdentity.AddClaim(new Claim(ClaimTypes.Email, ""));
                claimIdentity.AddClaim(new Claim(ClaimTypes.MobilePhone, ""));
                claimIdentity.AddClaim(new Claim(ClaimTypes.DateOfBirth, ""));

                var claimsPrincipal = new ClaimsPrincipal(claimIdentity);
                // 在上面注册AddAuthentication时，指定了默认的Scheme，在这里便可以不再指定Scheme。
                await HttpContext.SignInAsync(claimsPrincipal);
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [HttpGet, HttpPost]
        public async Task<IActionResult> Logout()
        {
            if (HttpContext.Request.Method == "GET")
            {
                return View();
            }
            else
            {
                await this.HttpContext.SignOutAsync();
                return RedirectToAction("Index");
            }
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
