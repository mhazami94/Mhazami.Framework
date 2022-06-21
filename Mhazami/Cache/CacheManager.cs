using System;
using System.Collections.Generic;
using System.Threading;

namespace Mhazami.Cache
{
    public class CacheManager : CacheResource
    {
        private CacheManager()
        {
            var thread = new Thread(CleanUp) { Name = "CacheCleanup" };
            thread.Start();
        }

        private static CacheManager _instance;
        public static CacheManager Instance
        {
            get { return _instance ?? (_instance = new CacheManager()); }
        }

        public void CleanUp()
        {
            while (true)
            {
                var categoryMustRemove = new List<string>();
                var dataMustRemove = new Dictionary<string, string>();

                var categoryInfos = this.Resource();
                foreach (var key in categoryInfos.Keys)
                {
                    switch (categoryInfos[key].ExpirationType)
                    {
                        case ExpirationTypes.NeverExpire:
                            break;
                        case ExpirationTypes.ExpireIfNotUsedAfterTime:
                        case ExpirationTypes.ExpireAfterExactTime:
                            if (categoryInfos[key].LastUsageDate + categoryInfos[key].ValidationTime < DateTime.Now)
                                categoryMustRemove.Add(key);

                            break;
                    }
                }

                foreach (var item in categoryMustRemove)
                    this.RemoveCategory(item);


                foreach (var key in categoryInfos.Keys)
                {

                    foreach (var infos in this.GetCategoryCache(key))
                    {
                        var cacheInfo = infos.Value;
                        switch (cacheInfo.ExpirationType)
                        {
                            case ExpirationTypes.NeverExpire:
                                break;
                            case ExpirationTypes.ExpireIfNotUsedAfterTime:
                            case ExpirationTypes.ExpireAfterExactTime:
                                if (cacheInfo.LastUsageDate + cacheInfo.ValidationTime < DateTime.Now)
                                    dataMustRemove.Add(key, infos.Key);

                                break;
                        }
                    }
                }
                foreach (var item in dataMustRemove)
                    this.Remove(item.Key, item.Value);

                 Thread.Sleep(1000 * 1 * 60);
            }
        }

    }
}
