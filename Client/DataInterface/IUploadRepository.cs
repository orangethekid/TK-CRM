using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TakraonlineCRM.Client.DataInterface
{
    public interface IUploadRepository
    {
        Task<string> UploadImage( MultipartFormDataContent content );
        Task<string> UploadTransferReceipt( MultipartFormDataContent content );
    }
}
