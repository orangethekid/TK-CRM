using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.Marketing;
using TakraonlineCRM.Shared.User;
using TakraonlineCRM.Shared.Models;
using Newtonsoft.Json;


namespace TakraonlineCRM.Client.Pages.Marketing.Line
{
    public partial class Detail
    {
        [Inject] NavigationManager uriHelper { get; set; }
        [Inject] IJSRuntime js { get; set; }
        [Inject] IMarketingRepository repository { get; set; }
        [Inject] IAuthService auth { get; set; }
        [Inject] ILogRepository log { get; set; }

        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; }

        [Parameter] public int orderId { get; set; }
        public LineAdsPlatform line = new LineAdsPlatform();
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
                line = await repository.GetLineAdsPlatform( orderId );
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
            activitiesLog.PageAction = "Marketing Line";
            activitiesLog.Actionlog = $"ลบการตลาด Line Ads {line.LineOA}";
            activitiesLog.BackupObject = JsonConvert.SerializeObject( line );
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
            if (await js.InvokeAsync<bool>( "confirm", $"ต้องการลบ Facebook Ads {line.Id}?" ))
            {
                try
                {
                    repository.DeleteLineAdsPlatform( line.Id );
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
