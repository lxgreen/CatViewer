using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace CatViewer
{
    public class ImageLoader
    {
        public async Task<Stream> GetImageAsync(string url)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode) return null;

            var stream = response.Content.ReadAsStreamAsync();

            return await stream;
        }
    }
}