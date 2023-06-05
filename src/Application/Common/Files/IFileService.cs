using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace mentor_v1.Application.Common.Files;
public interface IFileService
{
    public string SaveImage(IFormFile imageFile);
    public bool DeleteImage(string imageFileName);
    public string CovertToBase64(string imageFileName);


}
