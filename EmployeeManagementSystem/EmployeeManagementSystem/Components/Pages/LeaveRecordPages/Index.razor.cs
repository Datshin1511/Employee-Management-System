namespace EmployeeManagementSystem.Components.Pages.LeaveRecordPages
{
    public partial class Index
    {
        private bool isLoaded = false;

        protected override async Task OnInitializedAsync()
        {
            await Task.Delay(1000);
            isLoaded = true;
        }
    }
}
