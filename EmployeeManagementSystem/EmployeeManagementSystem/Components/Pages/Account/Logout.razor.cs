using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;

namespace EmployeeManagementSystem.Components.Pages.Account
{
    public partial class Logout
    {
        [CascadingParameter]
        public HttpContext? HttpContext { get; set; }

        public string? message = "Logging out..";
        public bool isLoggedOut = false;

        protected override async Task OnInitializedAsync()
        {

            if (HttpContext.User.Identity!.IsAuthenticated)
            {
                await HttpContext.SignOutAsync();
                isLoggedOut = true;
                message = "You have been logged out successfully.";

                await Task.Delay(1000);
                NavigationManager.NavigateTo("/login", true);
            }

        }
    }
}
