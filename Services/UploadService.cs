using CRUDNewsApi.Helpers.Exceptions;
using Org.BouncyCastle.Asn1.Ocsp;
using RandomStringCreator;
using System.Net.Http.Headers;

namespace CRUDNewsApi.Services
{
    public interface IUploadService
    {
        string UploadImage(IFormFile request);
    }
    public class UploadService : IUploadService
    {
        private readonly string ColectionRandomString = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789$_-";
        private readonly int maxWidth = 300;
        private readonly int maxHeigth = 300;

        public string UploadImage(IFormFile fileSend)
        {
            try
            {
                var file = fileSend;
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var fileExt = Path.GetExtension(file.FileName);

                if (file.Length > 0 && IsImage(file))
                {
                    if (!isImageDimensionsValid(file)) throw new BadRequestException($"invalid image dimension, height and width cannot be greater than ({maxWidth}x{maxHeigth})");

                    var fileName = new StringCreator(ColectionRandomString).Get(20);
                    var fullPath = Path.Combine(pathToSave, fileName + fileExt);
                    var dbPath = Path.Combine(folderName, fileName + fileExt);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return dbPath;
                }
                else
                {
                    throw new BadRequestException("Submitted file is not a valid image");
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private static bool IsImage(IFormFile file)
        {
            //-------------------------------------------
            //  Check the image mime types
            //-------------------------------------------
            if (!string.Equals(file.ContentType, "image/jpg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(file.ContentType, "image/jpeg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(file.ContentType, "image/pjpeg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(file.ContentType, "image/x-png", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(file.ContentType, "image/png", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            //-------------------------------------------
            //  Check the image extension
            //-------------------------------------------
            var postedFileExtension = Path.GetExtension(file.FileName);
            if (!string.Equals(postedFileExtension, ".jpg", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".png", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".gif", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".jpeg", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }

        public bool isImageDimensionsValid(IFormFile file)
        {
            using (var myImage = System.Drawing.Image.FromStream(file.OpenReadStream()))
            {
                return (myImage.Height <= maxHeigth && myImage.Width <= maxWidth);
            }
        }
    }
}
