using Newtonsoft.Json;
using Sibbo.Headless.Models.Backend;
using Sibbo.Headless.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;
using Umbraco.Core.Scoping;

namespace Sibbo.Headless.Providers
{
    public class SettingsProvider
    {
        private readonly IScopeProvider scopeProvider;
        private readonly ILogger logger;

        public SettingsProvider(IScopeProvider scopeProvider, ILogger logger)
        {
            this.scopeProvider = scopeProvider;
            this.logger = logger;
        }

        public SibboSolutionsPropertiesModel GetByName(string name)
        {
            using (var scope = scopeProvider.CreateScope(autoComplete: true))
            {
                var results = scope.Database.Fetch<SibboSolutionsPropertiesModel>();

                if (results != null && results.Any())
                {
                    return results.FirstOrDefault(x => x.Name == name);
                }
                return null;
            }
        }

        public BackendFooterPropertiesModel GetBackendProperties(bool onlySelectedProperties = false)
        {
            var footerPageId = GetByName(SibboHeadlessConstants.FooterDatabaseNameItem);
            var footerSelectedItems = GetByName(SibboHeadlessConstants.FooterPropertiesNameItem);

            var returnModel = new BackendFooterPropertiesModel();
            if (footerPageId == null) return returnModel;

            try
            {
                returnModel.Id = Convert.ToInt32(footerPageId.Value);
                returnModel.Properties = JsonConvert.DeserializeObject<List<BackendPropertyModel>>(footerSelectedItems.Value);
                if (onlySelectedProperties)
                {
                    returnModel.Properties = returnModel.Properties.Where(x => x.Selected).ToList();
                }
                returnModel.Ids.Add(returnModel.Id);
            }
            catch (Exception ex)
            {
                logger.Error<SettingsProvider>(ex);
                return returnModel;
            }

            return returnModel;
        }

        public void SaveFooterProperties(BackendFooterPropertiesModel backendFooterPropertiesModel)
        {
            using (var scope = scopeProvider.CreateScope(autoComplete: true))
            {
                var currentFooterId = scope.Database.Fetch<SibboSolutionsPropertiesModel>().FirstOrDefault(x => x.Name == SibboHeadlessConstants.FooterDatabaseNameItem);

                if (currentFooterId != null)
                {
                    currentFooterId.Value = backendFooterPropertiesModel.Id.ToString();
                    scope.Database.Update(currentFooterId);
                }
                else
                {
                    var footerDatabaseIdItem = new SibboSolutionsPropertiesModel();
                    footerDatabaseIdItem.Name = SibboHeadlessConstants.FooterDatabaseNameItem;
                    footerDatabaseIdItem.Value = backendFooterPropertiesModel.Id.ToString();
                    scope.Database.Insert(footerDatabaseIdItem);
                }

                var currentFooterValues = scope.Database.Fetch<SibboSolutionsPropertiesModel>().FirstOrDefault(x => x.Name == SibboHeadlessConstants.FooterPropertiesNameItem);

                if (currentFooterValues != null)
                {
                    currentFooterValues.Value = JsonConvert.SerializeObject(backendFooterPropertiesModel.Properties);
                    scope.Database.Update(currentFooterValues);
                }
                else
                {
                    var item = new SibboSolutionsPropertiesModel();
                    item.Name = SibboHeadlessConstants.FooterPropertiesNameItem;
                    item.Value = JsonConvert.SerializeObject(backendFooterPropertiesModel.Properties);
                    scope.Database.Insert(item);
                }
            }
        }
    }
}
