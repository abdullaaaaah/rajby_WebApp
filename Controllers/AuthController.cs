using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Rajby_web.Models;

namespace AspnetCoreMvcFull.Controllers
{
  public class AuthController : Controller
  {
    private readonly RajbyTextileContext context;

    public AuthController(RajbyTextileContext context)
    {
      this.context = context;
    }

    // Encryption and Decryption Methods
    public class PasswordHelper
    {
      // Encode the password by adding 50 to the ASCII value of each character
      public static string EncodePass(string password)
      {
        StringBuilder encodedPassword = new StringBuilder();

        if (!string.IsNullOrEmpty(password))
        {
          foreach (char c in password)
          {
            int encodedChar = (int)c + 50; // Add 50 to ASCII value
            encodedPassword.Append((char)encodedChar); // Convert back to char and append
          }
        }

        return encodedPassword.ToString();
      }

      // Decode the password by subtracting 50 from the ASCII value of each character
      public static string DecodePass(string encodedPassword)
      {
        StringBuilder decodedPassword = new StringBuilder();

        if (!string.IsNullOrEmpty(encodedPassword))
        {
          foreach (char c in encodedPassword)
          {
            int decodedChar = (int)c - 50; // Subtract 50 from ASCII value
            decodedPassword.Append((char)decodedChar); // Convert back to char and append
          }
        }

        return decodedPassword.ToString();
      }
    }

    // GET: Login page
    [HttpGet]
    public IActionResult LoginBasic()
    {
      return View();
    }

    // POST: Login handling
    [HttpPost]
    public async Task<IActionResult> LoginBasic(LoginViewModel model)
    {
      if (!ModelState.IsValid)
        return View(model);

      // Validate user credentials
      var user = context.SmsUsers.FirstOrDefault(u => u.UserName == model.Username);

      if (user == null || PasswordHelper.DecodePass(user.Password) != model.Password) // Use decoded password for comparison
      {
        ModelState.AddModelError(string.Empty, "Invalid username or password.");
        return View(model);
      }

      // Set up authentication
      var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
    };

      var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

      await HttpContext.SignInAsync(
          CookieAuthenticationDefaults.AuthenticationScheme,
          new ClaimsPrincipal(claimsIdentity),
          new AuthenticationProperties
          {
            IsPersistent = true // Optional: Remember login
          });

      return RedirectToAction("Index", "Dashboards"); // Redirect to a secure page after successful login
    }

    // Logout
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
      // Sign out the user
      await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

      // Redirect the user to the login page
      return RedirectToAction("LoginBasic");
    }

    // Example method to create a user (with encrypted password)
    public void CreateUser(string username, string password)
    {
      var encodedPassword = PasswordHelper.EncodePass(password);

      var newUser = new SmsUser
      {
        UserName = username,
        Password = encodedPassword,  // Store the encoded password
        IsActive = true
        // Other user properties
      };

      context.SmsUsers.Add(newUser);
      context.SaveChanges();
    }
  }
}
