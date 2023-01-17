using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.User;
using TakraonlineCRM.Shared.Models;
using Newtonsoft.Json;

namespace TakraonlineCRM.Client.Pages.Domain
{
    public partial class Detail
    {
        [Inject] NavigationManager uriHelper { get; set; }
        [Inject] IJSRuntime js { get; set; }
        [Inject] IAuthService auth { get; set; }
        [Inject] ILogRepository log { get; set; }
        [Inject] IDomainRepository repository { get; set; }

        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; }

        [Parameter] public int domainId { get; set; }
        public TakraonlineCRM.Shared.WebSites.Domain domain = new TakraonlineCRM.Shared.WebSites.Domain();
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
                domain = await repository.GetOne( domainId );
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
            activitiesLog.PageAction = "CustoemrEdit";
            activitiesLog.Actionlog = $"ลบโดเมน {domain.Name}";
            activitiesLog.BackupObject = JsonConvert.SerializeObject( domain );
        }
        #endregion

        #region protected
        protected async override Task OnParametersSetAsync()
        {
            await AuthenticateCheck();
            await PopulateControl();
            PopulateLog();
        }
        protected async Task Delete( int domainId )
        {
            if (await js.InvokeAsync<bool>( "confirm", $"ต้องการลบโดเมน {domainId} ?" ))
            {
                try
                {
                    repository.DeleteDomain( domainId );
                    await log.CreateLog( activitiesLog );
                    uriHelper.NavigateTo( "Order/Detail/" + domain.OrderId );
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
