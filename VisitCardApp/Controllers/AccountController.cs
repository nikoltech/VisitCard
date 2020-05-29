﻿namespace VisitCardApp.Controllers
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using VisitCardApp.BusinessLogic.Communications;
    using VisitCardApp.DataAccess.Entities;
    using VisitCardApp.DataAccess.Repositories;
    using VisitCardApp.DataAccess.Services.User;
    using VisitCardApp.Models;
    using VisitCardApp.Models.Account;

    public class AccountController : Controller
    {
        private readonly IRepository repo;
        private readonly UserManager<AppUser> UserManager;
        private readonly SignInManager<AppUser> SignInManager;
        private readonly IEmailService EmailService;
        private readonly UserService UserService;

        public AccountController(
            IRepository repository,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IOptions<EmailConfig> emailConfig,
            UserService userService)
        {
            this.repo = repository;
            this.UserManager = userManager;
            this.SignInManager = signInManager;
            this.EmailService = new EmailService(emailConfig.Value);
            this.UserService = userService;
        }

        #region Old Authorization
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            //return Redirect("~/api/Values/Index");
            return View(new LoginModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    //User signedUser = await this.UserManager.FindByEmailAsync(model.Email);
                    AppUser signedUser = await this.UserService.GetUserByEmailAsync(model.Email);
                    if (signedUser != null)
                    {
                        // проверяем, подтвержден ли email
                        if (!await this.UserManager.IsEmailConfirmedAsync(signedUser))
                        {
                            ModelState.AddModelError(string.Empty, "Вы не подтвердили свой email.\nПодтвердите или пройдите регистрацию снова.\nПисьмо повторно отправлено на указанную почту.");
                            await this.SendConfirmationEmailAsync(signedUser).ConfigureAwait(false);
                            return View(model);
                        }

                        var result = await this.SignInManager.PasswordSignInAsync(signedUser.UserName, model.Password, model.RememberMe, false);
                        if (result.Succeeded)
                        {
                            if (string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                            {
                                return Redirect(model.ReturnUrl);
                            }

                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Пользователь не найден. Некорректные логин и(или) пароль");
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Something get wrong!");
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
        public async Task<IActionResult> Register(RegisterModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    AppUser user = await this.UserService.GetUserByEmailAsync(model.Email);

                    if (user != null && !user.EmailConfirmed)
                    {
                        IList<string> roles = await this.UserManager.GetRolesAsync(user);
                        if (!roles.Any(r => r.Equals("admin")))
                        {
                            await this.UserManager.DeleteAsync(user);
                            user = null;
                        }
                    }

                    if (user == null)
                    {
                        user = new AppUser { /*EmailConfirmed = true/*for dev*/ Email = model.Email, UserName = this.GetUsernameFromEmail(model.Email) };
                        var result = await this.UserManager.CreateAsync(user, model.Password);

                        if (result.Succeeded)
                        {
                            await this.UserManager.AddToRoleAsync(user, "user");

                            // for dev
                            //await this.SignInManager.SignInAsync(user, false);
                            // for production 
                            await this.SendConfirmationEmailAsync(user).ConfigureAwait(false);

                            return View("RegisterInfo");
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Пользователь с таким Email уже существует!");
                    }
                }
            }
            catch
            {
                //User user = await this.UserManager.FindByEmailAsync(model.Email);
                AppUser user = await this.UserService.GetUserByEmailAsync(model.Email);
                if (user != null)
                {
                    await this.UserManager.DeleteAsync(user);
                }
                ModelState.AddModelError("", "Something got wrong!");
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string email, string code)
        {
            try
            {
                if (email == null || code == null)
                {
                    return View("Error", new ErrorViewModel { Message = "Некорректные данные." });
                }

                //var user = await this.UserManager.FindByIdAsync(userId);
                var user = await this.UserManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return View("Error", new ErrorViewModel { Message = "Пользователь не найден." });
                }

                var result = await this.UserManager.ConfirmEmailAsync(user, code);
                if (result.Succeeded)
                {
                    await this.SignInManager.SignInAsync(user, false);
                    await this.UserService.UpdateUserCacheByEmailAsync(email).ConfigureAwait(false);

                    return View(user);
                }
                else
                {
                    return View("Error", new ErrorViewModel { Message = "Ошибка подтверждения." });
                }
            }
            catch
            {
                return View("Error", new ErrorViewModel { Message = "Неизвестная ошибка. Обратитесь к администратору." });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await this.UserManager.FindByEmailAsync(model.Email);
                    if (user == null || !(await this.UserManager.IsEmailConfirmedAsync(user)))
                    {
                        // пользователь с данным email может отсутствовать в бд
                        // тем не менее мы выводим стандартное сообщение, чтобы скрыть 
                        // наличие или отсутствие пользователя в бд
                        return View("ForgotPasswordConfirmation");
                    }

                    var code = await this.UserManager.GeneratePasswordResetTokenAsync(user);
                    var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code, somes = "YAAA emaaill", kakaha = "BUKAAAha" }, protocol: HttpContext.Request.Scheme);

                    Message message = new Message(model.Email, "WebAppSome: Reset Password",
                                    $"Для сброса пароля пройдите по ссылке: <a href='{callbackUrl}'>сбросить пароль</a>!");
                    await this.EmailService.SendEmailAsync(message);

                    return View("ForgotPasswordConfirmation");
                }
                return View(model);
            }
            catch
            {
                return View("Error", new ErrorViewModel
                {
                    Message = "ForgotPassword"
                });
            }
        }

        /// <summary>
        /// Redirected From Email for input data for reseting password
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error", new ErrorViewModel { Message = "Некоректные данные." }) : View();
        }

        /// <summary>
        /// Reseting password
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                //var user = await this.UserManager.FindByEmailAsync(model.Email);
                var user = await this.UserService.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    return View("ResetPasswordConfirmation");
                }
                var result = await this.UserManager.ResetPasswordAsync(user, model.Code, model.Password);
                if (result.Succeeded)
                {
                    await this.UserService.UpdateUserCacheByIdAsync(user.Id).ConfigureAwait(false);

                    return View("ResetPasswordConfirmation");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
            catch
            {
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await this.SignInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        #endregion

        #region private methods
        private string GetUsernameFromEmail(string email)
        {
            return email?.Split('@')[0];
        }

        /// <summary>
        /// Create ClaimsIdentity from User entity
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        //private async Task<ClaimsIdentity> GetIdentity(string username, string password)
        //{
        //    //Person person = people.FirstOrDefault(x => x.Login == username && x.Password == password);
        //    User user = await this.repo.GetUserAsync(username, password);

        //    if (user != null)
        //    {
        //        var claims = new List<Claim>
        //        {
        //            new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
        //            //new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role)
        //        };
        //        ClaimsIdentity claimsIdentity =
        //        new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
        //            ClaimsIdentity.DefaultRoleClaimType);
        //        return claimsIdentity;
        //    }

        //    // если пользователя не найдено
        //    return null;
        //}

        private async Task Authenticate(string userName)
        {
            // create 1 claim
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            // install auth cookies
            await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        }

        private async Task SendConfirmationEmailAsync(AppUser user)
        {
            var code = await this.UserManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Action(
                "ConfirmEmail",
                "Account",
                new { email = user.Email, code = code },
                protocol: HttpContext.Request.Scheme);

            Message message = new Message(user.Email, "VisitCard: Email confirmation",
                $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>подтвердить</a>!");

            await this.EmailService.SendEmailAsync(message);
        }
        #endregion
    }
}
