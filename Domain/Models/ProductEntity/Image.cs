using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ProductEntity
{
    public class Image
    {
        public string imageId { get; set; } = null!;

        public string Url { get; set; } = null!;

        public string AbsolutePath { get; set; } = null!;
    }
}
