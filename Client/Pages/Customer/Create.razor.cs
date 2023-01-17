using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.Customers;
using TakraonlineCRM.Shared.User;

namespace TakraonlineCRM.Client.Pages.Customer
{
    public partial class Create
    {
        [Inject] NavigationManager uriHelper { get; set; }
        [Inject] ICustomerRepository repository { get; set; }
        [Inject] IAuthService auth { get; set; }

        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; }

        public TakraonlineCRM.Shared.Customers.Customer currentcustomer = new TakraonlineCRM.Shared.Customers.Customer();
        public CurrentUser user = new CurrentUser();

        #region private
        private async Task PopulateControl()
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
        #endregion

        #region protected
        protected override async Task OnParametersSetAsync()
        {
            await PopulateControl();
        }

        protected async Task CreateCustomer()
        {
            try
            {
                await repository.CreateCustomer( currentcustomer, user.Id );
                uriHelper.NavigateTo( "Customer" );
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }
        }
        #endregion
    }
}
