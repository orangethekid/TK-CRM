using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.User;
using TakraonlineCRM.Shared.Models;
using Newtonsoft.Json;

namespace TakraonlineCRM.Client.Pages.Graphic
{
    public partial class Edit
    {
        [Inject] NavigationManager uriHelper { get; set; }
        [Inject] IJSRuntime js { get; set; }
        [Inject] IGraphicRepository repository { get; set; }
        [Inject] IAuthService auth { get; set; }
        [Inject] ILogRepository log { get; set; }

        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; }

        [Parameter] public int graphicId { get; set; }
        public TakraonlineCRM.Shared.Graphics.Graphic graphic = new TakraonlineCRM.Shared.Graphics.Graphic();
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
            if(graphicId != 0)
            {
                graphic = await repository.GetByGraphicId( graphicId );
            }
        }
        private void PopulateLog()
        {
            activitiesLog.UserId = user.Id;
            activitiesLog.UserDisplayName = user.DisplayName;
            activitiesLog.UserRole = user.Role;
            activitiesLog.PageAction = "Graphic Edit";
            activitiesLog.Actionlog = $"แก้ไขบริการออกแบบ {graphic.Id}";
            activitiesLog.BackupObject = JsonConvert.SerializeObject( graphic );
        }
        #endregion

        protected async override Task OnParametersSetAsync()
        {
            await AuthenticateCheck();
            await PopulateControl();
            PopulateLog();
        }
        protected async Task EditGraphic()
        {
            try
            {
                await repository.EditGraphic( graphic );
                await js.InvokeVoidAsync( "alert", $"Updated Successfully!" );
                await log.CreateLog( activitiesLog );
                uriHelper.NavigateTo( "Order/detail/" + graphic.OrderId );
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }
        }
    }
}
