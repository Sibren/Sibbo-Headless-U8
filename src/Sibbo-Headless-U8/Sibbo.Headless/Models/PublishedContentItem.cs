using Newtonsoft.Json;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Sibbo.Headless.Models
{
    public class PublishedContentItem
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public PublishedContentItem()
        {

        }

        public PublishedContentItem(MediaWithCrops mediaWithCrops)
        {
            Url = mediaWithCrops.MediaItem.Url(mode: UrlMode.Absolute);
            Name = mediaWithCrops.MediaItem.Name;
        }

        public PublishedContentItem(IPublishedContent publishedContent)
        {
            Url = publishedContent.Url(mode: UrlMode.Absolute);
            Name = publishedContent.Name;
        }
    }
}
