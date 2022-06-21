namespace Mhazami.Framework
{
    [Serializable]
    [Schema("dbo")]
    public sealed  class Tracker : DataStructureBase<Tracker>
    {

        private Int64 _id;
        [Key(true)]
        [DbType("bigint")]
        public Int64 Id
        {
            get { return _id; }
            set { base.SetPropertyValue("Id", value); }
        }

        private string _refId;
        [IsNullable]
        [DbType("nvarchar(200)")]
        public string RefId
        {
            get { return _refId; }
            set { base.SetPropertyValue("RefId", value); }
        }

        private string _rootId;
        [IsNullable]
        [DbType("nvarchar(200)")]
        public string RootId
        {
            get { return _rootId; }
            set { base.SetPropertyValue("RootId", value); }
        }

        private string _rootTitle;
        [IsNullable]
        [DbType("nvarchar(200)")]
        public string RootTitle
        {
            get { return _rootTitle; }
            set { base.SetPropertyValue("RootTitle", value); }
        }


        private string _ipAddress;
        [IsNullable]
        [DbType("nvarchar(50)")]
        public string IpAddress
        {
            get { return _ipAddress; }
            set { base.SetPropertyValue("IpAddress", value); }
        }

        private string _userName;
        [IsNullable]
        [DbType("nvarchar(50)")]
        public string UserName
        {
            get { return _userName; }
            set { base.SetPropertyValue("UserName", value); }
        }

        private string _date;
        [IsNullable]
        [DbType("char(10)")]
        public string Date
        {
            get { return _date; }
            set { base.SetPropertyValue("Date", value); }
        }

        private string _time;
        [IsNullable]
        [DbType("char(10)")]
        public string Time
        {
            get { return _time; }
            set { base.SetPropertyValue("Time", value); }
        }

        private string _objectName;
        [IsNullable]
        [DbType("nvarchar(100)")]
        public string ObjectName
        {
            get { return _objectName; }
            set { base.SetPropertyValue("ObjectName", value); }
        }

        private string _fieldDesc;
        [IsNullable]
        [DbType("nvarchar(100)")]
        public string FieldDesc
        {
            get { return _fieldDesc; }
            set { base.SetPropertyValue("FieldDesc", value); }
        }

        private string _fieldName;
        [IsNullable]
        [DbType("nvarchar(50)")]
        public string FieldName
        {
            get { return _fieldName; }
            set { base.SetPropertyValue("FieldName", value); }
        }

        private string _oldValue;
        [IsNullable]
        [DbType("nvarchar(200)")]
        public string OldVal
        {
            get { return _oldValue; }
            set { this._oldValue = value; }
        }

        private string _operation;
        [IsNullable]
        [DbType("nvarchar(200)")]
        public string Operation
        {
            get { return _operation; }
            set { base.SetPropertyValue("Operation", value); }
        }

        private string _newValue;
        [IsNullable]
        [DbType("nvarchar(200)")]
        public string NewValue
        {
            get { return _newValue; }
            set { this._newValue = value; }
        }

        private string _refTitle;
        [IsNullable]
        [DbType("nvarchar(500)")]
        public string RefTitle
        {
            get { return _refTitle; }
            set { this._refTitle = value; }
        }

        private string _masterRefId;
        [IsNullable]
        [DbType("nvarchar(200)")]
        public string MasterRefId
        {
            get { return _masterRefId; }
            set { base.SetPropertyValue("MasterRefId", value); }
        }





        private string _masterObjectRefId;
        [IsNullable]
        [DbType("nvarchar(200)")]
        public string MasterObjectRefId
        {
            get { return _masterObjectRefId; }
            set { base.SetPropertyValue("MasterObjectRefId", value); }
        }

       
    }
   
}

