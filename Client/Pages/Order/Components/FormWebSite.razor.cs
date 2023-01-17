using Microsoft.AspNetCore.Components;
using TakraonlineCRM.Shared.Orders;

namespace TakraonlineCRM.Client.Pages.Order.Components
{
    public partial class FormWebSite
    {
        [Parameter] public OrderWebSite orderWebSite { get; set; }
    }
}
