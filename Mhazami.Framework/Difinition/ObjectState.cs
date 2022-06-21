namespace Mhazami.Framework
{
    public enum ObjectState
    {
        None,
        New,
        Deleted,
        Dirty,
    }
   
    public enum  SqlCompareOperator
    {
        [System.ComponentModel.Description("like")]
        Like,
        [System.ComponentModel.Description("=")]
        Equal,
        [System.ComponentModel.Description("<")]
        LessThan,
        [System.ComponentModel.Description("<=")]
        EqualLessThan,
        [System.ComponentModel.Description(">")]
        GreatThan,
        [System.ComponentModel.Description(">=")]
        EqualGreatThan,
        [System.ComponentModel.Description("<>")]
        NotEqual
    }
    public enum OperationLogType
    {
        [System.ComponentModel.Description("ایجاد رکورد")]
        InsertRecord = 1,
        [System.ComponentModel.Description("تغییر فیلد")]
        UpdateField = 2,
        [System.ComponentModel.Description("حذف رکورد")]
        DeleteRecord = 3,
        [System.ComponentModel.Description("ورود به سامانه")]
        Login = 4,
        [System.ComponentModel.Description("ایجاد مستند")]
        InsertDoc = 5,
        [System.ComponentModel.Description("ویرایش مستند")]
        UpdateDoc = 6,
        [System.ComponentModel.Description("حذف مستند")]
        DeleteDoc = 7,
    }

}
