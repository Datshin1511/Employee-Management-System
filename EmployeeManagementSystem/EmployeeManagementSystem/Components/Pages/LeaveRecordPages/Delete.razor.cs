using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Components.Pages.LeaveRecordPages
{
    public partial class Delete
    {
        LeaveRecord? leaverecord;

        [SupplyParameterFromQuery]
        public int Id { get; set; }

        protected override async Task OnInitializedAsync()
        {
            leaverecord = await DB.LeaveRecords.FirstOrDefaultAsync(m => m.Id == Id);

            if (leaverecord is null)
            {
                NavigationManager.NavigateTo("notfound");
            }
        }

        public async Task DeleteLeaveRecord()
        {
            DB.LeaveRecords.Remove(leaverecord!);
            await DB.SaveChangesAsync();
            NavigationManager.NavigateTo("/tickets");
        }
    }
}
