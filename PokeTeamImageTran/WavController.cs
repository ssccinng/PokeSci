using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFile = System.IO.File;
namespace PokeTeamImageTran;
[Route("api/[controller]")]
[ApiController]
public class WavController : ControllerBase
{
    int idx = 0;
    [HttpPost("GetVoice/{target}")]
    public async Task<byte[]> Get(string target)
    {
        //using StreamReader streamReader = new StreamReader(Request.Body);
        //var bytes1 = await streamReader.ReadToEndAsync();
        var bytes = new byte[(int)Request.ContentLength];
        //try
        //{
        //    var aa1a = Request.Body.Read(bytes, 0, (int)Request.ContentLength);

        //}
        //catch (Exception ee)
        //{

        //    throw;
        //}
        if (!Directory.Exists("voices"))
        {
            Directory.CreateDirectory("voices");
        }
        await Request.Body.CopyToAsync(new MemoryStream(bytes));
        string filename = $"voices/{idx}.wav";
        SFile.WriteAllBytes(filename, bytes);

        var newfiles = PyExtensions.CallVit(filename, target);
        var bs = SFile.ReadAllBytes(newfiles);
        idx++;
        return bs;
    }
    
}