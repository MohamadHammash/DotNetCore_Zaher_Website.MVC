using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Zaher.Core.Entities;
using Zaher.Ui.Areas.Identity.Pages.Account;

namespace Zaher.Ui.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResendEmailConfirmationModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ResendEmailConfirmationModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "الرجاء تعبئة الحقل")]
            [EmailAddress(ErrorMessage = "هذا البريد الالكتروني غير صالح")]
            [Display(Name = "البريد الالكتروني")]
            public string Email { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "تم ارسال بربد التأكيد الرجاء التحقق من بريدك الالكتروني");
                return Page();
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(Input.Email, "يرجى تأكيد بريدك الالكتروني في موقع الشيخ زاهر الشوا",
                        $"الرجاء تأكيد بريدك الالكتروني عبر الضغط  <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>هنا</a>.");

            ModelState.AddModelError(string.Empty, "تم ارسال بربد التأكيد الرجاء التحقق من بريدك الالكتروني");
            return Page();
        }
    }
}
