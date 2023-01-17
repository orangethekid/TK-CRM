using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.Marketing;
using TakraonlineCRM.Shared.WebSites;

namespace TakraonlineCRM.Client.Pages.Marketing.Component
{
    public partial class FormGoogle
    {
        [Inject] IWebSiteRepository webrepository { get; set; }

        [Parameter] public GoogleShop google { get; set; }
        [Parameter] public string ButtonText { get; set; } = "Save";
        [Parameter] public EventCallback OnValidSubmit { get; set; }
        private IList<TakraonlineCRM.Shared.WebSites.WebSite> webs = new List<TakraonlineCRM.Shared.WebSites.WebSite>();


        private async Task PopulateControl()
        {
            try
            {
                webs = await webrepository.GetAll();
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }
        }

        protected async override Task OnParametersSetAsync()
        {
            await PopulateControl();
        }
    }

}
