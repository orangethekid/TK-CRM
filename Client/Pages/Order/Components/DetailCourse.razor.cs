using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.Orders;
using TakraonlineCRM.Shared.User;

namespace TakraonlineCRM.Client.Pages.Order.Components
{
    public partial class DetailCourse
    {
        [Inject] NavigationManager uriHelper { get; set; }
        [Inject] IAuthService auth { get; set; }
        //[Inject] IOrderRepository repository { get; set; }

        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; }
        [Parameter] public OrderCourse orderCourse { get; set; }

        private CurrentUser user = new CurrentUser();

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

        //private async Task PopulateControl()
        //{
        //    if (orderCourse is not null)
        //    {
        //        graphics = await repository.GetGraphicByOrderId( orderCourse.OrderId );
        //    }
        //}
        #endregion

        #region protected
        protected async override Task OnParametersSetAsync()
        {
            await AuthenticateCheck();
            //await PopulateControl();

        }

        #endregion
    }
}
