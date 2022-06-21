namespace Mhazami.Framework
{
    public enum CachType
    {
        None = 0,
        AssosiationModelCache,
        TypeProperties,
        TypeFields,
        TypeBOClassess,
        PropertyAttributeModel,
        ClassAttributeModel,
        HasTable,
        AggregationCache,

    }
    public enum JoinCompareType
    {
        None,
        Like,
        StringCompare,

    }
    public enum JoinType
    {
        None,
        Inner,
        Left,
        Right,
        Full
    }
    public enum OrderType
    {
        ASC,
        DESC
    }
    public enum AggrigateFuntionType
    {
        Sum,
        Max,
        Min,
        Count,
        AVG,

    }
    public enum ExpressionMethods
    {

        Contains,
        StartsWith,
        EndsWith,
        Equals,
        CompareTo,
        Compare,
        ToLower,
        ToUpper,
        ToString,
        In,
        NotIn,
        IsNullOrEmpty,
        Trim,
        ToShortDateString,
        Substring,
        Length,
        IndexOf,
        Year,
        Month,
        Day,
        ToInt,
        ToLong,
        ToShort,
        ToByte,
        ToDouble,
        ToDecimal,
        ToFloat,
        ToBool,
        ToGuid,
        ToDateTime,




    }

}
