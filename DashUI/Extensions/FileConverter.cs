using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Http;

namespace DashUI.Extensions
{
    public static class FileConverter
    {
        public static IFormFile ToIFormFileFromBase64String(this string base64)
        {
            var stream = new MemoryStream();
            var bytes = Convert.FromBase64String(base64.Trim().Split(",")[1]);

            stream.Write(bytes);
            stream.Position = 0;
            return new FormFile(stream, 0, stream.Length, "file.png", "file-name.png");
        }
    }
}
