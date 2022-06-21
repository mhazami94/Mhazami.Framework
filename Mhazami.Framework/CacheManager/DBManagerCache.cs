using Mhazami.Cache;

namespace Mhazami.Framework.CacheManager
{


    internal class AssosiationModelCache : CacheCategoryManager
    {
        private static AssosiationModelCache _storageCache;

        internal static AssosiationModelCache StorageCache
        {
            get
            {
                if (_storageCache == null)
                    _storageCache = new AssosiationModelCache();
                return _storageCache;
            }
        }



        public AssosiationModelCache()
        {
            this.CategoryName = "ManageAssosiationModelCachableItems";
            this.CategoryInfo.ExpirationType = ExpirationTypes.ExpireIfNotUsedAfterTime;
            this.CategoryInfo.ValidationTime = new TimeSpan(0,5, 0, 0);



        }

    }
   


    internal class AggregationModelCache : CacheCategoryManager
    {
        private static AggregationModelCache _storageCache;

        internal static AggregationModelCache StorageCache
        {
            get
            {
                if (_storageCache == null)
                    _storageCache = new AggregationModelCache();
                return _storageCache;
            }
        }



        public AggregationModelCache()
        {
            this.CategoryName = "ManageAggregationModelCachableItems";
            this.CategoryInfo.ExpirationType = ExpirationTypes.ExpireIfNotUsedAfterTime;
            this.CategoryInfo.ValidationTime = new TimeSpan(0,5, 0, 0);



        }

    }


    internal class PropertyAttributeModelCache : CacheCategoryManager
    {
        private static PropertyAttributeModelCache _storageCache;

        internal static PropertyAttributeModelCache StorageCache
        {
            get
            {
                if (_storageCache == null)
                    _storageCache = new PropertyAttributeModelCache();
                return _storageCache;
            }
        }



        public PropertyAttributeModelCache()
        {
            this.CategoryName = "ManagePropertyAttributeModelCachableItems";
            this.CategoryInfo.ExpirationType = ExpirationTypes.ExpireIfNotUsedAfterTime;
            this.CategoryInfo.ValidationTime = new TimeSpan(0,5, 0, 0);



        }

    }

    internal class ClassAttributeModelCache : CacheCategoryManager
    {
        private static ClassAttributeModelCache _storageCache;

        internal static ClassAttributeModelCache StorageCache
        {
            get
            {
                if (_storageCache == null)
                    _storageCache = new ClassAttributeModelCache();
                return _storageCache;
            }
        }
        
        public ClassAttributeModelCache()
        {
            this.CategoryName = "ManageClassAttributeModelCachableItems";
            this.CategoryInfo.ExpirationType = ExpirationTypes.ExpireIfNotUsedAfterTime;
            this.CategoryInfo.ValidationTime = new TimeSpan(0,5, 0, 0);



        }

    }
    
    
    internal class TypeBOClassessCache : CacheCategoryManager
    {
        private static TypeBOClassessCache _storageCache;
        internal static TypeBOClassessCache StorageCache
        {
            get
            {
                if (_storageCache == null)
                    _storageCache = new TypeBOClassessCache();
                return _storageCache;
            }
        }
        public TypeBOClassessCache()
        {
            this.CategoryName = "ManageTypeBOClassessCacheCachableItems";
            this.CategoryInfo.ExpirationType = ExpirationTypes.ExpireIfNotUsedAfterTime;
            this.CategoryInfo.ValidationTime = new TimeSpan(0,5, 0, 0);


        }

    }
    

    
   
    internal class TypePropertiesCache : CacheCategoryManager
    {
        private static TypePropertiesCache _storageCache;
        internal static TypePropertiesCache StorageCache
        {
            get
            {
                if (_storageCache == null)
                    _storageCache = new TypePropertiesCache();
                return _storageCache;
            }
        }



        public TypePropertiesCache()
        {
            this.CategoryName = "ManageTypePropertiesCachableItems";
            this.CategoryInfo.ExpirationType = ExpirationTypes.ExpireIfNotUsedAfterTime;
            this.CategoryInfo.ValidationTime = new TimeSpan(0,5, 0, 0);


        }

    }
    internal class TypeFieldsCache : CacheCategoryManager
    {
        private static TypeFieldsCache _storageCache;
        internal static TypeFieldsCache StorageCache
        {
            get
            {
                if (_storageCache == null)
                    _storageCache = new TypeFieldsCache();
                return _storageCache;
            }
        }



        public TypeFieldsCache()
        {
            this.CategoryName = "ManageTypeFieldsCachableItems";
            this.CategoryInfo.ExpirationType = ExpirationTypes.ExpireIfNotUsedAfterTime;
            this.CategoryInfo.ValidationTime = new TimeSpan(0,5, 0, 0);


        }

    }
    internal class HasTableCashCache : CacheCategoryManager
    {
        private static HasTableCashCache _storageCache;
        internal static HasTableCashCache StorageCache
        {
            get
            {
                if (_storageCache == null)
                    _storageCache = new HasTableCashCache();
                return _storageCache;
            }
        }



        public HasTableCashCache()
        {
            this.CategoryName = "ManageHasTableCashCachableItems";
            this.CategoryInfo.ExpirationType = ExpirationTypes.ExpireIfNotUsedAfterTime;
            this.CategoryInfo.ValidationTime = new TimeSpan(0,5, 0, 0);


        }

    }




}
