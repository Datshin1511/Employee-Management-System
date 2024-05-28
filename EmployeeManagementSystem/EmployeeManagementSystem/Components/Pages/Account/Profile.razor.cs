using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace EmployeeManagementSystem.Components.Pages.Account
{
    public partial class Profile
    {
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        private UserProfileModel User = new UserProfileModel();

        private List<string> PhoneCodes = new List<string> { "+91", "+1", "+44" };
        private string? ButtonTitle = "Save";
        private string? editStatusMessage;

        private bool isDisabled { get; set; }
        private bool isEditMessageVisible { get; set; } = false;

        private Employee? LoggedUser { get; set; }
        private Credential? LoggedUserCredential { get; set; }

        private Employee? empProfile { get; set; }
        private Credential? empCredential { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Task.Delay(1000);

            isDisabled = true;

            var authenticationState = await authenticationStateTask;

            var LoggedEmployeeID = authenticationState.User.FindFirst(c => c.Type == "LoggedEmployeeID")?.Value;
            var LoggedEmployeeEmail = authenticationState.User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;

            LoggedUser = DB.Employees.Where(user => user.EmployeeID == LoggedEmployeeID).FirstOrDefault();
            LoggedUserCredential = DB.Credentials.Where(user => user.EmailID == LoggedEmployeeEmail).FirstOrDefault();

            User.FirstName = LoggedUser!.FirstName;
            User.LastName = LoggedUser.LastName;
            User.Age = LoggedUser.Age.ToString();
            User.PhoneCode = LoggedUser.PhoneNumber.Substring(0, 3);
            User.PhoneNumber = LoggedUser.PhoneNumber.Substring(3);
            User.EmployeeType = LoggedUser.EmployeeType;
            User.EmailID = LoggedUserCredential!.EmailID;
            User.EmailPassword = LoggedUserCredential.EmailPassword;

        }

        private async Task UpdateEmployeeDetails()
        {
            // Intro command.
            ButtonTitle = "Submitting..";

            UpdateModels();

            DB.Employees.Update(empProfile);
            DB.Credentials.Update(empCredential);

            DB.Attach(empProfile!).State = EntityState.Modified;
            DB.Attach(empCredential!).State = EntityState.Modified;

            try
            {
                await DB.SaveChangesAsync();
                editStatusMessage = "Profile successfully updated!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeaveRecordExists(LoggedUser!.Id))
                {
                    NavigationManager.NavigateTo("notfound");
                }
                else
                {
                    throw;
                    editStatusMessage = "Error updating profile!";
                }
            }

            // Outro command.
            isDisabled = true;
            isEditMessageVisible = true;
            ButtonTitle = "Submit";
        }

        bool LeaveRecordExists(int id)
        {
            return DB.LeaveRecords.Any(e => e.Id == id);
        }

        private void EditUserDetails()
        {
            isDisabled = false;
        }

        private void UpdateModels()
        {
            empProfile = new Employee
            {
                Id = LoggedUser!.Id,
                EmployeeID = LoggedUser!.EmployeeID,
                FirstName = User.FirstName,
                LastName = User.LastName,
                Age = int.Parse(User.Age),
                PhoneNumber = User.PhoneCode + User.PhoneNumber,
                EmployeeType = LoggedUser!.EmployeeType,
                EmailID = User.EmailID
            };

            empCredential = new Credential
            {
                Id = LoggedUserCredential!.Id,
                EmailID = User.EmailID,
                EmailPassword = User.EmailPassword
            };

            DB.Entry(LoggedUser).State = EntityState.Detached;
            DB.Entry(LoggedUserCredential).State = EntityState.Detached;
        }

        private class UserProfileModel
        {
            public string? PhoneCode { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide designation.")]
            public string? EmployeeType { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide first name.")]
            [StringLength(20, ErrorMessage = "Name should be of max. 20 characters.")]
            public string? FirstName { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide last name.")]
            [StringLength(20, ErrorMessage = "Name should be of max. 20 characters.")]
            public string? LastName { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide age.")]
            [RangeAttribute(18, 60, ErrorMessage = "Age must be between 18 and 60.")]
            public string? Age { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide phone number.")]
            [StringLength(10, ErrorMessage = "Phone number must be of 10 digits.")]
            public string? PhoneNumber { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide Email Address.")]
            [StringLength(30, ErrorMessage = "Email should be of max. 30 characters.")]
            public string? EmailID { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide password.")]
            [StringLength(30, ErrorMessage = "Password should be of max. 30 characters.")]
            public string? EmailPassword { get; set; }

        }
    }
}
