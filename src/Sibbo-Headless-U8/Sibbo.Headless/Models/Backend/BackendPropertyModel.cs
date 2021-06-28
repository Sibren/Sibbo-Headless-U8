using Newtonsoft.Json;

namespace Sibbo.Headless.Models.Backend
{
    public class BackendPropertyModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("selected")]
        public bool Selected { get; set; }

        public BackendPropertyModel()
        {

        }

        public BackendPropertyModel(string name, string id)
        {
            Name = name;
            Id = id;
        }
    }
}
