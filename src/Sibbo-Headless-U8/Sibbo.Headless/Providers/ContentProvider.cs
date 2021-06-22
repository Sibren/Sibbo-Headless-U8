using Sibbo.Headless.Models;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Core.Models.Blocks;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace Sibbo.Headless.Providers
{
    public class ContentProvider
    {
        private readonly IUmbracoContextFactory umbracoContextFactory;

        public ContentProvider(IUmbracoContextFactory umbracoContextFactory)
        {
            this.umbracoContextFactory = umbracoContextFactory;
        }

        public IPublishedContent GetPage(int id)
        {
            using (var umbracoContext = umbracoContextFactory.EnsureUmbracoContext())
            {
                var item = umbracoContext.UmbracoContext.Content.GetById(id);
                return item;
            }
        }

        public IPublishedContent GetPage(string url)
        {
            using (var umbracoContext = umbracoContextFactory.EnsureUmbracoContext())
            {
                var item = umbracoContext.UmbracoContext.Content.GetByRoute(url);
                return item;
            }
        }

        public List<MenuItemModel> GetMenuItems()
        {
            var list = new List<MenuItemModel>();
            using (var umbracoContext = umbracoContextFactory.EnsureUmbracoContext())
            {
                var rootMenuItem = umbracoContext.UmbracoContext.Content.GetAtRoot().First();
                foreach (var item in rootMenuItem.Children.Where(x => x.IsVisible()))
                {
                    list.Add(new MenuItemModel(item));
                }
            }

            return list;
        }

        /// <summary>
        /// Can be used for Nuxt or something
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllRoutes()
        {
            var blogOverviewPage = GetPage("/");
            return blogOverviewPage.DescendantsOrSelf().Select(x => x.Url()).ToList();
        }

        public Dictionary<string, object> GetPageContent(IPublishedContent publishedContent)
        {
            var dictionary = GetSettings(publishedContent.Id, publishedContent.Url(), publishedContent.Properties, publishedContent.ContentType.Alias);

            return dictionary;
        }

        public Dictionary<string, object> GetSettings(int id, string url, IEnumerable<IPublishedProperty> publishedProperties, string docType = "")
        {
            var dictionary = new Dictionary<string, object>();
            if (publishedProperties == null || !publishedProperties.Any()) return dictionary;

            foreach (var publishedProperty in publishedProperties)
            {
                var propertyType = publishedProperty.PropertyType.EditorAlias;
                if (propertyType == "Umbraco.MediaPicker3")
                {
                    if (publishedProperty.GetValue().GetType() == typeof(List<MediaWithCrops>))
                    {
                        var objectValues = (List<MediaWithCrops>)publishedProperty.GetValue();
                        var myData = new List<PublishedContentItem>();
                        foreach (var item in objectValues)
                        {
                            myData.Add(new PublishedContentItem(item));
                        }
                        dictionary.Add(publishedProperty.Alias, myData);
                    }
                    else if (publishedProperty.GetValue().GetType() == typeof(MediaWithCrops))
                    {
                        dictionary.Add(publishedProperty.Alias, new PublishedContentItem((MediaWithCrops)publishedProperty.GetValue()));
                    }
                }
                else if (propertyType == "Umbraco.ContentPicker")
                {
                    dictionary.Add(publishedProperty.Alias, new PublishedContentItem((IPublishedContent)publishedProperty.GetValue()));
                }
                else if (propertyType == "Umbraco.MultiNodeTreePicker")
                {
                    var objectValues = (IEnumerable<IPublishedContent>)publishedProperty.GetValue();
                    var myData = new List<PublishedContentItem>();
                    foreach (var item in objectValues)
                    {
                        myData.Add(new PublishedContentItem(item));
                    }
                    dictionary.Add(publishedProperty.Alias, myData);
                }
                else if (propertyType == "Umbraco.MultiUrlPicker")
                {
                    if (publishedProperty.GetValue().GetType() == typeof(IEnumerable<Link>))
                    {
                        var objectValues = (IEnumerable<Link>)publishedProperty.GetValue();
                        var myData = new List<MultiUrlPickerModel>();
                        foreach (var item in objectValues)
                        {
                            myData.Add(new MultiUrlPickerModel(item));
                        }
                        dictionary.Add(publishedProperty.Alias, myData);
                    }
                    else if (publishedProperty.GetValue().GetType() == typeof(Link))
                    {
                        dictionary.Add(publishedProperty.Alias, new MultiUrlPickerModel((Link)publishedProperty.GetValue()));
                    }
                }
                else if (propertyType == "Umbraco.TinyMCE")
                {
                    var value = publishedProperty.GetValue();
                    dictionary.Add(publishedProperty.Alias, value.ToString());
                }
                else if (propertyType == "Umbraco.Label")
                {
                    // do nothing with a label
                }
                else if (propertyType == "Umbraco.BlockList")
                {
                    var originalItems = (IEnumerable<BlockListItem>)publishedProperty.GetValue();
                    var customDictionary = new Dictionary<string, object>();
                    foreach (var blockListItem in originalItems)
                    {
                        var blockListItemDictionary = new Dictionary<string, object>();
                        blockListItemDictionary.Add("docType", blockListItem.Content.ContentType.Alias);
                        blockListItemDictionary.Add("content", GetSettings(0, "", blockListItem.Content?.Properties));
                        blockListItemDictionary.Add("settings", GetSettings(0, "", blockListItem.Settings?.Properties));
                        customDictionary.Add(blockListItem.Content.ContentType.Alias, blockListItemDictionary);
                    }

                    dictionary.Add(publishedProperty.Alias, customDictionary);
                }
                else
                {
                    dictionary.Add(publishedProperty.Alias, publishedProperty.GetValue());
                }
            }

            if (!string.IsNullOrEmpty(docType) && !dictionary.ContainsKey("docType"))
            {
                dictionary.Add("docType", docType);
            }

            if (id > 0)
            {
                dictionary.Add("id", id);
            }

            if (!string.IsNullOrEmpty(url))
            {
                dictionary.Add("url", url);
            }

            return dictionary;
        }
    }
}
