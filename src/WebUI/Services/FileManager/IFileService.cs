namespace WebUI.Services.FileManager;

public interface IFileService
{
    public string SaveImage(IFormFile imageFile);
    public bool DeleteImage(string imageFileName);

    public string CovertToBase64(string imageFileName);

    public string SaveImagePdf(IFormFile imageFile);

    public string CovertToBase64Pdf(string imageFileName);

}
