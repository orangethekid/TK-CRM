using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.Models;
using TakraonlineCRM.Shared.User;

namespace TakraonlineCRM.Client.Pages.WebSite
{
    public partial class Detail
    {
        [Inject] NavigationManager uriHelper { get; set; }
        [Inject] IJSRuntime js { get; set; }
        [Inject] ILogRepository log { get; set; }
        [Inject] IAuthService auth { get; set; }
        [Inject] IDomainRepository domainRepository { get; set; }
        [Inject] IWebSiteRepository websiteRepository { get; set; }

        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; }

        [Parameter] public int websiteId { get; set; }
        TakraonlineCRM.Shared.WebSites.WebSite website = new TakraonlineCRM.Shared.WebSites.WebSite();
        private CurrentUser currentUser = new CurrentUser();
        private ActivitiesLog activitiesLog = new ActivitiesLog();

        #region private
        private void PopulateLog()
        {
            activitiesLog.UserId = currentUser.Id;
            activitiesLog.UserDisplayName = currentUser.DisplayName;
            activitiesLog.UserRole = currentUser.Role;
            activitiesLog.PageAction = "WebsiteDetail";
        }

        private async Task AuthenticateCheck()
        {
            try
            {
                if (!(await AuthenticationState).User.Identity.IsAuthenticated)
                {
                    uriHelper.NavigateTo( "/index" );
                }
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }
        }

        #endregion

        #region protected

        protected async override Task OnParametersSetAsync()
        {
            await AuthenticateCheck();
            PopulateLog();
            try
            {
                website = await websiteRepository.GetOne( websiteId );
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }
        }

        async Task Delete( int webId )
        {
            if (await js.InvokeAsync<bool>( "confirm", $"ต้องการลบเว็บไซต์ {website.Name} ?" ))
            {
                try
                {
                    var _website = await websiteRepository.GetOne( webId );
                    activitiesLog.Actionlog = $"ลบเว็บไซต์ {webId} {website.Name}";
                    activitiesLog.BackupObject = JsonConvert.SerializeObject( _website );
                    websiteRepository.DeleteWebSite( webId );
                    uriHelper.NavigateTo( "Customer/Detail/" + website.CustomerId );
                }
                catch (Exception error)
                {
                    Console.WriteLine( error );
                }
            }
        }

        async Task DeleteDomain( int id, string name )
        {
            if (await js.InvokeAsync<bool>( "confirm", $"ต้องการลบโดเมน {name} ?" ))
            {
                try
                {
                    var domain = await domainRepository.GetOne( id );
                    activitiesLog.Actionlog = $"ลบโดเมน {id} {name}";
                    activitiesLog.BackupObject = JsonConvert.SerializeObject( domain );
                    domainRepository.DeleteDomain( id );
                    await OnParametersSetAsync();
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
