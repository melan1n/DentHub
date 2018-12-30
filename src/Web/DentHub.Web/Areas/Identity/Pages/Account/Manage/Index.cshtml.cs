using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DentHub.Data;
using DentHub.Data.Common;
using DentHub.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DentHub.Web.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<DentHubUser> _userManager;
		private readonly IRepository<DentHubUser> _userRepository;
        private readonly SignInManager<DentHubUser> _signInManager;
        private readonly IEmailSender _emailSender;
		private readonly IRepository<Specialty> _specialtyRepository;
		private readonly IRepository<Clinic> _clinicRepository;
		private readonly CloudinaryService _cloudinaryService;

		public IndexModel(
            UserManager<DentHubUser> userManager,
			IRepository<DentHubUser> userRepository,
            SignInManager<DentHubUser> signInManager,
            IEmailSender emailSender,
			IRepository<Specialty> specialtyRepository,
			IRepository<Clinic> clinicRepository,
			CloudinaryService cloudinaryService)
        {
            _userManager = userManager;
			_userRepository = userRepository;
            _signInManager = signInManager;
            _emailSender = emailSender;
			_specialtyRepository = specialtyRepository;
			_clinicRepository = clinicRepository;
			_cloudinaryService = cloudinaryService;
		}

        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
			
			[Required]
			[Display(Name = "Clinic")]
			public int Clinic { get; set; }

			[Required]
			[Display(Name = "Specialty")]
			public int Specialty { get; set; }

			[Display(Name = "Image")]
			public string ImageUrl { get; set; }

			[Display(Name = "File")]
			public string File { get; set; }
		}

        public async Task<IActionResult> OnGetAsync()
        {
			this.ViewData["ClinicList"] = _clinicRepository.All().ToList();
			this.ViewData["SpecialtyList"] = _specialtyRepository.All().ToList();

			var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

			Username = userName;

            Input = new InputModel
            {
                Email = email,
                PhoneNumber = phoneNumber,
			};

			if (this.User.IsInRole("Dentist"))
			{
				var specialtyId = _specialtyRepository.All().FirstOrDefault(s => s.Id == user.SpecialtyId).Id;
				var clinicId = user.Clinic.Id;
				var imageUrl = user.ImageUrl;

				Input.Specialty = specialtyId;
				Input.Clinic = clinicId;
				Input.ImageUrl = imageUrl;
			}

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
			var user = await _userManager.GetUserAsync(User);

			if (!ModelState.IsValid)
            {
				if (this.User.IsInRole("Dentist"))
				{
					

					Input.Clinic = (int)user.ClinicId;
					Input.Specialty = (int)user.SpecialtyId;
					Input.ImageUrl = user.ImageUrl;

					this.ViewData["ClinicList"] = _clinicRepository.All().ToList();
					this.ViewData["SpecialtyList"] = _specialtyRepository.All().ToList();
				}
								 
				return Page();
            }

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
                }
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

			var userFromRepository = _userRepository
				.All()
				.FirstOrDefault(u => u.Id == user.Id);

			var specialtyId = user.SpecialtyId;
			if (Input.Specialty != specialtyId)
			{
				userFromRepository.Specialty = _specialtyRepository
					.All()
					.FirstOrDefault(s => s.Id == Input.Specialty);
			}

			var clinicId = user.ClinicId;
			if (Input.Clinic != clinicId)
			{
				userFromRepository.Clinic = _clinicRepository
					.All()
					.FirstOrDefault(c => c.Id == Input.Clinic);
			}

			var files = HttpContext.Request.Form.Files.ToList();
			var cloudinaryUris = this._cloudinaryService.UploadFiles(files);

			Input.ImageUrl = cloudinaryUris[0];

			var imageUrl = user.ImageUrl;
			if (Input.ImageUrl != imageUrl)
			{
				userFromRepository.ImageUrl = Input.ImageUrl;
			}

			_userRepository.Update(userFromRepository);
			await _userRepository.SaveChangesAsync();

			await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToPage();
        }
    }
}
