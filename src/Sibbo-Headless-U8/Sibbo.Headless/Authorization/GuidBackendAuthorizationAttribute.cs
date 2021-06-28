using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Sibbo.Headless.Authorization
{
    public class GuidBackendAuthorizationAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var request = HttpContext.Current.Request;
            var authHeader = request.Headers["Authorization"];
            if (authHeader == null) return false;

            return true;
        }
    }
}
