using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.Marketing;
using TakraonlineCRM.Shared.User;
using TakraonlineCRM.Shared.Models;
using Newtonsoft.Json;


namespace TakraonlineCRM.Client.Pages.Marketing.Google
{
    public partial class Edit
    {
        [Inject] NavigationManager uriHelper { get; set; }
        [Inject] IJSRuntime js { get; set; }
        [Inject] IMarketingRepository repository { get; set; }
        [Inject] IAuthService auth { get; set; }
        [Inject] ILogRepository log { get; set; }

        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; }

        [Parameter] public int orderId { get; set; }
        public GoogleShop google = new GoogleShop();
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
                google = await repository.GetGoogleShop( orderId );
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }
        }
        private void PopulateLog()
        {
            activitiesLog.UserId = user.Id;
            activitiesLog.UserDisplayName = user.DisplayName;
            activitiesLog.UserRole = user.Role;
            activitiesLog.PageAction = "Marketing Google";
            activitiesLog.Actionlog = $"แก้ไขการตลาด Google Ads {google.Campaign}";
            activitiesLog.BackupObject = JsonConvert.SerializeObject( google );
        }
        #endregion

        protected async override Task OnParametersSetAsync()
        {
            await AuthenticateCheck();
            await PopulateControl();
            PopulateLog();
        }

        protected async Task EditGoogleShop()
        {
            try
            {
                google = await repository.EditGoogleShop( google );
                await log.CreateLog( activitiesLog );
                await js.InvokeVoidAsync( "alert", $"Updated Successfully!" );            
                uriHelper.NavigateTo( "Order/detail/" + orderId );
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }
        }
    }
}
