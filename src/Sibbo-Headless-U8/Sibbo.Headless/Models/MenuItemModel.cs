using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Sibbo.Headless.Models
{
    public class MenuItemModel
    {
        /// <summary>
        /// Uses the name of the page or a property 'pageTitle'
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets the UrlSegment of the page
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        public MenuItemModel()
        {

        }

        public MenuItemModel(IPublishedContent content)
        {
            Name = !string.IsNullOrWhiteSpace(content.Value<string>("pageTitle")) ? content.Value<string>("pageTitle") : content.Name;
            Url = content.UrlSegment;
        }
    }
}
