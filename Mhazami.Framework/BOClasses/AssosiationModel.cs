using System.Reflection;

namespace Mhazami.Framework.BOClasses
{

    [Serializable]
   public class AssosiationModel
    {

        public PropertyInfo PropNameProperty { get; set; }
        public PropertyInfo AssosiationProperty { get; set; }
        public PropertyInfo[] FromQueryProperteis { get; set; }
        public PropertyInfo[] FromQueryAssosiationKeyNames { get; set; }
        public PropertyInfo AssosiationKeyProperty { get; set; }

        public string AssosiationSchemaName { get; set; }
        public JoinCompareType JoinCompareType { get; set; }
        public JoinType JoinType { get; set; }





    }
    [Serializable]
    class AggregationModel
    {
        public PropertyInfo[] AggrigateObjectPropNamePropertys { get; set; }
        public PropertyInfo[] PropNamePropertys { get; set; }
      

    }


}
