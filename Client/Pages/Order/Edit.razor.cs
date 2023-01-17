using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.Models;
using TakraonlineCRM.Shared.User;

namespace TakraonlineCRM.Client.Pages.Order
{
    public partial class Edit
    {
        [Inject] NavigationManager uriHelper { get; set; }
        [Inject] IJSRuntime js { get; set; }
        [Inject] IAuthService auth { get; set; }
        [Inject] ILogRepository log { get; set; }
        [Inject] IOrderRepository repository { get; set; }

        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; }

        [Parameter] public int orderId { get; set; }

        public TakraonlineCRM.Shared.Orders.Order order = new TakraonlineCRM.Shared.Orders.Order();
        private CurrentUser user = new CurrentUser();
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
                    user = await auth.CurrentUserInfo();
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
                order = await repository.GetOne( orderId );
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }
        }
        private async Task SetOrderIsRead()
        {
            if (!order.IsRead)
            {
                order.IsRead = true;
                try
                {
                    await repository.EditOrder( order );
                }
                catch (Exception error)
                {
                    Console.WriteLine( error );
                }
            }
        }
        private void PopulateLog()
        {
            activitiesLog.UserId = user.Id;
            activitiesLog.UserDisplayName = user.DisplayName;
            activitiesLog.UserRole = user.Role;
            activitiesLog.PageAction = "OrderEdit";
            activitiesLog.Actionlog = $"แก้ไข Order {order.TakraOrderId}";
            activitiesLog.BackupObject = JsonConvert.SerializeObject( order );
        }
        #endregion

        #region protected
        protected async override Task OnParametersSetAsync()
        {
            await AuthenticateCheck();
            await PopulateControl();
            await SetOrderIsRead();
            PopulateLog();
        }
        protected async Task EditOrder()
        {
            try
            {
                await repository.EditOrder( order );
                await log.CreateLog( activitiesLog );
                await js.InvokeVoidAsync( "alert", $"Updated Successfully!" );
                uriHelper.NavigateTo( "Order/detail/" + orderId );
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }
        }
        #endregion
    }
}
