using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

namespace PetShopLibrary.Utility
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> cloudinarySettings)
        {
            var settings = cloudinarySettings.Value;
            var account = new Account(settings.CloudName, settings.ApiKey, settings.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            try
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream())
                };
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                return uploadResult?.Url?.ToString(); 
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi upload ảnh: " + ex.Message);
                return null;
            }
        }
    }
}
