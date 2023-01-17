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
    public partial class Detail
    {
        [Inject] NavigationManager uriHelper { get; set; }
        [Inject] IJSRuntime js { get; set; }
        [Inject] IMarketingRepository repository { get; set; }
        [Inject] IWebSiteRepository webrepository { get; set; }
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
            activitiesLog.Actionlog = $"ลบการตลาด Goole { google.Campaign }";
            activitiesLog.BackupObject = JsonConvert.SerializeObject( google );
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
            if (await js.InvokeAsync<bool>( "confirm", $"ต้องการลบ Facebook Ads {google.Id}?" ))
            {
                try
                {
                    repository.DeleteLineAdsPlatform( google.Id );
                    await log.CreateLog( activitiesLog );
                    uriHelper.NavigateTo( "Order/detail/" + orderId );
                }
                catch (Exception error)
                {
                    Console.WriteLine( error );
                }
            }
        }
        protected async Task<string> GetWebsiteName( int id )
        {
            var web = await webrepository.GetOne( id );
            return web.Name;
        }
        #endregion
    }
}
