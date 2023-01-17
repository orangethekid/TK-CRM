using Microsoft.AspNetCore.Components;

namespace TakraonlineCRM.Client.Pages.Setting.Components
{
    public partial class FormCourse
    {
        [Parameter] public TakraonlineCRM.Shared.Setting.Course.Course Course { get; set; }
        [Parameter] public string ButtonText { get; set; } = "Save";
        [Parameter] public EventCallback OnValidSubmit { get; set; }
    }
}
