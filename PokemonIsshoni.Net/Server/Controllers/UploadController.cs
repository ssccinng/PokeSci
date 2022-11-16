using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PokemonIsshoni.Net.Server.Controllers
{

    [ApiController]
    [Authorize]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public UploadController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost("upload/single")]
        public IActionResult Single(IFormFile file)
        {
            try
            {
                // Put your code here
                //UploadFile(file);
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("upload/avatar")]
        public IActionResult Avatar(IFormFile file)
        {
            try
            {
                if (file.Length > 1024 * 512)
                    throw new Exception("文件太大了! 最大为512kb");
                var uploadPath = _environment.WebRootPath + @"/ServerImages/Avatar";
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";



                using (var stream = new FileStream(Path.Combine(_environment.WebRootPath + @"/ServerImages/Avatar", fileName), FileMode.Create))
                {
                    // Save the file
                    file.CopyTo(stream);

                    // Return the URL of the file
                    var url = Url.Content($"~/ServerImage/Avatar/{fileName}");

                    return Ok(new { Url = url });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            //try
            //{
            //    // Put your code here
            //    UploadFile(file);
            //    return StatusCode(200);
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, ex.Message);
            //}
        }





        [HttpPost("upload/image")]
        public IActionResult Image(IFormFile file)
        {

            try
            {
                if (file.Length > 1024 * 512)
                    throw new Exception("文件太大了! 最大为512kb");
                var uploadPath = _environment.WebRootPath + @"/ServerImages/Upload";
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";



                using (var stream = new FileStream(Path.Combine(_environment.WebRootPath + @"/ServerImages/Upload", fileName), FileMode.Create))
                {
                    // Save the file
                    file.CopyTo(stream);

                    // Return the URL of the file
                    var url = Url.Content($"~/ServerImages/Upload/{fileName}");

                    return Ok(new { Url = url });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }
}
