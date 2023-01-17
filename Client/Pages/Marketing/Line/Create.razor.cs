using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.Marketing;

namespace TakraonlineCRM.Client.Pages.Marketing.Line
{
    public partial class Create
    {
        [Inject] NavigationManager uriHelper { get; set; }
        [Inject] IMarketingRepository repository { get; set; }

        [Parameter] public int orderID { get; set; }
        public LineAdsPlatform line = new LineAdsPlatform();

        protected async Task CreateLineAds()
        {
            try
            {
                line.Id = orderID;
                line = await repository.CreateLineAdsPlatform( line );
                uriHelper.NavigateTo( "Order/detail/" + orderID );
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }
        }
    }
}
