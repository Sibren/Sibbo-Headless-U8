using Newtonsoft.Json;
using System.Collections.Generic;

namespace Sibbo.Headless.Models.Backend
{
    public class BackendFooterPropertiesModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("ids")]
        public List<int> Ids { get; set; } = new List<int>();

        [JsonProperty("properties")]
        public List<BackendPropertyModel> Properties { get; set; } = new List<BackendPropertyModel>();
    }
}
