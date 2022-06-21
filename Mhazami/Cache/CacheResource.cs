using System;
using System.Collections.Generic;

namespace Mhazami.Cache
{
    public class CacheResource
    {
        private readonly Dictionary<string, CategoryInfo> resource = new Dictionary<string, CategoryInfo>();

        #region Resource

        internal Dictionary<string, CategoryInfo> Resource()
        {
            return this.resource;
        }

        internal Dictionary<string, CategoryInfo> Resource(string category)
        {
            if (!this.resource.ContainsKey(category))
            {

                lock (resource)
                {
                    this.resource.Add(category, new CategoryInfo(category, ExpirationTypes.ExpireIfNotUsedAfterTime, new TimeSpan(0, 1, 0, 0)));
                }
            }
            lock (resource)
            {
                this.resource[category].LastUsageDate = DateTime.Now;
            }
            return this.resource;
        }

        internal Dictionary<string, CategoryInfo> Resource(string category, ExpirationTypes expirationTypes, TimeSpan validationTime)
        {
            if (!this.resource.ContainsKey(category))
            {
                lock (resource)
                {
                    this.resource.Add(category, new CategoryInfo(category, expirationTypes, validationTime));
                }
            }
            lock (resource)
            {
                this.resource[category].LastUsageDate = DateTime.Now;
            }
            return this.resource;
        }

        internal Dictionary<string, CategoryInfo> Resource(string category, string key)
        {
            if (!this.resource.ContainsKey(category))
            {
                lock (resource)
                {
                    if (resource.ContainsKey(category))
                        this.resource[category] = new CategoryInfo(category, ExpirationTypes.ExpireIfNotUsedAfterTime, new TimeSpan(0, 1, 0, 0));
                    else
                        this.resource.Add(category, new CategoryInfo(category, ExpirationTypes.ExpireIfNotUsedAfterTime, new TimeSpan(0, 1, 0, 0)));
                }
            }
            var categoryInfo = this.resource[category];
            if (!categoryInfo.Data.ContainsKey(key))
            {
                lock (resource)
                {
                    if (categoryInfo.Data.ContainsKey(key))
                        categoryInfo.Data[key] = new CacheInfo(null, categoryInfo.ExpirationType,
                                                               categoryInfo.ValidationTime);
                    else
                        categoryInfo.Data.Add(key,
                                              new CacheInfo(null, categoryInfo.ExpirationType,
                                                            categoryInfo.ValidationTime));
                }
            }
            lock (resource)
            {
                categoryInfo.LastUsageDate = DateTime.Now;
                categoryInfo.Data[key].LastUsageDate = DateTime.Now;
            }
            return this.resource;
        }

        internal Dictionary<string, CategoryInfo> Resource(string category, string key, ExpirationTypes expirationTypes, TimeSpan validationTime)
        {
            if (!this.resource.ContainsKey(category))
            {
                lock (resource)
                {
                    this.resource.Add(category, new CategoryInfo(category, expirationTypes, validationTime));
                }
            }
            var categoryInfo = this.resource[category];
            if (!categoryInfo.Data.ContainsKey(key))
            {
                lock (resource)
                {
                    categoryInfo.Data.Add(key, new CacheInfo(null, expirationTypes, validationTime));
                }
            }
            lock (resource)
            {
                categoryInfo.LastUsageDate = DateTime.Now;
                categoryInfo.Data[key].LastUsageDate = DateTime.Now;
            }
            return this.resource;
        }

        #endregion

        #region Category

        public void AddCategory(string category)
        {
            this.Resource(category);
        }

        public void AddCategory(string category, ExpirationTypes expirationTypes, TimeSpan validationTime)
        {
            this.Resource(category, expirationTypes, validationTime);
        }

        public void RemoveCategory(string category)
        {
            if (this.resource.ContainsKey(category))
                lock (resource)
                {
                    this.resource.Remove(category);
                }
        }

        public void ClearCategory(string category)
        {
            if (this.resource.ContainsKey(category))
                lock (resource)
                {
                    this.resource[category].Data.Clear();
                }
        }

        public bool ExistsCategory(string category)
        {
            if (this.resource.ContainsKey(category))
                throw new Exception("Cache manager contains same category name : " + category);
            return false;
        }

        public CategoryInfo GetCategory(string category)
        {
            return this.Resource(category)[category];
        }

        public Dictionary<string, CacheInfo> GetCategoryCache(string category)
        {
            return this.Resource(category)[category].Data;
        }

        #endregion

        #region CachedInformation

        public void Add(string category, string key, object value)
        {
            lock (resource)
            {
                this.Resource(category, key)[category].Data[key].Data = value;
            }
        }

        public void Add(string category, string key, object value, ExpirationTypes expirationTypes, TimeSpan validationTime)
        {
            lock (resource)
            {
                this.Resource(category, key, expirationTypes, validationTime)[category].Data[key].Data = value;
            }
        }

        public bool Remove(string category, string key)
        {
            if (this.resource.ContainsKey(category) && this.resource[category].Data.ContainsKey(key))
                lock (resource)
                {
                    this.resource[category].Data.Remove(key);
                    if (this.resource[category].Data.Count == 0)
                        return this.resource.Remove(category);
                }
            return true;
        }

        #endregion

        public object this[string category, string key]
        {
            get
            {
                return this.Resource(category, key)[category].Data[key].Data;
            }
            set
            {
                lock (resource)
                {
                    this.Resource(category, key)[category].Data[key].Data = value;
                }
            }
        }
    }
}
