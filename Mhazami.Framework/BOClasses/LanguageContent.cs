namespace Mhazami.Framework
{
    [Serializable]
    [Schema("Common")]
    public sealed class LanguageContent : DataStructureBase<LanguageContent>
    {
        private string _key;
        [Key(false)]
        [DbType("varchar(400)")]
        public string Key
        {
            get { return _key; }
            set { base.SetPropertyValue("Key", value); }
        }

        private string _value;
        [IsNullable]
        [DbType("nvarchar(max)")]
        public string Value
        {
            get { return _value; }
            set { base.SetPropertyValue("Value", value); }
        }

        private string _languageId;
        [Key(false)]
        [DbType("char(5)")]
        public string LanguageId
        {
            get
            {
                return this._languageId;
            }
            set
            {
                base.SetPropertyValue("LanguageId", value);
              
            }
        }

      

        private bool _isDefault;
        [DbType("bit")]
        public bool IsDefault
        {
            get { return _isDefault; }
            set { base.SetPropertyValue("IsDefault", value); }
        }

        [DisableAction(DisableInsert = true, DisableUpdate = true, DiableSelect = true)]
        public string FieldName { get; set; }

    }
}
