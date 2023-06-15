using Microsoft.AspNetCore.StaticFiles;
using WebUI.Helper;

namespace WebUI.Services.FileManager;

public class FileService : IFileService
{
    private IWebHostEnvironment environment;
    public FileService(IWebHostEnvironment env)
    {
        this.environment = env;
    }

    public  string SaveImage(IFormFile imageFile)
    {
        try
        {
            var contentPath = this.environment.ContentRootPath;
            // path = "c://projects/productminiapi/uploads" ,not exactly something like that
            var path = Path.Combine(contentPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // Check the allowed extenstions
            var ext = Path.GetExtension(imageFile.FileName);
            var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
            if (!allowedExtensions.Contains(ext))
            {
                string msg = string.Format("Only {0} extensions are allowed", string.Join(",", allowedExtensions));
                throw new("Tải hình ảnh đã xảy ra lỗi!");
            }
            string uniqueString = Guid.NewGuid().ToString();
            // we are trying to create a unique filename here
            var newFileName = uniqueString + ext;
            var fileWithPath = Path.Combine(path, newFileName);
            var stream = new FileStream(fileWithPath, FileMode.Create);
            imageFile.CopyTo(stream);
            stream.Close();
            return newFileName;
        }
        catch (Exception ex)
        {
            throw new("Tải hình ảnh đã xảy ra lỗi!");
        }
    }

    public bool DeleteImage(string imageFileName)
    {
        try
        {
            var wwwPath = this.environment.WebRootPath;
            var path = Path.Combine(wwwPath, "Uploads\\", imageFileName);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public string CovertToBase64(string imageFileName)
    {
        try
        {
            var wwwPath = this.environment.WebRootPath;
            var path = Path.Combine(wwwPath, "Uploads\\", imageFileName);
            if (System.IO.File.Exists(path))
            {
                string base64 = File.ReadAllText(path);
                return base64;
            }
            return null;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<string> UploadFile(IFormFile _IFormFile)
    {
        string FileName = "";
        try
        {
            FileInfo _FileInfo = new FileInfo(_IFormFile.FileName);
            FileName =_FileInfo.Name+ "_"+ Guid.NewGuid().ToString() + _FileInfo.Extension;
            var _GetFilePath = Common.GetFilePath(FileName);
            using (var _FileStream = new FileStream(_GetFilePath, FileMode.Create))
            {
                await _IFormFile.CopyToAsync(_FileStream);
            }
            return FileName;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public async Task<(byte[], string, string)> DownloadFile(string FileName)
    {
        try
        {
            var _GetFilePath = Common.GetFilePath(FileName);
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(_GetFilePath, out var _ContentType))
            {
                _ContentType = "application/octet-stream";
            }
            var _ReadAllBytesAsync = await File.ReadAllBytesAsync(_GetFilePath);
            return (_ReadAllBytesAsync, _ContentType, Path.GetFileName(_GetFilePath));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}