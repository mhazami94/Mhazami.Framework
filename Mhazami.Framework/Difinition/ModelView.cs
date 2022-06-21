using System.Linq.Expressions;

namespace Mhazami.Framework
{
    public class GroupByModel<T>
    {
        public AggrigateFuntionType AggrigateFuntionType { get; set; }
        public Expression<Func<T, Object>> Expression { get; set; }


    }

    public class OrderByModel<T>
    {
        public OrderType OrderType { get; set; }
        public Expression<Func<T, Object>> Expression { get; set; }


    }

}
