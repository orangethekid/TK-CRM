using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.User;

namespace TakraonlineCRM.Client.Pages.Order
{
    public partial class Create
    {
        [Inject] NavigationManager uriHelper { get; set; }
        [Inject] IOrderRepository repository { get; set; }
        [Inject] IAuthService auth { get; set; }

        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; }

        [Parameter] public int customerID { get; set; }

        public TakraonlineCRM.Shared.Orders.Order order = new TakraonlineCRM.Shared.Orders.Order();

        private CurrentUser user = new CurrentUser();

        private async Task AuthenticateCheck()
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

        private async Task SetTakraOrderID()
        {
            TakraonlineCRM.Shared.Orders.Order orderMaxValueID = new TakraonlineCRM.Shared.Orders.Order();
            orderMaxValueID = await repository.GetMaxTakraOrderID();
            if (orderMaxValueID is not null)
            {
                int maxValueID = Convert.ToInt32( orderMaxValueID.TakraOrderId );
                if (maxValueID < 10000)
                    maxValueID = 9999;

                order.TakraOrderId = (maxValueID + 1).ToString();
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            await AuthenticateCheck();
            await SetTakraOrderID();
        }

        protected async Task CreateOrder()
        {
            try
            {
                if (Convert.ToInt32( order.TakraOrderId ) > 9999)
                    await SetTakraOrderID();

                order = await repository.CreateOrder( order, customerID, user.Id );
                if (order.OrderType.ToLower() == "website")
                {
                    uriHelper.NavigateTo( "/Web/create/" + customerID + "/" + order.Id );
                }
                else
                {
                    uriHelper.NavigateTo( "Customer/Detail/" + customerID );
                }
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }
        }
    }
}
