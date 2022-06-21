using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mhazami.Framework.BOClasses;
using Mhazami.Framework.CacheManager;

namespace Mhazami.Framework.DbHelper
{
    public static class Helper
    {
        public static PropertyInfo[] GetPropertyInAttribute<TSource, TAttribute>() where TAttribute : Attribute
        {
            return
                GetTypeProperties(typeof(TSource)).Where(
                    propertyInfo =>
                    {
                        var customAttributes = propertyInfo.GetCustomAttributes(typeof(TAttribute), true);
                        return customAttributes.Length > 0 && customAttributes is TAttribute[];
                    }).ToArray();
        }

        public static PropertyInfo[] GetPropertyInAttribute<TAttribute>(Type type) where TAttribute : Attribute
        {
            return
                 GetTypeProperties(type).Where(
                    propertyInfo =>
                    {
                        var customAttributes = propertyInfo.GetCustomAttributes(typeof(TAttribute), true);
                        return customAttributes.Length > 0 && customAttributes is TAttribute[];
                    }).ToArray();
        }

        public static TAttribute GetClassAttribute<TClass, TAttribute>() where TAttribute : Attribute
        {
            var attributes = typeof(TClass).GetCustomAttributes(typeof(TAttribute), false);
            if (attributes != null && attributes.Length > 0)
                return (TAttribute)attributes[0];
            return null;
        }

        public static TAttribute GetClassAttribute<TAttribute>(Type type) where TAttribute : Attribute
        {
            var attributes = type.GetCustomAttributes(typeof(TAttribute), false);
            if (attributes != null && attributes.Length > 0)
                return (TAttribute)attributes[0];
            return null;
        }

        public static TAttribute GetPropertyAttribute<TAttribute>(PropertyInfo property) where TAttribute : Attribute
        {
            var attributes = property.GetCustomAttributes(typeof(TAttribute), false);
            if (attributes != null && attributes.Length > 0)
                return (TAttribute)attributes[0];
            return null;
        }


        public static void SetAttributeValue<TAttribute>(PropertyInfo property, object attributeValue) where TAttribute : Attribute
        {
            var attributes = property.GetCustomAttributes(typeof(TAttribute), false);
            if (attributes != null && attributes.Length > 0)
                attributes.SetValue(attributeValue, 0);
            else
            {
                throw new Exception("");
            }
        }

