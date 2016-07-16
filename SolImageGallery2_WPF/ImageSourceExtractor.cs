using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace CatViewer
{
    public class ImageSourceExtractor
    {
        public IEnumerable<string> ExtractImageURLs(string json)
        {
            var searchResult = JObject.Parse(json);

            return searchResult.SelectTokens("$..thumbnailUrl")
                .Select(url => url.Value<string>());
        }
    }
}