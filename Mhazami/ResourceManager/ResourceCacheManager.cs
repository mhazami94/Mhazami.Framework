using System;
using Mhazami.Cache;

namespace Mhazami.ResourceManager
{
    public class ResourceCacheManager : CacheResource
    {
        private string cacheCategoryKey = "ResourceCacheManager";

        public ResourceCacheManager()
        {
            this.AddCategory(cacheCategoryKey, ExpirationTypes.ExpireIfNotUsedAfterTime, new TimeSpan(1, 0, 0, 0));
        }
        private static ResourceCacheManager resourceCacheManager = new ResourceCacheManager();
        public static ResourceCacheManager ResourceCache
        {
            get { return resourceCacheManager; }
        }

        public void AddItem(string key, string culture, string value)
        {
            var theKey = key + "." + culture;
            var item = this[this.cacheCategoryKey, theKey];
            if (item == null)
                this.Add(this.cacheCategoryKey, theKey, value);
            this[this.cacheCategoryKey, theKey] = value;
        }
        public string GetItem(string key, string culture)
        {
            var theKey = key + "." + culture;
            var item = this[this.cacheCategoryKey, theKey];
            if (item == null)
            {
                return "";
            }
            return item.ToString();
        }

        public void RemoveItem(string key, string culture)
        {
            var theKey = key + "." + culture;
            this.Remove(this.cacheCategoryKey, theKey);
        }

    }
}