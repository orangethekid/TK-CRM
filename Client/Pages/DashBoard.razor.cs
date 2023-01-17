using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.User;

namespace TakraonlineCRM.Client.Pages
{
    public partial class DashBoard
    {
        [Inject] IAuthService auth { get; set; }
        [Inject] ICustomerRepository customerRepository { get; set; }
        [Inject] IOrderRepository orderRepository { get; set; }
        [Inject] IUserRepository userRepository { get; set; }

        private CurrentUser currentUser = new CurrentUser();
        private IList<TakraonlineCRM.Shared.Customers.Customer> customers { get; set; }
        private int customerTotalpagesQuantity;
        private int customerCurrentPage = 1;
        private IList<TakraonlineCRM.Shared.Orders.Order> orders { get; set; }
        private int ordersrTotalpagesQuantity;
        private int ordersCurrentPage = 1;

        #region private
        private async Task PopulateControl()
        {
            try
            {
                currentUser = await auth.CurrentUserInfo();

                await SelectedOrderPage( 1 );
                await SelectedCustomerPage( 1 );
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }
        }

        private string SetStyleUnRead( bool isRead )
        {
            if (currentUser == null || currentUser.Role.ToLower() == "sale")
            {
                return "";
            }
            else
            {
                if (isRead)
                    return "";
                else
                    return "background-color: #FFE321;";
            }
        }
        #endregion

        #region protected
        protected override async Task OnInitializedAsync()
        {
            await PopulateControl();
        }
        protected async Task<string> GetDisplayName( string id )
        {
            return await userRepository.GetUserDisplayNameById( id );
        }
        protected async Task SelectedOrderPage( int page )
        {
            ordersCurrentPage = page;
            if (currentUser.Role.ToLower() == "sale")
            {
                var getOrderBySaleId = await orderRepository.GetBySaleId( page, 5, currentUser.Id );
                ordersrTotalpagesQuantity = getOrderBySaleId.totalPageQuantity;
                orders = getOrderBySaleId.orderList;
            }
            else
            {
                var getOrder = await orderRepository.Get( page, 5 );
                ordersrTotalpagesQuantity = getOrder.totalPageQuantity;
                orders = getOrder.orderList;
            }

        }
        protected async Task SelectedCustomerPage( int page )
        {
            customerCurrentPage = page;
            if (currentUser.Role.ToLower() == "sale")
            {
                var getCustomerBySaleId = await customerRepository.GetAllBySaleId( page, 5, currentUser.Id );
                customerTotalpagesQuantity = getCustomerBySaleId.totalPageQuantity;
                customers = getCustomerBySaleId.customerList;
            }
            else
            {
                var getCustomer = await customerRepository.Get( page, 5 );
                customerTotalpagesQuantity = getCustomer.totalPageQuantity;
                customers = getCustomer.customerList;
            }

            foreach (TakraonlineCRM.Shared.Customers.Customer c in customers)
            {
                c.SetCreatorName( await GetDisplayName( c.CreatorId ) );
                c.SetSaleName( await GetDisplayName( c.SaleId ) );
            }
        }

        protected async Task SetUnRead( int orderID )
        {
            var order = orders.First( o => o.Id == orderID );
            order.IsRead = false;

            try
            {
                await orderRepository.EditOrder( order );
                await SelectedOrderPage( ordersCurrentPage );
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }
        }

        protected async Task SetRead( int orderID )
        {
            var order = orders.First( o => o.Id == orderID );
            order.IsRead = true;

            try
            {
                await orderRepository.EditOrder( order );
                await SelectedOrderPage( ordersCurrentPage );
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }
        }

        protected bool CanAccess()
        {
            if (currentUser == null)
                return false;
            else
                return currentUser.Role.ToLower() is not (not "admin" and not "subadmin" and not "customerservice");
        }
        #endregion
    }
}
