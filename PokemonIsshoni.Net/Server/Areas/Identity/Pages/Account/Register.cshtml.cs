// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using PokemonIsshoni.Net.Server.Areas.Identity.Data;

namespace PokemonIsshoni.Net.Server.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<PokemonIsshoniNetServerUser> _signInManager;
        private readonly UserManager<PokemonIsshoniNetServerUser> _userManager;
        private readonly IUserStore<PokemonIsshoniNetServerUser> _userStore;
        private readonly IUserEmailStore<PokemonIsshoniNetServerUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<PokemonIsshoniNetServerUser> userManager,
            IUserStore<PokemonIsshoniNetServerUser> userStore,
            SignInManager<PokemonIsshoniNetServerUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "{0}字段为必填项")]
            [EmailAddress]
            //[RegularExpression(@".+?@linxrobot\.com", ErrorMessage = "请用指定企业邮箱注册")]
            [Display(Name = "邮箱(尽可能不要使用gmail)")]
            public string Email { get; set; }
            [Required(ErrorMessage = "{0}字段为必填项")]
            [StringLength(100, ErrorMessage = "长度至少为2", MinimumLength = 2)]
            [Display(Name = "昵称")]
            public string NickName { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "{0}字段为必填项")]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "密码")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "确认密码")]
            [Compare("Password", ErrorMessage = "输入的密码不匹配")]
            public string ConfirmPassword { get; set; }

            [Required]
            [DataType(DataType.Text) ]
            [Display(Name = "QQ号")]
            public string QQ { get; set; }

            [Required]
            [Display(Name = "生日")]
            [DataType(DataType.Date)]
            public DateTime DOB { get; set; }

            //[Required]
            [DataType(DataType.Text)]
            [Display(Name = "所在城市")]
            public string City { get; set; } // 城市
            //[Required]
            [DataType(DataType.Text)]
            [Display(Name = "游戏内昵称")]
            public string HomeName { get; set; } // 剑盾游戏名字
            //[DataType(DataType.b)]
            [Display(Name = "再问一次，你喜欢宝可梦吗？")]
            //[DataType(DataType.Currency)]

            public bool Ok { get; set; } // 剑盾游戏名字
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
            if (ModelState.IsValid)
            {

                Random rnd = new Random();
                int b = rnd.Next(99999999);
                while (_userManager.Users.Any(s => s.TrainerIdInt == b))
                {
                    b = rnd.Next(99999999);
                }

                var user = CreateUser();
                user.QQ = Input.QQ;
                user.HomeName = Input.HomeName;
                user.NickName = Input.NickName;
                user.City = Input.City;
                user.DOB = Input.DOB;
                user.TrainerIdInt = b;
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private PokemonIsshoniNetServerUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<PokemonIsshoniNetServerUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(PokemonIsshoniNetServerUser)}'. " +
                    $"Ensure that '{nameof(PokemonIsshoniNetServerUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<PokemonIsshoniNetServerUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<PokemonIsshoniNetServerUser>)_userStore;
        }
    }
}