        public static object GetPropertyValue(object obj, PropertyInfo property)
        {
            return property.GetValue(obj, null);
        }
        internal static PropertyInfo[] GetTypeKeyProperties(this Type type)
        {
            return GetTypeProperties(type).Where(propertyInfo => propertyInfo.GetPropertyAttributes().Key != null).ToArray();
        }
        public static Dictionary<string, PropertyInfo> GetTypePropertiesDictionary(this Type type)
        {
            return type.GetTypeProperties().ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);
        }
        public static Schema GetSchema(this Type type)
        {
            return type.GetClassAttributes().Schema ?? new Schema("dbo");
        }
        internal static PropertyInfo[] GetAllowInsertProperties(this Type type)
        {
            return GetTypeProperties(type).Where(propertyInfo => !IsDisableInsertAndUpdate(propertyInfo, false)).ToArray();
        }
        internal static PropertyInfo[] GetAllowTrackProperties(this Type type)
        {
            return GetTypeProperties(type).Where(propertyInfo => !IsDisableTrack(propertyInfo)).ToArray();
        }
        internal static PropertyInfo[] GetAllowUpdateProperties(this Type type)
        {
            return GetTypeProperties(type).Where(propertyInfo => !IsDisableInsertAndUpdate(propertyInfo, true)).ToArray();
        }
        internal static PropertyInfo[] GetAllowSelectProperties(this Type type)
        {
            return GetTypeProperties(type).Where(propertyInfo => !IsDiableSelectProp(propertyInfo)).ToArray();
        }
        public static PropertyInfo[] GetAllowMultiLangProperties(this Type type)
        {
            return GetTypeProperties(type).Where(x => x.GetPropertyAttributes().MultiLanguage != null).ToArray();
        }
        public static PropertyInfo[] GetUniqueProperties(this Type type)
        {
            return GetTypeProperties(type).Where(x => x.GetPropertyAttributes().Unique != null).ToArray();
        }
        public static PropertyInfo[] GetTrackMasterProperties(this Type type)
        {
            return GetTypeProperties(type).Where(x => x.GetPropertyAttributes().TrackMaster != null).ToArray();
        }
        public static PropertyInfo[] GetIdentityProperties(this Type type)
        {
            return GetTypeProperties(type).Where(x => x.GetPropertyAttributes().Identity != null).ToArray();
        }
        internal static Dictionary<string, PropertyInfo> GetObjectProperties(this Type type, string key)
        {
            return GetTypeProperties(type).ToDictionary(x => (key + x.Name), StringComparer.OrdinalIgnoreCase);
        }
        public static PropertyInfo[] GetTypeProperties(this Type type)
        {
            var obj = TypePropertiesCache.StorageCache[type.FullName];
            if (obj != null) return (PropertyInfo[])obj;
            var getProperties = type.GetProperties();
            TypePropertiesCache.StorageCache.Add(type.FullName, getProperties);
            return getProperties;
        }
        public static Dictionary<string, FieldInfo> GetTypeFields(this Type type)
        {
            var obj = TypeFieldsCache.StorageCache[type.FullName];
            if (obj != null) return (Dictionary<string, FieldInfo>)obj;
            var getProperties = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic).ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);
            TypeFieldsCache.StorageCache.Add(type.FullName, getProperties);
            return getProperties;



        }
        public static AssosiationModel[] GetAssosiationModel(this Type type)
        {
            var obj = AssosiationModelCache.StorageCache[type.FullName];
            if (obj != null) return (AssosiationModel[])obj;
            var inAssosiation = GetValidAssosiationModel(type);
            AssosiationModelCache.StorageCache.Add(type.FullName, inAssosiation);
            return inAssosiation;
        }
        public static bool HasIdentity<TDataStructure>()
        {
            var inKey = typeof(TDataStructure).GetTypeKeyProperties();
            return inKey.Any(propertyInfo => propertyInfo.GetPropertyAttributes().Key.Identity) ||
                   typeof(TDataStructure).GetIdentityProperties().Any();
        }

        public static PropertyAttributeModel GetPropertyAttributes(this PropertyInfo propertyInfo)
        {
            if (propertyInfo.DeclaringType == null)
                return PropertyAttributeModel(propertyInfo);
            var obj = PropertyAttributeModelCache.StorageCache[propertyInfo.DeclaringType.FullName + "." + propertyInfo.Name];
            if (obj != null) return (PropertyAttributeModel)obj;
            var assosiationAttribute = PropertyAttributeModel(propertyInfo);
            PropertyAttributeModelCache.StorageCache.Add(propertyInfo.DeclaringType.FullName + "." + propertyInfo.Name, assosiationAttribute);
            return assosiationAttribute;

        }
        internal static AggregationModel GetPropertyAggregationAttributesModel(this PropertyInfo propertyInfo, Type aggregationtype)
        {
            if (propertyInfo.DeclaringType == null)
                return PropertyAggregationModelModel(propertyInfo, aggregationtype);
            var obj = AggregationModelCache.StorageCache[propertyInfo.DeclaringType.FullName + "." + propertyInfo.Name];
            if (obj != null) return (AggregationModel)obj;
            var assosiationAttribute = PropertyAggregationModelModel(propertyInfo, aggregationtype);
            AggregationModelCache.StorageCache.Add(propertyInfo.DeclaringType.FullName + "." + propertyInfo.Name, assosiationAttribute);
            return assosiationAttribute;

        }

        

        public static ClassAttributeModel GetClassAttributes(this Type type)
        {

            var obj = ClassAttributeModelCache.StorageCache[type.FullName];
            if (obj != null) return (ClassAttributeModel)obj;
            var assosiationAttribute = ClassAttributeModel(type);
            ClassAttributeModelCache.StorageCache.Add(type.FullName, assosiationAttribute);
            return assosiationAttribute;

        }
        private static ClassAttributeModel ClassAttributeModel(Type type)
        {
            var assosiationAttribute = new ClassAttributeModel();
            var customAttributes = type.GetCustomAttributes(false).ToDictionary(o => o.GetType());

            object description;
            if (customAttributes.TryGetValue(typeof(Description), out description))
                assosiationAttribute.Description = (Description)description;
            object track;
            if (customAttributes.TryGetValue(typeof(Track), out track))
                assosiationAttribute.Track = (Track)track;
            object schema;
            if (customAttributes.TryGetValue(typeof(Schema), out schema))
                assosiationAttribute.Schema = (Schema)schema;

            object connectionHandlerType;
            if (customAttributes.TryGetValue(typeof(ConnectionHandlerType), out connectionHandlerType))
                assosiationAttribute.ConnectionHandlerType = (ConnectionHandlerType)connectionHandlerType;
            return assosiationAttribute;
        }


        private static PropertyAttributeModel PropertyAttributeModel(PropertyInfo propertyInfo)
        {
            var assosiationAttribute = new PropertyAttributeModel();
            var customAttributes = propertyInfo.GetCustomAttributes(false).ToDictionary(o => o.GetType());
            object action;
            if (customAttributes.TryGetValue(typeof(DisableAction), out action))
                assosiationAttribute.DisableAction = (DisableAction)action;
            object key;
            if (customAttributes.TryGetValue(typeof(Key), out key))
                assosiationAttribute.Key = (Key)key;
            object assosiation;
            if (customAttributes.TryGetValue(typeof(Assosiation), out assosiation))
                assosiationAttribute.Assosiation = (Assosiation)assosiation;

            object identity;
            if (customAttributes.TryGetValue(typeof(Identity), out identity))
                assosiationAttribute.Identity = (Identity)identity;

            object trackMaster;
            if (customAttributes.TryGetValue(typeof(TrackMaster), out trackMaster))
                assosiationAttribute.TrackMaster = (TrackMaster)trackMaster;

            object layout;
            if (customAttributes.TryGetValue(typeof(Layout), out layout))
                assosiationAttribute.Layout = (Layout)layout;

            object isNullable;
            if (customAttributes.TryGetValue(typeof(IsNullable), out isNullable))
                assosiationAttribute.IsNullable = (IsNullable)isNullable;

            object unique;
            if (customAttributes.TryGetValue(typeof(Unique), out unique))
                assosiationAttribute.Unique = (Unique)unique;

            object dbType;
            if (customAttributes.TryGetValue(typeof(DbType), out dbType))
                assosiationAttribute.DbType = (DbType)dbType;

            object multiLanguage;
            if (customAttributes.TryGetValue(typeof(MultiLanguage), out multiLanguage))
                assosiationAttribute.MultiLanguage = (MultiLanguage)multiLanguage;

            object trackPropertyName;
            if (customAttributes.TryGetValue(typeof(TrackPropertyName), out trackPropertyName))
                assosiationAttribute.TrackPropertyName = (TrackPropertyName)trackPropertyName;

            object aggregation;
            if (customAttributes.TryGetValue(typeof(Aggregation), out aggregation))
                assosiationAttribute.Aggregation = (Aggregation)aggregation;

            return assosiationAttribute;
        }

        internal static string GetTableKey(this Type type, string parentkey = "")
        {
            return BOUtility.TableNameKey(type.GetSchema(), type, parentkey);
        }

        internal static string GetTableKey(this Type type, string parentkey, PropertyInfo propertyInfo)
        {
            return BOUtility.TableNameKey(type.GetSchema(), type, parentkey, propertyInfo);
        }




        private static bool IsDisableTrack(PropertyInfo propertyInfo)
        {
           
            if (propertyInfo.Name == "DescriptionField")
                return true;
            var attributeModel = propertyInfo.GetPropertyAttributes();
            if (attributeModel.DisableAction != null && (attributeModel.DisableAction.DiableAllAction || attributeModel.DisableAction.DisableTrack))
                return true;
            if (attributeModel.Key != null&& !attributeModel.Key.EnableTrack)
                return true;
            if (attributeModel.Assosiation != null)
                return true;
            if (attributeModel.Aggregation != null)
                return true;
            if (attributeModel.Identity != null && attributeModel.Identity.IsIdentity)
                return true;
            return false;
        }
        private static bool IsDisableInsertAndUpdate(PropertyInfo propertyInfo, bool forupdate)
        {

            var attributeModel = propertyInfo.GetPropertyAttributes();
            if (attributeModel.DisableAction != null &&
                (attributeModel.DisableAction.DiableAllAction ||
                 (!forupdate && attributeModel.DisableAction.DisableInsert) ||
                  (forupdate && attributeModel.DisableAction.DisableUpdate)))
                return true;
            if (attributeModel.Key != null && attributeModel.Key.Identity)
                return true;
            if (attributeModel.Assosiation != null)
                return true;
            if (attributeModel.Aggregation != null)
                return true;
            if (attributeModel.MultiLanguage != null)
                return true;
            if (attributeModel.Identity != null && attributeModel.Identity.IsIdentity)
                return true;
            return false;
        }
        private static bool IsDiableSelectProp(PropertyInfo propertyInfo)
        {
            var attributeModel = propertyInfo.GetPropertyAttributes();
            if (attributeModel.DisableAction != null && (attributeModel.DisableAction.DiableAllAction || attributeModel.DisableAction.DiableSelect))
                return true;
            if (attributeModel.Assosiation != null)
                return true;
            if (attributeModel.Aggregation != null)
                return true;
            if (attributeModel.MultiLanguage != null)
                return true;
            return false;
        }
        private static AggregationModel PropertyAggregationModelModel(PropertyInfo propertyInfo, Type aggregationtype)
        {
            var aggregationModel = new AggregationModel();
            var attributes = propertyInfo.GetPropertyAttributes();
            var aggregation = attributes.Aggregation;
            if (aggregation == null|| propertyInfo.DeclaringType==null||string.IsNullOrEmpty(aggregation.AggigatePropName)) return aggregationModel;
            var properties = aggregationtype.GetTypePropertiesDictionary();
           
            var names = aggregation.AggigatePropName.Split(',');
            var propertyInfoList = new List<PropertyInfo>();
            foreach (string key in names)
            {
                PropertyInfo info;
                if (!properties.TryGetValue(key, out info) || Helper.IsDiableSelectProp(info))
                    throw new KnownException(String.Format(" Property :{0} Not Found In {1} Class ", key, aggregationtype.Name));
                propertyInfoList.Add(info);
            }
            
            var assosproperty = propertyInfo.DeclaringType.GetTypeKeyProperties();
            var keyname = assosproperty.ToList();
            if (!string.IsNullOrEmpty(aggregation.PropName))
            {
                var declaringTypeproperties = propertyInfo.DeclaringType.GetTypePropertiesDictionary();
                keyname =new List<PropertyInfo>();
                var joinPropName = aggregation.PropName.Split(',');
                foreach (string key in joinPropName)
                {

                    PropertyInfo info;
                    if (!declaringTypeproperties.TryGetValue(key, out info) || Helper.IsDiableSelectProp(info))
                        throw new KnownException(String.Format(" Property :{0} Not Found In {1} Class ", key, propertyInfo.DeclaringType.Name));
                    keyname.Add(info);
                }
               
            }
            aggregationModel.AggrigateObjectPropNamePropertys = propertyInfoList.ToArray();
            aggregationModel.PropNamePropertys= keyname.ToArray();
            return aggregationModel;
        }

        private static AssosiationModel[] GetValidAssosiationModel(Type type)
        {
            var infos = type.GetTypeProperties();
            var properties = type.GetTypePropertiesDictionary();
            var propertyInfos = new List<AssosiationModel>();
            foreach (var propertyInfo in infos)
            {
                var customAttributes = propertyInfo.GetPropertyAttributes().Assosiation;
                if (customAttributes == null || string.IsNullOrEmpty(customAttributes.PropName) ||
                    !customAttributes.FillData)
                    continue;
                var names = customAttributes.PropName.Split(',');
                var propertyInfoList = new List<PropertyInfo>();
                foreach (string key in names)
                {
                    PropertyInfo info;
                    if (!properties.TryGetValue(key, out info) || Helper.IsDiableSelectProp(info))
                        throw new KnownException(String.Format(" Property :{0} Not Found In {1} Class ", key, type.Name));
                    propertyInfoList.Add(info);
                }

                var assosproperty = propertyInfo.PropertyType.GetTypeKeyProperties();
                if (!assosproperty.Any()) continue;
                var keyname = assosproperty.ToList();
                if (!string.IsNullOrEmpty(customAttributes.JoinPropName))
                {
                    var assostionproperties = propertyInfo.PropertyType.GetTypeProperties();
                    keyname = new List<PropertyInfo>();
                    var assosiationproperties = assostionproperties.ToDictionary(propertyInfo1 => propertyInfo1.Name);
                    var strings = customAttributes.JoinPropName.Split(',');
                    foreach (var name in strings)
                    {
                        PropertyInfo getJoininfo;
                        if (!assosiationproperties.TryGetValue(name, out getJoininfo))
                            throw new KnownException(String.Format(" Property :{0} Not Found In {1} Class ", name, propertyInfo.PropertyType));
                        if (!Helper.IsDiableSelectProp(getJoininfo))
                            keyname.Add(getJoininfo);
                    }
                }
                var schema = propertyInfo.PropertyType.GetSchema();
                propertyInfos.Add(new AssosiationModel()
                {
                    FromQueryProperteis = propertyInfoList.ToArray(),
                    FromQueryAssosiationKeyNames = keyname.ToArray(),
                    PropNameProperty = propertyInfoList[0],
                    AssosiationKeyProperty = keyname[0],
                    AssosiationProperty = propertyInfo,
                    AssosiationSchemaName = schema.SchemaName,
                    JoinCompareType = customAttributes.JoinCompareType,
                    JoinType = customAttributes.JoinType,
                });
            }


            return propertyInfos.ToArray();

        }
    }
}
