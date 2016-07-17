using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace CatViewer
{
    public class ImageSourceExtractor
    {
        public IEnumerable<string> ExtractImageUrls(string json)
        {
            var searchResult = JObject.Parse(json);

            return searchResult.SelectTokens("$..thumbnailUrl")
                .Select(url => url.Value<string>());
        }
    }
}