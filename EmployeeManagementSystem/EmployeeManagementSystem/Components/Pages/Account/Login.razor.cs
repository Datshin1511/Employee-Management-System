using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EmployeeManagementSystem.Components.Pages.Account
{
    public partial class Login
    {
        [CascadingParameter]
        public HttpContext? HttpContext { get; set; }

        [SupplyParameterFromForm]
        public LoginViewModel Model { get; set; } = new LoginViewModel();

        public bool IsLoggingIn { get; set; } = false;

        private string? errorMessage;
        private string? buttonTitle = "Login";

        private async Task Authenticate()
        {
            IsLoggingIn = true;
            buttonTitle = "Logging in..";

            var userAccount = DB.Credentials.Where(user => user.EmailID == Model.EmailID).FirstOrDefault() ?? null;
            var userDetails = DB.Employees.Where(user => user.EmailID == Model.EmailID).FirstOrDefault() ?? null;

            if (userAccount is null || userAccount.EmailPassword != Model.EmailPassword)
            {
                errorMessage = "Invalid Email ID or Password.";
                buttonTitle = "Login";
                return;
            }

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, $"{userDetails!.FirstName} {userDetails.LastName}"),
            new Claim(ClaimTypes.Role, userDetails!.EmployeeType),
            new Claim(ClaimTypes.Email, userAccount.EmailID),
            new Claim("LoggedEmployeeID", userDetails.EmployeeID)
        };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext!.SignInAsync(claimsPrincipal);

            buttonTitle = "Login";

            IsLoggingIn = false;
            NavigationManager.NavigateTo("/home");
        }


        public class LoginViewModel
        {
            [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide Email ID.")]
            public string? EmailID { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide Password.")]
            public string? EmailPassword { get; set; }
        }
    }
}
