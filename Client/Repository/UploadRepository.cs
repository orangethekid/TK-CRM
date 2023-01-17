using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;


namespace TakraonlineCRM.Client.Repository
{
    public class UploadRepository : IUploadRepository
    {
        private readonly HttpClient _httpClient;

        public UploadRepository( HttpClient httpClient )
        {
            _httpClient = httpClient;
        }

        public async Task<string> UploadImage( MultipartFormDataContent content )
        {
            var postResult = await _httpClient.PostAsync( "api/Upload/UploadImage", content );
            var postContent = await postResult.Content.ReadAsStringAsync();
            if (!postResult.IsSuccessStatusCode)
            {
                throw new ApplicationException( postContent );
            }
            else
            {
                //var imgUrl = Path.Combine( "http://tkcrm.takraonline.com:80/", postContent );
                var imgUrl = Path.Combine( "http://localhost:64135/", postContent );
                return imgUrl;
            }
        }

        public async Task<string> UploadTransferReceipt( MultipartFormDataContent content )
        {
            var postResult = await _httpClient.PostAsync( "api/Upload/UploadReceipt", content );
            var postContent = await postResult.Content.ReadAsStringAsync();
            if (!postResult.IsSuccessStatusCode)
            {
                throw new ApplicationException( postContent );
            }
            else
            {
                //var imgUrl = Path.Combine( "http://tkcrm.takraonline.com:80/", postContent );
                var imgUrl = Path.Combine( "http://localhost:64135/", postContent );
                return imgUrl;
            }
        }
    }
}
