using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.Models;
using TakraonlineCRM.Shared.User;

namespace TakraonlineCRM.Client.Pages.Customer
{
    public partial class Edit
    {
        [Inject] NavigationManager uriHelper { get; set; }
        [Inject] IJSRuntime js { get; set; }
        [Inject] IAuthService auth { get; set; }
        [Inject] ILogRepository log { get; set; }
        [Inject] ICustomerRepository repository { get; set; }

        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; }

        [Parameter] public int customerId { get; set; }
        public TakraonlineCRM.Shared.Customers.Customer customer = new TakraonlineCRM.Shared.Customers.Customer();
        private CurrentUser currentUser = new CurrentUser();
        private ActivitiesLog activitiesLog = new ActivitiesLog();

        #region private
        private async Task AuthenticateCheck()
        {
            try
            {
                if (!(await AuthenticationState).User.Identity.IsAuthenticated)
                {
                    uriHelper.NavigateTo( "/index" );
                }
                else
                {
                    currentUser = await auth.CurrentUserInfo();
                    await PopulateControl();
                    if (currentUser.Role.ToLower() == "sale")
                    {
                        if (customer.SaleId != currentUser.Id)
                        {
                            uriHelper.NavigateTo( "/" );
                        }
                    }
                    else if (currentUser.Role.ToLower() != "admin" && currentUser.Role.ToLower() != "subadmin" && currentUser.Role.ToLower() != "customerservice")
                    {
                        uriHelper.NavigateTo( "/" );
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }
        }
        private async Task PopulateControl()
        {
            try
            {
                customer = await repository.GetOneById( customerId );
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }
        }
        private void PopulateLog()
        {
            activitiesLog.UserId = currentUser.Id;
            activitiesLog.UserDisplayName = currentUser.DisplayName;
            activitiesLog.UserRole = currentUser.Role;
            activitiesLog.PageAction = "CustoemrEdit";
            activitiesLog.Actionlog = $"แก้ไขลูกค้า {customer.FirstName} {customer.LastName}";
            activitiesLog.BackupObject = JsonConvert.SerializeObject( customer );
        }
        #endregion

        #region protected
        protected async override Task OnParametersSetAsync()
        {
            await AuthenticateCheck();
            PopulateLog();
        }
        protected async Task EditCustomer()
        {
            try
            {
                await repository.EditCustomer( customer );
                await log.CreateLog( activitiesLog );
                await js.InvokeVoidAsync( "alert", $"Updated Successfully!" );
                uriHelper.NavigateTo( "Customer/detail/" + customerId );
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }
        }
        #endregion
    }
}
