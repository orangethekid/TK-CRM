using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakraonlineCRM.Shared.Graphics;

namespace TakraonlineCRM.Client.Pages.Graphic.Components
{
    public partial class FormGraphic
    {
        [Parameter] public TakraonlineCRM.Shared.Graphics.Graphic graphic { get; set; }
        [Parameter] public string ButtonText { get; set; } = "Save";
        [Parameter] public EventCallback OnValidSubmit { get; set; }

        protected void AssignImageUrl( string imgUrl )
        {
            graphic.DraftFile = imgUrl;
        }
    }
}
