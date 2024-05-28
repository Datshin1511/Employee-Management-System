using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace EmployeeManagementSystem.Components.Pages
{
    public partial class Home
    {
        [Parameter]
        public string? empType { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        private string? username { get; set; }

        protected async override Task OnInitializedAsync()
        {
            var authenticationState = await authenticationStateTask;

            username = authenticationState.User.Identity!.Name ?? "User";
        }

        private async Task DisplayGreetingAlert()
        {
            var message = $"Hello, {username}!";

            await JSRuntime.InvokeVoidAsync("alert", message);
        }
    }
}
