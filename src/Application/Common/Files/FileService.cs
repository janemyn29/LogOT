using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace mentor_v1.Application.Common.Files;
public class FileService : IFileService
{
    private IWebHostEnvironment environment;
    public FileService(IWebHostEnvironment env)
    {
        this.environment = env;
    }

    public string SaveImage(IFormFile imageFile)
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
}
