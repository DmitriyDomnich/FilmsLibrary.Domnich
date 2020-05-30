using System;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace Web.Utils
{
    public static class ImageHelper
    {
        public static string UploadImage(IFormFile formFile, string path)
        {
            var fileName = ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName.Trim('"');
            var myUniqueFileName = Convert.ToString(Guid.NewGuid());
            var FileExtension = Path.GetExtension(fileName);
            var newFileName = myUniqueFileName + FileExtension;
            fileName = path + $@"\{newFileName}";

            using (FileStream fs = File.Create(fileName))
            {
                formFile.CopyTo(fs);
                fs.Flush();
            }

            return newFileName;
        }
    }
}
