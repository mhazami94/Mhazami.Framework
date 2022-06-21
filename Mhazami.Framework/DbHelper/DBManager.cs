using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using Framework;
using Mhazami.Framework;
using Mhazami.Framework.BOClasses;
using Mhazami.Framework.CacheManager;
using Mhazami.Framework.Difinition;
using Mhazami.Framework.NonPersistTemprory;
using Mhazami.Utility;
using Convert = System.Convert;

namespace Mhazami.Framework.DbHelper
{
    public sealed class DBManager 
    {


        #region Manual
        #region Frameworktools

        private static DataTable GetDataTable(IConnectionHandler connectionHandler, string commandText)
        {
            var dt = new DataTable();
            try
            {
                if (string.IsNullOrEmpty(commandText)) return dt;
                var command = DbProvider.GetDbCommand(connectionHandler, commandText);
                var sdr = command.ExecuteReader();
                dt.Load(sdr);
                sdr.Close();
                return dt;

            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        private static DataTable GetDataTable(IConnectionHandler connectionHandler, IDbCommand command)
        {
            var dt = new DataTable();
            try
            {


                if (string.IsNullOrEmpty(command.CommandText)) return dt;
                foreach (var param in command.Parameters.OfType<IDataParameter>())
                {
                    try
                    {
                        if (param.DbType == System.Data.DbType.String)
                            param.Value = param.Value.ToString();
                    }
                    catch
                    {

                    }
                }

                DbProvider.GetDbCommand(connectionHandler, command);
                var sdr = command.ExecuteReader();
                dt.Load(sdr);
                sdr.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public static int ExecuteNonQuery(IConnectionHandler connectionHandler, string commandText)
        {
            try
            {

                var command = DbProvider.GetDbCommand(connectionHandler, commandText);
                return command.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                var str = commandText.ToLower().Split();
                var message = str[0] == "delete" ? Messages.DeletingDataFailed : Messages.SavingDataFailed;
                var knownException = new KnownException(message, ex);
                throw knownException;
            }
        }
        public static int ExecuteNonQuery(IConnectionHandler connectionHandler, IDbCommand command)
        {
            try
            {

                if (string.IsNullOrEmpty(command.CommandText)) return 1;
                DbProvider.GetDbCommand(connectionHandler, command);
                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.SavingDataFailed, ex);
            }
        }

        public static object ExecuteScalar(IConnectionHandler connectionHandler, string commandText)
        {
            try
            {

                var command = DbProvider.GetDbCommand(connectionHandler, commandText);
                var retVal = command.ExecuteScalar();
                return retVal is DBNull ? null : retVal;



            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public static object ExecuteScalar(IConnectionHandler connectionHandler, IDbCommand command)
        {
            try
            {

                DbProvider.GetDbCommand(connectionHandler, command);
                var retVal = command.ExecuteScalar();
                return retVal is DBNull ? null : retVal;

            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public static TResult ExecuteScalar<TResult>(IConnectionHandler connectionHandler, IDbCommand command)
        {
            try
            {

                DbProvider.GetDbCommand(connectionHandler, command);
                var result = command.ExecuteScalar();
                if (result == DBNull.Value || result == null)
                    return default(TResult);
                try
                {
                    return (TResult)result;
                }
                catch
                {

                    return (TResult)Convert.ChangeType(result, typeof(TResult));
                }



            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public static TResult ExecuteScalar<TResult>(IConnectionHandler connectionHandler, string commandText)
        {
            try
            {

                var command = DbProvider.GetDbCommand(connectionHandler, commandText);
                var result = command.ExecuteScalar();
                if (result == DBNull.Value || result == null)
                    return default(TResult);
                try
                {
                    return (TResult)result;
                }
                catch
                {

                    return (TResult)Convert.ChangeType(result, typeof(TResult));
                }

            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        private static DataTable GetTableKeys<TDataStructure>(IConnectionHandler connectionHandler)
        {
            var commandText =
                "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE (TABLE_NAME = '" +
                typeof(TDataStructure).Name + "') AND (TABLE_SCHEMA= '" +
                typeof(TDataStructure).GetSchema().SchemaName +
                "') AND (CONSTRAINT_NAME LIKE 'pk%') order by ORDINAL_POSITION";
            return GetDataTable(connectionHandler, commandText);
        }



        private static void CheckUnique<TDataStructure>(IConnectionHandler connectionHandler, TDataStructure obj)
        {
            var str = GetUniqueWhereCluase(connectionHandler, obj);
            if (!string.IsNullOrEmpty(str))
            {
                throw new KnownException(str + Messages.Duplicated);
            }
        }
        private static void CheckUniqueWithKeys<TDataStructure>(IConnectionHandler connectionHandler, TDataStructure obj)
        {
            var unique = typeof(TDataStructure).GetUniqueProperties();
            if (unique.Any())
            {
                var inKey = typeof(TDataStructure).GetTypeKeyProperties();
                if (!inKey.Any())
                    throw new KnownException(string.Format("براي جدول {0} هيچ كليدي در نظر گرفته نشده.", typeof(TDataStructure).Name));
                var str = GetUniqueWhereCluase(connectionHandler, obj, true);
                if (!string.IsNullOrEmpty(str))
                    throw new KnownException(str + Messages.Duplicated);
            }
        }
        private static string GetUniqueWhereCluase<TDataStructure>(IConnectionHandler connectionHandler, TDataStructure obj, bool forUpdate = false)
        {
            var inKey = typeof(TDataStructure).GetTypeKeyProperties();
            if (!inKey.Any())
                throw new KnownException(string.Format("براي جدول {0} هيچ كليدي در نظر گرفته نشده.",
                                                       typeof(TDataStructure).Name));
            var temp = string.Empty;
            foreach (var property in inKey)
            {
                temp += string.Format("[{0}] <> ", property.Name);
                var propertyValue = Helper.GetPropertyValue(obj, property);
                temp += GetPropertyValue(propertyValue);
                temp += " AND ";
            }

            var key = string.Format(" AND ({0})  ", temp.Substring(0, temp.Length - 5));



            var type = typeof(TDataStructure);
            var unique = typeof(TDataStructure).GetUniqueProperties();
            if (unique.Any())
            {
                var query = string.Format("SELECT Count(*) FROM [{0}].[{1}] WHERE ",
                                        typeof(TDataStructure).GetSchema().SchemaName,
                                        type.Name);
                var uniqueGroup = new Dictionary<string, List<PropertyInfo>>();
                foreach (var propertyInfo in
                    unique.Where(property => !string.IsNullOrEmpty(property.GetPropertyAttributes().Unique.Group)))
                {
                    var groupName = propertyInfo.GetPropertyAttributes().Unique.Group;
                    if (!uniqueGroup.ContainsKey(groupName))
                        uniqueGroup.Add(groupName, new List<PropertyInfo>());
                    uniqueGroup[groupName].Add(propertyInfo);
                }
                string whereClause;
                string commandText;
                foreach (var property in
                    unique.Where(
                        property =>
                        string.IsNullOrEmpty(property.GetPropertyAttributes().Unique.Group)))
                {
                    var propertyValue = Helper.GetPropertyValue(obj, property);
                    if (propertyValue == null && property.GetPropertyAttributes().Unique.IgnoreNull) continue;
                    whereClause = string.Format("[{0}] = ", property.Name);
                    whereClause += GetPropertyValue(propertyValue);
                    commandText = string.Format("{0}{1}{2}", query, whereClause, (forUpdate ? key : ""));
                    if (ExecuteScalar<int>(connectionHandler, commandText) > 0)
                        return property.GetPropertyAttributes().Layout.Caption;
                }

                foreach (var ug in uniqueGroup)
                {
                    whereClause = string.Empty;
                    foreach (var property in ug.Value)
                    {
                        var propertyValue = Helper.GetPropertyValue(obj, property);
                        if (propertyValue != null && !propertyValue.Equals("null"))
                        {
                            whereClause += string.Format("[{0}] = ", property.Name);
                            whereClause += GetPropertyValue(propertyValue);
                            whereClause += " AND ";
                        }
                        else if (property.GetPropertyAttributes().Unique.IgnoreNull)
                        {
                            whereClause += string.Format("[{0}] = ", property.Name);
                            whereClause += GetPropertyValue(propertyValue);
                            whereClause += " AND ";
                        }
                    }
                    whereClause = whereClause.Substring(0, whereClause.Length - 5);
                    commandText = string.Format("{0}{1}{2}", query, whereClause, (forUpdate ? key : ""));
                    if (ExecuteScalar<int>(connectionHandler, commandText) > 0)
                    {
                        var layouts =
                            ug.Value.Select(
                                propertyInfo =>
                                propertyInfo.GetPropertyAttributes().Layout ??
                                new Layout { Caption = propertyInfo.Name });
                        return string.Join(",", layouts.Select(x => x.Caption));
                    }
                }
            }
            return string.Empty;
        }

        #endregion

        ////==========================================================================================================================////

        #region InsertAndUpdateAndDelete


        private static void SetIdentity<TDataStructure>(IConnectionHandler connectionHandler, TDataStructure obj)
        {
            var inKey = typeof(TDataStructure).GetTypeKeyProperties();
            PropertyInfo identity;
            if (!inKey.Any(propertyInfo => propertyInfo.GetPropertyAttributes().Key.Identity))
            {
                inKey = typeof(TDataStructure).GetIdentityProperties();
                if (!inKey.Any()) return;
                identity = inKey.First();
            }
            else
                identity = inKey.FirstOrDefault(propertyInfo => propertyInfo.GetPropertyAttributes().Key.Identity);

            if (identity == null) return;

            var query = string.Format("select max({0}) from [{1}].[{2}]", identity.Name,
               typeof(TDataStructure).GetSchema().SchemaName, typeof(TDataStructure).Name);
            var id = ExecuteScalar(connectionHandler, query);
            identity.SetValue(obj, id, null);
        }
        private static TDataStructure RealodObjForTrack<TDataStructure>(IConnectionHandler connectionHandler, TDataStructure obj, object[] keys) where TDataStructure : class
        {

            var structureBase = obj as DataStructureBase;
            if (structureBase == null) return null;
            var getobj = GetObject<TDataStructure>(connectionHandler, keys);
            var dataStructureBase = getobj as DataStructureBase;
            if (dataStructureBase == null) return null;
            dataStructureBase.RootId = structureBase.RootId;
            dataStructureBase.RootObject = structureBase.RootObject;
            return getobj;

        }
        internal static int Update<TDataStructure>(IConnectionHandler connectionHandler, TDataStructure obj, Delegates.LogMethod method = null, object[] parameters = null) where TDataStructure : class
        {
            if (Contrans.IgnorePersistData)
            {
                Items.Add(obj, ObjectState.Dirty);
                return 1;
            }

            var transactionImported = false;
            try
            {

                var inKey = typeof(TDataStructure).GetTypeKeyProperties();
                if (inKey == null || !inKey.Any())
                    throw new Exception("کلیدی برای جدول مورد نظر وجود ندارد");
                var keys = obj.GetObjectKeyValue();
                TDataStructure oldObj = null;
                var enableTrack = EnableTrack(obj);
                if (enableTrack)
                    oldObj = RealodObjForTrack(connectionHandler, obj, keys);
                CheckUniqueWithKeys(connectionHandler, obj);
                var commandText = CreateUpdateCommand(obj);
                if (connectionHandler.Transaction != null)
                    transactionImported = true;
                if (!transactionImported)
                    connectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                var result = ExecuteNonQuery(connectionHandler, commandText);
                if (result > 0)
                {

                    UpdateLanguageContent(connectionHandler, obj);
                    if (enableTrack && oldObj != null)
                    {
                        obj = GetObject<TDataStructure>(connectionHandler, keys);
                        InsertTrackersOnUpdate(connectionHandler, obj, oldObj, method, parameters);
                    }


                }
                if (!transactionImported)
                {
                    connectionHandler.CommitTransaction();

                }

                return result;
            }
            catch (KnownException)
            {
                if (!transactionImported)
                    connectionHandler.RollBack();
                throw;
            }
            catch (Exception ex)
            {
                if (!transactionImported)
                    connectionHandler.RollBack();
                throw new KnownException(Messages.SavingDataFailed, ex);
            }
        }
        internal static int Delete<TDataStructure>(IConnectionHandler connectionHandler, params object[] keys) where TDataStructure : class
        {
            return Delete<TDataStructure>(connectionHandler, null, keys);
        }
        internal static int Delete<TDataStructure>(IConnectionHandler connectionHandler, TDataStructure obj, Delegates.LogMethod method = null, object[] parameters = null) where TDataStructure : class
        {
            return Delete(connectionHandler, obj, null, method, parameters);
        }
        private static int Delete<TDataStructure>(IConnectionHandler connectionHandler, TDataStructure obj, object[] keys, Delegates.LogMethod method = null, object[] parameters = null) where TDataStructure : class
        {
            if (keys == null && obj != null)
                keys = obj.GetObjectKeyValue();
            if (Contrans.IgnorePersistData)
            {
                Items.Add(keys, ObjectState.Deleted);
                return 1;
            }

            var transactionImported = false;
            try
            {

                var oldObj = obj;
                var enableTrack = EnableTrack(obj);
                if (enableTrack && oldObj == null)
                    oldObj = GetObject<TDataStructure>(connectionHandler, keys);
                var dt = GetTableKeys<TDataStructure>(connectionHandler);
                var commandText = CreateDeleteCommand<TDataStructure>(keys, dt);
                if (connectionHandler.Transaction != null)
                    transactionImported = true;
                if (!transactionImported)
                    connectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                var result = ExecuteNonQuery(connectionHandler, commandText);
                if (result > 0)
                {
                    DeleteLanguageContent<TDataStructure>(connectionHandler, keys);
                    if (enableTrack && oldObj != null)
                        InsertTrackersOnDelete(connectionHandler, oldObj, method, parameters);

                }
                if (!transactionImported)
                {
                    connectionHandler.CommitTransaction();

                }
                return result;
            }
            catch (KnownException)
            {
                if (!transactionImported)
                    connectionHandler.RollBack();
                throw;
            }
            catch (Exception ex)
            {
                if (!transactionImported)
                    connectionHandler.RollBack();
                throw new KnownException(Messages.DeletingDataFailed, ex);
            }
        }
        internal static int Insert<TDataStructure>(IConnectionHandler connectionHandler, TDataStructure obj, Delegates.LogMethod method = null, object[] parameters = null) where TDataStructure : class
        {

            var inKey = typeof(TDataStructure).GetTypeKeyProperties();
            if (inKey == null || !inKey.Any())
                throw new Exception("کلیدی برای جدول مورد نظر وجود ندارد");
            if (Contrans.IgnorePersistData)
            {
                Items.Add(obj, ObjectState.New);
                return 1;
            }
            var transactionImported = false;

            try
            {

                CheckUnique(connectionHandler, obj);
                var commandText = CreateInsertCommand(obj);
                var hasIdentity = Helper.HasIdentity<TDataStructure>();
                if (hasIdentity && string.IsNullOrEmpty(commandText)) return 0;
                if (connectionHandler.Transaction != null)
                    transactionImported = true;
                if (!transactionImported)
                    connectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                var result = ExecuteNonQuery(connectionHandler, commandText);
                if (hasIdentity)
                    SetIdentity(connectionHandler, obj);
                if (result > 0)
                {
                    InsertLanguageContent(connectionHandler, obj);
                    if (EnableTrack(obj))
                    {
                        var keys = obj.GetObjectKeyValue();
                        obj = RealodObjForTrack(connectionHandler, obj, keys);
                        InsertTrackersOnInsert(connectionHandler, obj, method, parameters);
                    }

                }

                if (!transactionImported)
                {
                    connectionHandler.CommitTransaction();

                }

                return result;
            }
            catch (KnownException)
            {
                if (!transactionImported)
                    connectionHandler.RollBack();
                throw;
            }
            catch (Exception ex)
            {
                if (!transactionImported)
                    connectionHandler.RollBack();
                throw new KnownException(Messages.SavingDataFailed, ex);
            }
        }


        #endregion

        ////==========================================================================================================================////

        #region Track

        private static int InsertTrackersOnInsert<TDataStructure>(IConnectionHandler connectionHandler, TDataStructure obj, Delegates.LogMethod method = null, object[] parameters = null) where TDataStructure : class
        {
            return InsertTrackers(connectionHandler, obj, null, OperationLogType.InsertRecord, method, parameters);
        }
        private static int InsertTrackersOnUpdate<TDataStructure>(IConnectionHandler connectionHandler, TDataStructure obj, TDataStructure oldobj, Delegates.LogMethod method = null, object[] parameters = null) where TDataStructure : class
        {
            return InsertTrackers(connectionHandler, obj, oldobj, OperationLogType.UpdateField, method, parameters);
        }
        private static int InsertTrackersOnDelete<TDataStructure>(IConnectionHandler connectionHandler, TDataStructure obj, Delegates.LogMethod method = null, object[] parameters = null) where TDataStructure : class
        {
            return InsertTrackers(connectionHandler, null, obj, OperationLogType.DeleteRecord, method, parameters);
        }
        private static bool CheckTrackerTable(IConnectionHandler connectionHandler)
        {

            var commandText =
                "SELECT SCHEMA_NAME(schema_id) + '.' + name as TableName from sys.tables where" +
                " lower(SCHEMA_NAME(schema_id) + '.' + name) = 'dbo.tracker' ORDER BY name";
            return GetDataTable(connectionHandler, commandText).Rows.Count > 0;
        }
        private static bool HasTrackerTable(IConnectionHandler connectionHandler)
        {
            var key = "Radyn.Framework.DbHelper.DBManager.HasTrackerTable";
            var obj = HasTableCashCache.StorageCache[key];
            if (obj != null) return (bool)obj;
            var hasMultiLangTable = CheckTrackerTable(connectionHandler);
            HasTableCashCache.StorageCache.Add(key, hasMultiLangTable);
            return hasMultiLangTable;


        }


        private static int InsertTrackers<TDataStructure>(IConnectionHandler connectionHandler, TDataStructure obj, TDataStructure oldobj, OperationLogType logType, Delegates.LogMethod method = null, object[] parameters = null) where TDataStructure : class

        {
            var transactionImported = false;
            try
            {

                var trackers = PrepareTrack(obj, oldobj, logType);
                if (!trackers.Any()) return 1;
                if (!HasTrackerTable(connectionHandler))
                {
                    if (method != null)
                        method(parameters);
                    return 1;
                }

                var type = typeof(Tracker);
                var allowInsertProp = type.GetAllowInsertProperties();
                var commandText = trackers.Aggregate(String.Empty, (current, tracker) =>
                         current + (" " + CreateInsertCommand(tracker, allowInsertProp) + "\n"));

                if (connectionHandler.Transaction != null)
                    transactionImported = true;
                if (!transactionImported)
                    connectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                var result = ExecuteNonQuery(connectionHandler, commandText);
                if (method != null)
                    method(parameters);

                if (!transactionImported)
                {
                    connectionHandler.CommitTransaction();

                }
                return result;


            }
            catch (KnownException)
            {
                if (!transactionImported)
                    connectionHandler.RollBack();
                throw;
            }
            catch (Exception ex)
            {
                if (!transactionImported)
                    connectionHandler.RollBack();
                throw new KnownException(Messages.SavingDataFailed, ex);
            }


        }





        #endregion


        /////=================================================================================================================================////
        #region Collection


        internal static List<TDataStructure> GetCollection<TDataStructure>(IConnectionHandler connectionHandler, bool simpleload = false)
        {
            try
            {
                var prepareModel = FetchDataHelper.Instance(simpleload);
                var CommandText = CreateSelectCommand<TDataStructure>(connectionHandler, prepareModel);
                return GetCollection<TDataStructure>(connectionHandler, CommandText, prepareModel);


            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public static List<TDataStructure> GetCollection<TDataStructure>(IConnectionHandler connectionHandler, string commandText)
        {
            try
            {

                var command = DbProvider.GetDbCommand(connectionHandler, commandText);
                return GetCollection<TDataStructure>(connectionHandler, command, 0, 0);

            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public static List<TDataStructure> GetCollection<TDataStructure>(IConnectionHandler connectionHandler, IDbCommand command)
        {
            return GetCollection<TDataStructure>(connectionHandler, command, 0, 0);
        }
        public static List<TDataStructure> GetCollection<TDataStructure>(IConnectionHandler connectionHandler, string commandText, int PageIndex, int pagesize)
        {
            try
            {

                var command = DbProvider.GetDbCommand(connectionHandler, commandText);
                return GetCollection<TDataStructure>(connectionHandler, command, PageIndex, pagesize);

            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public static List<TDataStructure> GetCollection<TDataStructure>(IConnectionHandler connectionHandler, IDbCommand command, int PageIndex, int pagesize)
        {
            try
            {

                var dt = GetDataTable(connectionHandler, command);
                var columnNames = GetDataTableColumnNames(dt);
                var dataRows = dt.Rows.Cast<DataRow>().ToArray();
                var fetchDataHelper = FetchDataHelper.Instance(true);
                var items = (pagesize == 0) ? dataRows : dataRows.Skip(PageIndex * pagesize).Take(pagesize).ToArray();
                var result = new List<TDataStructure>();
                var type = typeof(TDataStructure);
                var infos = type.GetTypePropertiesDictionary();
                var isValueType = Utils.IsValueType<TDataStructure>();
                foreach (var dataRow in dataRows)
                {
                    if (pagesize > 0 && !items.Contains(dataRow))
                    {
                        result.Add(default(TDataStructure));
                        continue;
                    }
                    if (isValueType)
                    {
                        result.Add(SetIsValueTypeValue<TDataStructure>(dataRow[0]));
                        continue;
                    }
                    var dataStructure = Activator.CreateInstance(type);
                    SetObjectPropertiesValue(dataRow, dataStructure, columnNames, infos);
                    result.Add((TDataStructure)dataStructure);
                }

                if (!isValueType)
                    GetLanguageContents(connectionHandler, fetchDataHelper, result.ToArray());
                return result;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }





        private static List<TDataStructure> GetCollection<TDataStructure>(IConnectionHandler connectionHandler, IDbCommand command, FetchDataHelper fetchDataHelper)
        {
            return GetCollection<TDataStructure>(connectionHandler, command, fetchDataHelper, 0, 0);
        }
        private static List<TDataStructure> GetCollection<TDataStructure>(IConnectionHandler connectionHandler, IDbCommand command, FetchDataHelper fetchDataHelper, int PageIndex, int PageSize)
        {
            try
            {
                var dt = GetDataTable(connectionHandler, command);
                var dataRows = dt.Rows.Cast<DataRow>().ToArray();
                var items = (PageSize == 0 || (dataRows.Count() < (PageIndex * PageSize))) ? dataRows : dataRows.Skip((PageIndex) * PageSize).Take(PageSize).ToArray();
                GetDataTableColumnNames(fetchDataHelper, dt);
                var isValueType = Utils.IsValueType<TDataStructure>();
                var result = new List<TDataStructure>();
                foreach (var dataRow in dataRows)
                {

                    if (PageSize > 0 && !items.Contains(dataRow))
                    {
                        result.Add(default(TDataStructure));
                        continue;
                    }
                    if (isValueType)
                    {
                        result.Add(SetIsValueTypeValue<TDataStructure>(dataRow[0]));
                        continue;
                    }
                    result.Add(MapObject<TDataStructure>(dataRow, fetchDataHelper));
                }

                if (!isValueType)
                    GetLanguageContents(connectionHandler, fetchDataHelper, result.ToArray());
                return result;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        private static List<KeyValuePair<string, string>> GetKeyValuePairCollection(IConnectionHandler connectionHandler, IDbCommand command)
        {
            try
            {
                var dt = GetDataTable(connectionHandler, command);
                var dataRows = dt.Rows.Cast<DataRow>().ToArray();
                var result = new List<KeyValuePair<string, string>>();
                foreach (var dataRow in dataRows)
                    result.Add(new KeyValuePair<string, string>(dataRow[0].ToString(), dataRow[1].ToString()));
                return result;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        private static List<TResult> GetCollection<TDataStructure, TResult>(IConnectionHandler connectionHandler, IDbCommand command, FetchDataHelper fetchDataHelper, Expression<Func<TDataStructure, TResult>> expression, bool distinct = false)
        {
            try
            {
                var dt = GetDataTable(connectionHandler, command);
                var dataRows = dt.Rows.Cast<DataRow>().ToArray();
                GetDataTableColumnNames(fetchDataHelper, dt);
                var isValueType = Utils.IsValueType<TResult>();
                var result = new List<TResult>();
                var dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                var compile = isValueType ? null : expression.Compile();
                foreach (var dataRow in dataRows)
                {
                    if (isValueType)
                    {
                        result.Add(SetIsValueTypeValue<TResult>(dataRow[0]));
                        continue;
                    }
                    var obj = compile.Invoke(MapObject<TDataStructure>(dataRow, fetchDataHelper));
                    if (!distinct)
                    {
                        result.Add(obj);
                        continue;
                    }
                    var key = obj == null ? String.Empty : String.Join(",", obj.GetObjectKeyValue());
                    string objkey;
                    if (dictionary.TryGetValue(key, out objkey)) continue;
                    dictionary.Add(key, key);
                    result.Add(obj);
                }

                if (!isValueType)
                    GetLanguageContents(connectionHandler, fetchDataHelper, result.ToArray());
                return result;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public static List<dynamic> GetDynamicCollection(IConnectionHandler connectionHandler, string commandText)
        {
            try
            {

                var command = DbProvider.GetDbCommand(connectionHandler, commandText);
                return GetDynamicCollection(connectionHandler, commandText);

            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public static List<dynamic> GetDynamicCollection(IConnectionHandler connectionHandler, DbCommand command)
        {
            var dt = GetDataTable(connectionHandler, command);
            var dataRows = dt.Rows.Cast<DataRow>().ToArray();
            var columnnames = GetDataTableColumnNames(dt);
            var result = new List<dynamic>();
            foreach (DataRow dataRow in dataRows)
            {
                dynamic obj = new ExpandoObject();
                foreach (var columnName in columnnames)
                {
                    MapDynamicObjectColumns(dataRow, columnName, obj);
                }
                result.Add(obj);
            }

            return result;
        }



        #endregion


        ////==========================================================================================================================////

        #region Linq

        internal static dynamic SelectFirstOrDefault<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression)
        {

            return GenerateDynamicFirstOrDefault(connectionHandler, expression);

        }
        internal static dynamic SelectFirstOrDefault<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {

            return GenerateDynamicFirstOrDefault(connectionHandler, expression, conditionsexpression);

        }
        internal static dynamic SelectFirstOrDefault<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, OrderByModel<TDataStructure>[] orderbymodels)
        {

            return GenerateDynamicFirstOrDefault(connectionHandler, expression, null, orderbymodels);

        }
        internal static dynamic SelectFirstOrDefault<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels)
        {
            return GenerateDynamicFirstOrDefault(connectionHandler, expression, conditionsexpression, orderbymodels);

        }
        private static dynamic GenerateDynamicFirstOrDefault<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression = null, OrderByModel<TDataStructure>[] orderbymodels = null)
        {

            try
            {

                var prepareModel = FetchDataHelper.InstanceFilterAssosation(true);
                var command = CreateDynamicSelectCommand(connectionHandler, prepareModel, expression, conditionsexpression, orderbymodels, true);
                return GetDynamicObject(connectionHandler, command);
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }


        internal static List<dynamic> Select<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, bool distinct = false)
        {

            return GenerateDynamicSelect(connectionHandler, expression, null, null, distinct);

        }
        internal static List<dynamic> Select<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {

            return GenerateDynamicSelect(connectionHandler, expression, conditionsexpression, null, distinct);

        }
        internal static List<dynamic> Select<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, OrderByModel<TDataStructure>[] orderbymodels, bool distinct = false)
        {

            return GenerateDynamicSelect(connectionHandler, expression, null, orderbymodels, distinct);

        }
        internal static List<dynamic> Select<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression = null, OrderByModel<TDataStructure>[] orderbymodels = null, bool distinct = false)
        {
            return GenerateDynamicSelect(connectionHandler, expression, conditionsexpression, orderbymodels, distinct);

        }
        private static List<dynamic> GenerateDynamicSelect<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression = null, OrderByModel<TDataStructure>[] orderbymodels = null, bool distinct = false)
        {

            try
            {

                var prepareModel = FetchDataHelper.InstanceFilterAssosation(true);
                var command = CreateDynamicSelectCommand(connectionHandler, prepareModel, expression, conditionsexpression, orderbymodels, false, distinct);
                return GetDynamicCollection(connectionHandler, command);
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }




        internal static List<KeyValuePair<string, string>> SelectKeyValuePair<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, bool distinct = false)
        {

            return GenerateSelectKeyValuePair<TDataStructure>(connectionHandler, DataValueField, DataTextField, null, null, distinct);

        }
        internal static List<KeyValuePair<string, string>> SelectKeyValuePair<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, Expression<Func<TDataStructure, bool>> expression, bool distinct = false)
        {

            return GenerateSelectKeyValuePair(connectionHandler, DataValueField, DataTextField, expression, null, distinct);

        }
        internal static List<KeyValuePair<string, string>> SelectKeyValuePair<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, Expression<Func<TDataStructure, bool>> expression, OrderByModel<TDataStructure>[] orderByModel = null, bool distinct = false)
        {

            return GenerateSelectKeyValuePair(connectionHandler, DataValueField, DataTextField, expression, orderByModel, distinct);

        }
        public static List<KeyValuePair<string, string>> SelectKeyValuePair<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, OrderByModel<TDataStructure>[] orderByModel = null, bool distinct = false)
        {
            return GenerateSelectKeyValuePair(connectionHandler, DataValueField, DataTextField, null, orderByModel, distinct);
        }
        private static List<KeyValuePair<string, string>> GenerateSelectKeyValuePair<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, Expression<Func<TDataStructure, bool>> conditionsexpression = null, OrderByModel<TDataStructure>[] orderByModel = null, bool distinct = false)
        {

            try
            {


                var dataHelper = FetchDataHelper.Instance(true);
                var command = CreateSelectKeyValuePairCommand(connectionHandler, dataHelper, DataValueField, DataTextField, conditionsexpression, orderByModel, distinct);
                return GetKeyValuePairCollection(connectionHandler, command);


            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }





        internal static List<KeyValuePair<string, string>> SelectKeyValuePair<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, string culture, bool distinct = false)
        {

            return GenerateSelectKeyValuePair<TDataStructure>(connectionHandler, DataValueField, DataTextField, culture, null, null, distinct);

        }
        internal static List<KeyValuePair<string, string>> SelectKeyValuePair<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, string culture, Expression<Func<TDataStructure, bool>> expression, bool distinct = false)
        {

            return GenerateSelectKeyValuePair(connectionHandler, DataValueField, DataTextField, culture, expression, null, distinct);

        }
        internal static List<KeyValuePair<string, string>> SelectKeyValuePair<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, string culture, Expression<Func<TDataStructure, bool>> expression, OrderByModel<TDataStructure>[] orderByModel = null, bool distinct = false)
        {

            return GenerateSelectKeyValuePair(connectionHandler, DataValueField, DataTextField, culture, expression, orderByModel, distinct);

        }
        public static List<KeyValuePair<string, string>> SelectKeyValuePair<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, string culture, OrderByModel<TDataStructure>[] orderByModel = null, bool distinct = false)
        {
            return GenerateSelectKeyValuePair(connectionHandler, DataValueField, DataTextField, culture, null, orderByModel, distinct);
        }
        private static List<KeyValuePair<string, string>> GenerateSelectKeyValuePair<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, string culture, Expression<Func<TDataStructure, bool>> conditionsexpression = null, OrderByModel<TDataStructure>[] orderByModel = null, bool distinct = false)
        {

            try
            {


                var dataHelper = FetchDataHelper.Instance(true, culture);
                var command = CreateSelectKeyValuePairCommand(connectionHandler, dataHelper, DataValueField, DataTextField, conditionsexpression, orderByModel, distinct);
                return GetKeyValuePairCollection(connectionHandler, command);


            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }





        internal static List<TDataStructure> SelectTop<TDataStructure>(IConnectionHandler connectionHandler, int topcount, bool simpleload = false)
        {
            return GenerateSelectTop<TDataStructure>(connectionHandler, topcount, null, null, simpleload);

        }
        internal static List<TDataStructure> SelectTop<TDataStructure>(IConnectionHandler connectionHandler, int topcount, OrderByModel<TDataStructure>[] orderByModels, bool simpleload = false)
        {
            return GenerateSelectTop(connectionHandler, topcount, null, orderByModels, simpleload);

        }
        internal static List<TDataStructure> SelectTop<TDataStructure>(IConnectionHandler connectionHandler, int topcount, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            return GenerateSelectTop(connectionHandler, topcount, conditionsexpression, null, simpleload);

        }
        internal static List<TDataStructure> SelectTop<TDataStructure>(IConnectionHandler connectionHandler, int topcount, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels, bool simpleload = false)
        {
            return GenerateSelectTop(connectionHandler, topcount, conditionsexpression, orderbymodels, simpleload);

        }
        private static List<TDataStructure> GenerateSelectTop<TDataStructure>(IConnectionHandler connectionHandler, int topcount, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels, bool simpleload = false)
        {
            try
            {

                var model = FetchDataHelper.Instance(simpleload);
                var command = CreateSelectTopCommand(connectionHandler, model, topcount, conditionsexpression, orderbymodels);
                return GetCollection<TDataStructure>(connectionHandler, command, model);
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }


        internal static TResult SelectFirstOrDefault<TDataStructure, TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression)
        {

            return GenerateSelectFirstOrDefault(connectionHandler, expression);

        }
        internal static TResult SelectFirstOrDefault<TDataStructure, TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, OrderByModel<TDataStructure>[] orderByModel)
        {

            return GenerateSelectFirstOrDefault(connectionHandler, expression, null, orderByModel);

        }
        internal static TResult SelectFirstOrDefault<TDataStructure, TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {

            return GenerateSelectFirstOrDefault(connectionHandler, expression, conditionsexpression);

        }
        internal static TResult SelectFirstOrDefault<TDataStructure, TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModel)
        {

            return GenerateSelectFirstOrDefault(connectionHandler, expression, conditionsexpression, orderByModel);

        }
        private static TResult GenerateSelectFirstOrDefault<TDataStructure, TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression = null, OrderByModel<TDataStructure>[] orderByModel = null)
        {

            try
            {
                var isValueType = Utils.IsValueType<TResult>();

                var dataHelper = FetchDataHelper.InstanceFilterAssosation(isValueType);
                var command = CreateSelectCommand(connectionHandler, dataHelper, expression, conditionsexpression, orderByModel, true);
                return GetObject(connectionHandler, command, dataHelper, expression);


            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }


        internal static List<TResult> Select<TDataStructure, TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, bool distinct = false)
        {

            return GenerateSelect(connectionHandler, expression, null, null, distinct);

        }
        internal static List<TResult> Select<TDataStructure, TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, OrderByModel<TDataStructure>[] orderByModel, bool distinct = false)
        {

            return GenerateSelect(connectionHandler, expression, null, orderByModel, distinct);

        }
        internal static List<TResult> Select<TDataStructure, TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {

            return GenerateSelect(connectionHandler, expression, conditionsexpression, null, distinct);

        }
        internal static List<TResult> Select<TDataStructure, TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModel, bool distinct = false)
        {

            return GenerateSelect(connectionHandler, expression, conditionsexpression, orderByModel, distinct);

        }
        private static List<TResult> GenerateSelect<TDataStructure, TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression = null, OrderByModel<TDataStructure>[] orderByModel = null, bool distinct = false)
        {

            try
            {


                var isValueType = Utils.IsValueType<TResult>();
                var dataHelper = FetchDataHelper.InstanceFilterAssosation(isValueType);
                var command = CreateSelectCommand(connectionHandler, dataHelper, expression, conditionsexpression, orderByModel, false, distinct);
                return GetCollection(connectionHandler, command, dataHelper, expression, distinct);


            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }






        internal static TDataStructure IncludeFirstOrDefault<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, object>>[] Includeexpression)
        {
            return GenerateIncludeFirstOrDefault(connectionHandler, Includeexpression);

        }
        internal static TDataStructure IncludeFirstOrDefault<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, object>>[] Includeexpression, OrderByModel<TDataStructure>[] orderByModels)
        {
            return GenerateIncludeFirstOrDefault(connectionHandler, Includeexpression, null, orderByModels);

        }
        internal static TDataStructure IncludeFirstOrDefault<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            return GenerateIncludeFirstOrDefault(connectionHandler, Includeexpression, conditionsexpression);

        }
        internal static TDataStructure IncludeFirstOrDefault<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels)
        {
            return GenerateIncludeFirstOrDefault(connectionHandler, Includeexpression, conditionsexpression, orderbymodels);

        }
        private static TDataStructure GenerateIncludeFirstOrDefault<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression = null, OrderByModel<TDataStructure>[] orderbymodels = null)
        {
            try
            {

                var prepareModel = FetchDataHelper.InstanceFilterIncludeAssosation(true, true);
                var command = CreateIncludeCommand(connectionHandler, prepareModel, Includeexpression, conditionsexpression, orderbymodels, true);
                return GetObject<TDataStructure>(connectionHandler, command, prepareModel);
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }


        internal static List<TDataStructure> IncludeTop<TDataStructure>(IConnectionHandler connectionHandler, int topcount, Expression<Func<TDataStructure, object>>[] Includeexpression)
        {
            return GenerateIncludeTop(connectionHandler, topcount, Includeexpression);

        }
        internal static List<TDataStructure> IncludeTop<TDataStructure>(IConnectionHandler connectionHandler, int topcount, Expression<Func<TDataStructure, object>>[] Includeexpression, OrderByModel<TDataStructure>[] orderByModels)
        {
            return GenerateIncludeTop(connectionHandler, topcount, Includeexpression, null, orderByModels);

        }
        internal static List<TDataStructure> IncludeTop<TDataStructure>(IConnectionHandler connectionHandler, int topcount, Expression<Func<TDataStructure, object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            return GenerateIncludeTop(connectionHandler, topcount, Includeexpression, conditionsexpression);

        }
        internal static List<TDataStructure> IncludeTop<TDataStructure>(IConnectionHandler connectionHandler, int topcount, Expression<Func<TDataStructure, object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels)
        {
            return GenerateIncludeTop(connectionHandler, topcount, Includeexpression, conditionsexpression, orderbymodels);

        }
        private static List<TDataStructure> GenerateIncludeTop<TDataStructure>(IConnectionHandler connectionHandler, int topcount, Expression<Func<TDataStructure, object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression = null, OrderByModel<TDataStructure>[] orderbymodels = null)
        {
            try
            {

                var prepareModel = FetchDataHelper.InstanceFilterIncludeAssosation(true, true);
                var command = CreateIncludeCommand(connectionHandler, prepareModel, Includeexpression, conditionsexpression, orderbymodels, true, topcount);
                return GetCollection<TDataStructure>(connectionHandler, command, prepareModel);
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }




        internal static List<TDataStructure> IncludePagedList<TDataStructure>(IConnectionHandler connectionHandler, int pageIndex, int pagesize, Expression<Func<TDataStructure, object>>[] Includeexpression)
        {
            return GenerateIncludePagedList(connectionHandler, pageIndex, pagesize, Includeexpression);

        }
        internal static List<TDataStructure> IncludePagedList<TDataStructure>(IConnectionHandler connectionHandler, int pageIndex, int pagesize, Expression<Func<TDataStructure, object>>[] Includeexpression, OrderByModel<TDataStructure>[] orderByModels)
        {
            return GenerateIncludePagedList(connectionHandler, pageIndex, pagesize, Includeexpression, null, orderByModels);

        }
        internal static List<TDataStructure> IncludePagedList<TDataStructure>(IConnectionHandler connectionHandler, int pageIndex, int pagesize, Expression<Func<TDataStructure, object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            return GenerateIncludePagedList(connectionHandler, pageIndex, pagesize, Includeexpression, conditionsexpression);

        }
        internal static List<TDataStructure> IncludePagedList<TDataStructure>(IConnectionHandler connectionHandler, int pageIndex, int pagesize, Expression<Func<TDataStructure, object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels)
        {
            return GenerateIncludePagedList(connectionHandler, pageIndex, pagesize, Includeexpression, conditionsexpression, orderbymodels);

        }
        private static List<TDataStructure> GenerateIncludePagedList<TDataStructure>(IConnectionHandler connectionHandler, int pageIndex, int pagesize, Expression<Func<TDataStructure, object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression = null, OrderByModel<TDataStructure>[] orderbymodels = null)
        {
            try
            {

                var prepareModel = FetchDataHelper.InstanceFilterIncludeAssosation(true, true);
                var command = CreateIncludeCommand(connectionHandler, prepareModel, Includeexpression, conditionsexpression, orderbymodels);
                return GetCollection<TDataStructure>(connectionHandler, command, prepareModel, pageIndex, pagesize);
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }


        internal static List<TDataStructure> Include<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, object>>[] Includeexpression)
        {
            return GenerateInclude(connectionHandler, Includeexpression);

        }
        internal static List<TDataStructure> Include<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, object>>[] Includeexpression, OrderByModel<TDataStructure>[] orderByModels)
        {
            return GenerateInclude(connectionHandler, Includeexpression, null, orderByModels);

        }
        internal static List<TDataStructure> Include<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            return GenerateInclude(connectionHandler, Includeexpression, conditionsexpression);

        }
        internal static List<TDataStructure> Include<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels)
        {
            return GenerateInclude(connectionHandler, Includeexpression, conditionsexpression, orderbymodels);

        }
        private static List<TDataStructure> GenerateInclude<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression = null, OrderByModel<TDataStructure>[] orderbymodels = null)
        {
            try
            {

                var prepareModel = FetchDataHelper.InstanceFilterIncludeAssosation(true, true);
                var command = CreateIncludeCommand(connectionHandler, prepareModel, Includeexpression, conditionsexpression, orderbymodels);
                return GetCollection<TDataStructure>(connectionHandler, command, prepareModel);
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }


        internal static List<TDataStructure> PagedList<TDataStructure>(IConnectionHandler connectionHandler, int pageIndex, int pagesize, bool simpleload = false)
        {
            return GeneratePaged<TDataStructure>(connectionHandler, pageIndex, pagesize, null, null, simpleload);

        }
        internal static List<TDataStructure> PagedList<TDataStructure>(IConnectionHandler connectionHandler, int pageIndex, int pagesize, OrderByModel<TDataStructure>[] orderByModels, bool simpleload = false)
        {
            return GeneratePaged(connectionHandler, pageIndex, pagesize, null, orderByModels, simpleload);

        }
        internal static List<TDataStructure> PagedList<TDataStructure>(IConnectionHandler connectionHandler, int pageIndex, int pagesize, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            return GeneratePaged(connectionHandler, pageIndex, pagesize, conditionsexpression, null, simpleload);

        }
        internal static List<TDataStructure> PagedList<TDataStructure>(IConnectionHandler connectionHandler, int pageIndex, int pagesize, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels, bool simpleload = false)
        {
            return GeneratePaged(connectionHandler, pageIndex, pagesize, conditionsexpression, orderbymodels, simpleload);

        }
        private static List<TDataStructure> GeneratePaged<TDataStructure>(IConnectionHandler connectionHandler, int pageIndex, int pagesize, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels, bool simpleload = false)
        {
            try
            {

                var prepareModel = FetchDataHelper.Instance(simpleload);
                var command = CreateSelectCommand(connectionHandler, prepareModel, conditionsexpression, orderbymodels);
                return GetCollection<TDataStructure>(connectionHandler, command, prepareModel, pageIndex, pagesize);
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }

        internal static List<TDataStructure> GetAggregation<TDataStructure>(IConnectionHandler connectionHandler, PropertyInfo property, Object obj) where TDataStructure : class
        {
            try
            {
                var prepareModel = FetchDataHelper.Instance();
                var command = CreateAggregationSelectCommand<TDataStructure>(connectionHandler, prepareModel, property, obj);
                return GetCollection<TDataStructure>(connectionHandler, command, prepareModel);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        internal static List<TDataStructure> Where<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, bool>> expression, bool simpleload = false) where TDataStructure : class
        {
            try
            {
                var prepareModel = FetchDataHelper.Instance(simpleload);
                var command = CreateSelectCommand(connectionHandler, prepareModel, expression);
                return GetCollection<TDataStructure>(connectionHandler, command, prepareModel);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }


        private static TResult GetAggrigateGeneric<TDataStructure, TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, AggrigateFuntionType aggrigateFuntionType)
        {
            try
            {

                var fetchDataHelper = FetchDataHelper.InstanceFilterAssosation(true);
                var command = DbProvider.GetDbCommand(connectionHandler);
                var queryBuilder = new ExpressionTranslatorQuery().TranslateSelectColumnExpression(command, fetchDataHelper, expression, CheckIsNullInAggrigateColumn(aggrigateFuntionType));
                return GetAggrigateColumn<TDataStructure, TResult>(connectionHandler, command, fetchDataHelper, queryBuilder.Key, conditionsexpression, aggrigateFuntionType);

            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }



        private static TResult GetAggrigateColumn<TDataStructure, TResult>(IConnectionHandler connectionHandler, DbCommand command, FetchDataHelper prepareModel, string columnname, Expression<Func<TDataStructure, bool>> conditionsexpression, AggrigateFuntionType aggrigateFuntionType, bool distinct = false)
        {
            try
            {
                if (string.IsNullOrEmpty(columnname)) return default(TResult);
                command.CommandText = CreateAggrigateFuntionCommand(command, prepareModel, columnname, conditionsexpression, aggrigateFuntionType, distinct);
                return ExecuteScalar<TResult>(connectionHandler, command);




            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }


        internal static TResult Sum<TDataStructure, TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression = null)
        {
            return GetAggrigateGeneric(connectionHandler, expression, conditionsexpression, AggrigateFuntionType.Sum);
        }
        internal static TResult Min<TDataStructure, TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression = null)
        {
            return GetAggrigateGeneric(connectionHandler, expression, conditionsexpression, AggrigateFuntionType.Min);
        }



        internal static List<dynamic> GroupBy<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {
            return GenerateGroupBy(connectionHandler, groupexpression, null, conditionsexpression, null, null, distinct);
        }
        internal static List<dynamic> GroupBy<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, bool distinct = false)
        {
            return GenerateGroupBy(connectionHandler, groupexpression, null, null, null, null, distinct);
        }
        internal static List<dynamic> GroupBy<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            return GenerateGroupBy(connectionHandler, groupexpression, null, null, null, orderbymodel, distinct);
        }
        internal static List<dynamic> GroupBy<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            return GenerateGroupBy(connectionHandler, groupexpression, null, conditionsexpression, null, orderbymodel, distinct);
        }

