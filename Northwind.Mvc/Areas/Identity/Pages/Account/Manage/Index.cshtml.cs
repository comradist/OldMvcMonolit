// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Northwind.Mvc.Models;

namespace Northwind.Mvc.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        //! Додавання UserStore викликає рантайм ексепшен май на увазі
        public readonly UserManager<UserExtendedForIdentity> _userManager;
        private readonly SignInManager<UserExtendedForIdentity> _signInManager;

        public IndexModel(
            UserManager<UserExtendedForIdentity> userManager,
            SignInManager<UserExtendedForIdentity> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public List<string> StatusMessage { get; set; } = new();

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
        public class InputModel
        {
            [Required]
            [Display(Name = "Last Name")]
            [StringLength(25, ErrorMessage = "Max 25 charecres long")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            [StringLength(25, ErrorMessage = "Max 25 charecres long")]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "User Name")]
            public string Username { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Profile Picture")]
            public byte[] ProfilePicture { get; set; }
        }

        private void Load(UserExtendedForIdentity user)
        {
            Input = new InputModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture,
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Load(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                Load(user);
                return Page();
            }
            
            var firstName = user.FirstName;
            if (Input.FirstName != firstName)
            {
                user.FirstName = Input.FirstName;
                await _userManager.UpdateAsync(user);
            }

            var lastName = user.LastName;
            if (Input.LastName != lastName)
            {
                user.LastName = Input.LastName;
                await _userManager.UpdateAsync(user);
            }

            var userName = await _userManager.GetUserNameAsync(user);
            if(Input.Username != userName)
            {
                if (user.UsernameChangeLimit == 0)
                {
                    StatusMessage.Add($"Error: You can't anymore change your user name.");
                    return Page();
                }
                var setUserNameResult = await _userManager.SetUserNameAsync(user, Input.Username);
                user.UsernameChangeLimit--;
                StatusMessage.Add($"You have left number for attempts: {user.UsernameChangeLimit} to change user name.");
                if (!setUserNameResult.Succeeded)
                {
                    StatusMessage.Add("Unexpected error when trying to set phone number.");
                    return Page();
                }
                await _userManager.UpdateAsync(user);
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage.Add("Unexpected error when trying to set phone number.");
                    return Page();
                }
            }

            if(Request.Form.Files.Count > 0)
            {
                IFormFile file = HttpContext.Request.Form.Files.FirstOrDefault();
                if(file == null && file.Length <= 0)
                {
                    StatusMessage.Add("Error when try handle user input file.");
                    return Page();
                }
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    user.ProfilePicture = stream.ToArray();
                }
                await _userManager.UpdateAsync(user);

            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage.Add("Your profile has been updated");
            return Page();
        }
    }
}
