using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;

namespace TakraonlineCRM.Client.Pages.Domain
{
    public partial class Create
    {
        [Inject] NavigationManager uriHelper { get; set; }
        [Inject] IDomainRepository repository { get; set; }

        [Parameter] public int websiteId { get; set; } = 0;
        [Parameter] public int orderId { get; set; }

        public TakraonlineCRM.Shared.WebSites.Domain domain = new TakraonlineCRM.Shared.WebSites.Domain();

        protected async Task CreateDomain()
        {
            try
            {
                    domain.WebSiteId = websiteId;
                domain.OrderId = orderId;
                await repository.CreateDomain( domain );
                uriHelper.NavigateTo( "Web/Detail/" + websiteId );
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }
        }
    }
}
