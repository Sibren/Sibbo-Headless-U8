using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sibbo.Headless.Providers;
using System.Web.Http;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace Sibbo.Headless
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class SibboHeadlessComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Components().Insert<AttributeRoutingComponent>();
            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            composition.Register<ContentProvider>(Lifetime.Request);
        }
    }

    public class AttributeRoutingComponent : IComponent
    {
        public void Initialize()
        {
            GlobalConfiguration.Configuration.MapHttpAttributeRoutes();

        }

        public void Terminate()
        {

        }
    }
}
