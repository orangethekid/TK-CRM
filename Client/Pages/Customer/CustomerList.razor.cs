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

namespace TakraonlineCRM.Client.Pages.Customer
{
    public partial class CustomerList
    {
        [Inject] NavigationManager uriHelper { get; set; }
        [Inject] IJSRuntime js { get; set; }
        [Inject] IAuthService auth { get; set; }
        [Inject] ICustomerRepository customerRepository { get; set; }
        [Inject] IUserRepository userRepository { get; set; }

        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; }

        //Search And Paging
        private SearchResult search = new SearchResult() { Page = 1, CurrentFilter = "name", SortOrder = "Descending" };
        private List<SearchOption> searchField = new List<SearchOption>();
        private int totalPagesQuantity;
        private int currentPage = 1;

        private IList<TakraonlineCRM.Shared.Customers.Customer> Customers { get; set; }
        private CurrentUser currentUser { get; set; }

        #region privatte
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
        private async Task<string> GetDisplayName( string id )
        {
            return await userRepository.GetUserDisplayNameById( id );
        }
        private void SetupSearchFilter()
        {
            searchField.Add( new SearchOption() { Key = "name", Name = "ชื่อ" } );
            searchField.Add( new SearchOption() { Key = "phone", Name = "เบอร์โทรศัพท์" } );
            searchField.Add( new SearchOption() { Key = "email", Name = "อีเมล" } );
        }

        private bool CanAccessBtn( string role, string currentUserID, string customerAttendantID )
        {
            if (role == "sale")
            {
                if (customerAttendantID == currentUserID)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (role == "admin" || role == "subadmin" || role == "customerservice")
            {
                return true;
            }
            else
            {
                return false;
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

            var getCustomer = await customerRepository.Get( search );
            totalPagesQuantity = getCustomer.totalPageQuantity;
            Customers = getCustomer.customerList;

            foreach (var c in Customers)
            {
                c.SetCreatorName( await GetDisplayName( c.CreatorId ) );
                c.SetSaleName( await GetDisplayName( c.SaleId ) );
            }
        }
        protected async Task Search()
        {
            await SelectedPage( 1 );
        }
        protected async Task Delete( int CustomerId )
        {
            var customer = Customers.First( x => x.Id == CustomerId );
            if (await js.InvokeAsync<bool>( "confirm", $"Do you want to delete {customer.FirstName}'s ({customer.Id}) Record?" ))
            {
                try
                {
                    customerRepository.DeleteCustomer( customer.Id );
                }
                catch (Exception error)
                {
                    Console.WriteLine( error );
                }
                await OnInitializedAsync();
            }
            uriHelper.NavigateTo( "/Customer" );
        }

        protected string HrefURL( string btnType, TakraonlineCRM.Shared.Customers.Customer customer )
        {
            if (CanAccessBtn( currentUser.Role.ToLower(), currentUser.Id.ToString(), customer.SaleId ))
            {
                if (btnType.ToLower() == "createorder")
                    return "Order/create/" + customer.Id;
                else
                    return "Customer/detail/" + customer.Id;
            }
            else
            {
                return "javascript:void(0)";
            }
        }

        protected string SetPointerEvent( TakraonlineCRM.Shared.Customers.Customer customer )
        {
            return CanAccessBtn( currentUser.Role.ToLower(), currentUser.Id.ToString(), customer.SaleId ) ? "" : "pointer-events: none;";
        }

        protected bool DisableDeleteBtn()
        {
            return currentUser.Role.ToLower() != "admin" && currentUser.Role.ToLower() != "subadmin" && currentUser.Role.ToLower() != "customerservice";
        }

        #endregion
    }
}
