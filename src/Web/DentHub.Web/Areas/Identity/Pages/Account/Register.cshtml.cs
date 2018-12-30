using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DentHub.Data;
using DentHub.Data.Common;
using DentHub.Data.Models;
using DentHub.Web.Models;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.IO;

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
		private readonly CloudinaryService _cloudinaryService;

		public RegisterModel(
			UserManager<DentHubUser> userManager,
			SignInManager<DentHubUser> signInManager,
			ILogger<RegisterModel> logger,
			IEmailSender emailSender,
			IRepository<Clinic> clinicRepository,
			IRepository<Specialty> specialtyRepository,
			CloudinaryService cloudinaryService)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_logger = logger;
			_emailSender = emailSender;
			_clinicRepository = clinicRepository;
			_specialtyRepository = specialtyRepository;
			_cloudinaryService = cloudinaryService;
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

			[Required]
			[Display(Name = "Clinic")]
			public string Clinic { get; set; }

			[Required]
			[Display(Name = "Specialty")]
			public string Specialty { get; set; }

			[Display(Name = "Image")]
			public string ImageUrl { get; set; }

			[Display(Name = "File")]
			public string File { get; set; }
		}

		public void OnGet(string returnUrl = null)
		{
			ReturnUrl = returnUrl;
			this.ViewData["ClinicList"] = _clinicRepository.All().ToList();
			this.ViewData["SpecialtyList"] = _specialtyRepository.All().ToList();
		}

		public async Task<IActionResult> OnPostAsync(string returnUrl = null)
		{
			returnUrl = returnUrl ?? Url.Content("~/");
			if (ModelState.IsValid)
			{
                var clinic = this._clinicRepository
                                .All()
                                .FirstOrDefault(c => c.Id == int.Parse(Input.Clinic));

                var specialty = this._specialtyRepository
                                .All()
                                .FirstOrDefault(s => s.Id == int.Parse(Input.Specialty));

				var files = HttpContext.Request.Form.Files.ToList();
				var cloudinaryUris = this._cloudinaryService.UploadFiles(files);
			
				var user = new DentHubUser
				{
					UserName = Input.Email,
					Email = Input.Email,
					FirstName = Input.FirstName,
					LastName = Input.LastName,
					ClinicId = clinic.Id,
					SpecialtyId = specialty.Id,
					ImageUrl = cloudinaryUris[0]
                };

				var result = await _userManager.CreateAsync(user, Input.Password);
				if (result.Succeeded)
				{
                    var resultRole = await _userManager.AddToRoleAsync(user, "Dentist");
                    var resultClaimRole = await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimTypes.Role, "Dentist"));

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
