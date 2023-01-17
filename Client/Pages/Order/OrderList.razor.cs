using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.Models;
using TakraonlineCRM.Shared.User;

namespace TakraonlineCRM.Client.Pages.Order
{
    public partial class OrderList
    {
        [Inject] NavigationManager uriHelper { get; set; }
        [Inject] IJSRuntime js { get; set; }
        [Inject] IAuthService auth { get; set; }
        [Inject] IOrderRepository repository { get; set; }

        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; }

        //Search And Paging
        private SearchResult search = new SearchResult() { Page = 1, CurrentFilter = "takraid", SortOrder = "Descending" };
        private List<SearchOption> searchField = new List<SearchOption>();
        private int totalPagesQuantity;
        private int currentPage = 1;

        private IList<TakraonlineCRM.Shared.Orders.Order> orders = new List<TakraonlineCRM.Shared.Orders.Order>();
        private CurrentUser currentUser { get; set; }

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
            await SelectedPage( 1 );
        }
        private void SetupSearchFilter()
        {
            searchField.Add( new SearchOption() { Key = "takraid", Name = "หมายเลขใบสั่งซื้อ" } );
            searchField.Add( new SearchOption() { Key = "type", Name = "ประเภทใบสั่งซื้อ" } );
            searchField.Add( new SearchOption() { Key = "orderstatus", Name = "สถานะใบสั่งซื้อ" } );
            searchField.Add( new SearchOption() { Key = "priceUp", Name = "ราคาสูงกว่า" } );
            searchField.Add( new SearchOption() { Key = "priceDown", Name = "ราคาต่ำกว่า" } );
            searchField.Add( new SearchOption() { Key = "date", Name = "วันที่สร้าง" } );
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
        private async Task GetOrder()
        {
            if (currentUser != null)
            {
                if (currentUser.Role.ToLower() == "sale")
                {
                    var getOrderBysale = await repository.GetBySaleId( search, currentUser.Id );
                    totalPagesQuantity = getOrderBysale.totalPageQuantity;
                    orders = getOrderBysale.orderList;
                }
                else
                {
                    var getOrder = await repository.Get( search );
                    totalPagesQuantity = getOrder.totalPageQuantity;
                    orders = getOrder.orderList;
                }
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
            await AuthenticateCheck();
            await PopulateControl();
            SetupSearchFilter();
        }
        protected async Task SelectedPage( int page )
        {
            currentPage = page;
            search.Page = page;
            await GetOrder();
        }
        protected async Task Search()
        {
            await SelectedPage( 1 );
        }
        protected async Task Delete( int OrderId )
        {
            var order = orders.First( x => x.Id == OrderId );
            if (await js.InvokeAsync<bool>( "confirm", $"Do you want to delete {order.Id}'s ?" ))
            {
                try
                {
                    repository.Delete( order.Id );
                }

                catch (Exception error)
                {
                    Console.WriteLine( error );
                }
            }
            uriHelper.NavigateTo( "/Order" );
        }
        protected async Task SetUnRead( int orderID )
        {
            var order = orders.First( o => o.Id == orderID );
            order.IsRead = false;

            try
            {
                await repository.EditOrder( order );
                await GetOrder();
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
                await repository.EditOrder( order );
                await GetOrder();
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }
        }

        protected bool DisableDeleteBtn()
        {
            if (currentUser == null)
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
