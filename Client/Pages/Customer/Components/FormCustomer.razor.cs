using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using TakraonlineCRM.Shared.User;
using TakraonlineCRM.Client.DataInterface;

namespace TakraonlineCRM.Client.Pages.Customer.Components
{
    public partial class FormCustomer
    {
        [Inject] IAuthService auth { get; set; }

        [Parameter] public TakraonlineCRM.Shared.Customers.Customer customer { get; set; }
        [Parameter] public string ButtonText { get; set; } = "Save";
        [Parameter] public EventCallback OnValidSubmit { get; set; }
        [Parameter] public CurrentUser currentUser { get; set; }

        public IList<TakraonlineCRM.Shared.User.User> sales { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            sales = await auth.GetAllByRole( "sale" );
            currentUser = await auth.CurrentUserInfo();
        }
    }
}
