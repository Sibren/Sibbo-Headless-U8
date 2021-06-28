using Sibbo.Headless.Authorization;
using System.Web.Http;
using Umbraco.Web.WebApi;

namespace Sibbo.Headless.Controllers
{
    [RoutePrefix("sibbo-headless-post")]
    public class SibboPostApiController : UmbracoApiController
    {
        [GuidBackendAuthorizationAttribute]
        [HttpPost]
        [Route("postform")]
        public void PostForm()
        {

        }
    }
}
