using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using Mhazami.Framework.DbHelper;
using Mhazami.Utility;
using Convert = System.Convert;

namespace Mhazami.Framework
{
    public static class BOUtility
    {
        //public static IEnumerable<T> Filter<T>(ObjectContext context, T filter, IEnumerable<T> source)
        //{
        //    var propertyInfos = filter.GetType().GetProperties().Where(
        //        propertyInfo =>
        //        {
        //            var customAttributes = propertyInfo.GetCustomAttributes(typeof(DataMemberAttribute), true);
        //            return customAttributes.Length > 0 && customAttributes is DataMemberAttribute[];
        //        });
        //    return propertyInfos.Where(info =>
        //    {
        //        var value = info.GetValue(filter, null);
        //        return !info.PropertyType.IsClass && (value != null && value.GetType().Name.ToLower().Equals("guid") && !value.Equals(Guid.Empty));
        //    }).Aggregate(source, (current, propertyInfo) => current.Where(x => x.GetType().GetProperty(propertyInfo.Name).GetValue(x, null) != null && x.GetType().GetProperty(propertyInfo.Name).GetValue(x, null).Equals(propertyInfo.GetValue(filter, null))).ToList());
        //}

        internal static string TableNameKey(Schema schema, Type type, string parentkey = "")
        {
            return "TBLK" + Math.Abs((parentkey + (schema != null ? schema.SchemaName : "dbo") + type.Name).GetHashCode());
        }

        internal static string TableNameKey(Schema schema, Type type, string parentkey, PropertyInfo propertyInfo)
        {
            var tableNameKey = TableNameKey(schema, type, parentkey) + propertyInfo.Name;
            if (!parentkey.Contains(propertyInfo.Name)) return tableNameKey;
            var charcharkter = parentkey.Count(Char.IsDigit) + 4;
            return parentkey.Substring(charcharkter, parentkey.Length - charcharkter) ==
                   propertyInfo.Name
                ? parentkey
                : tableNameKey;
        }
        public static object[] GetObjectKeyValue(this object obj)
        {
            if (obj == null) return null;
            var inKey = obj.GetType().GetTypeKeyProperties();
            return GetObjectKeyValue(obj, inKey);
        }
        private static string GetParameterRadonName(this DbCommand command)
        {
         
            var str = String.Format("@params{0}", Math.Abs((Guid.NewGuid()).GetHashCode())) ;
            if (command.Parameters.Cast<DbParameter>().All(x => x.ParameterName.ToLower() != str.ToLower())) return str;
            return command.GetParameterRadonName();




        }
        internal static string GenerateNewParameter(this DbCommand command, Type type, object value)
        {
            type = type.GetTypeValidValue();
            var sqlParameter = new SqlParameter { ParameterName = GetParameterRadonName(command) };
            if (type.IsEnum)
            {
                sqlParameter.SqlDbType = SqlDbType.Int;
                sqlParameter.DbType = System.Data.DbType.Int32;

            }
            else if (type == typeof(Guid))
            {
                sqlParameter.SqlDbType = SqlDbType.UniqueIdentifier;
                sqlParameter.DbType = System.Data.DbType.Guid;
                
                
            }
            else
            {
                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.Boolean:
                        sqlParameter.SqlDbType = SqlDbType.Bit;
                        sqlParameter.DbType = System.Data.DbType.Boolean;
                        break;
                    case TypeCode.Char:
                        sqlParameter.SqlDbType = SqlDbType.Char;
                        sqlParameter.DbType = System.Data.DbType.StringFixedLength;
                        break;
                    case TypeCode.Byte:
                        sqlParameter.SqlDbType = SqlDbType.TinyInt;
                        sqlParameter.DbType = System.Data.DbType.Byte;
                        break;
                    case TypeCode.SByte:
                        sqlParameter.SqlDbType = SqlDbType.TinyInt;
                        sqlParameter.DbType = System.Data.DbType.SByte;
                        break;
                    case TypeCode.Int16:
                        sqlParameter.SqlDbType = SqlDbType.SmallInt;
                        sqlParameter.DbType = System.Data.DbType.Int16;
                        break;
                    case TypeCode.UInt16:
                        sqlParameter.SqlDbType = SqlDbType.SmallInt;
                        sqlParameter.DbType = System.Data.DbType.UInt16;
                        break;
                    case TypeCode.Int32:
                        sqlParameter.SqlDbType = SqlDbType.Int;
                        sqlParameter.DbType = System.Data.DbType.Int32;
                        break;
                    case TypeCode.UInt32:
                        sqlParameter.SqlDbType = SqlDbType.Int;
                        sqlParameter.DbType = System.Data.DbType.UInt32;
                        break;
                    case TypeCode.Int64:
                        sqlParameter.SqlDbType = SqlDbType.BigInt;
                        sqlParameter.DbType = System.Data.DbType.Int64;
                        break;
                    case TypeCode.UInt64:
                        sqlParameter.SqlDbType = SqlDbType.BigInt;
                        sqlParameter.DbType = System.Data.DbType.UInt64;
                        break;
                    case TypeCode.Double:
                        sqlParameter.SqlDbType = SqlDbType.Float;
                        sqlParameter.DbType = System.Data.DbType.Double;
                        break;
                    case TypeCode.Single:
                        sqlParameter.SqlDbType = SqlDbType.Float;
                        sqlParameter.DbType = System.Data.DbType.Single;
                        break;
                    case TypeCode.Decimal:
                        sqlParameter.SqlDbType = SqlDbType.Decimal;
                        sqlParameter.DbType = System.Data.DbType.Decimal;
                        break;
                    case TypeCode.DateTime:
                        sqlParameter.SqlDbType = SqlDbType.DateTime;
                        sqlParameter.DbType = System.Data.DbType.DateTime;
                        break;
                    case TypeCode.String:
                        sqlParameter.SqlDbType = SqlDbType.NVarChar;
                        sqlParameter.DbType = System.Data.DbType.String;
                        break;

                }
            }

