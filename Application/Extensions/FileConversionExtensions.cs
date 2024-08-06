using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public static class FileConversionExtensions
    {
        public static async Task<IFormFile> ToFormFileAsync(this IBrowserFile browserFile)
        {
            var memoryStream = new MemoryStream();
            await browserFile.OpenReadStream().CopyToAsync(memoryStream);
            return new FormFile(memoryStream, 0, browserFile.Size, browserFile.Name, browserFile.Name)
            {
                Headers = new HeaderDictionary(),
                ContentType = browserFile.ContentType
            };
        }
    }
}
