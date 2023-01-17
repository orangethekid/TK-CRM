using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.User;
using TakraonlineCRM.Shared.Models;
using Newtonsoft.Json;

namespace TakraonlineCRM.Client.Pages.Domain
{
    public partial class Edit
    {
        [Inject] NavigationManager uriHelper { get; set; }
        [Inject] IJSRuntime js { get; set; }
        [Inject] IAuthService auth { get; set; }
        [Inject] ILogRepository log { get; set; }
        [Inject] IDomainRepository repository { get; set; }

        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; }

        [Parameter] public int domianId { get; set; }
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
        private async Task PopulateContorl()
        {
            try
            {
                domain = await repository.GetOne( domianId );
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
            activitiesLog.Actionlog = $"แก้ไขโดเมน {domain.Name}";
            activitiesLog.BackupObject = JsonConvert.SerializeObject( domain );
        }
        #endregion

        #region protected
        protected async override Task OnParametersSetAsync()
        {
            await AuthenticateCheck();
            await PopulateContorl();
            PopulateLog();
        }
        protected async Task EditDomain()
        {
            try
            {
                await repository.EditDomain( domain );
                await js.InvokeVoidAsync( "alert", $"Updated Successfully!" );
                await log.CreateLog( activitiesLog );
                uriHelper.NavigateTo( "Web/detail/" + domain.WebSiteId );
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }
        }
        #endregion
    }
}
