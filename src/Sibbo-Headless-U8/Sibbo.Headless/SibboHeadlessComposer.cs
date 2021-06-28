using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sibbo.Headless.Migrations;
using Sibbo.Headless.Providers;
using System.Web.Http;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Logging;
using Umbraco.Core.Migrations;
using Umbraco.Core.Migrations.Upgrade;
using Umbraco.Core.Scoping;
using Umbraco.Core.Services;

namespace Sibbo.Headless
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class SibboHeadlessComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Components().Insert<AttributeRoutingComponent>();
            composition.Components().Insert<DatabseMigrationsComponent>();
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
            composition.Register<DocTypeProvider>(Lifetime.Request);
            composition.Register<SettingsProvider>();
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

    public class DatabseMigrationsComponent : IComponent
    {
        private IScopeProvider _scopeProvider;
        private IMigrationBuilder _migrationBuilder;
        private IKeyValueService _keyValueService;
        private ILogger _logger;
        public DatabseMigrationsComponent(IScopeProvider scopeProvider, IMigrationBuilder migrationBuilder, IKeyValueService keyValueService, ILogger logger)
        {
            _scopeProvider = scopeProvider;
            _migrationBuilder = migrationBuilder;
            _keyValueService = keyValueService;
            _logger = logger;
        }
        public void Initialize()
        {
            var upgrader = new Upgrader(new SibboHeadlessMigrationPlan());
            upgrader.Execute(_scopeProvider, _migrationBuilder, _keyValueService, _logger);
        }

        public void Terminate()
        {
            
        }
    }
}
