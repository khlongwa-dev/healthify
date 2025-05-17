using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace backend.Helpers
{
    public static class CloudinaryHelper
    {
        public static async Task<string> UploadImageAsync(Cloudinary cloudinary, IFormFile file, string folder)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Folder = folder
            };

            var result = await cloudinary.UploadAsync(uploadParams);
            return result.SecureUrl.ToString();
        }
    }
}