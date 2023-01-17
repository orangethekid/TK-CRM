using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.Graphics;

namespace TakraonlineCRM.Client.Pages.Graphic
{
    public partial class Create
    {
        [Inject] NavigationManager uriHelper { get; set; }
        [Inject] IGraphicRepository repository { get; set; }

        [Parameter] public int orderId { get; set; }

        public TakraonlineCRM.Shared.Graphics.Graphic graphic = new TakraonlineCRM.Shared.Graphics.Graphic();

        protected async Task CreateGraphic()
        {
            try
            {
                graphic.OrderId = orderId;
                graphic = await repository.CreateGraphic( graphic );
                uriHelper.NavigateTo( "Order/detail/" + graphic.OrderId );
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }
        }
    }
}
