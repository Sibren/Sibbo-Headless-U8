using Umbraco.Web.Models;

namespace Sibbo.Headless.Models
{
    public class MultiUrlPickerModel
    {
        public string Url { get; set; }

        public string Name { get; set; }

        public string Target { get; set; }

        public MultiUrlPickerModel()
        {

        }

        public MultiUrlPickerModel(Link link)
        {
            Url = link.Url;
            Name = link.Name;
            Target = link.Target;
        }
    }
}
