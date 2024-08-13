using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Models.ProductEntity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class FilesController : ControllerBase
    {
        private readonly IFilesRepository _filesRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IEnumerable<string> _allowedFileExtensions = [".jpeg", ".png", ".jpg", ".webp"];

        public FilesController(IFilesRepository filesRepository, IWebHostEnvironment environment)
        {
            _filesRepository = filesRepository;
            _environment = environment;
        }

        [HttpPost("upload-plate-image/{plateId}")]
        public async Task<IActionResult> UploadPlateImageAsync(IFormFile file, [FromRoute] string plateId)
        {
            try
            {
                // TODO: When u add the Plate Not Found Exception, wrap all this code in a try catch (NotFoundException)

                // create the image file path
                var fileExtension = Path.GetExtension(file.FileName);

                // use of unacceptable extension
                if (!_allowedFileExtensions.Contains(fileExtension))
                {
                    return BadRequest($"File extension ({fileExtension}) is not allowed");
                }

                // VERY NECESSARY TO AVOID DUPLICATE IMAGE FILE NAMES
                var newFileName = $"{Guid.NewGuid().ToString()}{fileExtension}";

                // if the wwwroot/images directory doesn't exist, create it first before saving the image
                if (!Directory.Exists(Path.Combine(_environment.WebRootPath, "images")))
                {
                    Directory.CreateDirectory(Path.Combine(_environment.WebRootPath, "images"));
                }

                var imageFilePath = Path.Combine(_environment.WebRootPath, "images", newFileName);

                await using var fileStream = new FileStream(imageFilePath, FileMode.Create);
                await file.CopyToAsync(fileStream);

                var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
                var imageUrl = $"{baseUrl}/images/{newFileName}";

                // Save the image record to the database
                var image = new Image()
                {
                    imageId = Guid.NewGuid().ToString(),
                    Url = imageUrl,
                    AbsolutePath = imageFilePath
                };

                var response = await _filesRepository.SaveImageAsync(plateId, image);

                if (response.Flag == true) // Everything went well
                {
                    return Ok(new ImageResponse(image.imageId, image.Url));
                }
                else // An exception was thrown
                {
                    return BadRequest("Some shit happened");
                }

            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Exception occurred: {ex}");

                // Return a 500 Internal Server Error with a generic message
                return StatusCode(500, "Internal server error occurred. Please contact support.");
            }




        }

        [HttpDelete("delete-image/{imageId}")]
        public async Task<IActionResult> DeleteImage(string imageId)
        {
            try
            {
                await _filesRepository.DeleteImageAsync(imageId);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}