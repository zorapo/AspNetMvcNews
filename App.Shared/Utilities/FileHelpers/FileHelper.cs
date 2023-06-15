using Microsoft.AspNetCore.Http;

namespace App.Shared.Utilities.FileHelpers
{
	public class FileHelper
    {
        public static async Task<string> ImageUpload(IFormFile formFile, string filePath = "~/wwwroot/")
        {
            var fileName = "";

            if (formFile != null && formFile.Length > 0)
            {
                fileName = formFile.FileName;
                string directory = Directory.GetCurrentDirectory() + filePath + fileName;
                using var stream = new FileStream(directory, FileMode.Create);
                await formFile.CopyToAsync(stream);
            }

            return fileName;
        }

        public static bool FileRemover(string fileName, string filePath = "/wwwroot/")
        {
            string directory = Directory.GetCurrentDirectory() + filePath + fileName;

            if (File.Exists(directory))
            {
                File.Delete(directory);
                return true;
            }

            return false;
        }
    }
}
