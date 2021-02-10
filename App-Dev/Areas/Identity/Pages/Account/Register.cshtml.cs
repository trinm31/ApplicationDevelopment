using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using App_Dev.DataAccess.Repository.IRepository;
using App_Dev.Models;
using App_Dev.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace App_Dev.Areas.Identity.Pages.Account
{
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Staff)]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork unitOfWork
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
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
            public string Name { get; set; }
            [Required]
            public string PhoneNumber { get; set; }
            [Required] 
            public string Education { get; set; }
            [Required]
            public DateTime DateOfBirth { get; set; }
            [Required]
            public string Role { get; set; }
            public IEnumerable<SelectListItem> RoleList { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            Input = new InputModel()
            {
                RoleList = _roleManager.Roles.Where(u=> u.Name != SD.Role_Trainee).Select(x=> x.Name).Select(i=> new SelectListItem
                {
                    Text = i,
                    Value = i
                })
            };
            if (User.IsInRole(SD.Role_Staff))
            {
                Input = new InputModel()
                {
                    RoleList = _roleManager.Roles.Where(u=> u.Name == SD.Role_Trainee).Select(x=> x.Name).Select(i=> new SelectListItem
                    {
                        Text = i,
                        Value = i
                    })
                };
            }
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new ApplicationUser();
                TraineeProfile traineeProfile = new TraineeProfile();
                TrainerProfile trainerProfile = new TrainerProfile();
                IdentityResult result = new IdentityResult();
                if (Input.Role == SD.Role_Trainee)
                {
                    traineeProfile = new TraineeProfile()
                    {
                        UserName = Input.Email, 
                        Email = Input.Email,
                        PhoneNumber = Input.PhoneNumber,
                        Education = Input.Education,
                        Role = Input.Role,
                        Name = Input.Name,
                        DateOfBirth = Input.DateOfBirth
                    };
                    result = await _userManager.CreateAsync(traineeProfile, Input.Password);
                    
                }else if (Input.Role == SD.Role_Trainer)
                {
                    trainerProfile = new TrainerProfile()
                    {
                        UserName = Input.Email, 
                        Email = Input.Email,
                        PhoneNumber = Input.PhoneNumber,
                        Education = Input.Education,
                        Role = Input.Role,
                        Name = Input.Name,
                        DateOfBirth = Input.DateOfBirth
                    };
                    result = await _userManager.CreateAsync(trainerProfile, Input.Password);
                }
                else
                {
                    applicationUser = new ApplicationUser()
                    {
                        UserName = Input.Email, 
                        Email = Input.Email,
                        PhoneNumber = Input.PhoneNumber,
                        Education = Input.Education,
                        Role = Input.Role,
                        Name = Input.Name,
                        DateOfBirth = Input.DateOfBirth
                    };
                    result = await _userManager.CreateAsync(applicationUser, Input.Password);
                }
                
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    if (Input.Role == SD.Role_Trainee)
                    {
                        await _userManager.AddToRoleAsync(traineeProfile, traineeProfile.Role);
                        
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(traineeProfile);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = traineeProfile.Id, code = code, returnUrl = returnUrl },
                            protocol: Request.Scheme);
                    
                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                    }else if (Input.Role == SD.Role_Trainer)
                    {
                        await _userManager.AddToRoleAsync(trainerProfile, trainerProfile.Role);
                        
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(trainerProfile);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = trainerProfile.Id, code = code, returnUrl = returnUrl },
                            protocol: Request.Scheme);
                    
                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(applicationUser, applicationUser.Role);
                        
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = applicationUser.Id, code = code, returnUrl = returnUrl },
                            protocol: Request.Scheme);
                    
                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                    }
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        //await _signInManager.SignInAsync(user, isPersistent: false);
                        //return LocalRedirect(returnUrl);
                        if (User.IsInRole(SD.Role_Staff))
                        {
                            return RedirectToAction("TraineeManager", "Users", new {Area = "Authenticated"});
                        }
                        return RedirectToAction("Index", "Users", new {Area = "Authenticated"});
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            Input = new InputModel()
            {
                RoleList = _roleManager.Roles.Where(u=> u.Name != SD.Role_Trainee).Select(x=> x.Name).Select(i=> new SelectListItem
                {
                    Text = i,
                    Value = i
                })
            };
            if (User.IsInRole(SD.Role_Staff))
            {
                Input = new InputModel()
                {
                    RoleList = _roleManager.Roles.Where(u=> u.Name == SD.Role_Trainee).Select(x=> x.Name).Select(i=> new SelectListItem
                    {
                        Text = i,
                        Value = i
                    })
                };
            }
            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
