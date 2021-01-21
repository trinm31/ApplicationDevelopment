using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using App_Dev.DataAccess.Repository.IRepository;
using App_Dev.Utility;
using App_Dev.Utility.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App_Dev.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            [Required]
            public string Name { get; set; }
            [Required] 
            public string Education { get; set; }
            [Required]
            public DateTime DateOfBirth { get; set; }
            
            
            [Required] 
            public string MainProgrammingLanguage { get; set; }
            [Required] 
            public int ToeicScore { get; set; }
            [Required]
            public string  ExperimentDetail { get; set; }
            [Required]
            public Department department { get; set; }
            [Required]
            public string Location { get; set; }
            
            [Required]
            public TypeOfTrainer typeOfTrainer { get; set; }
            [Required]
            public string  WorkingPlace { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var claimsIdentity = (ClaimsIdentity) User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var usertemp = await _unitOfWork.ApplicationUser.GetFirstOrDefaultAsync(u => u.Id == claims.Value);
            var role = await _userManager.GetRolesAsync(usertemp);

            Username = userName;
            if (role.FirstOrDefault() == SD.Role_Trainee)
            {
                var userFromDb = await _unitOfWork.TraineeProfile.GetAsync(user.Id);
                Input = new InputModel
                {
                    PhoneNumber = phoneNumber,
                    Name = userFromDb.Name,
                    Education = userFromDb.Education,
                    DateOfBirth = userFromDb.DateOfBirth,
                    MainProgrammingLanguage = userFromDb.MainProgrammingLanguage,
                    ToeicScore = userFromDb.ToeicScore,
                    ExperimentDetail= userFromDb.ExperimentDetail,
                    department = userFromDb.Department,
                    Location = userFromDb.Location
                };
            }else if (role.FirstOrDefault() == SD.Role_Trainer)
            {
                var userFromDb = await _unitOfWork.TrainerProfile.GetAsync(user.Id);
                Input = new InputModel
                {
                    PhoneNumber = phoneNumber,
                    Name = userFromDb.Name,
                    Education = userFromDb.Education,
                    DateOfBirth = userFromDb.DateOfBirth,
                    typeOfTrainer = userFromDb.TypeOfTrainer,
                    WorkingPlace = userFromDb.WorkingPlace
                };
            }
            else
            {
                var userFromDb = await _unitOfWork.ApplicationUser.GetAsync(user.Id);
                Input = new InputModel
                {
                    PhoneNumber = phoneNumber,
                    Name = userFromDb.Name,
                    Education = userFromDb.Education,
                    DateOfBirth = userFromDb.DateOfBirth,
                };
            }

        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            

            if (User.IsInRole(SD.Role_Trainee))
            {
                var profile = await _unitOfWork.TraineeProfile.GetAsync(user.Id);
                if (Input.PhoneNumber != profile.PhoneNumber)
                {
                    profile.PhoneNumber = Input.PhoneNumber;
                }
                if (Input.Name != profile.Name)
                {
                    profile.Name = Input.Name;
                }
                if (Input.Education != profile.Education)
                {
                    profile.Education = Input.Education;
                }
                if (Input.DateOfBirth != profile.DateOfBirth)
                {
                    profile.DateOfBirth = Input.DateOfBirth;
                }
                if (Input.MainProgrammingLanguage != profile.MainProgrammingLanguage)
                {
                    profile.MainProgrammingLanguage = Input.MainProgrammingLanguage;
                }
                if (Input.ToeicScore != profile.ToeicScore)
                {
                    profile.ToeicScore = Input.ToeicScore;
                }
                if (Input.ExperimentDetail != profile.ExperimentDetail)
                {
                    profile.ExperimentDetail = Input.ExperimentDetail;
                }
                if (Input.department != profile.Department)
                {
                    profile.Department = Input.department;
                }
                if (Input.Location != profile.Location)
                {
                    profile.Location = Input.Location;
                }
                
                await _unitOfWork.TraineeProfile.Update(profile);
                _unitOfWork.Save();
            }else if (User.IsInRole(SD.Role_Trainer))
            {
                var profile = await _unitOfWork.TrainerProfile.GetAsync(user.Id);
                if (Input.PhoneNumber != profile.PhoneNumber)
                {
                    profile.PhoneNumber = Input.PhoneNumber;
                }
                if (Input.Name != profile.Name)
                {
                    profile.Name = Input.Name;
                }
                if (Input.Education != profile.Education)
                {
                    profile.Education = Input.Education;
                }
                if (Input.DateOfBirth != profile.DateOfBirth)
                {
                    profile.DateOfBirth = Input.DateOfBirth;
                }
                if (Input.typeOfTrainer != profile.TypeOfTrainer)
                {
                    profile.TypeOfTrainer = Input.typeOfTrainer;
                }
                if (Input.WorkingPlace != profile.WorkingPlace)
                {
                    profile.WorkingPlace = Input.WorkingPlace;
                }
                await _unitOfWork.TrainerProfile.Update(profile);
                _unitOfWork.Save();
            }
            else
            {
                var profile = await _unitOfWork.ApplicationUser.GetAsync(user.Id);
                if (Input.PhoneNumber != profile.PhoneNumber)
                {
                    profile.PhoneNumber = Input.PhoneNumber;
                }
                if (Input.Name != profile.Name)
                {
                    profile.Name = Input.Name;
                }
                if (Input.Education != profile.Education)
                {
                    profile.Education = Input.Education;
                }
                if (Input.DateOfBirth != profile.DateOfBirth)
                {
                    profile.DateOfBirth = Input.DateOfBirth;
                }
                await _unitOfWork.ApplicationUser.Update(profile);
                _unitOfWork.Save();
            }
            
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
