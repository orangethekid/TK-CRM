using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.Marketing;

namespace TakraonlineCRM.Client.Pages.Marketing.Google
{
    public partial class Create
    {
        [Inject] NavigationManager uriHelper { get; set; }
        [Inject] IMarketingRepository repository { get; set; }

        [Parameter] public int orderID { get; set; }
        public GoogleShop google = new GoogleShop();

        protected async Task CreateGoogle()
        {
            try
            {
                google.OrderID = orderID;
                google = await repository.CreateGoogleShop( google );
                uriHelper.NavigateTo( "Order/detail/" + orderID );
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }
        }

    }
}
