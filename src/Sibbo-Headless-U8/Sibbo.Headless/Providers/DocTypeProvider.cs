using Sibbo.Headless.Models.Backend;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Web;

namespace Sibbo.Headless.Providers
{
    public class DocTypeProvider
    {
        private readonly IUmbracoContextFactory umbracoContextFactory;

        public DocTypeProvider(IUmbracoContextFactory umbracoContextFactory)
        {
            this.umbracoContextFactory = umbracoContextFactory;
        }

        public List<BackendPropertyModel> GetAllPropertiesForDoctype(int docTypeId)
        {
            var dictionary = new List<BackendPropertyModel>();
            using (var umbracoContext = umbracoContextFactory.EnsureUmbracoContext())
            {
                var item = umbracoContext.UmbracoContext.Content.GetContentType(docTypeId);
                foreach (var property in item.PropertyTypes)
                {
                    dictionary.Add(new BackendPropertyModel(property.Alias, property.Alias));
                }
                
            }

            return dictionary;
        }
    }
}
