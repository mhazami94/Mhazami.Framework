namespace Mhazami.Framework
{
    [Serializable]
    public class ChangeTracker
    {
        public string ObjectName { get; set; }

        public string FieldDesc { get; set; }

        public string FieldName { get; set; }

        public object OldValue { get; set; }

        public object NewValue { get; set; }


    }
}
