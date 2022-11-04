using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AttachController : ControllerBase
    {
        //public async Task<MetadataModel> UploadFile([FromForm]IFormFile file)
        //{
        //    var tempPath = Path.GetTempPath();
        //    var meta = new MetadataModel
        //    {
        //        TempId = Guid.NewGuid(),
        //        Name = file.Name,
        //        MimeType = file.ContentType
        //    };
        //}
    }
}
