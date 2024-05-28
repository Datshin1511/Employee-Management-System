using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Components.Pages.Account
{
    public partial class Register
    {
        [SupplyParameterFromForm]
        public RegisterViewModel Model { get; set; } = new();

        private List<string> EmployeeTypes = new List<string> { "Manager", "Staff" };
        private List<string> PhoneCodes = new List<string> { "+91", "+1", "+44" };

        private string? buttonTitle = "Sign up";

        private async Task CreateEmployeeRecord()
        {
            buttonTitle = "Signing in..";

            DB.Employees.Add(new Employee
            {
                EmployeeID = GenerateEmployeeID(Model.EmployeeType!),
                EmployeeType = Model.EmployeeType!,
                FirstName = Model.FirstName!,
                LastName = Model.LastName!,
                Age = Int32.Parse(Model.Age!),
                PhoneNumber = GetPhoneNumber().ToString(),
                EmailID = Model.EmailID!
            });

            DB.Credentials.Add(new Credential
            {
                EmailID = Model.EmailID!,
                EmailPassword = Model.EmailPassword!
            });

            await DB.SaveChangesAsync();

            NavigationManager.NavigateTo("/login");
        }

        private string GetPhoneNumber()
        {
            return Model.PhoneCode + Model.PhoneNumber;
        }

        private string GenerateEmployeeID(string EmpType)
        {
            var emp = DB.Employees.OrderByDescending(e => e.EmployeeID).Where(e => e.EmployeeType == EmpType).FirstOrDefault() ?? null;

            if (emp == null)
            {
                return EmpType == "Manager" ? "MA1" : "SF1";
            }

            var empID = emp.EmployeeID;
            return empID.ToString().Substring(0, 2) + (Int32.Parse(empID.ToString().Substring(2)) + 1).ToString();
        }

        public class RegisterViewModel
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
