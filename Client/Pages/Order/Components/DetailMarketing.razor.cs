using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.Marketing;
using TakraonlineCRM.Shared.Orders;
using TakraonlineCRM.Shared.User;
using TakraonlineCRM.Shared.Models;
using Newtonsoft.Json;

namespace TakraonlineCRM.Client.Pages.Order.Components
{
    public partial class DetailMarketing
    {
        [Inject] NavigationManager uriHelper { get; set; }
        [Inject] IMarketingRepository repository { get; set; }
        [Inject] IWebSiteRepository webrepository { get; set; }
        [Inject] IJSRuntime js { get; set; }
        [Inject] IAuthService auth { get; set; }
        [Inject] ILogRepository log { get; set; }

        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; }

        [Parameter] public OrderMarketing orderMarketing { get; set; }

        private FacebookAds facebook = new FacebookAds();
        private LineAdsPlatform line = new LineAdsPlatform();
        private GoogleShop google = new GoogleShop();
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
            if (orderMarketing.Facebook)
            {
                facebook = await repository.GetFacebookAds( orderMarketing.OrderId );
            }
            else if (orderMarketing.LineAdsPlatform)
            {
                line = await repository.GetLineAdsPlatform( orderMarketing.OrderId );
            }
            else if (orderMarketing.GoogleShop)
            {
                google = await repository.GetGoogleShop( orderMarketing.OrderId );
            }
        }
        private void PopulateLog()
        {
            activitiesLog.UserId = user.Id;
            activitiesLog.UserDisplayName = user.DisplayName;
            activitiesLog.UserRole = user.Role;
            activitiesLog.PageAction = "Marketing";          
        }
        #endregion

        #region protected
        protected async override Task OnParametersSetAsync()
        {
            await PopulateControl();
        }
        protected async Task DeleteFacebook( int id )
        {
            if (await js.InvokeAsync<bool>( "confirm", $"ต้องการลบแคมเปญ Facebook Ads {facebook.Id}?" ))
            {
                try
                {
                    activitiesLog.Actionlog = $"ลบการตลาก Facebook {facebook.Campaign}";
                    activitiesLog.BackupObject = JsonConvert.SerializeObject( facebook );
                    repository.DeleteFacebookAds( facebook.Id );
                    await log.CreateLog( activitiesLog );
                    uriHelper.NavigateTo( "Order/detail/" + orderMarketing.OrderId );
                }
                catch (Exception error)
                {
                    Console.WriteLine( error );
                }
            }
        }
        protected async Task DeleteLine( int id )
        {
            if (await js.InvokeAsync<bool>( "confirm", $"ต้องการลบแคมเปญ Line Ads Platform {line.Id} ?" ))
            {
                try
                {
                    activitiesLog.Actionlog = $"ลบการตลาด Line {line.Campaign}";
                    activitiesLog.BackupObject = JsonConvert.SerializeObject(line);
                    repository.DeleteLineAdsPlatform( line.Id );
                    await log.CreateLog( activitiesLog );
                    uriHelper.NavigateTo( "Order/detail/" + orderMarketing.OrderId );
                }
                catch (Exception error)
                {
                    Console.WriteLine( error );
                }
            }
        }
        protected async Task DeleteGoogle( int id )
        {
            if (await js.InvokeAsync<bool>( "confirm", $"ต้องการลบแคมเปญ Google Shop {google.Id} ?" ))
            {
                try
                {
                    activitiesLog.Actionlog = $"ลบการตลาด Google {google.Campaign}";
                    activitiesLog.BackupObject = JsonConvert.SerializeObject( google );
                    repository.DeleteGoogleShop( google.Id );
                    await log.CreateLog( activitiesLog );
                    uriHelper.NavigateTo( "Order/detail/" + orderMarketing.OrderId );
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
