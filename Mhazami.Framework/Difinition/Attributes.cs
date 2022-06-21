



namespace Mhazami.Framework
{
    [AttributeUsage(AttributeTargets.Property)]
    [Serializable]
    public sealed class Key : Attribute
    {
        public Key(bool identity)
        {
            this.Identity = identity;
            
        }
        public Key(bool identity,bool EnableTrack)
        {
            this.Identity = identity;
            this.EnableTrack = EnableTrack;

        }
        internal bool Identity { get; set; }
        internal bool EnableTrack { get; set; }
    }

    

    [AttributeUsage(AttributeTargets.Property)]
    [Serializable]
    public sealed class Identity : Attribute
    {
        public Identity() : this(true) { }

        public Identity(bool identity)
        {
            this.IsIdentity = identity;
        }

        public bool IsIdentity { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    [Serializable]
    public sealed class Layout : Attribute
    {
        public string Caption { get; set; }

        public int Width { get; set; }
    }


    [AttributeUsage(AttributeTargets.Property)]
    [Serializable]
    public sealed class DisableAction : Attribute
    {
        /// <summary>
        /// غیر فعال بودن Insert , Update , Select , Track
        /// </summary>
        public bool DiableAllAction { get; set; }
        public bool DiableSelect { get; set; }
        public bool DisableInsert { get; set; }
        public bool DisableUpdate { get; set; }
        public bool DisableTrack { get; set; }
    }
    [AttributeUsage(AttributeTargets.Class)]
    [Serializable]
    public sealed class Schema : Attribute
    {
        public Schema(string schemaName)
        {
            this.SchemaName = schemaName;
        }

        public string SchemaName { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    [Serializable]
    public sealed class ConnectionHandlerType : Attribute
    {
        public ConnectionHandlerType(Type connectionHandlerType)
        {
            this.Type = connectionHandlerType;
        }

        public Type Type { get; set; }
    }

    [AttributeUsage(AttributeTargets.All)]
    [Serializable]
    public sealed class Description : Attribute
    {
        private string _layoutDescription;

        public Description(string layoutDescription)
        {
            this.LayoutDescription = layoutDescription;
        }

        private string description = "";


        public string LayoutDescription
        {
            get
            {
                GetValue();
                return description;
            }
            set { _layoutDescription = value; }
        }

        public Type Type { get; set; }

        private void GetValue()
        {
            if (Type == null)
            {
                description = _layoutDescription;
                return;
            }

            var propertyInfos = Type.GetProperty(_layoutDescription);
            if (propertyInfos != null)
            {
                var value = Type.GetProperty(_layoutDescription).GetValue(Type, null);
                if (value != null)
                    description = value.ToString();
            }
        }
    }


    [AttributeUsage(AttributeTargets.Field)]
    [Serializable]
    public sealed class Application : Attribute
    {
        public Application(object application)
        {
            this.ApplicationId = application;
        }

        public object ApplicationId { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    [Serializable]
    public sealed class IsNullable : Attribute { }

    

    [AttributeUsage(AttributeTargets.Property)]
    [Serializable]
    public sealed class Assosiation : Attribute
    {
        public Assosiation()
        {
            this.FillData = true;
            
        }
        /// <summary>
        ///  Property Name Is Class Has Relation  thia Assosiation
        /// </summary>
        public string  PropName { get; set; }
        public string JoinPropName { get; set; }
        public bool FillData { get; set; }
        public JoinCompareType JoinCompareType { get; set; }
        public JoinType JoinType { get; set; }
       
       

    }


    [AttributeUsage(AttributeTargets.Property)]
    [Serializable]
    public sealed class Aggregation : Attribute
    {
        
        public string PropName { get; set; }
        public string AggigatePropName { get; set; }

    }

    [AttributeUsage(AttributeTargets.Property)]
    [Serializable]
    public sealed class Unique : Attribute
    {
        public Unique()
        {
            ThrowException = true;
        }

        public bool IgnoreNull { get; set; }

        public string Group { get; set; }

        public bool ThrowException { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    [Serializable]
    public sealed class RSS : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property)]
    [Serializable]
    public sealed class DocumentReferenceField : Attribute
    {
        public DocumentReferenceField(string fieldName)
        {
            this.FieldName = fieldName;
        }

        public string FieldName { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    [Serializable]
    public sealed class DbType : Attribute
    {
        public DbType(string dbtype)
        {
            this.dbType = dbtype;
        }

        public string dbType { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    [Serializable]
    public sealed class Filter : Attribute
    {
        public Filter(SqlCompareOperator compareOperator = SqlCompareOperator.Like)
        {
            this.Operator = compareOperator;
        }

        public SqlCompareOperator Operator { get; set; }

    }

    [AttributeUsage(AttributeTargets.Property)]
    [Serializable]
    public sealed class MultiLanguage : Attribute
    {
        public MultiLanguage()
        {
           
            this.FillAnyLanguage = false;
        }

       
        public bool FillAnyLanguage { get; set; }

    }

    [AttributeUsage(AttributeTargets.Property)]
    [Serializable]
    public sealed class TrackPropertyName : Attribute
    {
        public TrackPropertyName(string propName)
        {
            PropName = propName;
        }

        public string PropName { get; set; }



    }
    [AttributeUsage(AttributeTargets.Class)]
    [Serializable]
    public sealed class Track : Attribute
    {
        public Track()
        {
            Enabled = true;
        }
        public bool Enabled { get; set; }

    }

    [AttributeUsage(AttributeTargets.Property)]
    [Serializable]
    public sealed class TrackMaster : Attribute
    {
        public TrackMaster(Type mastertype)
        {
            MasterType = mastertype;
          
        }
        public Type MasterType { get; set; }
      
    }

    [AttributeUsage(AttributeTargets.Class)]
    [Serializable]
    public sealed class Cacheable : Attribute
    {
        public Cacheable()
        {
          
        }
    }


}
