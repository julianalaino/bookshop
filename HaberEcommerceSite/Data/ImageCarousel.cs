using Microsoft.AspNetCore.Http;

namespace HaberEcommerceSite.Data
{
    public class ImageCarousel
    {
        public IFormFile Image { get; set; }

        public string contentPath { get; set; }

        public string Name { get; set; }
    }
}
