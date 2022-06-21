namespace Mhazami.Framework.NonPersistTemprory
{
    public static class Contrans
    {
        public static bool IgnorePersistData { get; set; }
    }

    public static class Items
    {
        public static Dictionary<object, ObjectState> List { get; set; }

        internal static void Add(object item, ObjectState state)
        {
            if (List == null)
                List = new Dictionary<object, ObjectState>();
            if (List.ContainsKey(item))
                List.Remove(item);
            List.Add(item, state);
        }
    }
}
