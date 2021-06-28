using Newtonsoft.Json;
using Sibbo.Headless.Models;
using Sibbo.Headless.Providers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Umbraco.Core.Logging;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.WebApi;

namespace Sibbo.Headless.Controllers
{
    [RoutePrefix("sibbo-headless")]
    public class SibboContentApiController : UmbracoApiController
    {
        private readonly ContentProvider contentProvider;
        private readonly SettingsProvider settingsProvider;

        public SibboContentApiController(ContentProvider contentService, SettingsProvider settingsProvider)
        {
            this.contentProvider = contentService;
            this.settingsProvider = settingsProvider;
        }

        [HttpGet]
        [Route("routes")]
        public List<string> GetAllRoutes()
        {
            return contentProvider.GetAllRoutes();
        }

        [HttpGet]
        [Route("menu")]
        public List<MenuItemModel> GetMenuItems()
        {
            return contentProvider.GetMenuItems();
        }

        [HttpGet]
        [Route("id/{id}")]
        public string GetContentById(int id)
        {
            var item = contentProvider.GetPage(id);
            return GetPageContent(item);

        }

        [HttpGet]
        [Route("footer")]
        public string GetFooter()
        {
            var footer = settingsProvider.GetBackendProperties(true);
            if (footer == null)
            {
                return "";
            }

            return GetPageContent(footer.Id, footer.Properties.Select(x => x.Name).ToList());
        }

        [HttpGet]
        [Route("url")]
        public string GetContentByUrl()
        {
            var item = contentProvider.GetPage("/");
            return GetPageContent(item);
        }

        [HttpGet]
        [Route("url/{url}")]
        public string GetContentByUrl(string url)
        {
            var item = contentProvider.GetPage("/" + url.Replace("-_slash_-", "/").TrimEnd('/'));
            if (item == null) throw new System.ArgumentNullException("Page not found");

            return GetPageContent(item);
        }

        private string GetPageContent(IPublishedContent publishedContent)
        {
            try
            {
                var content = contentProvider.GetPageContent(publishedContent);
                return JsonConvert.SerializeObject(content);
            }
            catch (System.Exception ex)
            {
                Logger.Error<SibboContentApiController>("Error getting content", ex);
                throw new System.Exception("Error getting values");
            }
        }

        private string GetPageContent(int id, List<string> properties = null)
        {
            try
            {
                var content = contentProvider.GetPageContent(id, properties);
                return JsonConvert.SerializeObject(content);
            }
            catch (System.Exception ex)
            {
                Logger.Error<SibboContentApiController>("Error getting content", ex);
                throw new System.Exception("Error getting values");
            }
        }
    }
}
