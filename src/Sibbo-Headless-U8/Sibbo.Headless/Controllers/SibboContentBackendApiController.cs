using Sibbo.Headless.Models.Backend;
using Sibbo.Headless.Providers;
using System.Collections.Generic;
using System.Web.Http;
using Umbraco.Web.WebApi;

namespace Sibbo.Headless.Controllers
{
    [RoutePrefix("sibbo-headless-backend")]
    public class SibboContentBackendApiController : UmbracoApiController
    {
        private readonly ContentProvider contentProvider;
        private readonly DocTypeProvider docTypeProvider;
        private readonly SettingsProvider settingsProvider;

        public SibboContentBackendApiController(ContentProvider contentService, DocTypeProvider docTypeProvider, SettingsProvider settingsProvider)
        {
            this.contentProvider = contentService;
            this.docTypeProvider = docTypeProvider;
            this.settingsProvider = settingsProvider;
        }

        [HttpGet]
        [Route("getproperties")]
        public List<BackendPropertyModel> GetProperties(int docTypeId)
        {
            return docTypeProvider.GetAllPropertiesForDoctype(docTypeId);
        }

        [HttpPost]
        [Route("setfooterproperties")]
        public void SetFooterProperties(BackendFooterPropertiesModel model)
        {
            settingsProvider.SaveFooterProperties(model);
        }

        [HttpGet]
        [Route("getfooterproperties")]
        public BackendFooterPropertiesModel GetFooterProperties()
        {
            return settingsProvider.GetBackendProperties();
        }
    }
}
