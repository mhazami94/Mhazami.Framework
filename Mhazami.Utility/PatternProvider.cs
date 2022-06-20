namespace Mhazami.Utility
{
    public class PatternContentProvider
    {
        private readonly Dictionary<string, PatternInput> dic = new Dictionary<string, PatternInput>();

     

        public void RegisterMethod(string keyword, RegisteredMethod method, params object[] input)
        {
            this.dic.Add(keyword, new PatternInput
                                      {
                                          Method = method,
                                          Inputs = input
                                      });
        }

        public string GetPatterContent(string pattern , char seprator)
        {
            pattern = pattern.ToLower();
            foreach (var str in pattern.Split(new[] { seprator }))
            {
                if (this.dic.ContainsKey(str))
                    pattern = pattern.Replace(str, this.dic[str].Method(this.dic[str].Inputs).ToString());
            }
            return pattern;
        }

       

        public delegate object RegisteredMethod(params object[] input);
    }

    internal sealed class PatternInput
    {
        public PatternContentProvider.RegisteredMethod Method { get; set; }

        public object[] Inputs { get; set; }
    }
}
