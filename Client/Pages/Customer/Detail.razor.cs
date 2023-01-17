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

namespace TakraonlineCRM.Client.Pages.Customer
{
    public partial class Detail
    {
        [Inject] NavigationManager uriHelper { get; set; }
        [Inject] IJSRuntime js { get; set; }
        [Inject] ILogRepository log { get; set; }
        [Inject] IAuthService auth { get; set; }
        [Inject] IUserRepository userRepository { get; set; }
        [Inject] ICustomerRepository customerRepository { get; set; }
        [Inject] IOrderRepository orderRepository { get; set; }
        [Inject] IWebSiteRepository websiteRepository { get; set; }

        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; }

        [Parameter] public int customerId { get; set; }
        public TakraonlineCRM.Shared.Customers.Customer customer = new TakraonlineCRM.Shared.Customers.Customer();
        public List<TakraonlineCRM.Shared.WebSites.WebSite> webList = new List<TakraonlineCRM.Shared.WebSites.WebSite>();
        public List<TakraonlineCRM.Shared.Orders.Order> orderList = new List<TakraonlineCRM.Shared.Orders.Order>();
        private CurrentUser currentUser = new CurrentUser();
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
                    currentUser = await auth.CurrentUserInfo();
                    await PopulateControl();
                    if (currentUser.Role.ToLower() == "sale")
                    {
                        if (customer.SaleId != currentUser.Id)
                        {
                            uriHelper.NavigateTo( "/" );
                        }
                    }
                    else if (currentUser.Role.ToLower() != "admin" && currentUser.Role.ToLower() != "subadmin" && currentUser.Role.ToLower() != "customerservice")
                    {
                        uriHelper.NavigateTo( "/" );
                    }
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
                customer = await customerRepository.GetOneById( customerId );
                customer.SetCreatorName( await GetDisplayName( customer.CreatorId ) );
                customer.SetSaleName( await GetDisplayName( customer.SaleId ) );
                webList.AddRange( customer.WebSites );
                orderList.AddRange( customer.Orders );
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
            activitiesLog.PageAction = "CustomerDetail";
        }
        #endregion

        #region protected
        protected async override Task OnParametersSetAsync()
        {
            await AuthenticateCheck();
            PopulateLog();
        }
        protected async Task Delete( int id )
        {
            if (await js.InvokeAsync<bool>( "confirm", $"ต้องการลบลูกค้า {customer.FirstName} ?" ))
            {
                try
                {
                    activitiesLog.Actionlog = $"ลบลูกค้า {customer.FirstName}";
                    activitiesLog.BackupObject = JsonConvert.SerializeObject( customer );

                    customerRepository.DeleteCustomer( customer.Id );
                    await log.CreateLog( activitiesLog );

                    uriHelper.NavigateTo( "Customer" );
                }
                catch (Exception error)
                {
                    Console.WriteLine( error );
                }
                await OnInitializedAsync();
            }
        }
        protected async Task DeleteOrder( int OrderId )
        {
            if (await js.InvokeAsync<bool>( "confirm", $"ต้องการลบ ใบสั่งซื้อหมายเลข {OrderId} ?" ))
            {
                try
                {
                    var _order = await orderRepository.GetOne( OrderId );
                    activitiesLog.Actionlog = $"ลบใบสั่งซื้อ {OrderId}";
                    activitiesLog.BackupObject = JsonConvert.SerializeObject( _order );

                    orderRepository.Delete( OrderId );
                    await log.CreateLog( activitiesLog );
                }
                catch (Exception error)
                {
                    Console.WriteLine( error );
                }
                await OnInitializedAsync();
            }
        }
        protected async Task DeleteWebSite( int WebSiteId, string name )
        {
            if (await js.InvokeAsync<bool>( "confirm", $"ต้องการลบ เว็บไซต์ {name} ?" ))
            {
                try
                {
                    var _web = await websiteRepository.GetOne( WebSiteId );
                    activitiesLog.Actionlog = $"ลบเว็บไซต์ {name}";
                    activitiesLog.BackupObject = JsonConvert.SerializeObject( _web );

                    websiteRepository.DeleteWebSite( WebSiteId );
                    await log.CreateLog( activitiesLog );
                }
                catch (Exception error)
                {
                    Console.WriteLine( error );
                }
                await OnInitializedAsync();
            }
        }

        protected bool DisableDeleteBtn()
        {
            if (string.IsNullOrWhiteSpace( currentUser.Role ))
                return true;
            else
                return currentUser.Role.ToLower() != "admin" && currentUser.Role.ToLower() != "subadmin" && currentUser.Role.ToLower() != "customerservice";
        }
        #endregion
    }
}
