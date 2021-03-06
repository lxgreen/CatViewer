﻿using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace CatViewer
{
    public class ImageLoader
    {
        public async Task<Stream> GetImageStreamAsync(string url)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode) return null;

                return await response.Content.ReadAsStreamAsync();
            }
        }
    }
}