using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.User;
using TakraonlineCRM.Shared.Models;
using Newtonsoft.Json;

namespace TakraonlineCRM.Client.Pages.Marketing.Facebook
{
    public partial class Detail
    {
        [Inject] NavigationManager uriHelper { get; set; }
        [Inject] IJSRuntime js { get; set; }
        [Inject] IAuthService auth { get; set; }
        [Inject] ILogRepository log { get; set; }
        [Inject] IMarketingRepository repository { get; set; }

        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; }

        [Parameter] public int orderId { get; set; }
        public TakraonlineCRM.Shared.Marketing.FacebookAds facebook = new TakraonlineCRM.Shared.Marketing.FacebookAds();
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
            activitiesLog.Actionlog = $"ลบการตลาด Facebook Ads {facebook.FacebookPage}";
            activitiesLog.BackupObject = JsonConvert.SerializeObject( facebook );
        }
        #endregion

        #region protected
        protected async override Task OnParametersSetAsync()
        {
            await AuthenticateCheck();
            await PopulateControl();
            PopulateLog();
        }
        protected async Task Delete( int id )
        {
            if (await js.InvokeAsync<bool>( "confirm", $"ต้องการลบ Facebook Ads {facebook.Id}?" ))
            {
                try
                {
                    repository.DeleteFacebookAds( facebook.Id );
                    await log.CreateLog( activitiesLog );
                    uriHelper.NavigateTo( "Order/detail/" + orderId );
                }
                catch (Exception error)
                {
                    Console.WriteLine( error );
                }
            }
        }
        #endregion
    }
}
