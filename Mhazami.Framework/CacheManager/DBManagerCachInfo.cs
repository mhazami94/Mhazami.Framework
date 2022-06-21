using Mhazami.Framework.CacheManager;
using Mhazami.Utility;

namespace Mhazami.Framework
{

    public static class DBManagerCachInfo
    {
        public static bool RemoveCash(string key, CachType type)
        {
            switch (type)
            {

                case CachType.AssosiationModelCache:
                    return AssosiationModelCache.StorageCache.Remove(key);
                case CachType.TypeProperties:
                    return TypePropertiesCache.StorageCache.Remove(key);
                case CachType.TypeFields:
                    return TypeFieldsCache.StorageCache.Remove(key);
                case CachType.TypeBOClassess:
                    return TypeBOClassessCache.StorageCache.Remove(key);
                case CachType.PropertyAttributeModel:
                    return PropertyAttributeModelCache.StorageCache.Remove(key);
                case CachType.ClassAttributeModel:
                    return ClassAttributeModelCache.StorageCache.Remove(key);
               case CachType.HasTable:
                    return HasTableCashCache.StorageCache.Remove(key);
                case CachType.AggregationCache:
                    return AggregationModelCache.StorageCache.Remove(key);
            }
            return false;


        }
        public static void ClearCash(CachType type)
        {
            switch (type)
            {

                case CachType.AssosiationModelCache:
                    AssosiationModelCache.StorageCache.Clear();
                    break;
                case CachType.TypeProperties:
                    TypePropertiesCache.StorageCache.Clear();
                    break;
                case CachType.TypeFields:
                    TypeFieldsCache.StorageCache.Clear();
                    break;
                case CachType.TypeBOClassess:
                    TypeBOClassessCache.StorageCache.Clear();
                    break;
                case CachType.PropertyAttributeModel:
                    PropertyAttributeModelCache.StorageCache.Clear();
                    break;
                case CachType.ClassAttributeModel:
                    ClassAttributeModelCache.StorageCache.Clear();
                    break;
             
                case CachType.HasTable:
                    HasTableCashCache.StorageCache.Clear();
                    break;
                case CachType.AggregationCache:
                    AggregationModelCache.StorageCache.Clear();
                    break;
            }

        }
        public static bool ContainKey(string key, CachType type)
        {
            switch (type)
            {

                case CachType.AssosiationModelCache:
                    return AssosiationModelCache.StorageCache[key] != null;
                case CachType.TypeProperties:
                    return TypePropertiesCache.StorageCache[key] != null;
                case CachType.TypeFields:
                    return TypeFieldsCache.StorageCache[key] != null;
                case CachType.TypeBOClassess:
                    return TypeBOClassessCache.StorageCache[key] != null;
                case CachType.PropertyAttributeModel:
                    return PropertyAttributeModelCache.StorageCache[key] != null;
                case CachType.ClassAttributeModel:
                    return ClassAttributeModelCache.StorageCache[key] != null;
                case CachType.HasTable:
                    return HasTableCashCache.StorageCache[key] != null;
                case CachType.AggregationCache:
                   return AggregationModelCache.StorageCache[key] != null;
                  
            }
            return false;


        }
        public static long CashDataCount(CachType type)
        {
            switch (type)
            {
                case CachType.None:
                    break;
                case CachType.AssosiationModelCache:
                    return AssosiationModelCache.StorageCache.ValidData.Count;
                case CachType.TypeProperties:
                    return TypePropertiesCache.StorageCache.ValidData.Count;
                case CachType.TypeFields:
                    return TypeFieldsCache.StorageCache.ValidData.Count;
                case CachType.TypeBOClassess:
                    return TypeBOClassessCache.StorageCache.ValidData.Count;
                case CachType.PropertyAttributeModel:
                    return PropertyAttributeModelCache.StorageCache.ValidData.Count;
                case CachType.ClassAttributeModel:
                    return ClassAttributeModelCache.StorageCache.ValidData.Count;
                case CachType.HasTable:
                    return HasTableCashCache.StorageCache.ValidData.Count;
                case CachType.AggregationCache:
                    return AggregationModelCache.StorageCache.ValidData.Count;
            }
            return 0;

        }
        public static double CashDataSize(CachType type)
        {
            switch (type)
            {
                case CachType.None:
                    break;
                case CachType.AssosiationModelCache:
                    return (long)AssosiationModelCache.StorageCache.ValidData.Select(x => x.Value).ToList().GetObjectSize();
                case CachType.TypeProperties:
                    return (long)TypePropertiesCache.StorageCache.ValidData.Select(x => x.Value).ToList().GetObjectSize();
                case CachType.TypeFields:
                    return (long)TypeFieldsCache.StorageCache.ValidData.Select(x => x.Value).ToList().GetObjectSize();
                case CachType.TypeBOClassess:
                    return (long)TypeBOClassessCache.StorageCache.ValidData.Select(x => x.Value).ToList().GetObjectSize();
                case CachType.PropertyAttributeModel:
                    return PropertyAttributeModelCache.StorageCache.ValidData.Select(x => x.Value).ToList().GetObjectSize();
                case CachType.ClassAttributeModel:
                    return ClassAttributeModelCache.StorageCache.ValidData.Select(x => x.Value).ToList().GetObjectSize();
               case CachType.HasTable:
                    return HasTableCashCache.StorageCache.ValidData.Select(x => x.Value).ToList().GetObjectSize();
                case CachType.AggregationCache:
                    return AggregationModelCache.StorageCache.ValidData.Select(x => x.Value).ToList().GetObjectSize();


            }
            return 0;




        }
        public static long CashAllDataCount()
        {
            var count = 0;
            count += AssosiationModelCache.StorageCache.ValidData.Count;
            count += TypePropertiesCache.StorageCache.ValidData.Count;
            count += TypeFieldsCache.StorageCache.ValidData.Count;
            count += TypeBOClassessCache.StorageCache.ValidData.Count;
            count += PropertyAttributeModelCache.StorageCache.ValidData.Count;
            count += ClassAttributeModelCache.StorageCache.ValidData.Count;
           count += HasTableCashCache.StorageCache.ValidData.Count;
            count += AggregationModelCache.StorageCache.ValidData.Count;

            return count;

        }
        public static double CashAllDataSize()
        {
            double Sum = 0;
            Sum += (long)AssosiationModelCache.StorageCache.ValidData.Select(x => x.Value).ToList().GetObjectSize();
            Sum += (long)TypePropertiesCache.StorageCache.ValidData.Select(x => x.Value).ToList().GetObjectSize();
            Sum += (long)TypeFieldsCache.StorageCache.ValidData.Select(x => x.Value).ToList().GetObjectSize();
            Sum += (long)TypeBOClassessCache.StorageCache.ValidData.Select(x => x.Value).ToList().GetObjectSize();
            Sum += (long)PropertyAttributeModelCache.StorageCache.ValidData.Select(x => x.Value).ToList().GetObjectSize();
            Sum += (long)ClassAttributeModelCache.StorageCache.ValidData.Select(x => x.Value).ToList().GetObjectSize();
            Sum += (long)HasTableCashCache.StorageCache.ValidData.Select(x => x.Value).ToList().GetObjectSize();
            Sum += (long)AggregationModelCache.StorageCache.ValidData.Select(x => x.Value).ToList().GetObjectSize();
            return Sum;


        }
        public static void ClearAllCash()
        {
            AssosiationModelCache.StorageCache.Clear();
            TypePropertiesCache.StorageCache.Clear();
            TypeFieldsCache.StorageCache.Clear();
            TypeBOClassessCache.StorageCache.Clear();
            PropertyAttributeModelCache.StorageCache.Clear();
            ClassAttributeModelCache.StorageCache.Clear();
           HasTableCashCache.StorageCache.Clear();
            AggregationModelCache.StorageCache.Clear();

        }
    }
}
