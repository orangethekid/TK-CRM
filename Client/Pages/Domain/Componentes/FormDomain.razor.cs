using Microsoft.AspNetCore.Components;

namespace TakraonlineCRM.Client.Pages.Domain.Componentes
{
    public partial class FormDomain
    {
        [Parameter] public TakraonlineCRM.Shared.WebSites.Domain domain { get; set; }
        [Parameter] public string ButtonText { get; set; } = "Save";
        [Parameter] public EventCallback OnValidSubmit { get; set; }
    }
}
