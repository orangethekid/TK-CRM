using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Tewr.Blazor.FileReader;
using TakraonlineCRM.Client.DataInterface;

namespace TakraonlineCRM.Client.Shared
{
    public partial class Upload
    {
        private ElementReference _input;

        [Parameter]
        public string ImgUrl { get; set; }
        [Parameter]
        public EventCallback<string> OnChange { get; set; }
        [Inject]
        public IFileReaderService FileReaderService { get; set; }
        [Inject]
        public IUploadRepository Repository { get; set; }

        private string PrefixName = DateTime.Now.ToString( "MMddyyyy-HHmmss" ) + "_";

        private async Task HandleSelected()
        {
            foreach (var file in await FileReaderService.CreateReference( _input ).EnumerateFilesAsync())
            {
                if (file != null)
                {
                    var fileInfo = await file.ReadFileInfoAsync();
                    using (var ms = await file.CreateMemoryStreamAsync( 4 * 1024 ))
                    {
                        var content = new MultipartFormDataContent();
                        content.Headers.ContentDisposition = new ContentDispositionHeaderValue( "form-data" );
                        content.Add( new StreamContent( ms, Convert.ToInt32( ms.Length ) ), "image", PrefixName + fileInfo.Name );

                        ImgUrl = await Repository.UploadImage( content );

                        await OnChange.InvokeAsync( ImgUrl );
                    }
                }
            }
        }
    }
}