        internal static List<dynamic> GroupBy<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, bool distinct = false)
        {
            return GenerateGroupBy(connectionHandler, groupexpression, aggrigateexpression, null, null, null, distinct);
        }
        internal static List<dynamic> GroupBy<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {
            return GenerateGroupBy(connectionHandler, groupexpression, aggrigateexpression, conditionsexpression, null, null, distinct);
        }
        internal static List<dynamic> GroupBy<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            return GenerateGroupBy(connectionHandler, groupexpression, aggrigateexpression, null, null, orderbymodel, distinct);
        }
        internal static List<dynamic> GroupBy<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            return GenerateGroupBy(connectionHandler, groupexpression, aggrigateexpression, conditionsexpression, null, orderbymodel, distinct);
        }


        internal static List<dynamic> GroupByWithHaving<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, bool distinct = false)
        {
            return GenerateGroupBy(connectionHandler, groupexpression, null, conditionsexpression, havingconditionsexpression, null, distinct);
        }
        internal static List<dynamic> GroupByWithHaving<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, OrderByModel<TDataStructure>[] orderbymode, bool distinct = false)
        {

            return GenerateGroupBy(connectionHandler, groupexpression, null, conditionsexpression, havingconditionsexpression, orderbymode, distinct);
        }
        internal static List<dynamic> GroupByWithHaving<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, bool distinct = false)
        {
            return GenerateGroupBy(connectionHandler, groupexpression, null, null, havingconditionsexpression, null, distinct);
        }
        internal static List<dynamic> GroupByWithHaving<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, OrderByModel<TDataStructure>[] orderbymode, bool distinct = false)
        {

            return GenerateGroupBy(connectionHandler, groupexpression, null, null, havingconditionsexpression, orderbymode, distinct);
        }



        internal static List<dynamic> GroupByWithHaving<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, bool distinct = false)
        {
            return GenerateGroupBy(connectionHandler, groupexpression, aggrigateexpression, null, havingconditionsexpression, null, distinct);
        }
        internal static List<dynamic> GroupByWithHaving<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, bool distinct = false)
        {
            return GenerateGroupBy(connectionHandler, groupexpression, aggrigateexpression, conditionsexpression, havingconditionsexpression, null, distinct);
        }
        internal static List<dynamic> GroupByWithHaving<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {

            return GenerateGroupBy(connectionHandler, groupexpression, aggrigateexpression, null, havingconditionsexpression, orderbymodel, distinct);
        }
        internal static List<dynamic> GroupByWithHaving<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {

            return GenerateGroupBy(connectionHandler, groupexpression, aggrigateexpression, conditionsexpression, havingconditionsexpression, orderbymodel, distinct);
        }
        private static List<dynamic> GenerateGroupBy<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> conditionsexpression = null, Expression<Func<TDataStructure, bool>> havingconditionsexpression = null, OrderByModel<TDataStructure>[] orderbymodel = null, bool distinct = false)
        {
            try
            {

                var prepareModel = FetchDataHelper.InstanceFilterAssosation(true);
                var command = CreateGroupByCommand(connectionHandler, prepareModel, groupexpression, aggrigateexpression, conditionsexpression, havingconditionsexpression, orderbymodel, distinct);
                return GetDynamicCollection(connectionHandler, command);
            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }



        internal static TResult Max<TDataStructure, TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression = null)
        {
            return GetAggrigateGeneric(connectionHandler, expression, conditionsexpression, AggrigateFuntionType.Max);
        }
        internal static double Average<TDataStructure, TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression = null)
        {

            var fetchDataHelper = FetchDataHelper.InstanceFilterAssosation(true);
            var command = DbProvider.GetDbCommand(connectionHandler);
            var queryBuilder = new ExpressionTranslatorQuery().TranslateSelectColumnExpression(command, fetchDataHelper, expression, CheckIsNullInAggrigateColumn(AggrigateFuntionType.AVG));
            return GetAggrigateColumn<TDataStructure, double>(connectionHandler, command, fetchDataHelper, queryBuilder.Key, conditionsexpression, AggrigateFuntionType.AVG);

        }


        internal static List<TDataStructure> OrderBy<TDataStructure>(IConnectionHandler connectionHandler, OrderByModel<TDataStructure>[] expression, bool simpleload = false)
        {
            return GenerateOrderBy(connectionHandler, expression, null, simpleload);
        }
        internal static List<TDataStructure> OrderBy<TDataStructure>(IConnectionHandler connectionHandler, OrderByModel<TDataStructure>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            return GenerateOrderBy(connectionHandler, expression, conditionsexpression, simpleload);
        }
        internal static List<TDataStructure> GenerateOrderBy<TDataStructure>(IConnectionHandler connectionHandler, OrderByModel<TDataStructure>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            try
            {
                var model = FetchDataHelper.Instance(simpleload);
                var command = CreateSelectCommand(connectionHandler, model, conditionsexpression, expression);
                return GetCollection<TDataStructure>(connectionHandler, command, model);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }


        internal static List<TDataStructure> OrderBy<TDataStructure, TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> expression, bool simpleload = false)
        {
            return GenerateOrderBy(connectionHandler, expression, null, OrderType.ASC, simpleload);
        }
        internal static List<TDataStructure> OrderBy<TDataStructure, TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            return GenerateOrderBy(connectionHandler, expression, conditionsexpression, OrderType.ASC, simpleload);
        }
        internal static List<TDataStructure> OrderByDescending<TDataStructure, TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            return GenerateOrderBy(connectionHandler, expression, conditionsexpression, OrderType.DESC, simpleload);
        }
        internal static List<TDataStructure> OrderByDescending<TDataStructure, TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> expression, bool simpleload = false)
        {
            return GenerateOrderBy(connectionHandler, expression, null, OrderType.DESC, simpleload);
        }
        private static List<TDataStructure> GenerateOrderBy<TDataStructure, TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderType orderType, bool simpleload = false)
        {
            try
            {
                var model = FetchDataHelper.Instance(simpleload);

                var command = CreateOrderByCommand(connectionHandler, model, expression, conditionsexpression, orderType);
                return GetCollection<TDataStructure>(connectionHandler, command, model);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }


        internal static TDataStructure FirstOrDefault<TDataStructure>(IConnectionHandler connectionHandler, bool simpleload = false) where TDataStructure : class
        {
            return GenerateFirstOrDefault<TDataStructure>(connectionHandler, null, simpleload);
        }
        internal static TDataStructure FirstOrDefault<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, bool>> expression, bool simpleload = false)
        {
            return GenerateFirstOrDefault(connectionHandler, expression, simpleload);
        }
        internal static TDataStructure GenerateFirstOrDefault<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, bool>> expression, bool simpleload = false)
        {
            try
            {
                var model = FetchDataHelper.Instance(simpleload);

                var command = CreateSelectCommand(connectionHandler, model, expression, true);
                return GetObject<TDataStructure>(connectionHandler, command, model);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }



        internal static TDataStructure FirstOrDefaultWithOrderBy<TDataStructure>(IConnectionHandler connectionHandler, OrderByModel<TDataStructure>[] orderByexpression, bool simpleload = false) where TDataStructure : class
        {
            return GenerateFirstOrDefaultWithOrderBy(connectionHandler, orderByexpression, null, simpleload);
        }
        internal static TDataStructure FirstOrDefaultWithOrderBy<TDataStructure>(IConnectionHandler connectionHandler, OrderByModel<TDataStructure>[] orderByexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false) where TDataStructure : class
        {
            return GenerateFirstOrDefaultWithOrderBy(connectionHandler, orderByexpression, conditionsexpression, simpleload);
        }
        internal static TDataStructure GenerateFirstOrDefaultWithOrderBy<TDataStructure>(IConnectionHandler connectionHandler, OrderByModel<TDataStructure>[] orderByexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false) where TDataStructure : class
        {
            try
            {
                var model = FetchDataHelper.Instance(simpleload);
                var command = CreateSelectCommand(connectionHandler, model, conditionsexpression, orderByexpression, true);
                return GetObject<TDataStructure>(connectionHandler, command, model);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }


        internal static TDataStructure FirstOrDefaultWithOrderBy<TDataStructure, TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, bool simpleload = false)
        {
            return GenerateFirstOrDefaultWithOrderBy(connectionHandler, orderByexpression, null, OrderType.ASC, simpleload);
        }
        internal static TDataStructure FirstOrDefaultWithOrderBy<TDataStructure, TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false) where TDataStructure : class
        {
            return GenerateFirstOrDefaultWithOrderBy(connectionHandler, orderByexpression, conditionsexpression, OrderType.ASC, simpleload);
        }
        internal static TDataStructure FirstOrDefaultWithOrderByDescending<TDataStructure, TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, bool simpleload = false)
        {
            return GenerateFirstOrDefaultWithOrderBy(connectionHandler, orderByexpression, null, OrderType.DESC, simpleload);
        }
        internal static TDataStructure FirstOrDefaultWithOrderByDescending<TDataStructure, TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false) where TDataStructure : class
        {
            return GenerateFirstOrDefaultWithOrderBy(connectionHandler, orderByexpression, conditionsexpression, OrderType.DESC, simpleload);
        }
        private static TDataStructure GenerateFirstOrDefaultWithOrderBy<TDataStructure, TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderType orderType, bool simpleload = false)
        {
            try
            {
                var model = FetchDataHelper.Instance(simpleload);

                var command = CreateOrderByCommand(connectionHandler, model, orderByexpression, conditionsexpression, orderType, true);
                return GetObject<TDataStructure>(connectionHandler, command, model);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        internal static TDataStructure SingleOrDefault<TDataStructure>(IConnectionHandler connectionHandler, bool simpleload = false)
        {
            return SingleOrDefault<TDataStructure>(connectionHandler, null, simpleload);
        }
        internal static TDataStructure SingleOrDefault<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, bool>> expression, bool simpleload = false)
        {
            try
            {
                if (Count(connectionHandler, expression) > 1)
                    throw new KnownException("Sequence contains more than one element");
                return GenerateFirstOrDefault(connectionHandler, expression);

            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        internal static bool Any<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, bool>> expression = null) where TDataStructure : class
        {
            try
            {

                var model = FetchDataHelper.Instance(true);

                var command = CreateAnyCommand(connectionHandler, model, expression);
                return ExecuteScalar<bool>(connectionHandler, command);
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }


        }


        internal static int Count<TDataStructure, TColumn>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TColumn>> expression, bool distinct = true) where TDataStructure : class
        {
            return Count(connectionHandler, expression, null, distinct);
        }
        internal static int Count<TDataStructure, TColumn>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TColumn>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = true) where TDataStructure : class
        {
            try
            {
                var fetchDataHelper = FetchDataHelper.InstanceFilterAssosation(true);
                var command = DbProvider.GetDbCommand(connectionHandler);
                var queryBuilder = new ExpressionTranslatorQuery().TranslateSelectColumnExpression(command, fetchDataHelper, expression);
                return GetAggrigateColumn<TDataStructure, int>(connectionHandler, command, fetchDataHelper, queryBuilder.Key, conditionsexpression, AggrigateFuntionType.Count, distinct);

            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        internal static int Count<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, bool>> conditionsexpression = null)
        {
            var fetchDataHelper = FetchDataHelper.InstanceFilterAssosation(true);
            var command = DbProvider.GetDbCommand(connectionHandler);
            return GetAggrigateColumn<TDataStructure, int>(connectionHandler, command, fetchDataHelper, "*", conditionsexpression, AggrigateFuntionType.Count);
        }


        #endregion

        ////==========================================================================================================================////

        #region GetObject
        internal static TDataStructure GetObject<TDataStructure>(IConnectionHandler connectionHandler, params object[] keys)
        {
            var model = FetchDataHelper.Instance();
            return GetObject<TDataStructure>(connectionHandler, model, keys);
        }
        internal static TDataStructure SimpleGetObject<TDataStructure>(IConnectionHandler connectionHandler, params object[] keys)
        {


            var model = FetchDataHelper.Instance(true);
            return GetObject<TDataStructure>(connectionHandler, model, keys);

        }
        private static TDataStructure GetObject<TDataStructure>(IConnectionHandler connectionHandler, FetchDataHelper model, params object[] keys)
        {
            try
            {



                var dt = GetTableKeys<TDataStructure>(connectionHandler);
                var command = CreateSelectGetObjectCommand<TDataStructure>(connectionHandler, model, keys, dt);
                return GetObject<TDataStructure>(connectionHandler, command, model);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public static TDataStructure GetObject<TDataStructure>(IConnectionHandler connectionHandler, string commandText)
        {
            try
            {

                var command = DbProvider.GetDbCommand(connectionHandler, commandText);
                return GetObject<TDataStructure>(connectionHandler, command);





            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public static TDataStructure GetObject<TDataStructure>(IConnectionHandler connectionHandler, IDbCommand command)
        {
            try
            {
                TDataStructure result = default(TDataStructure);
                var dt = GetDataTable(connectionHandler, command);
                if (dt.Rows.Count <= 0) return result;
                var columnNames = GetDataTableColumnNames(dt);
                var fetchDataHelper = FetchDataHelper.Instance(true);
                var type = typeof(TDataStructure);
                var infos = type.GetTypePropertiesDictionary();
                result = (TDataStructure)Activator.CreateInstance(type);
                var dataRow = dt.Rows[0];
                SetObjectPropertiesValue(dataRow, result, columnNames, infos);
                GetLanguageContents(connectionHandler, fetchDataHelper, result);
                return result;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        private static TDataStructure GetObject<TDataStructure>(IConnectionHandler connectionHandler, IDbCommand command, FetchDataHelper fetchDataHelper)
        {
            try
            {
                TDataStructure result = default(TDataStructure);
                var dt = GetDataTable(connectionHandler, command);
                if (dt.Rows.Count <= 0) return result;
                GetDataTableColumnNames(fetchDataHelper, dt);
                result = MapObject<TDataStructure>(dt.Rows[0], fetchDataHelper);
                GetLanguageContents(connectionHandler, fetchDataHelper, result);
                return result;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        private static TResult GetObject<TDataStructure, TResult>(IConnectionHandler connectionHandler, IDbCommand command, FetchDataHelper fetchDataHelper, Expression<Func<TDataStructure, TResult>> expression)
        {
            try
            {
                var dt = GetDataTable(connectionHandler, command);
                TResult result = default(TResult);
                if (dt.Rows.Count <= 0) return result;
                var dataRows = dt.Rows.Cast<DataRow>().ToArray();
                GetDataTableColumnNames(fetchDataHelper, dt);
                var isValueType = Utils.IsValueType<TResult>();
                if (isValueType)
                    return SetIsValueTypeValue<TResult>(dataRows[0][0]);
                var compile = expression.Compile();
                result = compile.Invoke(MapObject<TDataStructure>(dataRows[0], fetchDataHelper));
                GetLanguageContents(connectionHandler, fetchDataHelper, result);
                return result;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public static dynamic GetDynamicObject(IConnectionHandler connectionHandler, DbCommand command)
        {

            var dt = GetDataTable(connectionHandler, command);
            if (dt.Rows.Count <= 0) return null;
            var dataRows = dt.Rows.Cast<DataRow>().ToArray();
            var columnnames = GetDataTableColumnNames(dt);
            dynamic obj = new ExpandoObject();
            foreach (var columnName in columnnames)
            {
                MapDynamicObjectColumns(dataRows[0], columnName, obj);
            }

            return obj;

        }

        #endregion

        ////==========================================================================================================================////


        #region MultiLanguage

        internal static TDataStructure GetLanuageContent<TDataStructure>(IConnectionHandler connectionHandler, string culture, params object[] paramskeys)
        {
            var dataHelper = FetchDataHelper.Instance(culture);
            return GetLanuageContent<TDataStructure>(connectionHandler, dataHelper, paramskeys);
        }
        internal static TDataStructure GetLanuageContentsimple<TDataStructure>(IConnectionHandler connectionHandler, string culture, params object[] paramskeys)
        {
            var dataHelper = FetchDataHelper.Instance(true, culture);
            return GetLanuageContent<TDataStructure>(connectionHandler, dataHelper, paramskeys);
        }
        private static TDataStructure GetLanuageContent<TDataStructure>(IConnectionHandler connectionHandler, FetchDataHelper fetchDataHelper, params object[] paramskeys)
        {

            var dataStructure = GetObject<TDataStructure>(connectionHandler, fetchDataHelper, paramskeys);
            if (dataStructure == null) return default(TDataStructure);
            GetLanguageContents(connectionHandler, fetchDataHelper, dataStructure);
            return dataStructure;
        }
        internal static void GetLanuageContent<TDataStructure>(IConnectionHandler connectionHandler, string culture, TDataStructure obj)
        {
            var fetchDataHelper = FetchDataHelper.Instance(culture);
            GetLanguageContents(connectionHandler, fetchDataHelper, new[] { obj });
        }
        internal static void GetLanuageContent<TDataStructure>(IConnectionHandler connectionHandler, string culture, List<TDataStructure> objlist)
        {
            var fetchDataHelper = FetchDataHelper.Instance(culture);
            GetLanguageContents(connectionHandler, fetchDataHelper, objlist.ToArray());

        }
        private static void GetLanguageContents<TDataStructure>(IConnectionHandler connectionHandler, FetchDataHelper mapObjectModel, TDataStructure obj)
        {
            GetLanguageContents(connectionHandler, mapObjectModel, new[] { obj });
        }

        private static void GetLanguageContents<TDataStructure>(IConnectionHandler connectionHandler, FetchDataHelper mapObjectModel, TDataStructure[] objlist)
        {
            if (!HaslanguagecontentTable(connectionHandler)) return;
            if (objlist == null || !objlist.Any()) return;
            var keys = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var info in objlist)
                PrepareQueryLanguageContent(info, keys, mapObjectModel);
            var culture = string.Empty;
            if (mapObjectModel != null && !string.IsNullOrEmpty(mapObjectModel.Culture))
                culture = mapObjectModel.Culture;
            var datarows = GetLanguageContentDataRows(connectionHandler, keys, culture);

            foreach (var info in objlist)
            {
                MapObjectLanguageContent(info, datarows, mapObjectModel);

            }


        }
        private static void DeleteLanguageContent<TDataStructure>(IConnectionHandler connectionHandler, object[] objkeys)
        {
            var transactionImported = false;
            try
            {

                if (!HaslanguagecontentTable(connectionHandler)) return;
                var type = typeof(TDataStructure);
                var keys = LanguageContentKey(type, objkeys);
                if (!keys.Any()) return;

                var keylist = keys.Aggregate("", (s, s1) => s + ("N'" + s1.ToLower() + "'" + ","));
                keylist = keylist.Substring(0, keylist.Length - 1);
                var CommandText = string.Format(" Delete [Common].[LanguageContent] FROM  [Common].[LanguageContent]  WHERE lower([Key]) in ({0}) ", keylist);
                if (connectionHandler.Transaction != null)
                    transactionImported = true;
                if (!transactionImported)
                    connectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                ExecuteNonQuery(connectionHandler, CommandText);
                if (!transactionImported)
                {
                    connectionHandler.CommitTransaction();

                }
            }
            catch (KnownException)
            {
                if (!transactionImported)
                    connectionHandler.RollBack();
                throw;
            }
            catch (Exception ex)
            {
                if (!transactionImported)
                    connectionHandler.RollBack();
                throw new KnownException(Messages.DeletingDataFailed, ex);
            }




        }
        private static void InsertLanguageContent<TDataStructure>(IConnectionHandler connectionHandler, TDataStructure obj)
        {

            var transactionImported = false;
            try
            {

                if (!HaslanguagecontentTable(connectionHandler)) return;
                var culture = ObjCulture(obj);
                var keys = LanguageContentKeyWithValues(obj, culture);
                if (!keys.Any()) return;
                var allowInsertProp = typeof(LanguageContent).GetAllowInsertProperties();
                var CommandText = keys.Aggregate(String.Empty,
                  (current, languageContent) => current + (" " + CreateInsertCommand(languageContent, allowInsertProp) + "\n"));
                if (connectionHandler.Transaction != null)
                    transactionImported = true;
                if (!transactionImported)
                    connectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                ExecuteNonQuery(connectionHandler, CommandText);
                if (!transactionImported)
                {
                    connectionHandler.CommitTransaction();

                }

            }
            catch (KnownException)
            {
                if (!transactionImported)
                    connectionHandler.RollBack();
                throw;
            }
            catch (Exception ex)
            {
                if (!transactionImported)
                    connectionHandler.RollBack();
                throw new KnownException(Messages.SavingDataFailed, ex);
            }

        }
        private static void UpdateLanguageContent<TDataStructure>(IConnectionHandler connectionHandler, TDataStructure obj)
        {

            var transactionImported = false;
            try
            {

                if (!HaslanguagecontentTable(connectionHandler)) return;
                var culture = ObjCulture(obj);
                var keys = LanguageContentKeyWithValues(obj, culture);
                if (!keys.Any()) return;

                var CommandText = "";
                var allowUpdateProp = typeof(LanguageContent).GetAllowUpdateProperties();
                var allowInsertProp = typeof(LanguageContent).GetAllowInsertProperties();
                var datarows = GetLanguageContentDataRows(connectionHandler, keys.Select(x => x.Key).ToDictionary(s => s), culture);
                foreach (var languageContent in keys)
                {

                    if (datarows != null && datarows.Any(x => x.Key.ToLower() == languageContent.Key.ToLower()))
                        CommandText += CreateUpdateCommand(languageContent, allowUpdateProp) + "\n";
                    else
                        CommandText += CreateInsertCommand(languageContent, allowInsertProp) + "\n";

                }
                if (connectionHandler.Transaction != null)
                    transactionImported = true;
                if (!transactionImported)
                    connectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                ExecuteNonQuery(connectionHandler, CommandText);
                if (!transactionImported)
                {
                    connectionHandler.CommitTransaction();

                }

            }
            catch (KnownException)
            {
                if (!transactionImported)
                    connectionHandler.RollBack();
                throw;
            }
            catch (Exception ex)
            {
                if (!transactionImported)
                    connectionHandler.RollBack();
                throw new KnownException(Messages.SavingDataFailed, ex);
            }

        }




        private static List<LanguageContent> GetLanguageContentDataRows(IConnectionHandler connectionHandler, Dictionary<string, string> keys, string culture = null)
        {

            if (!keys.Any()) return null;
            var keylist = keys.Select(x => x.Key.ToLower()).Aggregate("", (s, s1) => s + ("N'" + s1 + "'" + ","));
            keylist = keylist.Substring(0, keylist.Length - 1);
            var CommandText = string.Format("SELECT * FROM  [Common].[LanguageContent]  WHERE lower([Key]) in ({0}) {1} ", keylist, (!string.IsNullOrEmpty(culture) ? string.Format(" and [Common].[LanguageContent].[LanguageId]='{0}' ", culture) : ""));
            var command = DbProvider.GetDbCommand(connectionHandler, CommandText);
            return GetCollection<LanguageContent>(connectionHandler, command);



        }
        internal static bool HaslanguagecontentTable(IConnectionHandler connectionHandler)
        {
            var key = "Radyn.Framework.DbHelper.DBManager.HasMultiLangTable";
            var obj = HasTableCashCache.StorageCache[key];
            if (obj != null) return (bool)obj;
            var hasMultiLangTable = ChecklanguagecontentTable(connectionHandler);
            HasTableCashCache.StorageCache.Add(key, hasMultiLangTable);
            return hasMultiLangTable;

        }
        private static bool ChecklanguagecontentTable(IConnectionHandler connectionHandler)
        {

            var commandText =
                "SELECT SCHEMA_NAME(schema_id) + '.' + name as TableName from sys.tables where" +
                " lower(SCHEMA_NAME(schema_id) + '.' + name) = 'common.languagecontent' ORDER BY name";
            return GetDataTable(connectionHandler, commandText).Rows.Count > 0;
        }



        #endregion

        ////==========================================================================================================================//// 
        #endregion


        #region Async
        #region FrameworktoolsAsync


        private static async Task<DataTable> GetDataTableAsync(IConnectionHandler connectionHandler, string commandText)
        {
            var dt = new DataTable();
            try
            {
                if (string.IsNullOrEmpty(commandText)) return dt;
                var command = DbProvider.GetDbCommand(connectionHandler, commandText);
                var reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
                dt.Load(reader);
                reader.Close();
                return dt;


            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        private static async Task<DataTable> GetDataTableAsync(IConnectionHandler connectionHandler, IDbCommand command)
        {
            var dt = new DataTable();
            try
            {
                if (string.IsNullOrEmpty(command.CommandText)) return dt;
                foreach (var param in command.Parameters.OfType<IDataParameter>())
                {
                    try
                    {
                        if (param.DbType == System.Data.DbType.String)
                            param.Value = param.Value.ToString();
                    }
                    catch
                    {

                    }
                }

                DbProvider.GetDbCommand(connectionHandler, command);
                var reader = await ((DbCommand)command).ExecuteReaderAsync().ConfigureAwait(false);
                dt.Load(reader);
                reader.Close();
                return dt;

            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public static async Task<int> ExecuteNonQueryAsync(IConnectionHandler connectionHandler, string commandText)
        {
            try
            {

                var command = DbProvider.GetDbCommand(connectionHandler, commandText);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                var str = commandText.ToLower().Split();
                var message = str[0] == "delete" ? Messages.DeletingDataFailed : Messages.SavingDataFailed;
                var knownException = new KnownException(message, ex);
                throw knownException;
            }
        }
        public static async Task<int> ExecuteNonQueryAsync(IConnectionHandler connectionHandler, IDbCommand command)
        {
            try
            {

                if (string.IsNullOrEmpty(command.CommandText)) return 1;
                DbProvider.GetDbCommand(connectionHandler, command);
                return await ((DbCommand)command).ExecuteNonQueryAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.SavingDataFailed, ex);
            }
        }

        public static async Task<object> ExecuteScalarAsync(IConnectionHandler connectionHandler, string commandText)
        {
            try
            {

                var command = DbProvider.GetDbCommand(connectionHandler, commandText);
                var retVal = await command.ExecuteScalarAsync().ConfigureAwait(false);
                if (retVal is DBNull)
                    return null;
                return retVal;

            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public static async Task<object> ExecuteScalarAsync(IConnectionHandler connectionHandler, IDbCommand command)
        {
            try
            {

                DbProvider.GetDbCommand(connectionHandler, command);
                var retVal = await ((DbCommand)command).ExecuteScalarAsync().ConfigureAwait(false);
                if (retVal is DBNull)
                    return null;
                return retVal;

            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        private static async Task<DataTable> GetTableKeysAsync<TDataStructure>(IConnectionHandler connectionHandler)
        {
            var commandText =
                "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE (TABLE_NAME = '" +
                typeof(TDataStructure).Name + "') AND (TABLE_SCHEMA= '" +
                typeof(TDataStructure).GetSchema().SchemaName +
                "') AND (CONSTRAINT_NAME LIKE 'pk%') order by ORDINAL_POSITION";
            return await GetDataTableAsync(connectionHandler, commandText);
        }

        public static async Task<TResult> ExecuteScalarAsync<TResult>(IConnectionHandler connectionHandler, IDbCommand command)
        {
            try
            {

                DbProvider.GetDbCommand(connectionHandler, command);
                var result = await ((DbCommand)command).ExecuteScalarAsync().ConfigureAwait(false);
                if (result == DBNull.Value || result == null)
                    return default(TResult);
                try
                {
                    return (TResult)result;
                }
                catch
                {

                    return (TResult)Convert.ChangeType(result, typeof(TResult));
                }



            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public static async Task<TResult> ExecuteScalarAsync<TResult>(IConnectionHandler connectionHandler, string commandText)
        {
            try
            {

                var command = DbProvider.GetDbCommand(connectionHandler, commandText);
                var result = await command.ExecuteScalarAsync().ConfigureAwait(false);
                if (result == DBNull.Value || result == null)
                    return default(TResult);
                try
                {
                    return (TResult)result;
                }
                catch
                {

                    return (TResult)Convert.ChangeType(result, typeof(TResult));
                }

            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        private static async Task CheckUniqueAsync<TDataStructure>(IConnectionHandler connectionHandler, TDataStructure obj)
        {
            var str = await GetUniqueWhereCluaseAsync(connectionHandler, obj);
            if (!string.IsNullOrEmpty(str))
            {
                throw new KnownException(str + Messages.Duplicated);
            }
        }
        private static async Task CheckUniqueWithKeysAsync<TDataStructure>(IConnectionHandler connectionHandler, TDataStructure obj)
        {
            var unique = typeof(TDataStructure).GetUniqueProperties();
            if (unique.Any())
            {
                var inKey = typeof(TDataStructure).GetTypeKeyProperties();
                if (!inKey.Any())
                    throw new KnownException(string.Format("براي جدول {0} هيچ كليدي در نظر گرفته نشده.", typeof(TDataStructure).Name));
                var str = await GetUniqueWhereCluaseAsync(connectionHandler, obj, true);
                if (!string.IsNullOrEmpty(str))
                    throw new KnownException(str + Messages.Duplicated);
            }
        }
        private static async Task<string> GetUniqueWhereCluaseAsync<TDataStructure>(IConnectionHandler connectionHandler, TDataStructure obj, bool forUpdate = false)
        {
            var inKey = typeof(TDataStructure).GetTypeKeyProperties();
            if (!inKey.Any())
                throw new KnownException(string.Format("براي جدول {0} هيچ كليدي در نظر گرفته نشده.",
                                                       typeof(TDataStructure).Name));
            var temp = string.Empty;
            foreach (var property in inKey)
            {
                temp += string.Format("[{0}] <> ", property.Name);
                var propertyValue = Helper.GetPropertyValue(obj, property);
                temp += GetPropertyValue(propertyValue);
                temp += " AND ";
            }

            var key = string.Format(" AND ({0})  ", temp.Substring(0, temp.Length - 5));



            var type = typeof(TDataStructure);
            var unique = typeof(TDataStructure).GetUniqueProperties();
            if (unique.Any())
            {
                var query = string.Format("SELECT Count(*) FROM [{0}].[{1}] WHERE ",
                                        typeof(TDataStructure).GetSchema().SchemaName,
                                        type.Name);
                var uniqueGroup = new Dictionary<string, List<PropertyInfo>>();
                foreach (var propertyInfo in
                    unique.Where(property => !string.IsNullOrEmpty(property.GetPropertyAttributes().Unique.Group)))
                {
                    var groupName = propertyInfo.GetPropertyAttributes().Unique.Group;
                    if (!uniqueGroup.ContainsKey(groupName))
                        uniqueGroup.Add(groupName, new List<PropertyInfo>());
                    uniqueGroup[groupName].Add(propertyInfo);
                }
                string whereClause;
                string commandText;
                foreach (var property in
                    unique.Where(
                        property =>
                        string.IsNullOrEmpty(property.GetPropertyAttributes().Unique.Group)))
                {
                    var propertyValue = Helper.GetPropertyValue(obj, property);
                    if (propertyValue == null && property.GetPropertyAttributes().Unique.IgnoreNull) continue;
                    whereClause = string.Format("[{0}] = ", property.Name);
                    whereClause += GetPropertyValue(propertyValue);
                    commandText = string.Format("{0}{1}{2}", query, whereClause, (forUpdate ? key : ""));
                    if (await ExecuteScalarAsync<int>(connectionHandler, commandText) > 0)
                        return property.GetPropertyAttributes().Layout.Caption;
                }

                foreach (var ug in uniqueGroup)
                {
                    whereClause = string.Empty;
                    foreach (var property in ug.Value)
                    {
                        var propertyValue = Helper.GetPropertyValue(obj, property);
                        if (propertyValue != null && !propertyValue.Equals("null"))
                        {
                            whereClause += string.Format("[{0}] = ", property.Name);
                            whereClause += GetPropertyValue(propertyValue);
                            whereClause += " AND ";
                        }
                        else if (property.GetPropertyAttributes().Unique.IgnoreNull)
                        {
                            whereClause += string.Format("[{0}] = ", property.Name);
                            whereClause += GetPropertyValue(propertyValue);
                            whereClause += " AND ";
                        }
                    }
                    whereClause = whereClause.Substring(0, whereClause.Length - 5);
                    commandText = string.Format("{0}{1}{2}", query, whereClause, (forUpdate ? key : ""));
                    if (await ExecuteScalarAsync<int>(connectionHandler, commandText) > 0)
                    {
                        var layouts =
                            ug.Value.Select(
                                propertyInfo =>
                                propertyInfo.GetPropertyAttributes().Layout ??
                                new Layout { Caption = propertyInfo.Name });
                        return string.Join(",", layouts.Select(x => x.Caption));
                    }
                }
            }
            return string.Empty;
        }

        #endregion
        ////==========================================================================================================================////
        #region InsertAndUpdateAndDeleteAsync


        private static async Task SetIdentityAsync<TDataStructure>(IConnectionHandler connectionHandler, TDataStructure obj)
        {
            var inKey = typeof(TDataStructure).GetTypeKeyProperties();
            PropertyInfo identity;
            if (!inKey.Any(propertyInfo => propertyInfo.GetPropertyAttributes().Key.Identity))
            {
                inKey = typeof(TDataStructure).GetIdentityProperties();
                if (!inKey.Any()) return;
                identity = inKey.First();
            }
            else
                identity = inKey.FirstOrDefault(propertyInfo => propertyInfo.GetPropertyAttributes().Key.Identity);

            if (identity == null) return;

            var query = string.Format("select max({0}) from [{1}].[{2}]", identity.Name,
               typeof(TDataStructure).GetSchema().SchemaName, typeof(TDataStructure).Name);
            var id = await ExecuteScalarAsync(connectionHandler, query);
            identity.SetValue(obj, id, null);
        }
        private static async Task<TDataStructure> RealodObjForTrackAsync<TDataStructure>(IConnectionHandler connectionHandler, TDataStructure obj, object[] keys) where TDataStructure : class
        {

            var structureBase = obj as DataStructureBase;
            if (structureBase == null) return null;
            var getobj = await GetObjectAsync<TDataStructure>(connectionHandler, keys);
            var dataStructureBase = getobj as DataStructureBase;
            if (dataStructureBase == null) return null;
            dataStructureBase.RootId = structureBase.RootId;
            dataStructureBase.RootObject = structureBase.RootObject;
            return getobj;

        }
        internal static async Task<int> UpdateAsync<TDataStructure>(IConnectionHandler connectionHandler, TDataStructure obj, Delegates.LogMethod method = null, object[] parameters = null) where TDataStructure : class
        {
            if (Contrans.IgnorePersistData)
            {
                Items.Add(obj, ObjectState.Dirty);
                return 1;
            }

            var transactionImported = false;
            try
            {

                var inKey = typeof(TDataStructure).GetTypeKeyProperties();
                if (inKey == null || !inKey.Any())
                    throw new Exception("کلیدی برای جدول مورد نظر وجود ندارد");

                var keys = obj.GetObjectKeyValue();
                TDataStructure oldObj = null;
                var enableTrack = EnableTrack(obj);
                if (enableTrack)
                    oldObj = await RealodObjForTrackAsync(connectionHandler, obj, keys);
                await CheckUniqueWithKeysAsync(connectionHandler, obj);
                var commandText = CreateUpdateCommand(obj);
                if (connectionHandler.Transaction != null)
                    transactionImported = true;
                if (!transactionImported)
                    connectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                var result = await ExecuteNonQueryAsync(connectionHandler, commandText);
                if (result > 0)
                {

                    await UpdateLanguageContentAsync(connectionHandler, obj);
                    if (enableTrack && oldObj != null)
                    {
                        obj = await GetObjectAsync<TDataStructure>(connectionHandler, keys);
                        await InsertTrackersOnUpdateAsync(connectionHandler, obj, oldObj, method, parameters);
                    }


                }


                if (!transactionImported)
                {
                    connectionHandler.CommitTransaction();

                }

                return result;
            }
            catch (KnownException)
            {
                if (!transactionImported)
                    connectionHandler.RollBack();
                throw;
            }
            catch (Exception ex)
            {
                if (!transactionImported)
                    connectionHandler.RollBack();
                throw new KnownException(Messages.SavingDataFailed, ex);
            }
        }
        internal static async Task<int> DeleteAsync<TDataStructure>(IConnectionHandler connectionHandler, params object[] keys) where TDataStructure : class
        {
            return await DeleteAsync<TDataStructure>(connectionHandler, null, keys);
        }
        internal static async Task<int> DeleteAsync<TDataStructure>(IConnectionHandler connectionHandler, TDataStructure obj, Delegates.LogMethod method = null, object[] parameters = null) where TDataStructure : class
        {
            return await DeleteAsync(connectionHandler, obj, null, method, parameters);
        }
        private static async Task<int> DeleteAsync<TDataStructure>(IConnectionHandler connectionHandler, TDataStructure obj, object[] keys, Delegates.LogMethod method = null, object[] parameters = null) where TDataStructure : class
        {
            if (keys == null && obj != null)
                keys = obj.GetObjectKeyValue();
            if (Contrans.IgnorePersistData)
            {
                Items.Add(keys, ObjectState.Deleted);
                return 1;
            }

            var transactionImported = false;
            try
            {

                var oldObj = obj;
                var enableTrack = EnableTrack(obj);
                if (enableTrack && oldObj == null)
                    oldObj = await GetObjectAsync<TDataStructure>(connectionHandler, keys);
                var dt = await GetTableKeysAsync<TDataStructure>(connectionHandler);
                var CommandText = CreateDeleteCommand<TDataStructure>(keys, dt);

                if (connectionHandler.Transaction != null)
                    transactionImported = true;
                if (!transactionImported)
                    connectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                var result = await ExecuteNonQueryAsync(connectionHandler, CommandText);
                if (result > 0)
                {
                    await DeleteLanguageContentAsync<TDataStructure>(connectionHandler, keys);
                    if (enableTrack && oldObj != null)
                        await InsertTrackersOnDeleteAsync(connectionHandler, oldObj, method, parameters);

                }

                if (!transactionImported)
                {

                    connectionHandler.CommitTransaction();

                }



                return result;
            }
            catch (KnownException)
            {
                if (!transactionImported)
                    connectionHandler.RollBack();
                throw;
            }
            catch (Exception ex)
            {
                if (!transactionImported)
                    connectionHandler.RollBack();
                throw new KnownException(Messages.DeletingDataFailed, ex);
            }
        }
        internal static async Task<int> InsertAsync<TDataStructure>(IConnectionHandler connectionHandler, TDataStructure obj, Delegates.LogMethod method = null, object[] parameters = null) where TDataStructure : class
        {

            var inKey = typeof(TDataStructure).GetTypeKeyProperties();
            if (inKey == null || !inKey.Any())
                throw new Exception("کلیدی برای جدول مورد نظر وجود ندارد");
            if (Contrans.IgnorePersistData)
            {
                Items.Add(obj, ObjectState.New);
                return 1;
            }
            var transactionImported = false;

            try
            {

                await CheckUniqueAsync(connectionHandler, obj);
                var commandText = CreateInsertCommand(obj);
                var hasIdentity = Helper.HasIdentity<TDataStructure>();
                if (hasIdentity && string.IsNullOrEmpty(commandText)) return 0;

                if (connectionHandler.Transaction != null)
                    transactionImported = true;
                if (!transactionImported)
                    connectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                var result = await ExecuteNonQueryAsync(connectionHandler, commandText);
                if (hasIdentity)
                    await SetIdentityAsync(connectionHandler, obj);
                if (result > 0)
                {

                    await InsertLanguageContentAsync(connectionHandler, obj);
                    if (EnableTrack(obj))
                    {
                        var keys = obj.GetObjectKeyValue();
                        obj = await RealodObjForTrackAsync(connectionHandler, obj, keys);
                        await InsertTrackersOnInsertAsync(connectionHandler, obj, method, parameters);
                    }

                }
                if (!transactionImported)
                {
                    connectionHandler.CommitTransaction();

                }

                return result;
            }
            catch (KnownException)
            {
                if (!transactionImported)
                    connectionHandler.RollBack();
                throw;
            }
            catch (Exception ex)
            {
                if (!transactionImported)
                    connectionHandler.RollBack();
                throw new KnownException(Messages.SavingDataFailed, ex);
            }
        }


        #endregion
        ////==========================================================================================================================////
        #region TrackAsync

        private static async Task<int> InsertTrackersOnInsertAsync<TDataStructure>(IConnectionHandler connectionHandler, TDataStructure obj, Delegates.LogMethod method = null, object[] parameters = null) where TDataStructure : class
        {
            return await InsertTrackersAsync(connectionHandler, obj, null, OperationLogType.InsertRecord, method, parameters);
        }
        private static async Task<int> InsertTrackersOnUpdateAsync<TDataStructure>(IConnectionHandler connectionHandler, TDataStructure obj, TDataStructure oldobj, Delegates.LogMethod method = null, object[] parameters = null) where TDataStructure : class
        {
            return await InsertTrackersAsync(connectionHandler, obj, oldobj, OperationLogType.UpdateField, method, parameters);
        }
        private static async Task<int> InsertTrackersOnDeleteAsync<TDataStructure>(IConnectionHandler connectionHandler, TDataStructure obj, Delegates.LogMethod method = null, object[] parameters = null) where TDataStructure : class
        {
            return await InsertTrackersAsync(connectionHandler, null, obj, OperationLogType.DeleteRecord, method, parameters);
        }

        private static async Task<bool> CheckTrackerTableAsync(IConnectionHandler connectionHandler)
        {

            var commandText =
                "SELECT SCHEMA_NAME(schema_id) + '.' + name as TableName from sys.tables where" +
                " lower(SCHEMA_NAME(schema_id) + '.' + name) = 'dbo.tracker' ORDER BY name";
            var dataTableAsync = await GetDataTableAsync(connectionHandler, commandText);
            return dataTableAsync.Rows.Count > 0;
        }
        private static async Task<bool> HasTrackerTableAsync(IConnectionHandler connectionHandler)
        {
            var key = "Radyn.Framework.DbHelper.DBManager.HasTrackerTable";
            var obj = HasTableCashCache.StorageCache[key];
            if (obj != null) return (bool)obj;
            var hasMultiLangTable = await CheckTrackerTableAsync(connectionHandler);
            HasTableCashCache.StorageCache.Add(key, hasMultiLangTable);
            return hasMultiLangTable;


        }
        private static async Task<int> InsertTrackersAsync<TDataStructure>(IConnectionHandler connectionHandler, TDataStructure obj, TDataStructure oldobj, OperationLogType logType, Delegates.LogMethod method = null, object[] parameters = null) where TDataStructure : class

        {
            var transactionImported = false;
            try
            {

                var trackers = PrepareTrack(obj, oldobj, logType);
                if (!trackers.Any()) return 1;
                if (!await HasTrackerTableAsync(connectionHandler))
                {
                    if (method != null)
                        method(parameters);
                    return 1;
                }

                var type = typeof(Tracker);
                var allowInsertProp = type.GetAllowInsertProperties();
                var commandText = trackers.Aggregate(String.Empty, (current, tracker) =>
                         current + (" " + CreateInsertCommand(tracker, allowInsertProp) + "\n"));

                if (connectionHandler.Transaction != null)
                    transactionImported = true;
                if (!transactionImported)
                    connectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                var result = await ExecuteNonQueryAsync(connectionHandler, commandText);
                if (method != null)
                    method(parameters);

                if (!transactionImported)
                {
                    connectionHandler.CommitTransaction();

                }
                return result;


            }
            catch (KnownException)
            {
                if (!transactionImported)
                    connectionHandler.RollBack();
                throw;
            }
            catch (Exception ex)
            {
                if (!transactionImported)
                    connectionHandler.RollBack();
                throw new KnownException(Messages.SavingDataFailed, ex);
            }


        }





        #endregion
        /////=================================================================================================================================////
        #region CollectionAsync


        internal static async Task<List<TDataStructure>> GetCollectionAsync<TDataStructure>(IConnectionHandler connectionHandler, bool simpleload = false)
        {
            try
            {
                var prepareModel = FetchDataHelper.Instance(simpleload);
                var command = CreateSelectCommand<TDataStructure>(connectionHandler, prepareModel);
                return await GetCollectionAsync<TDataStructure>(connectionHandler, command, prepareModel);




            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public static async Task<List<TDataStructure>> GetCollectionAsync<TDataStructure>(IConnectionHandler connectionHandler, string commandText)
        {
            try
            {

                var command = DbProvider.GetDbCommand(connectionHandler, commandText);
                return await GetCollectionAsync<TDataStructure>(connectionHandler, command, 0, 0);

            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public static async Task<List<TDataStructure>> GetCollectionAsync<TDataStructure>(IConnectionHandler connectionHandler, IDbCommand command)
        {
            return await GetCollectionAsync<TDataStructure>(connectionHandler, command, 0, 0);
        }
        public static async Task<List<TDataStructure>> GetCollectionAsync<TDataStructure>(IConnectionHandler connectionHandler, string commandText, int PageIndex, int pagesize)
        {
            try
            {

                var command = DbProvider.GetDbCommand(connectionHandler, commandText);
                return await GetCollectionAsync<TDataStructure>(connectionHandler, command, PageIndex, pagesize);

            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public static async Task<List<TDataStructure>> GetCollectionAsync<TDataStructure>(IConnectionHandler connectionHandler, IDbCommand command, int PageIndex, int pagesize)
        {
            try
            {

                var dt = await GetDataTableAsync(connectionHandler, command);
                var columnNames = GetDataTableColumnNames(dt);
                var dataRows = dt.Rows.Cast<DataRow>().ToArray();
                var fetchDataHelper = FetchDataHelper.Instance(true);
                var items = (pagesize == 0) ? dataRows : dataRows.Skip(PageIndex * pagesize).Take(pagesize).ToArray();
                var result = new List<TDataStructure>();
                var type = typeof(TDataStructure);
                var infos = type.GetTypePropertiesDictionary();
                var isValueType = Utils.IsValueType<TDataStructure>();
                foreach (var dataRow in dataRows)
                {
                    if (pagesize > 0 && !items.Contains(dataRow))
                    {
                        result.Add(default(TDataStructure));
                        continue;
                    }
                    if (isValueType)
                    {
                        result.Add(SetIsValueTypeValue<TDataStructure>(dataRow[0]));
                        continue;
                    }
                    var dataStructure = Activator.CreateInstance(type);
                    SetObjectPropertiesValue(dataRow, dataStructure, columnNames, infos);
                    result.Add((TDataStructure)dataStructure);
                }

                if (!isValueType)
                    await GetLanguageContentsAsync(connectionHandler, fetchDataHelper, result.ToArray());
                return result;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }





        private static async Task<List<TDataStructure>> GetCollectionAsync<TDataStructure>(IConnectionHandler connectionHandler, IDbCommand command, FetchDataHelper fetchDataHelper)
        {
            return await GetCollectionAsync<TDataStructure>(connectionHandler, command, fetchDataHelper, 0, 0);
        }
        private static async Task<List<TDataStructure>> GetCollectionAsync<TDataStructure>(IConnectionHandler connectionHandler, IDbCommand command, FetchDataHelper fetchDataHelper, int PageIndex, int PageSize)
        {
            try
            {
                var dt = await GetDataTableAsync(connectionHandler, command);
                var dataRows = dt.Rows.Cast<DataRow>().ToArray();

                var items = (PageSize == 0 || (dataRows.Count() < (PageIndex * PageSize))) ? dataRows : dataRows.Skip((PageIndex) * PageSize).Take(PageSize).ToArray();
                GetDataTableColumnNames(fetchDataHelper, dt);
                var isValueType = Utils.IsValueType<TDataStructure>();
                var result = new List<TDataStructure>();
                foreach (var dataRow in dataRows)
                {

                    if (PageSize > 0 && !items.Contains(dataRow))
                    {
                        result.Add(default(TDataStructure));
                        continue;
                    }
                    if (isValueType)
                    {
                        result.Add(SetIsValueTypeValue<TDataStructure>(dataRow[0]));
                        continue;
                    }
                    result.Add(MapObject<TDataStructure>(dataRow, fetchDataHelper));
                }

                if (!isValueType)
                    await GetLanguageContentsAsync(connectionHandler, fetchDataHelper, result.ToArray());
                return result;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        private static async Task<List<KeyValuePair<string, string>>> GetKeyValuePairCollectionAsync(IConnectionHandler connectionHandler, IDbCommand command)
        {
            try
            {
                var dt = await GetDataTableAsync(connectionHandler, command);
                var dataRows = dt.Rows.Cast<DataRow>().ToArray();
                var result = new List<KeyValuePair<string, string>>();
                foreach (var dataRow in dataRows)
                    result.Add(new KeyValuePair<string, string>(dataRow[0].ToString(), dataRow[1].ToString()));
                return result;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        private static async Task<List<TResult>> GetCollectionAsync<TDataStructure, TResult>(IConnectionHandler connectionHandler, IDbCommand command, FetchDataHelper fetchDataHelper, Expression<Func<TDataStructure, TResult>> expression, bool distinct = false)
        {
            try
            {
                var dt = await GetDataTableAsync(connectionHandler, command);
                var dataRows = dt.Rows.Cast<DataRow>().ToArray();
                GetDataTableColumnNames(fetchDataHelper, dt);
                var isValueType = Utils.IsValueType<TResult>();
                var result = new List<TResult>();
                var dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                var compile = isValueType ? null : expression.Compile();
                foreach (var dataRow in dataRows)
                {
                    if (isValueType)
                    {
                        result.Add(SetIsValueTypeValue<TResult>(dataRow[0]));
                        continue;
                    }
                    var obj = compile.Invoke(MapObject<TDataStructure>(dataRow, fetchDataHelper));
                    if (!distinct)
                    {
                        result.Add(obj);
                        continue;
                    }
                    var key = obj == null ? String.Empty : String.Join(",", obj.GetObjectKeyValue());
                    string objkey;
                    if (dictionary.TryGetValue(key, out objkey)) continue;
                    dictionary.Add(key, key);
                    result.Add(obj);
                }

                if (!isValueType)
                    await GetLanguageContentsAsync(connectionHandler, fetchDataHelper, result.ToArray());
                return result;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }



        public static async Task<List<dynamic>> GetDynamicCollectionAsync(IConnectionHandler connectionHandler, string commandText)
        {
            try
            {

                var command = DbProvider.GetDbCommand(connectionHandler, commandText);
                return await GetDynamicCollectionAsync(connectionHandler, command);

            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public static async Task<List<dynamic>> GetDynamicCollectionAsync(IConnectionHandler connectionHandler, DbCommand command)
        {
            var dt = await GetDataTableAsync(connectionHandler, command);
            var dataRows = dt.Rows.Cast<DataRow>().ToArray();
            var columnnames = GetDataTableColumnNames(dt);
            var result = new List<dynamic>();
            foreach (DataRow dataRow in dataRows)
            {
                dynamic obj = new ExpandoObject();
                foreach (var columnName in columnnames)
                {
                    MapDynamicObjectColumns(dataRow, columnName, obj);
                }
                result.Add(obj);
            }

            return result;
        }



        #endregion
        ////==========================================================================================================================////
        #region LinqAsync

        internal static async Task<dynamic> SelectFirstOrDefaultAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression)
        {

            return await GenerateDynamicFirstOrDefaultAsync(connectionHandler, expression);

        }
        internal static async Task<dynamic> SelectFirstOrDefaultAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {

            return await GenerateDynamicFirstOrDefaultAsync(connectionHandler, expression, conditionsexpression);

        }
        internal static async Task<dynamic> SelectFirstOrDefaultAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, OrderByModel<TDataStructure>[] orderbymodels)
        {

            return await GenerateDynamicFirstOrDefaultAsync(connectionHandler, expression, null, orderbymodels);

        }
        internal static async Task<dynamic> SelectFirstOrDefaultAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels)
        {
            return await GenerateDynamicFirstOrDefaultAsync(connectionHandler, expression, conditionsexpression, orderbymodels);

        }
        private static async Task<dynamic> GenerateDynamicFirstOrDefaultAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression = null, OrderByModel<TDataStructure>[] orderbymodels = null)
        {

            try
            {

                var prepareModel = FetchDataHelper.InstanceFilterAssosation(true);
                var command = CreateDynamicSelectCommand(connectionHandler, prepareModel, expression, conditionsexpression, orderbymodels, true);
                return await GetDynamicObjectAsync(connectionHandler, command);
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }


        internal static async Task<List<dynamic>> SelectAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, bool distinct = false)
        {

            return await GenerateDynamicSelectAsync(connectionHandler, expression, null, null, distinct);

        }
        internal static async Task<List<dynamic>> SelectAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {

            return await GenerateDynamicSelectAsync(connectionHandler, expression, conditionsexpression, null, distinct);

        }
        internal static async Task<List<dynamic>> SelectAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, OrderByModel<TDataStructure>[] orderbymodels, bool distinct = false)
        {

            return await GenerateDynamicSelectAsync(connectionHandler, expression, null, orderbymodels, distinct);

        }
        internal static async Task<List<dynamic>> SelectAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression = null, OrderByModel<TDataStructure>[] orderbymodels = null, bool distinct = false)
        {
            return await GenerateDynamicSelectAsync(connectionHandler, expression, conditionsexpression, orderbymodels, distinct);

        }
        private static async Task<List<dynamic>> GenerateDynamicSelectAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression = null, OrderByModel<TDataStructure>[] orderbymodels = null, bool distinct = false)
        {

            try
            {

                var prepareModel = FetchDataHelper.InstanceFilterAssosation(true);
                var command = CreateDynamicSelectCommand(connectionHandler, prepareModel, expression, conditionsexpression, orderbymodels, false, distinct);
                return await GetDynamicCollectionAsync(connectionHandler, command);
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }




        internal static async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, bool distinct = false)
        {

            return await GenerateSelectKeyValuePairAsync<TDataStructure>(connectionHandler, DataValueField, DataTextField, null, null, distinct);

        }
        internal static async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, Expression<Func<TDataStructure, bool>> expression, bool distinct = false)
        {

            return await GenerateSelectKeyValuePairAsync(connectionHandler, DataValueField, DataTextField, expression, null, distinct);

        }
        internal static async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, Expression<Func<TDataStructure, bool>> expression, OrderByModel<TDataStructure>[] orderByModel = null, bool distinct = false)
        {

            return await GenerateSelectKeyValuePairAsync(connectionHandler, DataValueField, DataTextField, expression, orderByModel, distinct);

        }
        public static async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, OrderByModel<TDataStructure>[] orderByModel = null, bool distinct = false)
        {
            return await GenerateSelectKeyValuePairAsync(connectionHandler, DataValueField, DataTextField, null, orderByModel, distinct);
        }
        private static async Task<List<KeyValuePair<string, string>>> GenerateSelectKeyValuePairAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, Expression<Func<TDataStructure, bool>> conditionsexpression = null, OrderByModel<TDataStructure>[] orderByModel = null, bool distinct = false)
        {

            try
            {


                var dataHelper = FetchDataHelper.Instance(true);
                var command = CreateSelectKeyValuePairCommand(connectionHandler, dataHelper, DataValueField, DataTextField, conditionsexpression, orderByModel, distinct);
                return await GetKeyValuePairCollectionAsync(connectionHandler, command);


            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }





        internal static async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, string culture, bool distinct = false)
        {

            return await GenerateSelectKeyValuePairAsync<TDataStructure>(connectionHandler, DataValueField, DataTextField, culture, null, null, distinct);

        }
        internal static async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, string culture, Expression<Func<TDataStructure, bool>> expression, bool distinct = false)
        {

            return await GenerateSelectKeyValuePairAsync(connectionHandler, DataValueField, DataTextField, culture, expression, null, distinct);

        }
        internal static async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, string culture, Expression<Func<TDataStructure, bool>> expression, OrderByModel<TDataStructure>[] orderByModel = null, bool distinct = false)
        {

            return await GenerateSelectKeyValuePairAsync(connectionHandler, DataValueField, DataTextField, culture, expression, orderByModel, distinct);

        }
        public static async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, string culture, OrderByModel<TDataStructure>[] orderByModel = null, bool distinct = false)
        {
            return await GenerateSelectKeyValuePairAsync(connectionHandler, DataValueField, DataTextField, culture, null, orderByModel, distinct);
        }
        private static async Task<List<KeyValuePair<string, string>>> GenerateSelectKeyValuePairAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, string culture, Expression<Func<TDataStructure, bool>> conditionsexpression = null, OrderByModel<TDataStructure>[] orderByModel = null, bool distinct = false)
        {

            try
            {


                var dataHelper = FetchDataHelper.Instance(true, culture);
                var command = CreateSelectKeyValuePairCommand(connectionHandler, dataHelper, DataValueField, DataTextField, conditionsexpression, orderByModel, distinct);
                return await GetKeyValuePairCollectionAsync(connectionHandler, command);


            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }





        internal static async Task<List<TDataStructure>> SelectTopAsync<TDataStructure>(IConnectionHandler connectionHandler, int topcount, bool simpleload = false)
        {
            return await GenerateSelectTopAsync<TDataStructure>(connectionHandler, topcount, null, null, simpleload);

        }
        internal static async Task<List<TDataStructure>> SelectTopAsync<TDataStructure>(IConnectionHandler connectionHandler, int topcount, OrderByModel<TDataStructure>[] orderByModels, bool simpleload = false)
        {
            return await GenerateSelectTopAsync(connectionHandler, topcount, null, orderByModels, simpleload);

        }
        internal static async Task<List<TDataStructure>> SelectTopAsync<TDataStructure>(IConnectionHandler connectionHandler, int topcount, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            return await GenerateSelectTopAsync(connectionHandler, topcount, conditionsexpression, null, simpleload);

        }
        internal static async Task<List<TDataStructure>> SelectTopAsync<TDataStructure>(IConnectionHandler connectionHandler, int topcount, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels, bool simpleload = false)
        {
            return await GenerateSelectTopAsync(connectionHandler, topcount, conditionsexpression, orderbymodels, simpleload);

        }
        private static async Task<List<TDataStructure>> GenerateSelectTopAsync<TDataStructure>(IConnectionHandler connectionHandler, int topcount, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels, bool simpleload = false)
        {
            try
            {

                var model = FetchDataHelper.Instance(simpleload);
                var command = CreateSelectTopCommand(connectionHandler, model, topcount, conditionsexpression, orderbymodels);
                return await GetCollectionAsync<TDataStructure>(connectionHandler, command, model);
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }


        internal static async Task<TResult> SelectFirstOrDefaultAsync<TDataStructure, TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression)
        {

            return await GenerateSelectFirstOrDefaultAsync(connectionHandler, expression);

        }
        internal static async Task<TResult> SelectFirstOrDefaultAsync<TDataStructure, TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, OrderByModel<TDataStructure>[] orderByModel)
        {

            return await GenerateSelectFirstOrDefaultAsync(connectionHandler, expression, null, orderByModel);

        }
        internal static async Task<TResult> SelectFirstOrDefaultAsync<TDataStructure, TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {

            return await GenerateSelectFirstOrDefaultAsync(connectionHandler, expression, conditionsexpression);

        }
        internal static async Task<TResult> SelectFirstOrDefaultAsync<TDataStructure, TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModel)
        {

            return await GenerateSelectFirstOrDefaultAsync(connectionHandler, expression, conditionsexpression, orderByModel);

        }
        private static async Task<TResult> GenerateSelectFirstOrDefaultAsync<TDataStructure, TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression = null, OrderByModel<TDataStructure>[] orderByModel = null)
        {

            try
            {
                var isValueType = Utils.IsValueType<TResult>();
                var dataHelper = FetchDataHelper.InstanceFilterAssosation(isValueType);
                var command = CreateSelectCommand(connectionHandler, dataHelper, expression, conditionsexpression, orderByModel, true);
                return await GetObjectAsync(connectionHandler, command, dataHelper, expression);


            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }


        internal static async Task<List<TResult>> SelectAsync<TDataStructure, TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, bool distinct = false)
        {

            return await GenerateSelectAsync(connectionHandler, expression, null, null, distinct);

        }
        internal static async Task<List<TResult>> SelectAsync<TDataStructure, TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, OrderByModel<TDataStructure>[] orderByModel, bool distinct = false)
        {

            return await GenerateSelectAsync(connectionHandler, expression, null, orderByModel, distinct);

        }
        internal static async Task<List<TResult>> SelectAsync<TDataStructure, TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {

            return await GenerateSelectAsync(connectionHandler, expression, conditionsexpression, null, distinct);

        }
        internal static async Task<List<TResult>> SelectAsync<TDataStructure, TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModel, bool distinct = false)
        {

            return await GenerateSelectAsync(connectionHandler, expression, conditionsexpression, orderByModel, distinct);

        }
        private static async Task<List<TResult>> GenerateSelectAsync<TDataStructure, TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression = null, OrderByModel<TDataStructure>[] orderByModel = null, bool distinct = false)
        {

            try
            {


                var isValueType = Utils.IsValueType<TResult>();
                var dataHelper = FetchDataHelper.InstanceFilterAssosation(isValueType);
                var command = CreateSelectCommand(connectionHandler, dataHelper, expression, conditionsexpression, orderByModel, false, distinct);
                return await GetCollectionAsync(connectionHandler, command, dataHelper, expression, distinct);


            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }






        internal static async Task<TDataStructure> IncludeFirstOrDefaultAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, object>>[] Includeexpression)
        {
            return await GenerateIncludeFirstOrDefaultAsync(connectionHandler, Includeexpression);

        }
        internal static async Task<TDataStructure> IncludeFirstOrDefaultAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, object>>[] Includeexpression, OrderByModel<TDataStructure>[] orderByModels)
        {
            return await GenerateIncludeFirstOrDefaultAsync(connectionHandler, Includeexpression, null, orderByModels);

        }
        internal static async Task<TDataStructure> IncludeFirstOrDefaultAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            return await GenerateIncludeFirstOrDefaultAsync(connectionHandler, Includeexpression, conditionsexpression);

        }
        internal static async Task<TDataStructure> IncludeFirstOrDefaultAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels)
        {
            return await GenerateIncludeFirstOrDefaultAsync(connectionHandler, Includeexpression, conditionsexpression, orderbymodels);

        }
        private static async Task<TDataStructure> GenerateIncludeFirstOrDefaultAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression = null, OrderByModel<TDataStructure>[] orderbymodels = null)
        {
            try
            {

                var prepareModel = FetchDataHelper.InstanceFilterIncludeAssosation(true, true);
                var command = CreateIncludeCommand(connectionHandler, prepareModel, Includeexpression, conditionsexpression, orderbymodels, true);
                return await GetObjectAsync<TDataStructure>(connectionHandler, command, prepareModel);
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }


        internal static async Task<List<TDataStructure>> IncludeTopAsync<TDataStructure>(IConnectionHandler connectionHandler, int topcount, Expression<Func<TDataStructure, object>>[] Includeexpression)
        {
            return await GenerateIncludeTopAsync(connectionHandler, topcount, Includeexpression);

        }
        internal static async Task<List<TDataStructure>> IncludeTopAsync<TDataStructure>(IConnectionHandler connectionHandler, int topcount, Expression<Func<TDataStructure, object>>[] Includeexpression, OrderByModel<TDataStructure>[] orderByModels)
        {
            return await GenerateIncludeTopAsync(connectionHandler, topcount, Includeexpression, null, orderByModels);

        }
        internal static async Task<List<TDataStructure>> IncludeTopAsync<TDataStructure>(IConnectionHandler connectionHandler, int topcount, Expression<Func<TDataStructure, object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            return await GenerateIncludeTopAsync(connectionHandler, topcount, Includeexpression, conditionsexpression);

        }
        internal static async Task<List<TDataStructure>> IncludeTopAsync<TDataStructure>(IConnectionHandler connectionHandler, int topcount, Expression<Func<TDataStructure, object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels)
        {
            return await GenerateIncludeTopAsync(connectionHandler, topcount, Includeexpression, conditionsexpression, orderbymodels);

        }
        private static async Task<List<TDataStructure>> GenerateIncludeTopAsync<TDataStructure>(IConnectionHandler connectionHandler, int topcount, Expression<Func<TDataStructure, object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression = null, OrderByModel<TDataStructure>[] orderbymodels = null)
        {
            try
            {

                var prepareModel = FetchDataHelper.InstanceFilterIncludeAssosation(true, true);
                var command = CreateIncludeCommand(connectionHandler, prepareModel, Includeexpression, conditionsexpression, orderbymodels, true, topcount);
                return await GetCollectionAsync<TDataStructure>(connectionHandler, command, prepareModel);
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }




        internal static async Task<List<TDataStructure>> IncludePagedListAsync<TDataStructure>(IConnectionHandler connectionHandler, int pageIndex, int pagesize, Expression<Func<TDataStructure, object>>[] Includeexpression)
        {
            return await GenerateIncludePagedListAsync(connectionHandler, pageIndex, pagesize, Includeexpression);

        }
        internal static async Task<List<TDataStructure>> IncludePagedListAsync<TDataStructure>(IConnectionHandler connectionHandler, int pageIndex, int pagesize, Expression<Func<TDataStructure, object>>[] Includeexpression, OrderByModel<TDataStructure>[] orderByModels)
        {
            return await GenerateIncludePagedListAsync(connectionHandler, pageIndex, pagesize, Includeexpression, null, orderByModels);

        }
        internal static async Task<List<TDataStructure>> IncludePagedListAsync<TDataStructure>(IConnectionHandler connectionHandler, int pageIndex, int pagesize, Expression<Func<TDataStructure, object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            return await GenerateIncludePagedListAsync(connectionHandler, pageIndex, pagesize, Includeexpression, conditionsexpression);

        }
        internal static async Task<List<TDataStructure>> IncludePagedListAsync<TDataStructure>(IConnectionHandler connectionHandler, int pageIndex, int pagesize, Expression<Func<TDataStructure, object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels)
        {
            return await GenerateIncludePagedListAsync(connectionHandler, pageIndex, pagesize, Includeexpression, conditionsexpression, orderbymodels);

        }
        private static async Task<List<TDataStructure>> GenerateIncludePagedListAsync<TDataStructure>(IConnectionHandler connectionHandler, int pageIndex, int pagesize, Expression<Func<TDataStructure, object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression = null, OrderByModel<TDataStructure>[] orderbymodels = null)
        {
            try
            {

                var prepareModel = FetchDataHelper.InstanceFilterIncludeAssosation(true, true);
                var command = CreateIncludeCommand(connectionHandler, prepareModel, Includeexpression, conditionsexpression, orderbymodels);
                return await GetCollectionAsync<TDataStructure>(connectionHandler, command, prepareModel, pageIndex, pagesize);
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }


        internal static async Task<List<TDataStructure>> IncludeAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, object>>[] Includeexpression)
        {
            return await GenerateIncludeAsync(connectionHandler, Includeexpression);

        }
        internal static async Task<List<TDataStructure>> IncludeAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, object>>[] Includeexpression, OrderByModel<TDataStructure>[] orderByModels)
        {
            return await GenerateIncludeAsync(connectionHandler, Includeexpression, null, orderByModels);

        }
        internal static async Task<List<TDataStructure>> IncludeAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            return await GenerateIncludeAsync(connectionHandler, Includeexpression, conditionsexpression);

        }
        internal static async Task<List<TDataStructure>> IncludeAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels)
        {
            return await GenerateIncludeAsync(connectionHandler, Includeexpression, conditionsexpression, orderbymodels);

        }
        private static async Task<List<TDataStructure>> GenerateIncludeAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression = null, OrderByModel<TDataStructure>[] orderbymodels = null)
        {
            try
            {

                var prepareModel = FetchDataHelper.InstanceFilterIncludeAssosation(true, true);
                var command = CreateIncludeCommand(connectionHandler, prepareModel, Includeexpression, conditionsexpression, orderbymodels);
                return await GetCollectionAsync<TDataStructure>(connectionHandler, command, prepareModel);
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }


        internal static async Task<List<TDataStructure>> PagedListAsync<TDataStructure>(IConnectionHandler connectionHandler, int pageIndex, int pagesize, bool simpleload = false)
        {
            return await GeneratePagedAsync<TDataStructure>(connectionHandler, pageIndex, pagesize, null, null, simpleload);

        }
        internal static async Task<List<TDataStructure>> PagedListAsync<TDataStructure>(IConnectionHandler connectionHandler, int pageIndex, int pagesize, OrderByModel<TDataStructure>[] orderByModels, bool simpleload = false)
        {
            return await GeneratePagedAsync(connectionHandler, pageIndex, pagesize, null, orderByModels, simpleload);

        }
        internal static async Task<List<TDataStructure>> PagedListAsync<TDataStructure>(IConnectionHandler connectionHandler, int pageIndex, int pagesize, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            return await GeneratePagedAsync(connectionHandler, pageIndex, pagesize, conditionsexpression, null, simpleload);

        }
        internal static async Task<List<TDataStructure>> PagedListAsync<TDataStructure>(IConnectionHandler connectionHandler, int pageIndex, int pagesize, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels, bool simpleload = false)
        {
            return await GeneratePagedAsync(connectionHandler, pageIndex, pagesize, conditionsexpression, orderbymodels, simpleload);

        }
        private static async Task<List<TDataStructure>> GeneratePagedAsync<TDataStructure>(IConnectionHandler connectionHandler, int pageIndex, int pagesize, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels, bool simpleload = false)
        {
            try
            {

                var prepareModel = FetchDataHelper.Instance(simpleload);
                var command = CreateSelectCommand(connectionHandler, prepareModel, conditionsexpression, orderbymodels);
                return await GetCollectionAsync<TDataStructure>(connectionHandler, command, prepareModel, pageIndex, pagesize);
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }

        internal static async Task<List<TDataStructure>> GetAggregationAsync<TDataStructure>(IConnectionHandler connectionHandler, PropertyInfo property, Object obj) where TDataStructure : class
        {
            try
            {
                var prepareModel = FetchDataHelper.Instance();
                var command = CreateAggregationSelectCommand<TDataStructure>(connectionHandler, prepareModel, property, obj);
                return await GetCollectionAsync<TDataStructure>(connectionHandler, command, prepareModel);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        internal static async Task<List<TDataStructure>> WhereAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, bool>> expression, bool simpleload = false) where TDataStructure : class
        {
            try
            {
                var prepareModel = FetchDataHelper.Instance(simpleload);

                var command = CreateSelectCommand(connectionHandler, prepareModel, expression);
                return await GetCollectionAsync<TDataStructure>(connectionHandler, command, prepareModel);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }


        private static async Task<TResult> GetAggrigateGenericAsync<TDataStructure, TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, AggrigateFuntionType aggrigateFuntionType)
        {
            try
            {

                var fetchDataHelper = FetchDataHelper.InstanceFilterAssosation(true);
                var command = DbProvider.GetDbCommand(connectionHandler);
                var queryBuilder = new ExpressionTranslatorQuery().TranslateSelectColumnExpression(command, fetchDataHelper, expression, CheckIsNullInAggrigateColumn(aggrigateFuntionType));
                return await GetAggrigateColumnAsync<TDataStructure, TResult>(connectionHandler, command, fetchDataHelper, queryBuilder.Key, conditionsexpression, aggrigateFuntionType);

            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        private static async Task<TResult> GetAggrigateColumnAsync<TDataStructure, TResult>(IConnectionHandler connectionHandler, DbCommand command, FetchDataHelper prepareModel, string columnname, Expression<Func<TDataStructure, bool>> conditionsexpression, AggrigateFuntionType aggrigateFuntionType, bool distinct = false)
        {
            try
            {

                if (string.IsNullOrEmpty(columnname)) return default(TResult);
                command.CommandText = CreateAggrigateFuntionCommand(command, prepareModel, columnname, conditionsexpression, aggrigateFuntionType, distinct);
                return await ExecuteScalarAsync<TResult>(connectionHandler, command);




            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        internal static async Task<TResult> SumAsync<TDataStructure, TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression = null)
        {
            return await GetAggrigateGenericAsync(connectionHandler, expression, conditionsexpression, AggrigateFuntionType.Sum);
        }
        internal static async Task<TResult> MinAsync<TDataStructure, TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression = null)
        {
            return await GetAggrigateGenericAsync(connectionHandler, expression, conditionsexpression, AggrigateFuntionType.Min);
        }



        internal static async Task<List<dynamic>> GroupByAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {
            return await GenerateGroupByAsync(connectionHandler, groupexpression, null, conditionsexpression, null, null, distinct);
        }
        internal static async Task<List<dynamic>> GroupByAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, bool distinct = false)
        {
            return await GenerateGroupByAsync(connectionHandler, groupexpression, null, null, null, null, distinct);
        }
        internal static async Task<List<dynamic>> GroupByAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            return await GenerateGroupByAsync(connectionHandler, groupexpression, null, null, null, orderbymodel, distinct);
        }
        internal static async Task<List<dynamic>> GroupByAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            return await GenerateGroupByAsync(connectionHandler, groupexpression, null, conditionsexpression, null, orderbymodel, distinct);
        }

        internal static async Task<List<dynamic>> GroupByAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, bool distinct = false)
        {
            return await GenerateGroupByAsync(connectionHandler, groupexpression, aggrigateexpression, null, null, null, distinct);
        }
        internal static async Task<List<dynamic>> GroupByAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {
            return await GenerateGroupByAsync(connectionHandler, groupexpression, aggrigateexpression, conditionsexpression, null, null, distinct);
        }
        internal static async Task<List<dynamic>> GroupByAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            return await GenerateGroupByAsync(connectionHandler, groupexpression, aggrigateexpression, null, null, orderbymodel, distinct);
        }
        internal static async Task<List<dynamic>> GroupByAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            return await GenerateGroupByAsync(connectionHandler, groupexpression, aggrigateexpression, conditionsexpression, null, orderbymodel, distinct);
        }


        internal static async Task<List<dynamic>> GroupByWithHavingAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, bool distinct = false)
        {
            return await GenerateGroupByAsync(connectionHandler, groupexpression, null, conditionsexpression, havingconditionsexpression, null, distinct);
        }
        internal static async Task<List<dynamic>> GroupByWithHavingAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, OrderByModel<TDataStructure>[] orderbymode, bool distinct = false)
        {

            return await GenerateGroupByAsync(connectionHandler, groupexpression, null, conditionsexpression, havingconditionsexpression, orderbymode, distinct);
        }
        internal static async Task<List<dynamic>> GroupByWithHavingAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, bool distinct = false)
        {
            return await GenerateGroupByAsync(connectionHandler, groupexpression, null, null, havingconditionsexpression, null, distinct);
        }
        internal static async Task<List<dynamic>> GroupByWithHavingAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, OrderByModel<TDataStructure>[] orderbymode, bool distinct = false)
        {

            return await GenerateGroupByAsync(connectionHandler, groupexpression, null, null, havingconditionsexpression, orderbymode, distinct);
        }



        internal static async Task<List<dynamic>> GroupByWithHavingAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, bool distinct = false)
        {
            return await GenerateGroupByAsync(connectionHandler, groupexpression, aggrigateexpression, null, havingconditionsexpression, null, distinct);
        }
        internal static async Task<List<dynamic>> GroupByWithHavingAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, bool distinct = false)
        {
            return await GenerateGroupByAsync(connectionHandler, groupexpression, aggrigateexpression, conditionsexpression, havingconditionsexpression, null, distinct);
        }
        internal static async Task<List<dynamic>> GroupByWithHavingAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {

            return await GenerateGroupByAsync(connectionHandler, groupexpression, aggrigateexpression, null, havingconditionsexpression, orderbymodel, distinct);
        }
        internal static async Task<List<dynamic>> GroupByWithHavingAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {

            return await GenerateGroupByAsync(connectionHandler, groupexpression, aggrigateexpression, conditionsexpression, havingconditionsexpression, orderbymodel, distinct);
        }
        private static async Task<List<dynamic>> GenerateGroupByAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> conditionsexpression = null, Expression<Func<TDataStructure, bool>> havingconditionsexpression = null, OrderByModel<TDataStructure>[] orderbymodel = null, bool distinct = false)
        {
            try
            {

                var prepareModel = FetchDataHelper.InstanceFilterAssosation(true);

                var command = CreateGroupByCommand(connectionHandler, prepareModel, groupexpression, aggrigateexpression, conditionsexpression, havingconditionsexpression, orderbymodel, distinct);
                return await GetDynamicCollectionAsync(connectionHandler, command);
            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }



        internal static async Task<TResult> MaxAsync<TDataStructure, TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression = null)
        {
            return await GetAggrigateGenericAsync(connectionHandler, expression, conditionsexpression, AggrigateFuntionType.Max);
        }
        internal static async Task<double> AverageAsync<TDataStructure, TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression = null)
        {

            var fetchDataHelper = FetchDataHelper.InstanceFilterAssosation(true);
            var command = DbProvider.GetDbCommand(connectionHandler);
            var queryBuilder = new ExpressionTranslatorQuery().TranslateSelectColumnExpression(command, fetchDataHelper, expression, CheckIsNullInAggrigateColumn(AggrigateFuntionType.AVG));
            return await GetAggrigateColumnAsync<TDataStructure, double>(connectionHandler, command, fetchDataHelper, queryBuilder.Key, conditionsexpression, AggrigateFuntionType.AVG);

        }


        internal static async Task<List<TDataStructure>> OrderByAsync<TDataStructure>(IConnectionHandler connectionHandler, OrderByModel<TDataStructure>[] expression, bool simpleload = false)
        {
            return await GenerateOrderByAsync(connectionHandler, expression, null, simpleload);
        }
        internal static async Task<List<TDataStructure>> OrderByAsync<TDataStructure>(IConnectionHandler connectionHandler, OrderByModel<TDataStructure>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            return await GenerateOrderByAsync(connectionHandler, expression, conditionsexpression, simpleload);
        }
        internal static async Task<List<TDataStructure>> GenerateOrderByAsync<TDataStructure>(IConnectionHandler connectionHandler, OrderByModel<TDataStructure>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            try
            {
                var model = FetchDataHelper.Instance(simpleload);
                var command = CreateSelectCommand(connectionHandler, model, conditionsexpression, expression);
                return await GetCollectionAsync<TDataStructure>(connectionHandler, command, model);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }


        internal static async Task<List<TDataStructure>> OrderByAsync<TDataStructure, TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> expression, bool simpleload = false)
        {
            return await GenerateOrderByAsync(connectionHandler, expression, null, OrderType.ASC, simpleload);
        }
        internal static async Task<List<TDataStructure>> OrderByAsync<TDataStructure, TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            return await GenerateOrderByAsync(connectionHandler, expression, conditionsexpression, OrderType.ASC, simpleload);
        }
        internal static async Task<List<TDataStructure>> OrderByDescendingAsync<TDataStructure, TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            return await GenerateOrderByAsync(connectionHandler, expression, conditionsexpression, OrderType.DESC, simpleload);
        }
        internal static async Task<List<TDataStructure>> OrderByDescendingAsync<TDataStructure, TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> expression, bool simpleload = false)
        {
            return await GenerateOrderByAsync(connectionHandler, expression, null, OrderType.DESC, simpleload);
        }
        private static async Task<List<TDataStructure>> GenerateOrderByAsync<TDataStructure, TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderType orderType, bool simpleload = false)
        {
            try
            {
                var model = FetchDataHelper.Instance(simpleload);

                var command = CreateOrderByCommand(connectionHandler, model, expression, conditionsexpression, orderType);
                return await GetCollectionAsync<TDataStructure>(connectionHandler, command, model);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }


        internal static async Task<TDataStructure> FirstOrDefaultAsync<TDataStructure>(IConnectionHandler connectionHandler, bool simpleload = false) where TDataStructure : class
        {
            return await GenerateFirstOrDefaultAsync<TDataStructure>(connectionHandler, null, simpleload);
        }
        internal static async Task<TDataStructure> FirstOrDefaultAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, bool>> expression, bool simpleload = false)
        {
            return await GenerateFirstOrDefaultAsync(connectionHandler, expression, simpleload);
        }
        internal static async Task<TDataStructure> GenerateFirstOrDefaultAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, bool>> expression, bool simpleload = false)
        {
            try
            {
                var model = FetchDataHelper.Instance(simpleload);
                var command = CreateSelectCommand(connectionHandler, model, expression, true);
                return await GetObjectAsync<TDataStructure>(connectionHandler, command, model);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }



        internal static async Task<TDataStructure> FirstOrDefaultWithOrderByAsync<TDataStructure>(IConnectionHandler connectionHandler, OrderByModel<TDataStructure>[] orderByexpression, bool simpleload = false) where TDataStructure : class
        {
            return await GenerateFirstOrDefaultWithOrderByAsync(connectionHandler, orderByexpression, null, simpleload);
        }
        internal static async Task<TDataStructure> FirstOrDefaultWithOrderByAsync<TDataStructure>(IConnectionHandler connectionHandler, OrderByModel<TDataStructure>[] orderByexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false) where TDataStructure : class
        {
            return await GenerateFirstOrDefaultWithOrderByAsync(connectionHandler, orderByexpression, conditionsexpression, simpleload);
        }
        internal static async Task<TDataStructure> GenerateFirstOrDefaultWithOrderByAsync<TDataStructure>(IConnectionHandler connectionHandler, OrderByModel<TDataStructure>[] orderByexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false) where TDataStructure : class
        {
            try
            {
                var model = FetchDataHelper.Instance(simpleload);
                var command = CreateSelectCommand(connectionHandler, model, conditionsexpression, orderByexpression, true);
                return await GetObjectAsync<TDataStructure>(connectionHandler, command, model);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }


        internal static async Task<TDataStructure> FirstOrDefaultWithOrderByAsync<TDataStructure, TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, bool simpleload = false)
        {
            return await GenerateFirstOrDefaultWithOrderByAsync(connectionHandler, orderByexpression, null, OrderType.ASC, simpleload);
        }
        internal static async Task<TDataStructure> FirstOrDefaultWithOrderByAsync<TDataStructure, TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false) where TDataStructure : class
        {
            return await GenerateFirstOrDefaultWithOrderByAsync(connectionHandler, orderByexpression, conditionsexpression, OrderType.ASC, simpleload);
        }
        internal static async Task<TDataStructure> FirstOrDefaultWithOrderByDescendingAsync<TDataStructure, TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, bool simpleload = false)
        {
            return await GenerateFirstOrDefaultWithOrderByAsync(connectionHandler, orderByexpression, null, OrderType.DESC, simpleload);
        }
        internal static async Task<TDataStructure> FirstOrDefaultWithOrderByDescendingAsync<TDataStructure, TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false) where TDataStructure : class
        {
            return await GenerateFirstOrDefaultWithOrderByAsync(connectionHandler, orderByexpression, conditionsexpression, OrderType.DESC, simpleload);
        }
        private static async Task<TDataStructure> GenerateFirstOrDefaultWithOrderByAsync<TDataStructure, TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderType orderType, bool simpleload = false)
        {
            try
            {
                var model = FetchDataHelper.Instance(simpleload);
                var command = CreateOrderByCommand(connectionHandler, model, orderByexpression, conditionsexpression, orderType, true);
                return await GetObjectAsync<TDataStructure>(connectionHandler, command, model);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        internal static async Task<TDataStructure> SingleOrDefaultAsync<TDataStructure>(IConnectionHandler connectionHandler, bool simpleload = false)
        {
            return await SingleOrDefaultAsync<TDataStructure>(connectionHandler, null, simpleload);
        }
        internal static async Task<TDataStructure> SingleOrDefaultAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, bool>> expression, bool simpleload = false)
        {
            try
            {
                if (await CountAsync(connectionHandler, expression) > 1)
                    throw new KnownException("Sequence contains more than one element");
                return await GenerateFirstOrDefaultAsync(connectionHandler, expression);

            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        internal static async Task<bool> AnyAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, bool>> expression = null) where TDataStructure : class
        {
            try
            {

                var model = FetchDataHelper.Instance(true);
                var command = CreateAnyCommand(connectionHandler, model, expression);
                return await ExecuteScalarAsync<bool>(connectionHandler, command);
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }


        }


        internal static async Task<int> CountAsync<TDataStructure, TColumn>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TColumn>> expression, bool distinct = true) where TDataStructure : class
        {
            return await CountAsync(connectionHandler, expression, null, distinct);
        }
        internal static async Task<int> CountAsync<TDataStructure, TColumn>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TColumn>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = true) where TDataStructure : class
        {
            try
            {
                var fetchDataHelper = FetchDataHelper.InstanceFilterAssosation(true);
                var command = DbProvider.GetDbCommand(connectionHandler);
                var queryBuilder = new ExpressionTranslatorQuery().TranslateSelectColumnExpression(command, fetchDataHelper, expression);
                return await GetAggrigateColumnAsync<TDataStructure, int>(connectionHandler, command, fetchDataHelper, queryBuilder.Key, conditionsexpression, AggrigateFuntionType.Count, distinct);

            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        internal static async Task<int> CountAsync<TDataStructure>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, bool>> conditionsexpression = null)
        {
            var fetchDataHelper = FetchDataHelper.InstanceFilterAssosation(true);
            var command = DbProvider.GetDbCommand(connectionHandler);
            return await GetAggrigateColumnAsync<TDataStructure, int>(connectionHandler, command, fetchDataHelper, "*", conditionsexpression, AggrigateFuntionType.Count);
        }


        #endregion
        ////==========================================================================================================================////
        #region GetObjectAsync
        internal static async Task<TDataStructure> GetObjectAsync<TDataStructure>(IConnectionHandler connectionHandler, params object[] keys)
        {
            var model = FetchDataHelper.Instance();
            return await GetObjectAsync<TDataStructure>(connectionHandler, model, keys);
        }
        internal static async Task<TDataStructure> SimpleGetObjectAsync<TDataStructure>(IConnectionHandler connectionHandler, params object[] keys)
        {


            var model = FetchDataHelper.Instance(true);
            return await GetObjectAsync<TDataStructure>(connectionHandler, model, keys);

        }
        private static async Task<TDataStructure> GetObjectAsync<TDataStructure>(IConnectionHandler connectionHandler, FetchDataHelper model, params object[] keys)
        {
            try
            {



                var dt = await GetTableKeysAsync<TDataStructure>(connectionHandler);
                var command = CreateSelectGetObjectCommand<TDataStructure>(connectionHandler, model, keys, dt);
                return await GetObjectAsync<TDataStructure>(connectionHandler, command, model);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public static async Task<TDataStructure> GetObjectAsync<TDataStructure>(IConnectionHandler connectionHandler, string commandText)
        {
            try
            {

                var command = DbProvider.GetDbCommand(connectionHandler, commandText);
                return await GetObjectAsync<TDataStructure>(connectionHandler, command);




            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public static async Task<TDataStructure> GetObjectAsync<TDataStructure>(IConnectionHandler connectionHandler, IDbCommand command)
        {
            try
            {
                TDataStructure result = default(TDataStructure);
                var dt = await GetDataTableAsync(connectionHandler, command);
                if (dt.Rows.Count <= 0) return result;
                var columnNames = GetDataTableColumnNames(dt);
                var fetchDataHelper = FetchDataHelper.Instance(true);
                var type = typeof(TDataStructure);
                var infos = type.GetTypePropertiesDictionary();
                result = (TDataStructure)Activator.CreateInstance(type);
                var dataRow = dt.Rows[0];
                SetObjectPropertiesValue(dataRow, result, columnNames, infos);
                await GetLanguageContentsAsync(connectionHandler, fetchDataHelper, result);
                return result;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        private static async Task<TDataStructure> GetObjectAsync<TDataStructure>(IConnectionHandler connectionHandler, IDbCommand command, FetchDataHelper fetchDataHelper)
        {
            try
            {
                TDataStructure result = default(TDataStructure);
                var dt = await GetDataTableAsync(connectionHandler, command);
                if (dt.Rows.Count <= 0) return result;
                GetDataTableColumnNames(fetchDataHelper, dt);
                result = MapObject<TDataStructure>(dt.Rows[0], fetchDataHelper);
                await GetLanguageContentsAsync(connectionHandler, fetchDataHelper, result);
                return result;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        private static async Task<TResult> GetObjectAsync<TDataStructure, TResult>(IConnectionHandler connectionHandler, IDbCommand command, FetchDataHelper fetchDataHelper, Expression<Func<TDataStructure, TResult>> expression)
        {
            try
            {
                var dt = await GetDataTableAsync(connectionHandler, command);
                TResult result = default(TResult);
                if (dt.Rows.Count <= 0) return result;
                var dataRows = dt.Rows.Cast<DataRow>().ToArray();
                GetDataTableColumnNames(fetchDataHelper, dt);
                var isValueType = Utils.IsValueType<TResult>();
                if (isValueType)
                    return SetIsValueTypeValue<TResult>(dataRows[0][0]);
                var compile = expression.Compile();
                result = compile.Invoke(MapObject<TDataStructure>(dataRows[0], fetchDataHelper));
                await GetLanguageContentsAsync(connectionHandler, fetchDataHelper, result);
                return result;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public static async Task<dynamic> GetDynamicObjectAsync(IConnectionHandler connectionHandler, DbCommand command)
        {

            var dt = await GetDataTableAsync(connectionHandler, command);
            if (dt.Rows.Count <= 0) return null;
            var dataRows = dt.Rows.Cast<DataRow>().ToArray();
            var columnnames = GetDataTableColumnNames(dt);
            dynamic obj = new ExpandoObject();
            foreach (var columnName in columnnames)
            {
                MapDynamicObjectColumns(dataRows[0], columnName, obj);
            }

            return obj;

        }

        #endregion
        ////==========================================================================================================================////
        #region MultiLanguageAsync

        internal static async Task<TDataStructure> GetLanuageContentAsync<TDataStructure>(IConnectionHandler connectionHandler, string culture, params object[] paramskeys)
        {
            var dataHelper = FetchDataHelper.Instance(culture);
            return await GetLanuageContentAsync<TDataStructure>(connectionHandler, dataHelper, paramskeys);
        }
        internal static async Task<TDataStructure> GetLanuageContentsimpleAsync<TDataStructure>(IConnectionHandler connectionHandler, string culture, params object[] paramskeys)
        {
            var dataHelper = FetchDataHelper.Instance(true, culture);
            return await GetLanuageContentAsync<TDataStructure>(connectionHandler, dataHelper, paramskeys);
        }
        private static async Task<TDataStructure> GetLanuageContentAsync<TDataStructure>(IConnectionHandler connectionHandler, FetchDataHelper fetchDataHelper, params object[] paramskeys)
        {

            var dataStructure = await GetObjectAsync<TDataStructure>(connectionHandler, fetchDataHelper, paramskeys);
            if (dataStructure == null) return default(TDataStructure);
            await GetLanguageContentsAsync(connectionHandler, fetchDataHelper, dataStructure);
            return dataStructure;
        }
        internal static async Task GetLanuageContentAsync<TDataStructure>(IConnectionHandler connectionHandler, string culture, TDataStructure obj)
        {
            var fetchDataHelper = FetchDataHelper.Instance(culture);
            await GetLanguageContentsAsync(connectionHandler, fetchDataHelper, new[] { obj });
        }
        internal static async Task GetLanuageContentAsync<TDataStructure>(IConnectionHandler connectionHandler, string culture, List<TDataStructure> objlist)
        {
            var fetchDataHelper = FetchDataHelper.Instance(culture);
            await GetLanguageContentsAsync(connectionHandler, fetchDataHelper, objlist.ToArray());

        }
        private static async Task GetLanguageContentsAsync<TDataStructure>(IConnectionHandler connectionHandler, FetchDataHelper mapObjectModel, TDataStructure obj)
        {
            await GetLanguageContentsAsync(connectionHandler, mapObjectModel, new[] { obj });
        }

        private static async Task GetLanguageContentsAsync<TDataStructure>(IConnectionHandler connectionHandler, FetchDataHelper mapObjectModel, TDataStructure[] objlist)
        {
            if (!await HaslanguagecontentTableAsync(connectionHandler)) return;
            if (objlist == null || !objlist.Any()) return;
            var keys = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var info in objlist)
                PrepareQueryLanguageContent(info, keys, mapObjectModel);

            var datarows = await GetLanguageContentDataRowsAsync(connectionHandler, keys);
            foreach (var info in objlist)
            {
                MapObjectLanguageContent(info, datarows, mapObjectModel);

            }


        }
        private static async Task DeleteLanguageContentAsync<TDataStructure>(IConnectionHandler connectionHandler, object[] objkeys)
        {
            var transactionImported = false;
            try
            {

                if (!await HaslanguagecontentTableAsync(connectionHandler)) return;
                var type = typeof(TDataStructure);
                var keys = LanguageContentKey(type, objkeys);
                if (!keys.Any()) return;

                var keylist = keys.Aggregate("", (s, s1) => s + ("N'" + s1.ToLower() + "'" + ","));
                keylist = keylist.Substring(0, keylist.Length - 1);
                var CommandText = string.Format(" Delete [Common].[LanguageContent] FROM  [Common].[LanguageContent]  WHERE lower([Key]) in ({0}) ", keylist);

                if (connectionHandler.Transaction != null)
                    transactionImported = true;
                if (!transactionImported)
                    connectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                await ExecuteNonQueryAsync(connectionHandler, CommandText);
                if (!transactionImported)
                {
                    connectionHandler.CommitTransaction();

                }
            }
            catch (KnownException)
            {
                if (!transactionImported)
                    connectionHandler.RollBack();
                throw;
            }
            catch (Exception ex)
            {
                if (!transactionImported)
                    connectionHandler.RollBack();
                throw new KnownException(Messages.DeletingDataFailed, ex);
            }




        }
        private static async Task InsertLanguageContentAsync<TDataStructure>(IConnectionHandler connectionHandler, TDataStructure obj)
        {

            var transactionImported = false;
            try
            {

                if (!await HaslanguagecontentTableAsync(connectionHandler)) return;
                var culture = ObjCulture(obj);
                var keys = LanguageContentKeyWithValues(obj, culture);
                if (!keys.Any()) return;

                var allowInsertProp = typeof(LanguageContent).GetAllowInsertProperties();
                var aggregate = keys.Aggregate(String.Empty,
                  (current, languageContent) => current + (" " + CreateInsertCommand(languageContent, allowInsertProp) + "\n"));
                if (connectionHandler.Transaction != null)
                    transactionImported = true;
                if (!transactionImported)
                    connectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                await ExecuteNonQueryAsync(connectionHandler, aggregate);
                if (!transactionImported)
                {

                    connectionHandler.CommitTransaction();

                }

            }
            catch (KnownException)
            {
                if (!transactionImported)
                    connectionHandler.RollBack();
                throw;
            }
            catch (Exception ex)
            {
                if (!transactionImported)
                    connectionHandler.RollBack();
                throw new KnownException(Messages.SavingDataFailed, ex);
            }

        }
        private static async Task UpdateLanguageContentAsync<TDataStructure>(IConnectionHandler connectionHandler, TDataStructure obj)
        {

            var transactionImported = false;
            try
            {

                if (!await HaslanguagecontentTableAsync(connectionHandler)) return;
                var culture = ObjCulture(obj);
                var keys = LanguageContentKeyWithValues(obj, culture);
                if (!keys.Any()) return;
                var CommandText = "";
                var allowUpdateProp = typeof(LanguageContent).GetAllowUpdateProperties();
                var allowInsertProp = typeof(LanguageContent).GetAllowInsertProperties();
                var datarows = await GetLanguageContentDataRowsAsync(connectionHandler, keys.Select(x => x.Key).ToDictionary(s => s), culture);
                foreach (var languageContent in keys)
                {

                    if (datarows != null && datarows.Any(x => x.Key.ToLower() == languageContent.Key.ToLower()))
                        CommandText += CreateUpdateCommand(languageContent, allowUpdateProp) + "\n";
                    else
                        CommandText += CreateInsertCommand(languageContent, allowInsertProp) + "\n";

                }
                if (connectionHandler.Transaction != null)
                    transactionImported = true;
                if (!transactionImported)
                    connectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                await ExecuteNonQueryAsync(connectionHandler, CommandText);
                if (!transactionImported)
                {
                    connectionHandler.CommitTransaction();

                }

            }
            catch (KnownException)
            {
                if (!transactionImported)
                    connectionHandler.RollBack();
                throw;
            }
            catch (Exception ex)
            {
                if (!transactionImported)
                    connectionHandler.RollBack();
                throw new KnownException(Messages.SavingDataFailed, ex);
            }

        }




        private static async Task<List<LanguageContent>> GetLanguageContentDataRowsAsync(IConnectionHandler connectionHandler, Dictionary<string, string> keys, string culture = null)
        {

            if (!keys.Any()) return null;
            var keylist = keys.Select(x => x.Key.ToLower()).Aggregate("", (s, s1) => s + ("N'" + s1 + "'" + ","));
            keylist = keylist.Substring(0, keylist.Length - 1);
            var CommandText = string.Format("SELECT * FROM  [Common].[LanguageContent]  WHERE lower([Key]) in ({0}) {1} ", keylist, (!string.IsNullOrEmpty(culture) ? string.Format(" and [Common].[LanguageContent].[LanguageId]='{0}' ", culture) : ""));
            var command = DbProvider.GetDbCommand(connectionHandler, CommandText);
            return await GetCollectionAsync<LanguageContent>(connectionHandler, command);




        }
        internal static async Task<bool> HaslanguagecontentTableAsync(IConnectionHandler connectionHandler)
        {
            var key = "Radyn.Framework.DbHelper.DBManager.HasMultiLangTable";
            var obj = HasTableCashCache.StorageCache[key];
            if (obj != null) return (bool)obj;
            var hasMultiLangTable = await ChecklanguagecontentTableAsync(connectionHandler);
            HasTableCashCache.StorageCache.Add(key, hasMultiLangTable);
            return hasMultiLangTable;

        }
        private static async Task<bool> ChecklanguagecontentTableAsync(IConnectionHandler connectionHandler)
        {

            var commandText =
                "SELECT SCHEMA_NAME(schema_id) + '.' + name as TableName from sys.tables where" +
                " lower(SCHEMA_NAME(schema_id) + '.' + name) = 'common.languagecontent' ORDER BY name";
            var dataTableAsync = await GetDataTableAsync(connectionHandler, commandText);
            return dataTableAsync.Rows.Count > 0;
        }




        #endregion
        ////==========================================================================================================================//// 
        #endregion




        ////=============================================================================================================================////
        #region DBManagerPrivateHelper

        private static dynamic SetIsValueTypeValue<T>(object value)
        {
            var type = typeof(T);
            if (value == DBNull.Value) return default(T);
            if (type.IsEnum) return Enum.Parse(type, value.ToString());
            try
            {
                return (T)value;
            }
            catch
            {

                return (T)Convert.ChangeType(value, type);
            }

        }
        private static string GetMapObjectKey(object result)
        {
            return GetMapObjectKey(result.GetType(), result.GetObjectKeyValue());
        }
        private static string GetMapObjectKey(Type type, object[] keys)
        {
            return type.FullName + ',' + String.Join(",", keys);

        }
        private static void MapObjectColumns(DataRow dr, object obj, string key, FetchDataHelper model)
        {
            var columnListDictionary = model.GetColumnArray(key);
            var typeproperties = obj.GetType().GetObjectProperties(key);
            SetObjectPropertiesValue(dr, obj, columnListDictionary, typeproperties);
        }

        private static void MapDynamicObjectColumns(DataRow dataRow, string columnName, dynamic obj)
        {
            if (dataRow[columnName] is DBNull)
                ((IDictionary<string, object>)obj)[columnName] = null;
            else
                ((IDictionary<string, object>)obj)[columnName] = dataRow[columnName];
        }
        private static void SetObjectPropertiesValue(DataRow dr, object obj, string[] columnListDictionary, Dictionary<string, PropertyInfo> typeproperties)
        {
            foreach (var column in columnListDictionary)
            {
                PropertyInfo propertyInfo;
                if (!typeproperties.TryGetValue(column, out propertyInfo)) continue;
                var val = dr[column];
                SetProperyVal(val, propertyInfo, obj);
            }
        }



        #region CreateCommand

        private static DbCommand CreateGroupByCommand<TDataStructure>(IConnectionHandler connectionHandler, FetchDataHelper model, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, OrderByModel<TDataStructure>[] orderByModel, bool distinct = false)
        {
            var command = DbProvider.GetDbCommand(connectionHandler);
            var orderby = CreateListOrderByCommand(command, model, orderByModel);
            var having = CreateConditionCommand(command, model, havingconditionsexpression, true);
            var column = new ExpressionTranslatorQuery().TranslateSelectMultiColumnsExpression(command, model, groupexpression);
            var aggrigateCommand = CreateGroupByAggrigateColumsCommand(command, model, aggrigateexpression, distinct);
            var where = CreateConditionCommand(command, model, conditionsexpression);
            var from = PrepareQueryFrom(typeof(TDataStructure), model);
            command.CommandText = string.Format(" SELECT {0}{3}  FROM  {1} {2} group by {6} {4} {5}", TranslateColumnList(column), from, where, aggrigateCommand, having, orderby, string.Join(",", column.Select(x => x.Key)));
            return command;
        }
        private static DbCommand CreateDynamicSelectCommand<TDataStructure>(IConnectionHandler connectionHandler, FetchDataHelper model, Expression<Func<TDataStructure, Object>>[] columeExpressions, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModel, bool selecttop = false, bool distinct = false)
        {
            var command = DbProvider.GetDbCommand(connectionHandler);
            var columns = new ExpressionTranslatorQuery().TranslateSelectMultiColumnsExpression(command, model, columeExpressions);

            var select = CreateSelectCommand(command, model, conditionsexpression, orderByModel, selecttop, distinct, columns);
            command.CommandText = string.Format(" {0} ", select);
            return command;
        }
        private static DbCommand CreateIncludeCommand<TDataStructure>(IConnectionHandler connectionHandler, FetchDataHelper model, Expression<Func<TDataStructure, Object>>[] columeExpressions, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModel, bool selecttop = false, int topcount = 1)
        {
            var command = DbProvider.GetDbCommand(connectionHandler);
            new ExpressionTranslatorQuery().TranslateSelectMultiColumnsExpression(command, model, columeExpressions);
            model.IncludeAssosiation.AddRange(model.SelectedAssosiation);
            var select = CreateSelectCommand(command, model, conditionsexpression, orderByModel, selecttop, false, null, topcount);
            command.CommandText = string.Format(" {0} ", select);
            return command;
        }
        private static DbCommand CreateSelectCommand<TDataStructure>(IConnectionHandler connectionHandler, FetchDataHelper model, Expression<Func<TDataStructure, bool>> conditionsexpression, bool selecttop = false, bool distinct = false, KeyValuePair<string, string>[] SelectedColumns = null, int topcount = 1)
        {
            var command = DbProvider.GetDbCommand(connectionHandler);
            command.CommandText = CreateSelectCommand(command, model, conditionsexpression, selecttop, distinct,
                SelectedColumns, topcount);
            return command;

        }
        private static DbCommand CreateAggregationSelectCommand<TDataStructure>(IConnectionHandler connectionHandler, FetchDataHelper model, PropertyInfo property, Object obj, bool selecttop = false, bool distinct = false, KeyValuePair<string, string>[] SelectedColumns = null, int topcount = 1)
        {
            var command = DbProvider.GetDbCommand(connectionHandler);
            var attributes = property.GetPropertyAggregationAttributesModel(typeof(TDataStructure));
            var objectKeyValue = obj.GetObjectKeyValue(attributes.PropNamePropertys);
            var tableNameKey = typeof(TDataStructure).GetTableKey();
            var where = "";
            for (int i = 0; i < attributes.AggrigateObjectPropNamePropertys.Length; i++)
            {
                where += String.Format("[{0}].[{1}]={2} and ", tableNameKey, attributes.AggrigateObjectPropNamePropertys[i].Name, objectKeyValue[i]);
            }
            var whereCommand = !string.IsNullOrEmpty(@where) ? String.Format(" where {0} ", @where.Substring(0, @where.Length - 4)) : String.Empty;
            command.CommandText = CreateSelectCommand<TDataStructure>(model, whereCommand, selecttop, distinct, SelectedColumns, topcount);
            return command;
        }
        private static DbCommand CreateSelectCommand<TDataStructure>(IConnectionHandler connectionHandler, FetchDataHelper model, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModel, bool selecttop = false, bool distinct = false, KeyValuePair<string, string>[] SelectedColumns = null, int topcount = 1)
        {
            var command = DbProvider.GetDbCommand(connectionHandler);
            command.CommandText = CreateSelectCommand(command, model, conditionsexpression, orderByModel, selecttop, distinct, SelectedColumns, topcount);
            return command;
        }
        private static DbCommand CreateSelectCommand<TDataStructure, TResult>(IConnectionHandler connectionHandler, FetchDataHelper prepareModel, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModel, bool selecttop = false, bool distinct = false)
        {
            var command = DbProvider.GetDbCommand(connectionHandler);
            var expressionTranslatorQuery = new ExpressionTranslatorQuery();
            var isValueType = Mhazami.Utility.Utils.IsValueType<TResult>();
            var queryBuilder = expressionTranslatorQuery.TranslateSelectColumnExpression(command, prepareModel, expression);
            var selectCommand = CreateSelectCommand(command, prepareModel, conditionsexpression, orderByModel, selecttop, distinct, (isValueType ? new[] { queryBuilder } : null));
            command.CommandText = string.Format(" {0} ", selectCommand);
            return command;

        }
        private static DbCommand CreateSelectCommand<TDataStructure>(IConnectionHandler connectionHandler, FetchDataHelper model, bool selecttop = false, bool distinct = false, KeyValuePair<string, string>[] selectedColumns = null, int topcount = 1)
        {
            var command = DbProvider.GetDbCommand(connectionHandler);
            command.CommandText = CreateSelectCommand<TDataStructure>(model, selecttop, distinct, selectedColumns, topcount);
            return command;
        }
        private static DbCommand CreateSelectTopCommand<TDataStructure>(IConnectionHandler connectionHandler, FetchDataHelper prepareModel, int topcount, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModel)
        {

            var command = DbProvider.GetDbCommand(connectionHandler);
            var selectCommand = CreateSelectCommand(command, prepareModel, conditionsexpression, orderByModel, true, false, null, topcount);
            command.CommandText = string.Format(" {0} ", selectCommand);
            return command;

        }
        private static DbCommand CreateAnyCommand<TDataStructure>(IConnectionHandler connectionHandler, FetchDataHelper prepareModel, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {

            var command = DbProvider.GetDbCommand(connectionHandler);
            var selectCommand = CreateSelectCommand(command, prepareModel, conditionsexpression, null);
            command.CommandText = string.Format("  IF EXISTS ({0})  SELECT 1 ELSE SELECT 0 ", selectCommand);
            return command;
        }
        private static DbCommand CreateSelectKeyValuePairCommand<TDataStructure>(IConnectionHandler connectionHandler, FetchDataHelper prepareModel, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModel, bool distinct)
        {
            var command = DbProvider.GetDbCommand(connectionHandler);
            var expressionTranslatorQuery = new ExpressionTranslatorQuery();
            var queryBuilder = expressionTranslatorQuery.TranslateSelectMultiColumnsExpression(command, prepareModel, new[] { DataValueField, DataTextField });
            var selectCommand = CreateSelectCommand(command, prepareModel, conditionsexpression, orderByModel, false, distinct, queryBuilder);
            command.CommandText = string.Format(" {0} ", selectCommand);
            return command;

        }
        private static DbCommand CreateOrderByCommand<TDataStructure, TOrderProperty>(IConnectionHandler connectionHandler, FetchDataHelper model, Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderType orderType, bool selecttop = false)
        {

            var command = DbProvider.GetDbCommand(connectionHandler);
            var queryBuilder = new ExpressionTranslatorQuery().TranslateSelectColumnExpression(command, model, orderByexpression);
            if (string.IsNullOrEmpty(queryBuilder.Key)) throw new KnownException(" orderBy expression is not valid ");
            var selectCommand = CreateSelectCommand(command, model, conditionsexpression, selecttop);
            command.CommandText = string.Format("{0}  order by {1} {2}", selectCommand, queryBuilder.Key, orderType);
            return command;

        }
        private static DbCommand CreateSelectGetObjectCommand<TDataStructure>(IConnectionHandler connectionHandler, FetchDataHelper model, object[] obj, DataTable dt)
        {
            var command = DbProvider.GetDbCommand(connectionHandler);
            var inKey = typeof(TDataStructure).GetTypeKeyProperties();
            if (!inKey.Any())
                throw new KnownException(string.Format("براي جدول {0} هيچ كليدي در نظر گرفته نشده.", typeof(TDataStructure).Name));
            if (inKey.Count() != obj.Length)
                throw new KnownException(string.Format("تعداد پارامترهاي ورودي با تعداد كليد برابري ندارد({0}).", typeof(TDataStructure).Name));
            var query = string.Format("{0} WHERE ", CreateSelectCommand<TDataStructure>(model));
            var counter = 0;
            foreach (var row in dt.Rows.Cast<DataRow>().ToArray())
            {
                query += string.Format("[{0}].[{1}] = ", typeof(TDataStructure).GetTableKey(), row[0]);
                query += GetPropertyValue(obj[counter]);
                query += " AND ";
                counter++;
            }
            command.CommandText = query.Substring(0, query.Length - 5);
            return command;

        }
        #endregion

        #region CreateCommandsText

        private static string CreateAggrigateFuntionCommand<TDataStructure>(DbCommand command, FetchDataHelper model, string columname, Expression<Func<TDataStructure, bool>> conditionsexpression, AggrigateFuntionType aggrigateFuntionType, bool distinct = false)
        {

            var whereCommand = CreateConditionCommand(command, model, conditionsexpression);
            var from = PrepareQueryFrom(typeof(TDataStructure), model);
            return string.Format(" SELECT {2}({3}{0}) FROM  {1} {4} ", columname, from, aggrigateFuntionType, (!distinct ? "" : " distinct "), whereCommand);
        }
        private static string CreateGroupByAggrigateColumsCommand<TDataStructure>(DbCommand dbCommand, FetchDataHelper model, GroupByModel<TDataStructure>[] aggrigateexpression, bool distinct)
        {
            var expressionTranslatorQuery = new ExpressionTranslatorQuery();
            if (aggrigateexpression == null) return String.Empty;
            var getaggrigate = "";
            foreach (var colum in aggrigateexpression)
            {
                if (!string.IsNullOrEmpty(getaggrigate)) getaggrigate += ",";
                var queryBuilder = expressionTranslatorQuery.TranslateSelectColumnExpression(dbCommand, model, colum.Expression, CheckIsNullInAggrigateColumn(colum.AggrigateFuntionType));
                getaggrigate += String.Format("{0}({2}{1}) as [{3}] ", colum.AggrigateFuntionType, queryBuilder.Key,
                    (!distinct ? "" : " distinct "), colum.AggrigateFuntionType + queryBuilder.Value);
            }
            return string.IsNullOrEmpty(getaggrigate) ? "" : String.Format(" ,{0} ", getaggrigate);
        }
        private static bool CheckIsNullInAggrigateColumn(AggrigateFuntionType aggrigateFuntionType)
        {
            return aggrigateFuntionType == AggrigateFuntionType.Sum ||
                                                aggrigateFuntionType == AggrigateFuntionType.AVG;

        }
        private static string CreateSelectCommand<TDataStructure>(FetchDataHelper model, bool selecttop = false, bool distinct = false, KeyValuePair<string, string>[] selectedColumns = null, int topcount = 1)
        {
            var selectcommand = (selectedColumns != null && selectedColumns.Any()) ? TranslateColumnList(selectedColumns) : PrepareQuerySelect(typeof(TDataStructure), model);
            var from = PrepareQueryFrom(typeof(TDataStructure), model);
            return string.Format("SELECT {3}{2}{0} FROM  {1} ", selectcommand, from, selecttop ? string.Format(" TOP {0} ", topcount) : "", distinct ? " distinct " : "");
        }
        private static string TranslateColumnList(KeyValuePair<string, string>[] selectedColumns)
        {
            var str = "";
            foreach (var selectedColumn in selectedColumns)
            {
                if (!string.IsNullOrEmpty(str)) str += ",";
                str += String.Format("{0} as [{1}]", selectedColumn.Key, selectedColumn.Value);
            }
            return str;
        }
        private static string CreateSelectCommand<TDataStructure>(DbCommand dbCommand, FetchDataHelper model, Expression<Func<TDataStructure, bool>> conditionsexpression, bool selecttop = false, bool distinct = false, KeyValuePair<string, string>[] SelectedColumns = null, int topcount = 1)
        {

            var whereCommand = CreateConditionCommand(dbCommand, model, conditionsexpression);
            return CreateSelectCommand<TDataStructure>(model, whereCommand, selecttop, distinct, SelectedColumns, topcount);

        }
        private static string CreateSelectCommand<TDataStructure>(FetchDataHelper model, string whereCommand, bool selecttop = false, bool distinct = false, KeyValuePair<string, string>[] SelectedColumns = null, int topcount = 1)
        {

            return string.Format(" {0} {1}", CreateSelectCommand<TDataStructure>(model, selecttop, distinct, SelectedColumns, topcount), whereCommand);
        }
        private static string CreateSelectCommand<TDataStructure>(DbCommand dbCommand, FetchDataHelper model, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModel, bool selecttop = false, bool distinct = false, KeyValuePair<string, string>[] SelectedColumns = null, int topcount = 1)
        {

            var orderby = CreateListOrderByCommand(dbCommand, model, orderByModel);
            var select = CreateSelectCommand(dbCommand, model, conditionsexpression, selecttop, distinct, SelectedColumns, topcount);
            return string.Format(" {0} {1} ", select, orderby);
        }
        private static string CreateListOrderByCommand<TDataStructure>(DbCommand dbCommand, FetchDataHelper model, OrderByModel<TDataStructure>[] orderByexpression)
        {
            var aggrigate = "";
            var expressionTranslatorQuery = new ExpressionTranslatorQuery();
            if (orderByexpression == null) return String.Empty;
            foreach (var orderByModel in orderByexpression)
            {
                if (orderByModel == null) continue;
                if (!string.IsNullOrEmpty(aggrigate)) aggrigate += ",";
                var queryBuilder = expressionTranslatorQuery.TranslateSelectColumnExpression(dbCommand, model, orderByModel.Expression);
                aggrigate += String.Format(" {0} {1} ", queryBuilder.Key, orderByModel.OrderType);
            }
            if (string.IsNullOrEmpty(aggrigate)) return String.Empty;
            return string.Format(" order by {0} ", aggrigate);
        }
        private static string CreateConditionCommand<TDataStructure>(DbCommand command, FetchDataHelper model, Expression<Func<TDataStructure, bool>> expression, bool ishaving = false)
        {
            var where = "";
            if (expression != null)
            {
                where = new ExpressionTranslatorQuery().TranslateConditionExpression(command, model, expression);
                if (string.IsNullOrEmpty(where)) throw new KnownException(" conditionsexpression is not valid ");
            }
            if (!ishaving)
            {
                foreach (var objectKeyName in model.SelectedMultiLangAssosiation)
                {
                    if (!string.IsNullOrEmpty(@where)) @where += " and ";
                    if (!objectKeyName.Value)
                    {
                        var parameterName = command.GenerateNewParameter(typeof(string), model.Culture);
                        @where += String.Format(" ([{0}].[Key] is null or [{0}].[LanguageId]={1}) ", objectKeyName.Key, parameterName);
                    }
                    else
                        @where += String.Format(" ([{0}].[Key] is null or ([{0}].[Value] is not null and [{0}].[Value]!=' ')  ) ", objectKeyName.Key);
                }
            }
            return string.IsNullOrEmpty(@where) ? String.Empty : string.Format(" {1} {0}", @where, ishaving ? " Having " : " where ");
        }


        private static string CreateDeleteCommand<TDataStructure>(object[] obj, DataTable dt)
        {
            var type = typeof(TDataStructure);

            var inKey = typeof(TDataStructure).GetTypeKeyProperties();
            if (!inKey.Any())
                throw new KnownException(string.Format("براي جدول {0} هيچ كليدي در نظر گرفته نشده.", typeof(TDataStructure).Name));
            if (inKey.Count() != obj.Length)
                throw new KnownException(string.Format("تعداد پارامترهاي ورودي با تعداد كليد برابري ندارد({0}).", typeof(TDataStructure).Name));
            string query = string.Format("DELETE FROM [{0}].[{1}] WHERE ",
                                         typeof(TDataStructure).GetSchema().SchemaName, type.Name);
            int counter = 0;
            foreach (var row in dt.Rows.Cast<DataRow>().ToArray())
            {
                query += string.Format("[{0}] = ", row[0]);
                var propertyValue = obj[counter];
                query += GetPropertyValue(propertyValue);
                query += " AND ";
                counter++;
            }
            query = query.Substring(0, query.Length - 5);
            return query;
        }
        private static string CreateUpdateCommand<TDataStructure>(TDataStructure obj)
        {
            var type = typeof(TDataStructure);
            var inKey = type.GetTypeKeyProperties();
            if (!inKey.Any())
                throw new KnownException(string.Format("براي جدول {0} هيچ كليدي در نظر گرفته نشده.", type.Name));
            var allowUpdateProp = type.GetAllowUpdateProperties();
            return CreateUpdateCommand(obj, allowUpdateProp);
        }
        private static string CreateUpdateCommand<TDataStructure>(TDataStructure obj, PropertyInfo[] allowUpdateproperties)
        {
            if (allowUpdateproperties == null || !allowUpdateproperties.Any()) return String.Empty;
            var type = typeof(TDataStructure);
            var inKey = typeof(TDataStructure).GetTypeKeyProperties();
            if (!inKey.Any())
                throw new KnownException(string.Format("براي جدول {0} هيچ كليدي در نظر گرفته نشده.", typeof(TDataStructure).Name));
            string query = string.Format("UPDATE [{0}].[{1}] SET ", typeof(TDataStructure).GetSchema().SchemaName, type.Name);
            var counter = 0;
            var param = string.Empty;
            var paramValue = string.Empty;
            foreach (var property in allowUpdateproperties)
            {
                counter++;
                query += string.Format("[{0}] = @{1},", property.Name, counter);
                var propertyValue = GetPropertyValue(Helper.GetPropertyValue(obj, property));
                if (property.GetPropertyAttributes().IsNullable != null || property.PropertyType.Name.ToLower().IndexOf("nullable") >= 0)
                {
                    if (propertyValue == null)
                    {
                        param += "@" + counter + " " + property.GetPropertyAttributes().DbType.dbType + " ,";
                        paramValue += "@" + counter + "= NULL,";
                        continue;
                    }
                }
                else
                {
                    if (propertyValue == null)
                    {
                        var caption = property.Name;
                        var layout = property.GetPropertyAttributes().Layout;
                        if (layout != null)
                            caption = layout.Caption;
                        throw new KnownException(string.Format(Messages.FillField, caption));
                    }
                }
                param += "@" + counter + " " + property.GetPropertyAttributes().DbType.dbType + " ,";
                paramValue += "@" + counter + "=" + propertyValue + ", ";
            }

            query = query.Substring(0, query.Length - 1);
            query += " WHERE ";

            foreach (var property in inKey)
            {
                counter++;
                query += string.Format("[{0}] = @{1}", property.Name, counter);
                var propertyValue = GetPropertyValue(Helper.GetPropertyValue(obj, property));
                query += " AND ";
                param += "@" + counter + " " + property.GetPropertyAttributes().DbType.dbType + " ,";
                paramValue += "@" + counter + "=" + propertyValue + ",";
            }
            query = query.Substring(0, query.Length - 5);
            param = param.Substring(0, param.Length - 1);
            paramValue = paramValue.Substring(0, paramValue.Length - 1);
            query = string.Format("exec sp_executesql N'{0} ',N'{1}',{2}", query, param, paramValue);
            return query;
        }
        private static string CreateInsertCommand<TDataStructure>(TDataStructure obj)
        {

            var type = typeof(TDataStructure);
            var inKey = type.GetTypeKeyProperties();
            if (!inKey.Any())
                throw new KnownException(string.Format("براي جدول {0} هيچ كليدي در نظر گرفته نشده.", type.Name));
            var allowInsertProp = type.GetAllowInsertProperties();
            return CreateInsertCommand(obj, allowInsertProp);

        }
        private static string CreateInsertCommand<TDataStructure>(TDataStructure obj, PropertyInfo[] allowInsertproperties)
        {
            if (allowInsertproperties == null || !allowInsertproperties.Any()) return String.Empty;
            var type = typeof(TDataStructure);
            var values = string.Empty;
            var query = string.Empty;
            var param = string.Empty;
            var paramValue = string.Empty;
            var counter = 0;
            foreach (var property in allowInsertproperties)
            {
                counter++;
                query += string.Format("[{0}] ,", property.Name);
                var propertyValue = GetPropertyValue(Helper.GetPropertyValue(obj, property));
                if (property.GetPropertyAttributes().IsNullable != null || property.PropertyType.Name.ToLower().IndexOf("nullable") >= 0)
                {
                    if (propertyValue == null)
                    {
                        values += "NULL ,";
                        continue;
                    }
                }
                else
                {
                    if (propertyValue == null)
                    {
                        var caption = property.Name;
                        var layout = property.GetPropertyAttributes().Layout;
                        if (layout != null)
                            caption = layout.Caption;
                        throw new KnownException(string.Format(Messages.FillField, caption));
                    }
                }
                values += "@" + counter + ", ";
                param += "@" + counter + " " + property.GetPropertyAttributes().DbType.dbType + " ,";
                paramValue += "@" + counter + "=" + propertyValue + ", ";
            }
            query = query.Substring(0, query.Length - 2);
            param = param.Substring(0, param.Length - 2);
            paramValue = paramValue.Substring(0, paramValue.Length - 2);
            values = values.Substring(0, values.Length - 2);
            query = string.Format("INSERT INTO [{0}].[{1}] ({2}) VALUES ({3})", typeof(TDataStructure).GetSchema().SchemaName, type.Name, query, values);
            query = string.Format("exec sp_executesql N'{0}',N'{1}',{2}", query, param, paramValue);
            return query;
        }

        private static void SetProperyVal(object val, PropertyInfo propertyInfo, object result)
        {
            try
            {
                if (val is DBNull)
                    propertyInfo.SetValue(result, null, null);
                else
                {
                    if (propertyInfo.PropertyType.IsEnum)
                    {
                        propertyInfo.SetValue(result, Enum.Parse(propertyInfo.PropertyType, val.ToString()), null);
                        return;
                    }

                    try
                    {
                        propertyInfo.SetValue(result, val, null);
                    }
                    catch 
                    {
                        var changeType = Convert.ChangeType(val, propertyInfo.PropertyType);
                        propertyInfo.SetValue(result, changeType, null);
                    }
                   
                    
                }
            }
            catch (Exception ex)
            {
            }
        }
        private static string GetPropertyValue(object propertyValue, bool isLike = false)
        {
            if (propertyValue == null) return "NULL";
            if (propertyValue.GetType().IsEnum)
            {
                try
                {
                    return Convert.ToInt32((Enum.Parse(propertyValue.GetType(), propertyValue.ToString()))).ToString();

                }
                catch
                {

                }
            }
            if (propertyValue is Guid)
            {
                if (Guid.Parse(propertyValue.ToString()).Equals(Guid.Empty))
                    return "NULL";
                return string.Format("'{0}'", propertyValue);
            }
            switch (Type.GetTypeCode(propertyValue.GetType()))
            {
                case TypeCode.Boolean:
                    return (bool.Parse(propertyValue.ToString()) ? "1" : "0");
                case TypeCode.String:
                    return string.Format(isLike ? "N'%{0}%'" : "N'{0}'", ((string)propertyValue).Replace("'", "''"));
                case TypeCode.DateTime:
                    {
                        var dateTime = (DateTime)propertyValue;
                        return string.Format("'{0:}'",
                string.Format("{0:D2}", dateTime.Month) + "/" + string.Format("{0:D2}", dateTime.Day) + "/" + dateTime.Year + " " + string.Format("{0:D2}", dateTime.Hour) + ":" +
                string.Format("{0:D2}", dateTime.Minute) + ":" + string.Format("{0:D2}", dateTime.Second));

                    }
                case TypeCode.Decimal:
                case TypeCode.Double:
                    return propertyValue.ToString().Trim().Replace("/", ".");
                default:
                    return propertyValue.ToString().Trim();

            }




        }

        #endregion

        private static bool EnableTrack<TDataStructure>(TDataStructure dataStructure)
        {
            var attributes = typeof(TDataStructure).GetClassAttributes();
            var enableTrack = attributes.Track != null &&
                              attributes.Track.Enabled;
            if (!(dataStructure is DataStructureBase)) return enableTrack;
            var dataStructureBase = dataStructure as DataStructureBase;
            return !dataStructureBase.DisableTrack && enableTrack;
        }

        ////==========================================================================================================================////

        #region PrepareSelectQuery

        private static string[] GetDataTableColumnNames(DataTable dr)
        {
            return dr.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();
        }
        private static void GetDataTableColumnNames(FetchDataHelper model, DataTable dr)
        {
            model.Columnnames = dr.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();
        }


        private static string PrepareQuerySelect(Type type, FetchDataHelper model)
        {

            var select = String.Empty;
            model = model ?? FetchDataHelper.Instance();
            model.RenewSelectedKeys();
            PrepareQuerySelectRecursive(type, ref @select, model);
            if (!string.IsNullOrEmpty(select))
                select = select.Substring(0, select.Length - 1);
            return select;
        }
        private static void PrepareQuerySelectRecursive(Type type, ref string @select, FetchDataHelper fetchDataHelper, string parentkey = "")
        {
            bool isroot = parentkey == "";
            if (isroot)
            {
                parentkey = type.GetTableKey();
                var allowSelectProp = type.GetAllowSelectProperties();
                foreach (var propertyInfo in allowSelectProp)
                    select += string.Format("[{0}].[{1}] as [{0}{1}],", parentkey, propertyInfo.Name);
                fetchDataHelper.AddSelectedKeys(parentkey);
            }
            if (fetchDataHelper.SimpleLoad) return;
            var inAssosiation = type.GetAssosiationModel();
            foreach (var propertyInfo in inAssosiation)
            {
                var key = type.GetTableKey(parentkey, propertyInfo.AssosiationProperty);
                if (!fetchDataHelper.IsValidAssosiationKey(parentkey, key, isroot)) continue;
                var allowSelectProp = propertyInfo.AssosiationProperty.PropertyType.GetAllowSelectProperties();
                foreach (var info in allowSelectProp)
                    @select += string.Format("[{0}].[{1}] as [{0}{1}],", key, info.Name);

                fetchDataHelper.AddSelectedKeys(key);
                PrepareQuerySelectRecursive(propertyInfo.AssosiationProperty.PropertyType, ref select, fetchDataHelper, key);
            }
        }

        private static string PrepareQueryFrom(Type type, FetchDataHelper model)
        {
            var from = String.Empty;
            model = model ?? FetchDataHelper.Instance();
            model.RenewSelectedKeys();
            PrepareQueryFromRecursive(type, ref @from, model);
            return from;
        }
        private static void PrepareQueryFromRecursive(Type type, ref string @from, FetchDataHelper fetchDataHelper, string parentkey = "")
        {

            var schema = type.GetSchema();
            var isrroot = parentkey == "";
            if (isrroot)
            {
                parentkey = type.GetTableKey();
                from += string.Format(" [{0}].[{1}] as [{2}] ", schema.SchemaName, type.Name, parentkey);
                fetchDataHelper.AddSelectedKeys(parentkey);
            }
            PrepareQueryFromMultiLanguage(fetchDataHelper, type, parentkey, isrroot, ref from);
            var assosiatione = type.GetAssosiationModel();
            foreach (var propertyInfo in assosiatione)
            {
                var key = type.GetTableKey(parentkey, propertyInfo.AssosiationProperty);
                if (!fetchDataHelper.IsValidAssosiationKey(parentkey, key, isrroot, true)) continue;
                var joinType = GetfromQueryJoinType(assosiatione, propertyInfo, isrroot);
                @from += string.Format(" {3} JOIN [{0}].[{1}] as [{2}] ", propertyInfo.AssosiationSchemaName,
                        propertyInfo.AssosiationProperty.PropertyType.Name, key, joinType);
                @from += " ON ";
                var count = propertyInfo.FromQueryProperteis.Count();
                for (int i = 0; i < count; i++)
                {
                    var joinleft = string.Format("[{0}].[{1}]", parentkey, propertyInfo.FromQueryProperteis[i].Name);
                    var joinright = string.Format("[{0}].[{1}]", key, propertyInfo.FromQueryAssosiationKeyNames[i].Name);
                    switch (propertyInfo.JoinCompareType)
                    {
                        case JoinCompareType.Like:
                            @from += string.Format("{0}{2}{3}{1}{4}", joinleft, joinright, " like ", " '%' + cast(", " as nvarchar(200)) +'%' ");
                            break;
                        case JoinCompareType.StringCompare:
                            @from += string.Format("{2}{0}{3}={2}{1}{3}", joinleft, joinright, " cast(", " as nvarchar(200)) ");
                            break;
                        default:
                            @from += string.Format("{0}={1}", joinleft, joinright);
                            break;
                    }
                    if (count > i + 1)
                        from += " AND ";

                }
                fetchDataHelper.AddSelectedKeys(key);
                PrepareQueryFromRecursive(propertyInfo.AssosiationProperty.PropertyType, ref @from, fetchDataHelper, key);
            }

        }

        private static JoinType GetfromQueryJoinType(AssosiationModel[] assosiatione, AssosiationModel propertyInfo, bool isroot)
        {
            if (!isroot)
                return JoinType.Left;
            if (propertyInfo.JoinType != JoinType.None)
                return propertyInfo.JoinType;
            var attributeModel = propertyInfo.PropNameProperty.GetPropertyAttributes();
            if (attributeModel.IsNullable != null)
                return JoinType.Left;
            if (propertyInfo.AssosiationProperty.GetPropertyAttributes().IsNullable != null)
                return JoinType.Left;
            if (propertyInfo.PropNameProperty.PropertyType.Name.ToLower().IndexOf("nullable") >= 0)
                return JoinType.Left;
            if (assosiatione.Count(x => x.PropNameProperty.Equals(propertyInfo.PropNameProperty)) > 1)
                return JoinType.Left;
            if (attributeModel.Key != null &&
                    assosiatione.Count(x => x.PropNameProperty.GetPropertyAttributes().Key != null &&
                                            x.AssosiationProperty.PropertyType ==
                                            propertyInfo.AssosiationProperty.PropertyType) > 1)
                return JoinType.Left;

            return JoinType.Inner;
        }
        private static void PrepareQueryFromMultiLanguage(FetchDataHelper fetchDataHelper, Type ParentType, string parentkey, bool isroot, ref string @from)
        {
            if (!fetchDataHelper.SelectedMultiLangAssosiation.Any()) return;

            var allowMultiLangProp = ParentType.GetAllowMultiLangProperties();
            if (!allowMultiLangProp.Any()) return;
            var multilangtype = typeof(LanguageContent);
            var multilangschema = multilangtype.GetSchema();
            var schemaParent = ParentType.GetSchema();
            var keyvalues = "";
            var keyProperties = ParentType.GetTypeKeyProperties();
            foreach (var keyName in keyProperties)
            {
                if (!string.IsNullOrEmpty(keyvalues)) keyvalues += "+'.'+";
                keyvalues += String.Format(" cast([{0}].[{1}] as nvarchar(200)) ", parentkey, keyName.Name);
            }

            foreach (var propertyInfo in allowMultiLangProp)
            {
                var tableKey = multilangtype.GetTableKey(parentkey, propertyInfo);
                if (!fetchDataHelper.IsValidMultiLangKey(parentkey, tableKey, isroot)) continue;
                @from += string.Format(" Left JOIN [{0}].[{1}] as [{2}] ", multilangschema.SchemaName, multilangtype.Name, tableKey);
                @from += string.Format(" ON [{0}].[Key]=('{1}.{2}.'+{3}+'.{4}') ", tableKey,
                    schemaParent.SchemaName, ParentType.Name, keyvalues, propertyInfo.Name);
                fetchDataHelper.AddSelectedKeys(tableKey);
            }



        }


        #endregion

        ////==========================================================================================================================////





        #region MultiLang

        private static void PrepareQueryLanguageContent(object obj, Dictionary<string, string> keys, FetchDataHelper fetchDataHelper)
        {
            if (obj == null) return;
            fetchDataHelper.RenewSelectedKeys();
            PrepareQueryLanguageContentRecursive(obj, keys, fetchDataHelper);

        }
        private static void PrepareQueryLanguageContentRecursive(object obj, Dictionary<string, string> keys, FetchDataHelper fetchDataHelper, string parentkey = "")
        {
            bool isroot = parentkey == "";
            if (isroot)
            {
                var key = LanguageContentKey(obj);
                parentkey = obj.GetType().GetTableKey();
                foreach (var propertyInfo in key)
                {
                    if (keys.ContainsKey(propertyInfo)) continue;
                    keys.Add(propertyInfo, propertyInfo);
                }
                fetchDataHelper.AddSelectedKeys(parentkey);

            }
            if (fetchDataHelper.SimpleLoad) return;
            var assosiatione = obj.GetType().GetAssosiationModel();
            foreach (var propertyInfo in assosiatione)
            {
                var tblkey = obj.GetType().GetTableKey(parentkey, propertyInfo.AssosiationProperty);
                if (!fetchDataHelper.IsValidAssosiationKey(parentkey, tblkey, isroot)) continue;
                var value = propertyInfo.AssosiationProperty.GetValue(obj, null);
                if (value == null) continue;
                var key = LanguageContentKey(value);
                foreach (var info in key)
                {
                    if (keys.ContainsKey(info)) continue;
                    keys.Add(info, info);
                }
                fetchDataHelper.AddSelectedKeys(tblkey);
                PrepareQueryLanguageContentRecursive(value, keys, fetchDataHelper, tblkey);
            }

        }
        private static void MapObjectLanguageContent(object obj, List<LanguageContent> datarows, FetchDataHelper fetchDataHelper)
        {
            if (obj == null) return;
            fetchDataHelper.RenewSelectedKeys();
            MapObjectRecursiveLanguageContent(obj, datarows, new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase), fetchDataHelper);
        }
        private static void MapObjectRecursiveLanguageContent(object obj, List<LanguageContent> dr, Dictionary<string, string> keyvalues, FetchDataHelper fetchDataHelper, string parentkey = "")
        {
            var isroot = parentkey == "";
            if (isroot)
            {

                parentkey = obj.GetType().GetTableKey();
                MapColumnLanguageContent(dr, obj, keyvalues, fetchDataHelper.Culture);
                fetchDataHelper.AddSelectedKeys(parentkey);
            }
            if (fetchDataHelper.SimpleLoad) return;
            var inAssosiation = obj.GetType().GetAssosiationModel();
            foreach (var propertyInfo in inAssosiation)
            {

                var key = obj.GetType().GetTableKey(parentkey, propertyInfo.AssosiationProperty);
                if (!fetchDataHelper.IsValidAssosiationKey(parentkey, key, isroot)) continue;
                var value = propertyInfo.AssosiationProperty.GetValue(obj, null);
                if (value == null) continue;

                MapColumnLanguageContent(dr, value, keyvalues, fetchDataHelper.Culture);
                fetchDataHelper.AddSelectedKeys(key);
                MapObjectRecursiveLanguageContent(value, dr, keyvalues, fetchDataHelper, key);

            }

        }

        private static void MapColumnLanguageContent(List<LanguageContent> dr, object obj, Dictionary<string, string> selectfiled, string culture)
        {

            var contentPropKey = MasterLanguageContentPropKey(obj);
            var type = obj.GetType().GetAllowMultiLangProperties().ToDictionary(x => contentPropKey + x.Name);
            foreach (var propertyInfo in type)
            {
                string value1;
                if (selectfiled.TryGetValue(propertyInfo.Key, out value1))
                {
                    propertyInfo.Value.SetValue(obj, value1, null);
                    continue;
                }
                string value = null;
                if (dr != null)
                {
                    LanguageContent content;
                    var contents = dr.Where(x => x.Key.ToLower() == propertyInfo.Key.ToLower());
                    var multiLanguage = propertyInfo.Value.GetPropertyAttributes().MultiLanguage;
                    if (multiLanguage == null) continue;
                    var deafultlangcontent = contents.FirstOrDefault(x => x.LanguageId == culture);
                    if (multiLanguage.FillAnyLanguage)
                    {
                        content =
                            (deafultlangcontent == null || deafultlangcontent.Value == null ||
                             string.IsNullOrEmpty(deafultlangcontent.Value.Trim()))
                                ? contents.FirstOrDefault(x => x.Value != null && !string.IsNullOrEmpty(x.Value.Trim()))
                                : deafultlangcontent;
                    }
                    else
                        content = deafultlangcontent;

                    if (content != null)
                        value = content.Value;

                }
                SetProperyVal(value, propertyInfo.Value, obj);
                selectfiled.Add(propertyInfo.Key, value);
            }

            if (obj is DataStructureBase)
                ((obj as DataStructureBase).CurrentUICultureName) = culture;

        }

        private static string MasterLanguageContentPropKey(Type type, object[] keys)
        {
            var key = "";
            var classAttribute = type.GetSchema();
            key += classAttribute.SchemaName + ".";
            key += type.Name + ".";
            key += String.Join(".", keys) + ".";
            return key;
        }
        private static string[] LanguageContentKey(object obj)
        {

            var prop = obj.GetType().GetAllowMultiLangProperties();
            if (!prop.Any()) return new string[] { };
            var keyValue = obj.GetObjectKeyValue();
            return LanguageContentKey(obj.GetType(), keyValue);
        }
        private static string[] LanguageContentKey(Type type, object[] keys)
        {

            var list = new List<string>();
            var prop = type.GetAllowMultiLangProperties();
            if (!prop.Any()) return new string[] { };
            var key = MasterLanguageContentPropKey(type, keys);
            foreach (var propertyInfo in prop)
                list.Add(key + propertyInfo.Name);
            return list.ToArray();
        }
        private static string ObjCulture<TDataStructure>(TDataStructure obj)
        {
            return obj is DataStructureBase ? (obj as DataStructureBase).CurrentUICultureName : Thread.CurrentThread.CurrentUICulture.Name;

        }
        private static List<LanguageContent> LanguageContentKeyWithValues(object obj, string culture)
        {
            var list = new List<LanguageContent>();
            var prop = obj.GetType().GetAllowMultiLangProperties();
            if (!prop.Any()) return list;
            var contentPropKey = MasterLanguageContentPropKey(obj);
            foreach (var propertyInfo in prop)
            {
                var content = new LanguageContent { Key = contentPropKey + propertyInfo.Name };
                var value = propertyInfo.GetValue(obj, null);
                content.Value = value != null ? value.ToString() : null;
                content.LanguageId = culture;
                list.Add(content);
            }

            return list;
        }
        private static string MasterLanguageContentPropKey(object obj)
        {

            var keyValue = obj.GetObjectKeyValue();
            return MasterLanguageContentPropKey(obj.GetType(), keyValue);
        }


        #endregion


        ////==========================================================================================================================////

        #region MapObject

        private static TDataStructure MapObject<TDataStructure>(DataRow dr, FetchDataHelper fetchDataHelper)
        {
            var result = (TDataStructure)Activator.CreateInstance(typeof(TDataStructure));
            MapObjectRecursive<TDataStructure>(dr, result, fetchDataHelper);
            return result;
        }
        private static void MapObjectRecursive<TDataStructure>(DataRow dr, object result, FetchDataHelper fetchDataHelper, string parentkey = "")
        {
            var type = result.GetType();
            if (parentkey == "")
            {

                parentkey = type.GetTableKey();
                MapObjectColumns(dr, result, parentkey, fetchDataHelper);
                var keysValues = GetMapObjectKey(result);
                fetchDataHelper.AddObjectList(keysValues, result);
            }
            if (fetchDataHelper.SimpleLoad) return;
            var inAssosiation = type.GetAssosiationModel();
            foreach (var propertyInfo in inAssosiation)
            {
                var key = type.GetTableKey(parentkey, propertyInfo.AssosiationProperty);
                var columnlist = fetchDataHelper.GetColumnArray(key);
                var assosiationkeyName = key + propertyInfo.AssosiationKeyProperty.Name;
                if (!columnlist.Any() || (!columnlist.Contains(assosiationkeyName))) continue;
                var assosiationkeyvalue = dr[assosiationkeyName];
                if (assosiationkeyvalue == DBNull.Value) continue;
                var value = propertyInfo.PropNameProperty.GetValue(result, null);
                if (value == null) continue;
                if ((propertyInfo.JoinCompareType != JoinCompareType.Like && !assosiationkeyvalue.ToString().Equals(value.ToString())) ||
                    (propertyInfo.JoinCompareType == JoinCompareType.Like && !value.ToString().Contains(assosiationkeyvalue.ToString())))
                    continue;

                var keysValues = GetMapObjectKey(propertyInfo.AssosiationProperty.PropertyType, new[] { assosiationkeyvalue });
                var getobj = fetchDataHelper.GetObjectValue(propertyInfo.AssosiationProperty.PropertyType, keysValues);
                if (getobj != null)
                {
                    propertyInfo.AssosiationProperty.SetValue(result, getobj, null);
                    continue;
                }
                var obj = Activator.CreateInstance(propertyInfo.AssosiationProperty.PropertyType);
                MapObjectColumns(dr, obj, key, fetchDataHelper);
                fetchDataHelper.AddObjectList(keysValues, obj);
                MapObjectRecursive<TDataStructure>(dr, obj, fetchDataHelper, key);
                propertyInfo.AssosiationProperty.SetValue(result, obj, null);
            }


        }






        #endregion

        ////==========================================================================================================================////

        #region TRack



        private static List<Tracker> PrepareTrack<TDataStructure>(TDataStructure obj, TDataStructure oldobj, OperationLogType logType) where TDataStructure : class
        {
            var trackers = new List<Tracker>();
            TDataStructure compareobj = null;
            switch (logType)
            {
                case OperationLogType.InsertRecord:
                    compareobj = obj;
                    oldobj = Activator.CreateInstance<TDataStructure>();
                    break;
                case OperationLogType.UpdateField:
                    compareobj = oldobj;
                    break;
                case OperationLogType.DeleteRecord:
                    obj = Activator.CreateInstance<TDataStructure>();
                    compareobj = oldobj;
                    break;
            }
            var change = oldobj.TrackChange(obj);
            if (!change.Any()) return trackers;
            if (compareobj == null) return trackers;
            var keys = string.Join(",", compareobj.GetObjectKeyValue());
            if (string.IsNullOrEmpty(keys)) return trackers;
            Type rootObject = null;
            var rootid = String.Empty;
            if (compareobj is DataStructureBase)
            {
                var dataStructureBase = compareobj as DataStructureBase;
                rootid = dataStructureBase.RootId;
                rootObject = dataStructureBase.RootObject;
            }
            var type = typeof(TDataStructure);
            var isMasterList = typeof(TDataStructure).GetTrackMasterProperties();
            var masterRefId = "";
            var masterObjectRefId = "";
            foreach (var propertyInfo in isMasterList)
            {
                var propertyAttribute = propertyInfo.GetPropertyAttributes().TrackMaster;
                masterObjectRefId += propertyAttribute.MasterType.Name + ",";
                masterRefId += propertyInfo.GetValue(compareobj, null) + ",";
            }
            var getmasterRefId = string.IsNullOrEmpty(masterRefId) ? keys : masterRefId.Substring(0, masterRefId.Length - 1);
            var getmasterObjectRefId = string.IsNullOrEmpty(masterObjectRefId) ? type.Name : masterObjectRefId.Substring(0, masterObjectRefId.Length - 1);
            foreach (var item in change)
            {

                var newtracker = new Tracker
                {
                    RefId = keys,
                    //IpAddress = !String.IsNullOrEmpty(WebUtility.GetClientIp()) ? WebUtility.GetClientIp() : WebUtility.GetServerIP(),
                    UserName = BOUtility.GetTrackerUserUserName(obj),
                    Date = DateTime.Now.ShamsiDate(),
                    Time = DateTime.Now.GetTime(),
                    Operation = logType.GetDescription(),
                    ObjectName = item.ObjectName,
                    MasterRefId = getmasterRefId,
                    MasterObjectRefId = getmasterObjectRefId,
                    RefTitle = type.Name,
                    RootId = !string.IsNullOrEmpty(rootid) ? rootid : getmasterRefId,
                    RootTitle = rootObject != null ? rootObject.Name : getmasterObjectRefId,
                    FieldDesc = item.FieldDesc,
                    FieldName = item.FieldName,
                    NewValue = item.NewValue != null ? item.NewValue.ToString() : String.Empty,
                    OldVal = item.OldValue != null ? item.OldValue.ToString() : String.Empty,
                };
                trackers.Add(newtracker);

            }
            return trackers;
        }

        #endregion 
        #endregion
    }
}
