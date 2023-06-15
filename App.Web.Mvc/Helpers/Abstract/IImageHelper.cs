namespace App.Web.Mvc.Helpers.Abstract
{
	public interface IImageHelper
	{
		Task<string> ImageUpload(string imageName, IFormFile pictureFile, string folderName);
		Task<bool> ImageDelete(string imagePath, string folderName);
	}
}
