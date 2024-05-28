using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Components.Pages.LeaveRecordPages
{
    public partial class Create
    {
        [SupplyParameterFromForm]
        public TicketModel Model { get; set; } = new();

        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        private string? LoggedEmployeeID { get; set; }
        private string? buttonTitle = "Submit";
        private string? leavesTaken = "0";

        public int? MaxTextLength = 400;
        public int? InputLength => Model.Reason?.Length ?? 0;
        private bool ticketLimitReached = false;

        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await authenticationStateTask;
            LoggedEmployeeID = authenticationState.User.FindFirst(c => c.Type == "LoggedEmployeeID")?.Value;

            var ticketCount = DB.LeaveRecords.Where(user => user.EmployeeID == LoggedEmployeeID && user.LeaveStatusMsg == "Accepted").Count();
            leavesTaken = ticketCount.ToString();

            if (ticketCount >= 3)
            {
                ticketLimitReached = true;
            }
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task AddLeaveRecord()
        {
            buttonTitle = "Submitting...";

            DB.LeaveRecords.Add(new LeaveRecord
            {
                EmployeeID = LoggedEmployeeID,
                LeaveReason = Model.Reason,
                LeaveStatusMsg = "Pending",
                LeaveStatus = true,
                TicketDate = DateTime.Now

            });

            try
            {
                await DB.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                JSRuntime.InvokeVoidAsync("alert", "Ticket raised successfully!");
            }

            NavigationManager.NavigateTo("/home");
        }

        private void HandleInputChange(ChangeEventArgs e)
        {
            Model.Reason = e.Value!.ToString();
        }

        public class TicketModel
        {
            [Required(AllowEmptyStrings = false, ErrorMessage = "Provide a valid reason.")]
            [StringLength(400, ErrorMessage = "The word limit is 30.")]
            public string? Reason { get; set; }
        }
    }
}
