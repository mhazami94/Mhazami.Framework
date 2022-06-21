using Mhazami.Framework.DbHelper;
using Mhazami.Framework.Difinition;
using System.Dynamic;
using System.Reflection;
using System.Text.Json.Serialization;


namespace Mhazami.Framework
{
    [Serializable]
    public abstract class DataStructureBase<T> : DataStructureBase, ICloneable where T : class
    {
        public event Delegates.LoadOnDemandProperty LoadOnDemandProperty;

        private T _oldValue;

        [DisableAction(DiableAllAction = true)]
        [JsonIgnore]
        public T OldValue
        {
            get { return _oldValue; }
            private set { _oldValue = value; }
        }

        public void SetPropertyValue(string propertyName, object value)
        {
            PropertyInfo property;
            this.GetType().GetTypePropertiesDictionary().TryGetValue(propertyName, out property);
            if (property == null)
                throw new Exception("Invalid Property '" + propertyName + "'");
            var fieldName = ("_" + propertyName.Substring(0, 1).ToLower() + propertyName.Substring(1, propertyName.Length - 1));
            fieldName = fieldName.Replace("ı", "i"); 
            FieldInfo fieldInfo;
            this.GetType().GetTypeFields().TryGetValue(fieldName, out fieldInfo);
            if (fieldInfo == null)
                throw new Exception("Invalid Field '" + fieldName + "'");
            var oldValue = property.GetValue(this, null);
            if (OldValue == null)
                OldValue = this.Clone() as T;
            fieldInfo.SetValue(OldValue, oldValue);
            fieldInfo.SetValue(this, value);

        }

        public List<TDataStructure> GetAggregationPropertyValue<TDataStructure>(string propertyName) where TDataStructure : class
        {
            PropertyInfo property;
            this.GetType().GetTypePropertiesDictionary().TryGetValue(propertyName, out property);
            if (property == null)
                throw new Exception("Invalid Property '" + propertyName + "'");
            var fieldName = ("_" + propertyName.Substring(0, 1).ToLower() + propertyName.Substring(1, propertyName.Length - 1));
            FieldInfo fieldInfo;
            this.GetType().GetTypeFields().TryGetValue(fieldName, out fieldInfo);
            if (fieldInfo == null)
                throw new Exception("Invalid Field '" + fieldName + "'");
            var value= DBManager.GetAggregation<TDataStructure>(this.ConnectionHandler, property, this);
            fieldInfo.SetValue(this, value);
            return value;


        }


    }

    [Serializable]
    public abstract class DataStructureBase
    {

        public object Clone()
        {
            return this.MemberwiseClone();
        }
        protected DataStructureBase()
        {
            this.State = ObjectState.None;
        }
        public bool Equal<T>(T targetdataStructure)
        {

            var inKey = this.GetType().GetTypeKeyProperties();
            return inKey.Aggregate(true,
                (current, propertyInfo) =>
                    current & propertyInfo.GetValue(this, null).Equals(propertyInfo.GetValue(targetdataStructure, null)));
        }
        [DisableAction(DiableAllAction = true)]
        [JsonIgnore]
        public ObjectState State { get; set; }

        private dynamic _dynamic;

        [DisableAction(DiableAllAction = true)]
        [JsonIgnore]
        public dynamic Dynamic
        {
            get { return _dynamic ?? (_dynamic = new ExpandoObject()); }
            set { _dynamic = value; }
        }



        [DisableAction(DiableAllAction = true)]
        [JsonIgnore]
        public string TrackerUserName { get; set; }

        [DisableAction(DiableAllAction = true)]
        [JsonIgnore]
        public string RootId { get; set; }

        private string _currentUICultureName;

        [DisableAction(DiableAllAction = true)]
        [JsonIgnore]
        public string CurrentUICultureName
        {
            get
            {
                return String.IsNullOrEmpty(_currentUICultureName)
                    ? Thread.CurrentThread.CurrentUICulture.Name
                    : _currentUICultureName;
            }
            set { _currentUICultureName = value; }
        }


        [DisableAction(DiableAllAction = true)]
        [JsonIgnore]
        public bool DisableTrack { get; set; }

        [DisableAction(DiableAllAction = true)]
        [JsonIgnore]
        public Type RootObject { get; set; }

        private IConnectionHandler _connectionHandler;
        protected internal virtual IConnectionHandler ConnectionHandler
        {
            get
            {
                if (_connectionHandler != null) return _connectionHandler;
                var classAttributeModel = this.GetType().GetClassAttributes();
                if (classAttributeModel.ConnectionHandlerType == null) return null;
                _connectionHandler = (IConnectionHandler) Activator.CreateInstance(classAttributeModel.ConnectionHandlerType.Type);
                return _connectionHandler;
            }
            set { this._connectionHandler = value; }
        }

    }
}