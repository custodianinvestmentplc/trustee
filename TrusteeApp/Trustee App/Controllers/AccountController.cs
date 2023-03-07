using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Identity.Client;
using TrusteeApp.Domain.Dtos;
using TrusteeApp.Email;
using TrusteeApp.Errors;
using TrusteeApp.Filters;
using TrusteeApp.Models;
using TrusteeApp.Repo;
using TrusteeApp.Services;
using TrusteeApp.ViewModels;
using System.Threading.Tasks;

namespace TrusteeApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;

        public AccountController(IConfiguration configuration, IHttpClientFactory httpClientFactory, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IWebHostEnvironment webHostEnvironment, IEmailSender emailSender)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        private async Task<UserModel> LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            return new UserModel
            {
                UserName = userName,
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null) throw new Exception($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

                var userInput = await LoadAsync(user);

                return View(userInput);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex); LogWriterController.Write(ex.Message);

                var urlString = $"/Home/Error?errorcode={(int)HttpStatusCode.InternalServerError}&errortype={"userProfile"}&message={ex.Message}&detail={"Sorry, We are unable to load the requested user profile."}";

                var url = new Uri(urlString).ToString();

                return Redirect(url);
            }
        }

        [HttpGet]
        [ViewLayout("_LoginLayout")]
        [AllowAnonymous]
        public IActionResult Register()
        {
            try
            {
                var Input = HttpContext.Session.Get<UserModel>("Input");

                if (Input?.FirstName != null) return View(Input);

                //var userModel = new UserModel();

                return View();
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex); LogWriterController.Write(ex.Message);

                return RedirectToAction("Register", "Account");
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterPost(UserModel Input)
        {
            try
            {
                if (!Input.TermsAgreed) throw new Exception($"Terms of Service was not agreed.");

                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    PhoneNumber = Input.PhoneNumber,
                };

                var result = await _userManager.CreateAsync(user, Input.Password!);

                if (!result.Succeeded)
                {
                    var err = result.Errors.ToList()[0];

                    throw new Exception(err.Description);
                }

                log.Info($"{DateTime.Now.ToString()} - Created the User {Input.Email}");

                TempData["Success"] = $"Your account creation was successful";
                TempData["regen"] = "true";
                return RedirectToAction("RegisterConfirmation", new { Input.Email });
            }
            catch (Exception ex)
            {
                HttpContext.Session.Set("Input", Input);

                log.Error(DateTime.Now.ToString(), ex); LogWriterController.Write(ex.Message);

                TempData["Error"] = $"User registration Error \r\n" + ex.Message;

                return RedirectToAction("Register");
            }
        }

        [HttpGet]
        [ViewLayout("_LoginLayout")]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            try
            {
                bool isLoggedIn = User.Identity!.IsAuthenticated;

                if (isLoggedIn) await _signInManager.SignOutAsync();
                //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

                return View();
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex); LogWriterController.Write(ex.Message);

                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> LoginPostRequest(UserModel Input)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(Input.Email!);

                if (user == null) throw new Exception("Invalid login attempt.");

                var result = await _signInManager.PasswordSignInAsync(Input.Email!, Input.Password!, Input.RememberMe, lockoutOnFailure: false);

                if (!result.Succeeded) throw new Exception("Invalid login attempt.");

                if (!user!.EmailConfirmed)
                {
                    TempData["regen"] = "true";
                    return RedirectToAction("RegisterConfirmation", new { Input.Email });
                }

                log.Info($"{DateTime.Now.ToString()} - Logged in the User {Input.Email}");

                return RedirectToAction("Index", "Home");

            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex); LogWriterController.Write(ex.Message);

                TempData["Error"] = ex.Message;

                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmEmail(UserModel Input)
        {
            try
            {
                var userId = TempData["Id"]?.ToString()!;
                Input.Email = TempData["Email"]?.ToString()!;

                if (Input.Email == null || userId == null || Input.Otp == null) throw new Exception($"Authentication faild");

                Input.Otp = HttpContext.Session.Get<string>(Input.Otp);

                if (string.IsNullOrWhiteSpace(Input.Otp))
                {
                    TempData["Error"] = "Error activating your account. Please confirm the OTP";
                    TempData["regen"] = "false";

                    string IsPasswordReset1 = TempData["IsPasswordReset"]?.ToString()!;

                    if (IsPasswordReset1 == "true") return RedirectToAction("RegisterConfirmation", new { Input.Email, IsPasswordReset = "true" });

                    return RedirectToAction("RegisterConfirmation", new { Input.Email });
                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null) throw new Exception($"Unable to load user with ID '{userId}'.");

                Input.Otp = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Input.Otp));

                var result = await _userManager.ConfirmEmailAsync(user, Input.Otp);

                if (result.Succeeded)
                {
                    string IsPasswordReset2 = TempData["IsPasswordReset"]?.ToString()!;

                    if (IsPasswordReset2 == "true") return RedirectToAction("ForgotPassword");

                    if (!User.Identity!.IsAuthenticated)
                    {
                        await _signInManager.SignInAsync(user, false);
                        log.Info($"{DateTime.Now.ToString()} - Logged in the User {Input.Email}");
                    }

                    return RedirectToAction("SuccessConfirmation");
                }

                TempData["Error"] = "Error activating your account. Please confirm the OTP";
                TempData["regen"] = "false";

                string IsPasswordReset = TempData["IsPasswordReset"]?.ToString()!;
                if (IsPasswordReset == "true") return RedirectToAction("RegisterConfirmation", new { Input.Email, IsPasswordReset = "true" });

                return RedirectToAction("RegisterConfirmation", new { Input.Email });
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex); LogWriterController.Write(ex.Message);

                TempData["Error"] = $"User registration Error \r\n " + ex.Message;

                return RedirectToAction("Register");
            }
        }

        [ViewLayout("_LoginLayout")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterConfirmation(string Email, string? IsPasswordReset = null)
        {
            try
            {
                if (Email == null) throw new Exception($"No was Email found, for confirmation.");

                var user = await _userManager.FindByEmailAsync(Email);

                if (user == null) throw new Exception($"Unable to load user with email '{Email}'.");

                var userId = await _userManager.GetUserIdAsync(user);

                string regen = TempData["regen"]?.ToString()!;

                if (regen == "true")
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var otp = new Random().Next(1000, 9999).ToString();

                    HttpContext.Session.Clear();
                    HttpContext.Session.Set(otp, code);

                    var PathToTemplate = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString() + "templates" + Path.DirectorySeparatorChar.ToString() + "Email.html";

                    var subject = "Account Activation";
                    string HtmlBody = "";

                    using (StreamReader sr = System.IO.File.OpenText(PathToTemplate))
                    {
                        HtmlBody = sr.ReadToEnd();
                    }

                    string messageBody = string.Format(HtmlBody, otp);

                    await _emailSender.SendEmailAsync(Email, subject, messageBody);

                    //await Task.Run(() =>
                    //{
                    //    var timer = new System.Timers.Timer(TimeSpan.FromMinutes(60).Milliseconds);

                    //    timer.Elapsed += (s, e) =>
                    //    {
                    //        ((System.Timers.Timer)s!).Stop();

                    //        try { HttpContext.Session.Clear();}
                    //        catch { }
                    //    };

                    //    timer.Start();
                    //});
                }

                TempData["Email"] = user.Email;
                TempData["Id"] = user.Id;
                TempData["IsPasswordReset"] = IsPasswordReset;

                return View();
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex); LogWriterController.Write(ex.Message);

                TempData["Error"] = $"User registration Error \r\n " + ex.Message;

                return RedirectToAction("Register", "Account");
            }
        }

        [ViewLayout("_LoginLayout")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult SuccessConfirmation() => View();

        [ViewLayout("_LoginLayout")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPasswordPost(UserModel Input)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(Input.Email!);

                if (user == null)
                {
                    TempData["Warning"] = "Password Reset\r\n No account was found with provided email";

                    return RedirectToAction("ForgotPassword");
                }

                TempData["regen"] = "true";

                if (!(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    TempData["Warning"] = "Password Reset\r\n Account with provided email has not been activated.";
                    return RedirectToAction("RegisterConfirmation", new { Input.Email, IsPasswordReset = "true" });
                }

                return RedirectToAction("PasswordConfirmation", Input);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex); LogWriterController.Write(ex.Message);

                TempData["Error"] = $"ForgotPasswordConfirmation Error \r\n {ex.Message}";

                return RedirectToAction("Register");
            }
        }

        [ViewLayout("_LoginLayout")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> PasswordConfirmation(UserModel Input)
        {
            try
            {
                if (Input.Email == null) Input.Email = TempData["Email"]?.ToString();

                if (Input.Email == null) throw new Exception($"No was Email found, for confirmation.");

                var user = await _userManager.FindByEmailAsync(Input.Email);

                if (user == null) throw new Exception($"Unable to load user with email '{Input.Email}'.");

                string regen = TempData["regen"]?.ToString()!;

                if (regen == "true")
                {
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var otp = new Random().Next(1000, 9999).ToString();

                    HttpContext.Session.Clear();
                    HttpContext.Session.Set(otp, code);

                    var PathToTemplate = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString() + "templates" + Path.DirectorySeparatorChar.ToString() + "Password.html";

                    var subject = "Password Reset";
                    string HtmlBody = "";

                    using (StreamReader sr = System.IO.File.OpenText(PathToTemplate))
                    {
                        HtmlBody = sr.ReadToEnd();
                    }

                    string messageBody = string.Format(HtmlBody, otp);

                    await _emailSender.SendEmailAsync(Input.Email, subject, messageBody);

                    //timer.AutoReset = false;
                    //timer.Enabled = true;
                    //await Task.Run(() =>
                    //{
                    //    var timer = new System.Timers.Timer(TimeSpan.FromMinutes(60).Milliseconds);

                    //    timer.Elapsed += (s, e) =>
                    //    {
                    //        ((System.Timers.Timer)s!).Stop();

                    //        try { HttpContext.Session.Clear(); }
                    //        catch { }
                    //    };

                    //    timer.Start();
                    //});
                    //if (task.Wait(TimeSpan.FromMinutes(10))) return task.Result;
                }

                TempData["Email"] = Input.Email;

                return View(Input);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex); LogWriterController.Write(ex.Message);

                TempData["Error"] = $"ForgotPasswordConfirmation Error \r\n {ex.Message}";

                return RedirectToAction("Register");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult ConfirmPasswordEmail(UserModel Input)
        {
            try
            {
                if (Input.Otp == null) throw new Exception($"Authentication faild");

                Input.Otp = HttpContext.Session.Get<string>(Input.Otp);

                if (string.IsNullOrWhiteSpace(Input.Otp))
                {
                    TempData["Error"] = "Authentication Error. Please confirm the OTP";
                    TempData["regen"] = "false";
                    return RedirectToAction("PasswordConfirmation", Input);
                }

                HttpContext.Session.Set("Input", Input);

                return RedirectToAction("ResetPassword");
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex); LogWriterController.Write(ex.Message);

                TempData["Error"] = $"User registration Error \r\n {ex.Message}";

                return RedirectToAction("Register");
            }
        }

        [ViewLayout("_LoginLayout")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword() => View();

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordPostAsync(UserModel Input)
        {
            try
            {
                var cachedInput = HttpContext.Session.Get<UserModel>("Input");

                if (cachedInput == null) throw new Exception($"Authentication faild");

                if (cachedInput.Email == null) throw new Exception($"No was Email found, for confirmation.");

                var user = await _userManager.FindByEmailAsync(cachedInput.Email);

                if (user == null) throw new Exception($"Unable to load user with email '{cachedInput.Email}'.");

                //var result = (await _userManager.PasswordValidators[0].ValidateAsync(_userManager, user, Input.Password));

                //if (!result.Succeeded)
                //{
                //    var err = result.Errors.ToList()[0];

                //    TempData["Error"] = err.Description;
                //    return RedirectToAction("ResetPassword");
                //}

                cachedInput.Otp = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(cachedInput.Otp!));

                var result = await _userManager.ResetPasswordAsync(user, cachedInput.Otp!, Input.Password!);

                if (!result.Succeeded)
                {
                    var err = result.Errors.ToList()[0];

                    TempData["Error"] = err.Description;
                    return RedirectToAction("ResetPassword");
                }

                log.Info($"{DateTime.Now.ToString()} - Reset User {cachedInput.Email} password");

                return RedirectToAction("ResetPasswordConfirmation");
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex); LogWriterController.Write(ex.Message);

                TempData["Error"] = $"Password operation was unsuccessful \r\n {ex.Message}";

                return RedirectToAction("Register");
            }
        }

        [ViewLayout("_LoginLayout")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation() => View();

        [AllowAnonymous]
        public async Task<IActionResult> ReConfirmEmail(string code, string userId)
        {
            try
            {
                if (userId == null || code == null) throw new Exception($"No user Id was found.");

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null) throw new Exception($"Unable to load user with ID '{userId}'.");

                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

                var result = await _userManager.ConfirmEmailAsync(user, code);

                if (result.Succeeded)
                {
                    TempData["SuccessAlert"] = "Thank you for confirming your email.";
                    //Input.StatusMessage = "Thank you for confirming your email.";

                    return RedirectToAction("Index", "Home");
                }
                throw new Exception("Error confirming your email.");
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex); LogWriterController.Write(ex.Message);

                var urlString = $"/Home/Error?errorcode={(int)HttpStatusCode.InternalServerError}&errortype={"emailReconfirmation"}&message={ex.Message}&detail={"Sorry, We could not confirm your email."}";

                var url = new Uri(urlString).ToString();

                return Redirect(url);
            }
        }

        [ViewLayout("_LoginLayout")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                log.Info($"{DateTime.Now.ToString()} - User logged out {userEmail}");

                await _signInManager.SignOutAsync();

                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex); LogWriterController.Write(ex.Message);

                var urlString = $"/Home/Error?errorcode={(int)HttpStatusCode.InternalServerError}&errortype={"logout"}&message={ex.Message}&detail={"Sorry, loging you out was not successful."}";

                var url = new Uri(urlString).ToString();

                return Redirect(url);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null) throw new Exception($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

                var hasPassword = await _userManager.HasPasswordAsync(user);

                if (!hasPassword) return RedirectToAction("SetPassword", "Account");

                return View();
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex); LogWriterController.Write(ex.Message);

                var urlString = $"/Home/Error?errorcode={(int)HttpStatusCode.InternalServerError}&errortype={"password"}&message={ex.Message}&detail={"Password operation was unsuccessful."}";

                var url = new Uri(urlString).ToString();

                return Redirect(url);
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePasswordPost(UserModel Input)
        {
            try
            {
                if (!ModelState.IsValid) return View();

                var user = await _userManager.GetUserAsync(User);

                if (user == null) throw new Exception($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

                var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword!, Input.NewPassword!);

                if (!changePasswordResult.Succeeded)
                {
                    foreach (var error in changePasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }

                await _signInManager.RefreshSignInAsync(user);

                log.Info($"{DateTime.Now.ToString()} - User changed their password successfully.");

                TempData["SuccessAlert"] = "Your password has been changed.";
                Input.StatusMessage = "Your password has been changed.";

                return RedirectToAction("ChangePassword", "Account");
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex); LogWriterController.Write(ex.Message);

                var urlString = $"/Home/Error?errorcode={(int)HttpStatusCode.InternalServerError}&errortype={"password"}&message={ex.Message}&detail={"Password operation was unsuccessful."}";

                var url = new Uri(urlString).ToString();

                return Redirect(url);
            }
        }

        [HttpGet]
        public async Task<IActionResult> SetPassword()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null) throw new Exception($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

                var hasPassword = await _userManager.HasPasswordAsync(user);

                if (hasPassword) return RedirectToAction("ChangePassword", "Account");

                return View();
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex); LogWriterController.Write(ex.Message);

                var urlString = $"/Home/Error?errorcode={(int)HttpStatusCode.InternalServerError}&errortype={"password"}&message={ex.Message}&detail={"Password operation was unsuccessful."}";

                var url = new Uri(urlString).ToString();

                return Redirect(url);
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPasswordPost(UserModel Input)
        {
            try
            {
                if (!ModelState.IsValid) return View();

                var user = await _userManager.GetUserAsync(User);

                if (user == null) throw new Exception($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

                var addPasswordResult = await _userManager.AddPasswordAsync(user, Input.NewPassword!);

                if (!addPasswordResult.Succeeded)
                {
                    foreach (var error in addPasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }

                await _signInManager.RefreshSignInAsync(user);

                TempData["SuccessAlert"] = "Your password has been set.";
                Input.StatusMessage = "Your password has been set.";

                return RedirectToAction("Hpmepage", "Home");
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex); LogWriterController.Write(ex.Message);

                var urlString = $"/Home/Error?errorcode={(int)HttpStatusCode.InternalServerError}&errortype={"password"}&message={ex.Message}&detail={"Password operation was unsuccessful."}";

                var url = new Uri(urlString).ToString();

                return Redirect(url);
            }
        }
    }
}