            sqlParameter.Value = value ?? DBNull.Value;
            command.Parameters.Add(sqlParameter);
            return sqlParameter.ParameterName;


        }


        public static object[] GetObjectKeyValue(this object obj, PropertyInfo[] propertyInfos)
        {
            if (obj == null) return null;
            var objects = new List<Object>();
            foreach (var propertyInfo in propertyInfos)
            {
                var value = propertyInfo.GetValue(obj, null);
                if (value == null) continue;
                if (propertyInfo.PropertyType.IsEnum)
                {
                    try
                    {
                        objects.Add(Convert.ToInt32((Enum.Parse(propertyInfo.PropertyType, value.ToString()))));

                    }
                    catch
                    {

                    }
                    continue;
                }
                objects.Add(value);

            }
            return objects.ToArray();
        }
        internal static ChangeTracker[] TrackChange<TEntity>(this TEntity oldEntity, TEntity newEntity) where TEntity : class
        {
            if (newEntity == null)
                throw new Exception("New Entity is null.");
            if (oldEntity == null)
                throw new Exception("Old Entity is null.");

            var trackers = new List<ChangeTracker>();
            var type = oldEntity.GetType();
            var objectName = type.Name;
            var attributes = type.GetClassAttributes();
            if (attributes.Description != null)
            {
                if (!String.IsNullOrEmpty(attributes.Description.LayoutDescription))
                    objectName = attributes.Description.LayoutDescription;
            }
            var allowproplist = new PropertyInfo[] { };
            allowproplist = type.GetAllowTrackProperties();
            var propertyInfos =
                 allowproplist.Where(
                         x =>
                             !x.PropertyType.Name.ToLower().Contains("entityreference") &&
                             !x.PropertyType.Name.ToLower().Contains("entitycollection") &&
                             !x.PropertyType.FullName.ToLower().Contains("system.collections.generic"));
            var inAssosiation = type.GetAssosiationModel();
            foreach (var property in propertyInfos.ToArray())
            {

                try
                {
                    object oldValue = null;
                    object newValue = null;
                    if (property.PropertyType.IsEnum)
                    {
                        dynamic oldEnumVal = property.GetValue(oldEntity, null);
                        if ((oldEnumVal as Enum) != null)
                            oldValue = (oldEnumVal as Enum).GetDescription();
                        dynamic newEnumVal = property.GetValue(newEntity, null);
                        if ((newEnumVal as Enum) != null)
                            newValue = (newEnumVal as Enum).GetDescription();
                    }
                    else
                    {
                        var propertyInfo = inAssosiation.FirstOrDefault(c => c.PropNameProperty.Name.Equals(property.Name));
                        if (propertyInfo == null)
                        {
                            oldValue = property.GetValue(oldEntity, null);
                            newValue = property.GetValue(newEntity, null);
                        }
                    }

                    var getoldval = oldValue ?? String.Empty;
                    var getnewval = newValue ?? String.Empty;
                    if ((String.IsNullOrEmpty(getoldval.ToString().Trim()) && String.IsNullOrEmpty(getnewval.ToString().Trim())) || (getoldval.Equals(getnewval))) continue;


                    var track = property.GetPropertyAttributes().TrackPropertyName;
                    SetOtherValuesTrack(oldEntity, newEntity, type, track, ref oldValue, ref newValue);

                    var fieldDesc = property.Name;
                    var layout = property.GetPropertyAttributes().Layout;
                    if (layout != null)
                    {
                        if (!String.IsNullOrEmpty(layout.Caption))
                            fieldDesc = layout.Caption;
                    }
                    trackers.Add(new ChangeTracker
                    {
                        ObjectName = objectName,
                        FieldName = property.Name,
                        FieldDesc = fieldDesc,
                        OldValue = oldValue,
                        NewValue = newValue,


                    });
                }
                catch
                {
                    break;
                }
            }
            return trackers.ToArray();
        }
        public static string GetTrackerUserUserName<TDataStructure>(TDataStructure obj)
        {
            var username = TrackerUtility.TrackerUserName;
            if (!(obj is DataStructureBase)) return username;
            var dataStructureBase = obj as DataStructureBase;
            if (!string.IsNullOrEmpty(dataStructureBase.TrackerUserName))
                username = dataStructureBase.TrackerUserName;
            return username;

        }
        private static void SetOtherValuesTrack<TEntity>(TEntity oldEntity, TEntity newEntity, Type type, TrackPropertyName track,
           ref object oldValue, ref object newValue) where TEntity : class
        {
            if (track == null || string.IsNullOrEmpty(track.PropName)) return;
            var split = track.PropName.Split(',');
            foreach (var propertyname in split)
            {
                var propertyInfo = type.GetProperty(propertyname);
                if (propertyInfo != null)
                {
                    var TrackPropertyNameoldvalue = propertyInfo.GetValue(oldEntity, null);
                    if (TrackPropertyNameoldvalue != null && !String.IsNullOrEmpty(TrackPropertyNameoldvalue.ToString()) &&
                        oldValue != null && oldValue != "")
                    {
                        if ((TrackPropertyNameoldvalue as Enum) != null)
                            TrackPropertyNameoldvalue = (TrackPropertyNameoldvalue as Enum).GetDescription();
                        oldValue += "-" + TrackPropertyNameoldvalue;
                    }

                    var TrackPropertyNamenewvalue = propertyInfo.GetValue(newEntity, null);
                    if (TrackPropertyNamenewvalue != null && !String.IsNullOrEmpty(TrackPropertyNamenewvalue.ToString()))
                    {
                        if ((TrackPropertyNamenewvalue as Enum) != null)
                            TrackPropertyNamenewvalue = (TrackPropertyNamenewvalue as Enum).GetDescription();
                        newValue += "-" + TrackPropertyNamenewvalue;
                    }
                }
            }


        }

        public static void GetGuidForId(ref Guid id)
        {
            if (String.IsNullOrEmpty(id.ToString()) || id.Equals(Guid.Empty))
                id = Guid.NewGuid();
        }
    }


}
