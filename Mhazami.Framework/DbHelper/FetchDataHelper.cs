using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Mhazami.Framework
{

    public class FetchDataHelper
    {

        public static FetchDataHelper Instance()
        {
            return new FetchDataHelper();
        }
        public static FetchDataHelper Instance(bool simpleLoad)
        {
            return new FetchDataHelper(simpleLoad);
        }
        public static FetchDataHelper Instance(bool simpleLoad,string culture)
        {
            return new FetchDataHelper(simpleLoad,culture);
        }
        public static FetchDataHelper Instance(string culture)
        {
            return new FetchDataHelper(culture);
        }
        public static FetchDataHelper InstanceFilterIncludeAssosation(bool filterAssosation, bool filterIncludeAssosation)
        {
            return new FetchDataHelper() { FilterAssosation = filterAssosation, FilterIncludeAssosation = filterIncludeAssosation };
        }
        public static FetchDataHelper InstanceFilterAssosation(bool filterAssosation)
        {
            return new FetchDataHelper() { FilterAssosation = filterAssosation };
        }
        private FetchDataHelper(string culture)
        {
            this.SimpleLoad = false;
            this.Culture = culture;
        }
        private FetchDataHelper()
        {
            this.SimpleLoad = false;
        }
        private FetchDataHelper(bool simpleLoad)
        {
            this.SimpleLoad = simpleLoad;
            this.FilterAssosation = simpleLoad;
        }
        private FetchDataHelper(bool simpleLoad, string culture)
        {
            this.SimpleLoad = simpleLoad;
            this.FilterAssosation = simpleLoad;
            this.Culture = culture;
        }

        #region Fileds
        private Dictionary<string, string[]> _columnArray;
        private Dictionary<string, object> _objectList;
       
        private Dictionary<string, string> _selectedKeys;
        private List<string> _selectedAssosiation;
        private List<string> _includeAssosiation;
        private Dictionary<string, bool> _selectedMultiLangAssosiation;
        private string _culture;

        #endregion

        #region Property
        internal string[] Columnnames { get; set; }

        internal string Culture
        {
            get{ return string.IsNullOrEmpty(_culture)? Thread.CurrentThread.CurrentUICulture.Name : _culture;}
            set { _culture = value; }
        }
        public bool FilterAssosation { get; set; }
        public bool FilterIncludeAssosation { get; set; }
        internal bool SimpleLoad { get; set; }

        private Dictionary<string, string[]> ColumnArray
        {
            get
            {
                return this._columnArray ?? (this._columnArray = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase));
            }
        }

       
        private Dictionary<string, object> ObjectList
        {
            get
            {
                return this._objectList ?? (this._objectList = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase));
            }
        }


        private Dictionary<string, string> SelectedKeys
        {
            get { return this._selectedKeys ?? (this._selectedKeys = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)); }
            set { _selectedKeys = value; }
        }
        internal List<string> SelectedAssosiation
        {
            get { return this._selectedAssosiation ?? (this._selectedAssosiation = new List<string>()); }
            set { _selectedAssosiation = value; }
        }
        internal List<string> IncludeAssosiation
        {
            get { return this._includeAssosiation ?? (this._includeAssosiation = new List<string>()); }
            set { _includeAssosiation = value; }
        }
        internal Dictionary<string,bool> SelectedMultiLangAssosiation
        {
            get { return this._selectedMultiLangAssosiation ?? (this._selectedMultiLangAssosiation = new Dictionary<string, bool>()); }
            set { _selectedMultiLangAssosiation = value; }
        }

        #endregion

        #region Methods


        internal object GetObjectValue(Type type, string key)
        {
            object getobj;
            ObjectList.TryGetValue(key, out getobj);
            return getobj;
        }

        internal void AddObjectList(string key, object value)
        {
            object getobj;
            if (!ObjectList.TryGetValue(key, out getobj))
            {
                ObjectList.Add(key, value);
            }
        }

        
        internal string[] GetColumnArray(string key)
        {
            string[] columnlist;
            if (!ColumnArray.TryGetValue(key, out columnlist))
            {
                columnlist = Columnnames != null ? Columnnames.Where(x => x.Contains(key)).ToArray() : new string[] { };
                ColumnArray.Add(key, columnlist);
            }
            return columnlist;
        }
        internal bool IsValidAssosiationKey(string parentkey, string key, bool isroot, bool isfrom = false)
        {
            if (SelectedKeysContaints(key)) return false;
            if (!isfrom && FilterIncludeAssosation)
                return IncludeAssosiation.Contains(key);
            return ((!SelectedAssosiation.Any() && !SimpleLoad) || SelectedAssosiation.Contains(key) ||
                                                                         (!FilterIncludeAssosation && !isroot && SelectedAssosiation.Contains(parentkey)));

        }

        internal bool IsValidMultiLangKey(string parentkey, string key, bool isroot)
        {
            if (SelectedKeysContaints(key)) return false;
            return (SelectedMultiLangAssosiation.ContainsKey(key) || (!isroot && SelectedMultiLangAssosiation.ContainsKey(parentkey)));
        }
        private bool SelectedKeysContaints(string key)
        {
            string value;
            var tryGetValue = SelectedKeys.TryGetValue(key, out value);
            return tryGetValue;
        }


        internal void AddSelectedKeys(string key)
        {
            SelectedKeys.Add(key, key);
        }
        internal void RenewSelectedKeys()
        {
            SelectedKeys = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }


       
     

      
        #endregion
    }
}
