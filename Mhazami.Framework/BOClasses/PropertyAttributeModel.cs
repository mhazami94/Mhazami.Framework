namespace Mhazami.Framework.BOClasses
{

    [Serializable]
    public class PropertyAttributeModel
    {

        public Assosiation Assosiation { get; set; }
        public Aggregation Aggregation{ get; set; }
        public Unique Unique { get; set; }
        public DbType DbType { get; set; }
        public MultiLanguage MultiLanguage { get; set; }
        public Key Key { get; set; }
        public IsNullable IsNullable { get; set; }
        public Layout Layout { get; set; }
        public TrackMaster TrackMaster { get; set; }
        public Identity Identity { get; set; }
        public DisableAction DisableAction { get; set; }
        public TrackPropertyName TrackPropertyName { get; set; }
      




    }
}
