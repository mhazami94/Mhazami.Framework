using System;
using System.Collections.Generic;

namespace Mhazami.Cache
{
    public class CategoryInfo
    {
        public CategoryInfo(string name, ExpirationTypes expirationTypes, TimeSpan validationTime)
        {
            this.Data = new Dictionary<string, CacheInfo>();
            this.Name = name;
            this.ExpirationType = expirationTypes;
            this.ValidationTime = validationTime;
            this.LastUsageDate = DateTime.Now;
        }

        public string Name { get; set; }

        public Dictionary<string, CacheInfo> Data { get; set; }

        public DateTime LastUsageDate { get; set; }

        public TimeSpan ValidationTime { get; set; }

        public ExpirationTypes ExpirationType { get; set; }

    }
}
