using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.Orders;
using TakraonlineCRM.Shared.Graphics;
using TakraonlineCRM.Shared.User;
using TakraonlineCRM.Shared.Models;
using Newtonsoft.Json;

namespace TakraonlineCRM.Client.Pages.Order.Components
{
    public partial class DetailGraphic
    {
        [Inject] NavigationManager uriHelper { get; set; }
        [Inject] IJSRuntime js { get; set; }
        [Inject] IAuthService auth { get; set; }
        [Inject] ILogRepository log { get; set; }
        [Inject] IOrderRepository repository { get; set; }
        [Inject] IGraphicRepository graphicRepository { get; set; }

        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; }

        [Parameter] public OrderGraphic orderGraphic { get; set; }
        [Parameter] public int customerId { get; set; }

        public IList<TakraonlineCRM.Shared.Graphics.Graphic> graphics = new List<TakraonlineCRM.Shared.Graphics.Graphic>();
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
            if (orderGraphic is not null)
            {
                graphics = await repository.GetGraphicByOrderId( orderGraphic.OrderId );
            }
        }
        private void PopulateLog()
        {
            activitiesLog.UserId = user.Id;
            activitiesLog.UserDisplayName = user.DisplayName;
            activitiesLog.UserRole = user.Role;
            activitiesLog.PageAction = "Order Graphic";
            activitiesLog.Actionlog = $"ลบบริการออกแบบ";
            activitiesLog.BackupObject = JsonConvert.SerializeObject( graphics );
        }
        #endregion

        #region protected
        protected async override Task OnParametersSetAsync()
        {
            await AuthenticateCheck();
            await PopulateControl();
            PopulateLog();
        }
        protected async Task DeleteGraphic( int id )
        {
            if (await js.InvokeAsync<bool>( "confirm", $"ต้องการลบ ข้อมูลการออกแบบ ?" ))
            {
                try
                {
                    graphicRepository.DeleteGraphic( id );
                    await log.CreateLog( activitiesLog );
                }
                catch (Exception error)
                {
                    Console.WriteLine( error );
                }
                await OnInitializedAsync();
            }
        }
        #endregion
    }
}
