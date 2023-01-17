using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.Models;
using TakraonlineCRM.Shared.User;

namespace TakraonlineCRM.Client.Pages.Order
{
    public partial class Detail
    {
        #region variable
        [Inject] NavigationManager uriHelper { get; set; }
        [Inject] IJSRuntime js { get; set; }
        [Inject] IAuthService auth { get; set; }
        [Inject] IOrderRepository repository { get; set; }
        [Inject] IUserRepository userRepository { get; set; }
        [Inject] ILogRepository log { get; set; }

        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; }

        [Parameter] public int orderId { get; set; }

        public TakraonlineCRM.Shared.Orders.Order order = new TakraonlineCRM.Shared.Orders.Order();
        public IList<TakraonlineCRM.Shared.Graphics.Graphic> graphic = new List<TakraonlineCRM.Shared.Graphics.Graphic>();
        private CurrentUser currentUser = new CurrentUser();
        private ActivitiesLog activitiesLog = new ActivitiesLog();

        #endregion

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
                    currentUser = await auth.CurrentUserInfo();
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
                order = await repository.GetOne( orderId );
                order.SetCreatorName( await GetDisplayName( order.CreatorId ) );
                if (String.IsNullOrWhiteSpace( order.OrderStatus ))
                    order.OrderStatus = "สร้างออเดอร์";
                switch (order.OrderType.ToLower())
                {
                    case "website":
                        break;
                    case "marketing":
                        break;
                    case "graphic":
                        graphic = await repository.GetGraphicByOrderId( orderId );
                        break;
                    case "course":
                        break;
                }
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }
        }
        private async Task<string> GetDisplayName( string id )
        {
            return await userRepository.GetUserDisplayNameById( id );
        }
        private void PopulateLog()
        {
            activitiesLog.UserId = currentUser.Id;
            activitiesLog.UserDisplayName = currentUser.DisplayName;
            activitiesLog.UserRole = currentUser.Role;
            activitiesLog.PageAction = "OrderDetail";
            activitiesLog.Actionlog = $"ลบ Order {order.TakraOrderId}";
            activitiesLog.BackupObject = JsonConvert.SerializeObject( order );
        }
        private string SetStatusClass( string status )
        {
            string css = string.Empty;
            switch (status)
            {
                case "สร้างออเดอร์":
                    break;
                case "กำลังดำเนินการ":
                    css = "text-info";
                    break;
                case "ดำเนินการแล้ว":
                    css = "text-success";
                    break;
                case "ยกเลิก":
                    css = "text-danger";
                    break;
            }
            return css;
        }
        private async Task SetOrderIsRead()
        {
            if (!order.IsRead)
            {
                order.IsRead = true;
                try
                {
                    await repository.EditOrder( order );
                }
                catch (Exception error)
                {
                    Console.WriteLine( error );
                }
            }
        }
        #endregion

        #region protected
        protected override async Task OnParametersSetAsync()
        {
            await AuthenticateCheck();
            await PopulateControl();
            await SetOrderIsRead();

            PopulateLog();
        }
        protected async Task Delete( int CustomerId )
        {
            if (await js.InvokeAsync<bool>( "confirm", $"ต้องการลบใบสั่งซื้อ {orderId} ?" ))
            {
                try
                {
                    repository.Delete( orderId );
                    await log.CreateLog( activitiesLog );
                    uriHelper.NavigateTo( "Customer/Detail/" + CustomerId );
                }
                catch (Exception error)
                {
                    Console.WriteLine( error );
                }
            }
        }

        protected bool DisableDeleteBtn()
        {
            if (string.IsNullOrWhiteSpace( currentUser.Role ))
                return true;
            else
                return currentUser.Role.ToLower() != "admin" && currentUser.Role.ToLower() != "subadmin" && currentUser.Role.ToLower() != "customerservice";
        }

        protected bool CanAccess()
        {
            if (currentUser == null)
                return false;
            else
                return !DisableDeleteBtn();
        }
        #endregion
    }
}
