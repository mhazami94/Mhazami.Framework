namespace Mhazami.Cache
{
    public class CacheCategoryManager
    {
        protected CacheManager CacheManager = CacheManager.Instance;

        public string CategoryName { get; set; }

        public CategoryInfo CategoryInfo
        {
            get
            {
                return this.CacheManager.GetCategory(this.CategoryName);
            }
        }

        public object this[string key]
        {
            get
            {
                return this.CacheManager[this.CategoryName, key];
            }
            set
            {
                this.CacheManager[this.CategoryName, key] = value;
            }
        }
        public Dictionary<string, object> ValidData
        {
            get
            {
                if (this.CategoryInfo == null || this.CategoryInfo.Data == null) return new Dictionary<string, object>();
                return
                    this.CategoryInfo.Data.Where(x => x.Value != null && x.Value.Data != null)
                        .ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value.Data);
            }
        }
        public void Add(string key, object data)
        {
            this.CacheManager.Add(this.CategoryName, key, data, this.CategoryInfo.ExpirationType, CategoryInfo.ValidationTime);
        }

        public void Add(string key, object data, ExpirationTypes expirationTypes, TimeSpan validationTime)
        {
            this.CacheManager.Add(this.CategoryName, key, data, expirationTypes, validationTime);
        }

        public bool Remove(string key)
        {
            return this.CacheManager.Remove(this.CategoryName, key);
        }

        public void Clear()
        {
            this.CacheManager.ClearCategory(this.CategoryName);
        }
    }
}
