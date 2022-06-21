using System;

namespace Mhazami.Cache
{
    public enum ExpirationTypes
    {
        NeverExpire,
        ExpireIfNotUsedAfterTime,
        ExpireAfterExactTime
    }

    public class CacheInfo
    {
        internal CacheInfo(object data, ExpirationTypes expirationType, TimeSpan validationTime)
        {
            this.Data = data;
            this.ValidationTime = validationTime;
            this.ExpirationType = expirationType;
            this.LastUsageDate = DateTime.Now;
        }

        public object Data { get; set; }

        public DateTime LastUsageDate { get; set; }

        public TimeSpan ValidationTime { get; set; }

        public ExpirationTypes ExpirationType { get; set; }
    }
}
