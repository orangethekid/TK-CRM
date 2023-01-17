using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net.Http.Headers;

namespace TakraonlineCRM.Server.Controllers
{
    [Route( "api/[controller]/[action]" )]
    [ApiController]
    public class UploadController : ControllerBase
    {
        [HttpPost]
        public IActionResult UploadImage()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine( "StaticFiles", "Images" );
                var pathToSave = Path.Combine( Directory.GetCurrentDirectory(), folderName );
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse( file.ContentDisposition ).FileName.Trim( '"' );
                    var fullPath = Path.Combine( pathToSave, fileName );
                    var dbPath = Path.Combine( folderName, fileName );
                    using (var stream = new FileStream( fullPath, FileMode.Create ))
                    {
                        file.CopyTo( stream );
                    }
                    return Ok( dbPath );
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode( 500, $"Internal server error: {ex}" );
            }
        }

        [HttpPost]
        public IActionResult UploadReceipt()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine( "StaticFiles", "TransferReceipt" );
                var pathToSave = Path.Combine( Directory.GetCurrentDirectory(), folderName );
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse( file.ContentDisposition ).FileName.Trim( '"' );
                    var fullPath = Path.Combine( pathToSave, fileName );
                    var dbPath = Path.Combine( folderName, fileName );
                    using (var stream = new FileStream( fullPath, FileMode.Create ))
                    {
                        file.CopyTo( stream );
                    }
                    return Ok( dbPath );
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode( 500, $"Internal server error: {ex}" );
            }
        }
    }
}
