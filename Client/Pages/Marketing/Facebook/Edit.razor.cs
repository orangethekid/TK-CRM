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


namespace TakraonlineCRM.Client.Pages.Marketing.Facebook
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
        public FacebookAds facebook = new FacebookAds();
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
                facebook = await repository.GetFacebookAds( orderId );
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
            activitiesLog.PageAction = "Marketing Facebook";
            activitiesLog.Actionlog = $"แก้ไขการตลาด Facebook Ads ของ {facebook.FacebookPage}";
            activitiesLog.BackupObject = JsonConvert.SerializeObject( facebook );
        }
        #endregion

        protected async override Task OnParametersSetAsync()
        {
            await AuthenticateCheck();
            await PopulateControl();
            PopulateLog();
        }

        protected async Task EditFacebookAds()
        {
            try
            {
                facebook = await repository.EditFacebookAds( facebook );
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
