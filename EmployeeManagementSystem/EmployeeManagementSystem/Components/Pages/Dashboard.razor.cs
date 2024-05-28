using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;

namespace EmployeeManagementSystem.Components.Pages
{
    public partial class Dashboard
    {
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        private string? LoggedEmployeeID;

        protected override async Task OnInitializedAsync()
        {
            await Task.Delay(2000);

            var authenticationState = await authenticationStateTask;
            LoggedEmployeeID = authenticationState.User.FindFirst(c => c.Type == "LoggedEmployeeID")?.Value;

        }
    }
}
