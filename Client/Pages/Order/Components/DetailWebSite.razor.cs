using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.Orders;

namespace TakraonlineCRM.Client.Pages.Order.Components
{
    public partial class DetailWebSite
    {
        [Inject] IOrderRepository repository { get; set; }

        [Parameter] public OrderWebSite orderWebSite { get; set; }
        [Parameter] public int customerId { get; set; }

        private TakraonlineCRM.Shared.WebSites.WebSite website = new TakraonlineCRM.Shared.WebSites.WebSite();
        private TakraonlineCRM.Shared.WebSites.Domain domain = new TakraonlineCRM.Shared.WebSites.Domain();

        private async Task PopulateControl()
        {
            if(orderWebSite is not null)
            {
                if (orderWebSite.Website)
                    website = await repository.GetWebSiteByOrderId( orderWebSite.OrderId );
                if (orderWebSite.Domain)
                    domain = await repository.GetDomainByOrderId( orderWebSite.OrderId );
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            await PopulateControl();
        }
    }
}
