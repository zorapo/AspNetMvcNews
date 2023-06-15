using App.Web.Mvc.Helpers.Abstract;

namespace App.Web.Mvc.Helpers.Concrete
{
	public class ImageHelper : IImageHelper
	{
		private readonly IWebHostEnvironment _env;
		private readonly string _wwwroot;
		private readonly string _imgFolder = "img";
		public ImageHelper(IWebHostEnvironment env)
		{
			_env = env;
			_wwwroot = _env.WebRootPath;

		}


		public async Task<string> ImageUpload(string imageName,IFormFile pictureFile, string folderName)
		{
			if (!Directory.Exists($"{_wwwroot}/{_imgFolder}/{folderName}"))
			{
				Directory.CreateDirectory($"{_wwwroot}/{_imgFolder}/{folderName}");
			}
			
			string fileExtension=Path.GetExtension(pictureFile.FileName);
			string fileName= $"{imageName}_{Guid.NewGuid()}_{fileExtension}";
			var path = Path.Combine($"{_wwwroot}/{_imgFolder}/{folderName}", fileName);
			await using(var stream= new FileStream(path, FileMode.Create))
			{

				await pictureFile.CopyToAsync(stream);
			}
			return fileName;
		}	
		public async Task<bool> ImageDelete(string imagePath,string folderName)
		{
			var fileToDelete = Path.Combine($"{_wwwroot}/{_imgFolder}/{folderName}", imagePath);
			if (File.Exists(fileToDelete))
			{
				await Task.Run(() => { File.Delete(fileToDelete); });
				return true;
			}
			return false;
		}
	}
}
