using Microsoft.AspNetCore.Components;
using TakraonlineCRM.Shared.Orders;
namespace TakraonlineCRM.Client.Pages.Order.Components
{
    public partial class FormGraphic
    {
        [Parameter] public OrderGraphic orderGraphic { get; set; }
    }
}
