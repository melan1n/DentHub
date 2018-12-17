using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DentHub.Data.Common;
using DentHub.Data.Models;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace DentHub.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<DentHubUser> _signInManager;
        private readonly UserManager<DentHubUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
		private readonly IRepository<Clinic> _clinicRepository;
		private readonly IRepository<Specialty> _specialtyRepository;

		public RegisterModel(
            UserManager<DentHubUser> userManager,
            SignInManager<DentHubUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
			IRepository<Clinic> clinicRepository,
			IRepository<Specialty> specialtyRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
			_clinicRepository = clinicRepository;		
			_specialtyRepository = specialtyRepository;
		}

		[BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
			[Required]
			[Display(Name = "First Name")]
			public string FirstName { get; set; }

			[Required]
			[Display(Name = "Last Name")]
			public string LastName { get; set; }

			[Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

			//[Required]
			[Display(Name = "Clinic")]
			public string Clinic { get; set; }

			//[Required]
			[Display(Name = "Specialty")]
			public string Specialty { get; set; }

		}

		public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
				//var clinic = this._clinicRepository
				//				.All()
				//				.First(c => c.Name == Input.Clinic);

				//var specialty = this._specialtyRepository
				//				.All()
				//				.First(s => s.Name == Input.Specialty);

				var user = new DentHubUser
				{
					UserName = Input.Email,
					Email = Input.Email,
					FirstName = Input.FirstName,
					LastName = Input.LastName,
					//ClinicId = clinic.Id,
					//SpecialtyId = specialty.Id,
				};
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
					var resultRole = await _userManager.AddToRoleAsync(user, "Dentist");

					_logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
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
    }
}
