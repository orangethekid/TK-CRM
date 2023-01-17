using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.Marketing;

namespace TakraonlineCRM.Client.Pages.Marketing
{
    public partial class MarketingList
    {
        [Inject] IMarketingRepository marketingRepository { get; set; }
        [Inject] IOrderRepository orderRepository { get; set; }
        [Inject] IWebSiteRepository webRepository { get; set; }

        public IList<FacebookAds> Facebooks { get; set; }
        public IList<LineAdsPlatform> LineAdsPlatforms { get; set; }
        public IList<GoogleShop> GoogleShops { get; set; }

        #region private
        private async Task PopulateControl()
        {
            try
            {
                Facebooks = await marketingRepository.GetFacebookAds();
                foreach (FacebookAds f in Facebooks)
                {
                    f.setOrderTakraId( await GetOrderTakraID( f.OrderID ) );
                }

                LineAdsPlatforms = await marketingRepository.GetLineAdsPlatform();
                foreach (LineAdsPlatform l in LineAdsPlatforms)
                {
                    l.setOrderTakraId( await GetOrderTakraID( l.OrderID ) );
                }

                GoogleShops = await marketingRepository.GetGoogleShop();
                foreach (GoogleShop g in GoogleShops)
                {
                    g.setOrderTakraId( await GetOrderTakraID( g.OrderID ) );
                    g.setWebsiteName( await GetWebsiteName( g.WebSiteId ) );
                }
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }
        }
        #endregion

        #region protected
        protected override async Task OnInitializedAsync()
        {
            await PopulateControl();
        }
        protected async Task<string> GetOrderTakraID( int id )
        {
            var order = await orderRepository.GetOne( id );
            return order.TakraOrderId;
        }
        protected async Task<string> GetWebsiteName( int id )
        {
            var web = await webRepository.GetOne( id );
            return web.Name;
        }
        #endregion
    }
}
