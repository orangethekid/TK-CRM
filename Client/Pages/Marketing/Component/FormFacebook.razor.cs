using Microsoft.AspNetCore.Components;
using TakraonlineCRM.Shared.Marketing;

namespace TakraonlineCRM.Client.Pages.Marketing.Component
{
    public partial class FormFacebook
    {
        [Parameter] public FacebookAds facebook { get; set; }
        [Parameter] public string ButtonText { get; set; } = "Save";
        [Parameter] public EventCallback OnValidSubmit { get; set; }
    }
}
