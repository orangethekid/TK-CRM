using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.Marketing;

namespace TakraonlineCRM.Client.Pages.Marketing.Facebook
{
    public partial class Create
    {
        [Inject] NavigationManager uriHelper { get; set; }
        [Inject] IMarketingRepository repository { get; set; }

        [Parameter] public int orderID { get; set; }
        public FacebookAds facebook = new FacebookAds();

        protected async Task CreateFacebookAds()
        {
            try
            {
                facebook.OrderID = orderID;
                facebook = await repository.CreateFacebookAds( facebook );
                uriHelper.NavigateTo( "Order/detail/" + orderID );
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }
        }
    }
}
