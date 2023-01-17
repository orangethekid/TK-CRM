using Microsoft.AspNetCore.Components;
using TakraonlineCRM.Shared.Orders;

namespace TakraonlineCRM.Client.Pages.Order.Components
{
    public partial class FormMarketing
    {
        [Parameter] public OrderMarketing orderMarketing { get; set; }
 
        private void OnChange( ChangeEventArgs args )
        {
            var select = args.Value.ToString();
            switch (select)
            {
                case "facebook":
                    orderMarketing.Facebook = true;
                    orderMarketing.LineAdsPlatform = false;
                    orderMarketing.GoogleShop = false;
                    break;
                case "line":
                    orderMarketing.Facebook = false;
                    orderMarketing.LineAdsPlatform = true;
                    orderMarketing.GoogleShop = false;
                    break;
                case "google":
                    orderMarketing.Facebook = false;
                    orderMarketing.LineAdsPlatform = false;
                    orderMarketing.GoogleShop = true;
                    break;
            }
        }
    }
}
