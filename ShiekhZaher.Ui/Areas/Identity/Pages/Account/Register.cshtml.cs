using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Zaher.Core.Entities;
using Zaher.Core.Repositories;
using Zaher.Ui.Areas.Identity.Pages.Account;
using Zaher.Ui.Helpers;

namespace Zaher.Ui.Areas.Identity.Pages.Account
{
    [Authorize(Roles ="SuperAdmin")]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IUoW uoW;

        public RegisterModel(
            IUoW uoW,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            this.uoW = uoW;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "الرجاء تعبئة الحقل")]
            [EmailAddress(ErrorMessage = "هذا البريد الالكتروني غير صالح")]
            [Display(Name = "البريد الالكتروني")]
            public string Email { get; set; }

            // [Required]
            [StringLength(100, ErrorMessage = "كلمة المرور يجب أن تتكون من 6 خانات على الأقل", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "كلمة المرور")]
            public string Password { get; set; }

            [Required(ErrorMessage = "الرجاء تعبئة الحقل")]
            [Display(Name = "الاسم")]
            public string FirstName { get; set; }

            [Display(Name = "الكنية")]
            [Required(ErrorMessage = "الرجاء تعبئة الحقل")]
            public string LastName { get; set; }

            [Display(Name = "الصلاحية")]
            [Required(ErrorMessage = "الرجاء اختيار الصلاحية")]
            public string Role { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            Input.FirstName = Input.FirstName.Replace(" ", "-");
            Input.LastName = Input.LastName.Replace(" ", "-");

            var password = Randomizer.GetCryptedRandomString(7, false);
            Input.Password = password;
            Input.ConfirmPassword = password;

            if (ModelState.IsValid)
            {
                var user = CreateUserWithRole(Input.Role);

                //var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    if (!String.IsNullOrWhiteSpace(Input.Role))
                    {
                    var role = await _userManager.AddToRoleAsync(user, Input.Role);
                    }

                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);
                    
                    var passwordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    passwordToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(passwordToken));
                    var resetPasswordCallbackUrl = Url.Page(
                        "/Account/ResetPassword",
                        pageHandler: null,
                        values: new { area = "Identity",code = passwordToken },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(
                        Input.Email,
                        $"تم تعيينك ك {GetRoleInArabic(Input.Role)} في موقع الشيخ زاهر الشوا",
                       $"<p> كلمة المرور الخاصة بك هي </p><p>\"{Input.Password}\"</p>\n لتغيير كلمة المرور الرجاء الضغط <a href ='{HtmlEncoder.Default.Encode(resetPasswordCallbackUrl)}'>هنا </a>");

                    await _emailSender.SendEmailAsync(Input.Email, "يرجى تأكيد بريدك الالكتروني في موقع الشيخ زاهر الشوا",
                        $"الرجاء تأكيد بريدك الالكتروني عبر الضغط  <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>هنا</a>.");

                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        } 

        private ApplicationUser CreateUserWithRole(string role)
        {
            switch (role)
            {
                case "Admin":
                    break;
                case "Editor":
                    break;
                case "Moderator":
                    break;
                default:
                    role = "Moderator";
                    break;
            }


            var appUser = new ApplicationUser
            {
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                UserName = Input.Email,
                Email = Input.Email,
                Role = role
            };

            return appUser;
        }

        private string GetRoleInArabic(string role)
        {
            string roleInArabic;

            switch (role.ToUpper())
            {
                case "ADMIN":
                    roleInArabic = RolesInArabic.Admin;
                    break;

                case "EDITOR":
                    roleInArabic = RolesInArabic.Editor;
                    break;

                case "MODERATOR":
                    roleInArabic = RolesInArabic.Moderator;
                    break;

                default:
                    roleInArabic = " ";
                    break;
            }
            return roleInArabic;
        }
    }
}
