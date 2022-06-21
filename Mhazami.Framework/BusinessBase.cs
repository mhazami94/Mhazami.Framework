using Mhazami.Framework.DbHelper;
using Mhazami.Framework.Difinition;
using System.Linq.Expressions;

namespace Mhazami.Framework
{
    [Serializable]
    public class BusinessBase<TDataStructure> where TDataStructure : class
    {
        #region Manual

        public virtual TDataStructure Get(IConnectionHandler connectionHandler, params object[] keys)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Get(keys);
        }
        public virtual TDataStructure SimpleGet(IConnectionHandler connectionHandler, params object[] keys)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.SimpleGet(keys);
        }
        public virtual void GetLanuageContent(IConnectionHandler connectionHandler, string culture, TDataStructure obj)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            dal.GetLanuageContent(culture, obj);
        }
        public virtual void GetLanuageContent(IConnectionHandler connectionHandler, string culture, List<TDataStructure> obj)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            dal.GetLanuageContent(culture, obj);
        }
        public virtual TDataStructure GetLanuageContent(IConnectionHandler connectionHandler, string culture, params object[] keys)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.GetLanuageContent(culture, keys);
        }
        public virtual TDataStructure GetLanuageContentsimple(IConnectionHandler connectionHandler, string culture, params object[] keys)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.GetLanuageContentsimple(culture, keys);
        }
        public virtual List<TDataStructure> GetAll(IConnectionHandler connectionHandler, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.GetAll(simpleload);

        }

        public virtual bool Insert(IConnectionHandler connectionHandler, TDataStructure obj)
        {
            this.CheckConstraint(connectionHandler, obj);
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Insert(obj) > 0;

        }
        public virtual bool Update(IConnectionHandler connectionHandler, TDataStructure obj)
        {
            this.CheckConstraint(connectionHandler, obj);
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Update(obj) > 0;


        }
        public virtual bool Delete(IConnectionHandler connectionHandler, TDataStructure obj)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Delete(obj) > 0;
        }
        public virtual bool Delete(IConnectionHandler connectionHandler, params object[] keys)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Delete(keys) > 0;
        }

        public virtual bool Insert(IConnectionHandler connectionHandler, TDataStructure obj, Delegates.LogMethod method, params object[] parameters)
        {
            this.CheckConstraint(connectionHandler, obj);
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Insert(obj, method, parameters) > 0;
        }
        public virtual bool Delete(IConnectionHandler connectionHandler, TDataStructure obj, Delegates.LogMethod method, params object[] parameters)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Delete(obj, method, parameters) > 0;
        }
        public virtual bool Update(IConnectionHandler connectionHandler, TDataStructure obj, Delegates.LogMethod method, params object[] parameters)
        {
            this.CheckConstraint(connectionHandler, obj);
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Update(obj, method, parameters) > 0;

        }
        protected virtual void CheckConstraint(IConnectionHandler connectionHandler, TDataStructure item)
        {
        }

        public virtual List<TDataStructure> Where(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, bool>> expression, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Where(expression, simpleload);

        }

        public virtual bool Any(IConnectionHandler connectionHandler)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Any();

        }
        public virtual bool Any(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, bool>> expression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Any(expression);

        }

        public virtual TResult Max<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Max(expression);

        }
        public virtual double Average<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Average(expression);

        }
        public virtual TResult Min<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Min(expression);

        }
        public virtual TResult Sum<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Sum(expression);

        }
        public virtual TResult Max<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Max(expression, conditionsexpression);

        }
        public virtual double Average<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Average(expression, conditionsexpression);

        }
        public virtual TResult Min<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Min(expression, conditionsexpression);

        }
        public virtual TResult Sum<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Sum(expression, conditionsexpression);

        }

        public virtual int Count(IConnectionHandler connectionHandler)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Count();

        }
        public virtual int Count(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, bool>> expression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Count(expression);

        }
        public virtual int Count<TColumn>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TColumn>> expression, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Count(expression, distinct);

        }
        public virtual int Count<TColumn>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TColumn>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Count(expression, conditionsexpression, distinct);

        }



        public virtual TDataStructure FirstOrDefault(IConnectionHandler connectionHandler, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.FirstOrDefault(simpleload);

        }
        public virtual TDataStructure FirstOrDefault(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, bool>> expression, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.FirstOrDefault(expression, simpleload);

        }
        public virtual TDataStructure FirstOrDefaultWithOrderBy(IConnectionHandler connectionHandler, OrderByModel<TDataStructure>[] orderByexpression, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.FirstOrDefaultWithOrderBy(orderByexpression, simpleload);

        }
        public virtual TDataStructure FirstOrDefaultWithOrderBy(IConnectionHandler connectionHandler, OrderByModel<TDataStructure>[] orderByexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.FirstOrDefaultWithOrderBy(orderByexpression, conditionsexpression, simpleload);

        }
        public virtual TDataStructure FirstOrDefaultWithOrderBy<TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.FirstOrDefaultWithOrderBy(orderByexpression, simpleload);

        }
        public virtual TDataStructure FirstOrDefaultWithOrderByDescending<TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.FirstOrDefaultWithOrderByDescending(orderByexpression, simpleload);

        }
        public virtual TDataStructure FirstOrDefaultWithOrderBy<TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.FirstOrDefaultWithOrderBy(orderByexpression, conditionsexpression, simpleload);

        }
        public virtual TDataStructure FirstOrDefaultWithOrderByDescending<TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.FirstOrDefaultWithOrderByDescending(orderByexpression, conditionsexpression, simpleload);

        }


        public virtual TDataStructure SingleOrDefault(IConnectionHandler connectionHandler, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.SingleOrDefault();

        }
        public virtual TDataStructure SingleOrDefault(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, bool>> expression, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.SingleOrDefault(expression);

        }




        public virtual TDataStructure IncludeFirstOrDefault(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] Includeexpression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.IncludeFirstOrDefault(Includeexpression);

        }
        public virtual TDataStructure IncludeFirstOrDefault(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] Includeexpression, OrderByModel<TDataStructure> orderByModel)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.IncludeFirstOrDefault(Includeexpression, orderByModel);

        }
        public virtual TDataStructure IncludeFirstOrDefault(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] Includeexpression, OrderByModel<TDataStructure>[] orderByModels)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.IncludeFirstOrDefault(Includeexpression, orderByModels);

        }
        public virtual TDataStructure IncludeFirstOrDefault(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.IncludeFirstOrDefault(Includeexpression, conditionsexpression);


        }
        public virtual TDataStructure IncludeFirstOrDefault(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.IncludeFirstOrDefault(Includeexpression, conditionsexpression, orderByModel);

        }
        public virtual TDataStructure IncludeFirstOrDefault(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModels)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.IncludeFirstOrDefault(Includeexpression, conditionsexpression, orderByModels);

        }



        public virtual List<TDataStructure> IncludeTop(IConnectionHandler connectionHandler, int topcount, Expression<Func<TDataStructure, Object>>[] Includeexpression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.IncludeTop(topcount, Includeexpression);

        }
        public virtual List<TDataStructure> IncludeTop(IConnectionHandler connectionHandler, int topcount, Expression<Func<TDataStructure, Object>>[] Includeexpression, OrderByModel<TDataStructure> orderByModel)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.IncludeTop(topcount, Includeexpression, orderByModel);

        }
        public virtual List<TDataStructure> IncludeTop(IConnectionHandler connectionHandler, int topcount, Expression<Func<TDataStructure, Object>>[] Includeexpression, OrderByModel<TDataStructure>[] orderByModels)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.IncludeTop(topcount, Includeexpression, orderByModels);

        }
        public virtual List<TDataStructure> IncludeTop(IConnectionHandler connectionHandler, int topcount, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.IncludeTop(topcount, Includeexpression, conditionsexpression);


        }
        public virtual List<TDataStructure> IncludeTop(IConnectionHandler connectionHandler, int topcount, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.IncludeTop(topcount, Includeexpression, conditionsexpression, orderByModel);

        }
        public virtual List<TDataStructure> IncludeTop(IConnectionHandler connectionHandler, int topcount, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModels)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.IncludeTop(topcount, Includeexpression, conditionsexpression, orderByModels);

        }



        public virtual List<TDataStructure> IncludePagedList(IConnectionHandler connectionHandler, int pageIndex, int pagesize, Expression<Func<TDataStructure, Object>>[] Includeexpression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.IncludePagedList(pageIndex, pagesize, Includeexpression);

        }
        public virtual List<TDataStructure> IncludePagedList(IConnectionHandler connectionHandler, int pageIndex, int pagesize, Expression<Func<TDataStructure, Object>>[] Includeexpression, OrderByModel<TDataStructure> orderByModel)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.IncludePagedList(pageIndex, pagesize, Includeexpression, orderByModel);

        }
        public virtual List<TDataStructure> IncludePagedList(IConnectionHandler connectionHandler, int pageIndex, int pagesize, Expression<Func<TDataStructure, Object>>[] Includeexpression, OrderByModel<TDataStructure>[] orderByModels)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.IncludePagedList(pageIndex, pagesize, Includeexpression, orderByModels);

        }
        public virtual List<TDataStructure> IncludePagedList(IConnectionHandler connectionHandler, int pageIndex, int pagesize, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.IncludePagedList(pageIndex, pagesize, Includeexpression, conditionsexpression);


        }
        public virtual List<TDataStructure> IncludePagedList(IConnectionHandler connectionHandler, int pageIndex, int pagesize, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.IncludePagedList(pageIndex, pagesize, Includeexpression, conditionsexpression, orderByModel);

        }
        public virtual List<TDataStructure> IncludePagedList(IConnectionHandler connectionHandler, int pageIndex, int pagesize, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModels)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.IncludePagedList(pageIndex, pagesize, Includeexpression, conditionsexpression, orderByModels);

        }




        public virtual List<TDataStructure> Include(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] Includeexpression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Include(Includeexpression);

        }
        public virtual List<TDataStructure> Include(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] Includeexpression, OrderByModel<TDataStructure> orderByModel)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Include(Includeexpression, orderByModel);

        }
        public virtual List<TDataStructure> Include(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] Includeexpression, OrderByModel<TDataStructure>[] orderByModels)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Include(Includeexpression, orderByModels);

        }
        public virtual List<TDataStructure> Include(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Include(Includeexpression, conditionsexpression);


        }
        public virtual List<TDataStructure> Include(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Include(Includeexpression, conditionsexpression, orderByModel);

        }
        public virtual List<TDataStructure> Include(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModels)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Include(Includeexpression, conditionsexpression, orderByModels);

        }


        public virtual List<TDataStructure> PagedList(IConnectionHandler connectionHandler, int pageIndex, int pagesize, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.PagedList(pageIndex, pagesize, simpleload);

        }
        public virtual List<TDataStructure> PagedList(IConnectionHandler connectionHandler, int pageIndex, int pagesize, OrderByModel<TDataStructure> orderByModel, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.PagedList(pageIndex, pagesize, orderByModel, simpleload);

        }
        public virtual List<TDataStructure> PagedList(IConnectionHandler connectionHandler, int pageIndex, int pagesize, OrderByModel<TDataStructure>[] orderByModels, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.PagedList(pageIndex, pagesize, orderByModels, simpleload);

        }
        public virtual List<TDataStructure> PagedList(IConnectionHandler connectionHandler, int pageIndex, int pagesize, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.PagedList(pageIndex, pagesize, conditionsexpression, simpleload);


        }
        public virtual List<TDataStructure> PagedList(IConnectionHandler connectionHandler, int pageIndex, int pagesize, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.PagedList(pageIndex, pagesize, conditionsexpression, orderByModel, simpleload);

        }
        public virtual List<TDataStructure> PagedList(IConnectionHandler connectionHandler, int pageIndex, int pagesize, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModels, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.PagedList(pageIndex, pagesize, conditionsexpression, orderByModels, simpleload);

        }




        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.SelectKeyValuePair(DataValueField, DataTextField, distinct);
        }
        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.SelectKeyValuePair(DataValueField, DataTextField, conditionsexpression, distinct);

        }
        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.SelectKeyValuePair(DataValueField, DataTextField, conditionsexpression, orderByModel, distinct);

        }
        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.SelectKeyValuePair(DataValueField, DataTextField, conditionsexpression, orderbymodels, distinct);
        }
        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, OrderByModel<TDataStructure> orderByModel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.SelectKeyValuePair(DataValueField, DataTextField, orderByModel, distinct);
        }
        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, OrderByModel<TDataStructure>[] orderbymodels, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.SelectKeyValuePair(DataValueField, DataTextField, orderbymodels, distinct);
        }



        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, string culture, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.SelectKeyValuePair(DataValueField, DataTextField, culture, distinct);
        }
        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, string culture, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.SelectKeyValuePair(DataValueField, DataTextField, culture, conditionsexpression, distinct);

        }
        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, string culture, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.SelectKeyValuePair(DataValueField, DataTextField, culture, conditionsexpression, orderByModel, distinct);

        }
        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, string culture, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.SelectKeyValuePair(DataValueField, DataTextField, culture, conditionsexpression, orderbymodels, distinct);
        }
        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, string culture, OrderByModel<TDataStructure> orderByModel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.SelectKeyValuePair(DataValueField, DataTextField, culture, orderByModel, distinct);
        }
        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, string culture, OrderByModel<TDataStructure>[] orderbymodels, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.SelectKeyValuePair(DataValueField, DataTextField, culture, orderbymodels, distinct);
        }



        public virtual List<TDataStructure> SelectTop(IConnectionHandler connectionHandler, int topcount, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.SelectTop(topcount, simpleload);
        }
        public virtual List<TDataStructure> SelectTop(IConnectionHandler connectionHandler, int topcount, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.SelectTop(topcount, conditionsexpression, simpleload);

        }
        public virtual List<TDataStructure> SelectTop(IConnectionHandler connectionHandler, int topcount, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.SelectTop(topcount, conditionsexpression, orderByModel, simpleload);

        }
        public virtual List<TDataStructure> SelectTop(IConnectionHandler connectionHandler, int topcount, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.SelectTop(topcount, conditionsexpression, orderbymodels, simpleload);
        }
        public virtual List<TDataStructure> SelectTop(IConnectionHandler connectionHandler, int topcount, OrderByModel<TDataStructure> orderByModel, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.SelectTop(topcount, orderByModel, simpleload);
        }
        public virtual List<TDataStructure> SelectTop(IConnectionHandler connectionHandler, int topcount, OrderByModel<TDataStructure>[] orderbymodels, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.SelectTop(topcount, orderbymodels, simpleload);
        }




        public virtual TResult SelectFirstOrDefault<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.SelectFirstOrDefault(expression);
        }
        public virtual TResult SelectFirstOrDefault<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.SelectFirstOrDefault(expression, conditionsexpression);
        }
        public virtual TResult SelectFirstOrDefault<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, OrderByModel<TDataStructure> orderByModel)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.SelectFirstOrDefault(expression, orderByModel);
        }
        public virtual TResult SelectFirstOrDefault<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.SelectFirstOrDefault(expression, conditionsexpression, orderByModel);
        }
        public virtual TResult SelectFirstOrDefault<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, OrderByModel<TDataStructure>[] orderByModels)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.SelectFirstOrDefault(expression, orderByModels);
        }
        public virtual TResult SelectFirstOrDefault<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModels)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.SelectFirstOrDefault(expression, conditionsexpression, orderByModels);
        }


        public virtual List<TResult> Select<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Select(expression, distinct);
        }
        public virtual List<TResult> Select<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Select(expression, conditionsexpression, distinct);
        }
        public virtual List<TResult> Select<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, OrderByModel<TDataStructure> orderByModel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Select(expression, orderByModel, distinct);
        }
        public virtual List<TResult> Select<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Select(expression, conditionsexpression, orderByModel, distinct);
        }
        public virtual List<TResult> Select<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, OrderByModel<TDataStructure>[] orderByModels, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Select(expression, orderByModels, distinct);
        }
        public virtual List<TResult> Select<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModels, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Select(expression, conditionsexpression, orderByModels, distinct);
        }





        public virtual dynamic SelectFirstOrDefault(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.SelectFirstOrDefault(expression);
        }
        public virtual dynamic SelectFirstOrDefault(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.SelectFirstOrDefault(expression, conditionsexpression);

        }

        public virtual dynamic SelectFirstOrDefault(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, OrderByModel<TDataStructure> orderbymodel)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.SelectFirstOrDefault(expression, orderbymodel);
        }
        public virtual dynamic SelectFirstOrDefault(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderbymodel)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.SelectFirstOrDefault(expression, conditionsexpression, orderbymodel);

        }


        public virtual dynamic SelectFirstOrDefault(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, OrderByModel<TDataStructure>[] orderbymodels)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.SelectFirstOrDefault(expression, orderbymodels);
        }
        public virtual dynamic SelectFirstOrDefault(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.SelectFirstOrDefault(expression, conditionsexpression, orderbymodels);

        }




        public virtual List<dynamic> Select(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Select(expression, distinct);
        }
        public virtual List<dynamic> Select(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Select(expression, conditionsexpression, distinct);

        }

        public virtual List<dynamic> Select(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Select(expression, orderbymodel, distinct);
        }
        public virtual List<dynamic> Select(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Select(expression, conditionsexpression, orderbymodel, distinct);

        }


        public virtual List<dynamic> Select(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, OrderByModel<TDataStructure>[] orderbymodels, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Select(expression, orderbymodels, distinct);
        }
        public virtual List<dynamic> Select(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.Select(expression, conditionsexpression, orderbymodels, distinct);

        }







        public virtual List<TDataStructure> OrderBy<TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.OrderBy(orderByexpression, simpleload);

        }
        public virtual List<TDataStructure> OrderBy(IConnectionHandler connectionHandler, OrderByModel<TDataStructure>[] orderByexpression, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.OrderBy(orderByexpression, simpleload);
        }
        public virtual List<TDataStructure> OrderBy(IConnectionHandler connectionHandler, OrderByModel<TDataStructure>[] orderByexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.OrderBy(orderByexpression, conditionsexpression, simpleload);
        }
        public virtual List<TDataStructure> OrderByDescending<TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.OrderByDescending(orderByexpression, simpleload);

        }
        public virtual List<TDataStructure> OrderBy<TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.OrderBy(orderByexpression, conditionsexpression, simpleload);

        }
        public virtual List<TDataStructure> OrderByDescending<TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.OrderByDescending(orderByexpression, conditionsexpression, simpleload);

        }



        public virtual List<dynamic> GroupBy(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, bool distinct = false)
        {

            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.GroupBy(groupexpression, distinct);
        }
        public virtual List<dynamic> GroupBy(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.GroupBy(groupexpression, conditionsexpression, distinct);
        }
        public virtual List<dynamic> GroupByWithHaving(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.GroupByWithHaving(groupexpression, havingconditionsexpression, distinct);
        }
        public virtual List<dynamic> GroupByWithHaving(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.GroupByWithHaving(groupexpression, conditionsexpression, havingconditionsexpression, distinct);
        }


        public virtual List<dynamic> GroupBy(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {

            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.GroupBy(groupexpression, orderbymodel, distinct);
        }
        public virtual List<dynamic> GroupBy(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {

            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.GroupBy(groupexpression, orderbymodel, distinct);
        }
        public virtual List<dynamic> GroupBy(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.GroupBy(groupexpression, conditionsexpression, orderbymodel, distinct);
        }
        public virtual List<dynamic> GroupBy(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.GroupBy(groupexpression, conditionsexpression, orderbymodel, distinct);
        }

        public virtual List<dynamic> GroupByWithHaving(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.GroupByWithHaving(groupexpression, havingconditionsexpression, orderbymodel, distinct);
        }
        public virtual List<dynamic> GroupByWithHaving(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.GroupByWithHaving(groupexpression, havingconditionsexpression, orderbymodel, distinct);
        }
        public virtual List<dynamic> GroupByWithHaving(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.GroupByWithHaving(groupexpression, conditionsexpression, havingconditionsexpression, orderbymodel, distinct);
        }
        public virtual List<dynamic> GroupByWithHaving(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.GroupByWithHaving(groupexpression, conditionsexpression, havingconditionsexpression, orderbymodel, distinct);
        }


        public virtual List<dynamic> GroupBy(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, bool distinct = false)
        {

            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.GroupBy(groupexpression, aggrigateexpression, distinct);
        }
        public virtual List<dynamic> GroupBy(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.GroupBy(groupexpression, aggrigateexpression, conditionsexpression, distinct);
        }
        public virtual List<dynamic> GroupByWithHaving(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.GroupByWithHaving(groupexpression, aggrigateexpression, havingconditionsexpression, distinct);
        }
        public virtual List<dynamic> GroupByWithHaving(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.GroupByWithHaving(groupexpression, aggrigateexpression, conditionsexpression, havingconditionsexpression, distinct);
        }


        public virtual List<dynamic> GroupBy(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {

            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.GroupBy(groupexpression, aggrigateexpression, orderbymodel, distinct);
        }
        public virtual List<dynamic> GroupBy(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.GroupBy(groupexpression, aggrigateexpression, conditionsexpression, orderbymodel, distinct);
        }
        public virtual List<dynamic> GroupByWithHaving(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.GroupByWithHaving(groupexpression, aggrigateexpression, havingconditionsexpression, orderbymodel, distinct);
        }
        public virtual List<dynamic> GroupByWithHaving(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.GroupByWithHaving(groupexpression, aggrigateexpression, conditionsexpression, havingconditionsexpression, orderbymodel, distinct);
        }

        public virtual List<dynamic> GroupBy(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {

            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.GroupBy(groupexpression, aggrigateexpression, orderbymodel, distinct);
        }
        public virtual List<dynamic> GroupBy(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.GroupBy(groupexpression, aggrigateexpression, conditionsexpression, orderbymodel, distinct);
        }
        public virtual List<dynamic> GroupByWithHaving(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.GroupByWithHaving(groupexpression, aggrigateexpression, havingconditionsexpression, orderbymodel, distinct);
        }
        public virtual List<dynamic> GroupByWithHaving(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return dal.GroupByWithHaving(groupexpression, aggrigateexpression, conditionsexpression, havingconditionsexpression, orderbymodel, distinct);
        }

        #endregion


        #region Async

        public virtual async Task<TDataStructure> GetAsync(IConnectionHandler connectionHandler, params object[] keys)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.GetAsync(keys);
        }
        public virtual async Task<TDataStructure> SimpleGetAsync(IConnectionHandler connectionHandler, params object[] keys)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SimpleGetAsync(keys);
        }
        public virtual async Task GetLanuageContentAsync(IConnectionHandler connectionHandler, string culture, TDataStructure obj)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            await dal.GetLanuageContentAsync(culture, obj);
        }
        public virtual async Task GetLanuageContentAsync(IConnectionHandler connectionHandler, string culture, List<TDataStructure> obj)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            await dal.GetLanuageContentAsync(culture, obj);
        }
        public virtual async Task<TDataStructure> GetLanuageContentAsync(IConnectionHandler connectionHandler, string culture,
            params object[] keys)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.GetLanuageContentAsync(culture, keys);
        }
        public virtual async Task<TDataStructure> GetLanuageContentsimpleAsync(IConnectionHandler connectionHandler,
            string culture, params object[] keys)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.GetLanuageContentsimpleAsync(culture, keys);
        }
        public virtual async Task<List<TDataStructure>> GetAllAsync(IConnectionHandler connectionHandler, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.GetAllAsync(simpleload);

        }

        public virtual async Task<bool> InsertAsync(IConnectionHandler connectionHandler, TDataStructure obj)
        {
            this.CheckConstraint(connectionHandler, obj);
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.InsertAsync(obj) > 0;

        }
        public virtual async Task<bool> UpdateAsync(IConnectionHandler connectionHandler, TDataStructure obj)
        {
            this.CheckConstraint(connectionHandler, obj);
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.UpdateAsync(obj) > 0;


        }
        public virtual async Task<bool> DeleteAsync(IConnectionHandler connectionHandler, TDataStructure obj)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.DeleteAsync(obj) > 0;
        }
        public virtual async Task<bool> DeleteAsync(IConnectionHandler connectionHandler, params object[] keys)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.DeleteAsync(keys) > 0;
        }

        public virtual async Task<bool> InsertAsync(IConnectionHandler connectionHandler, TDataStructure obj, Delegates.LogMethod method, params object[] parameters)
        {
            this.CheckConstraint(connectionHandler, obj);
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.InsertAsync(obj, method, parameters) > 0;
        }
        public virtual async Task<bool> DeleteAsync(IConnectionHandler connectionHandler, TDataStructure obj, Delegates.LogMethod method, params object[] parameters)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.DeleteAsync(obj, method, parameters) > 0;
        }
        public virtual async Task<bool> UpdateAsync(IConnectionHandler connectionHandler, TDataStructure obj, Delegates.LogMethod method, params object[] parameters)
        {
            this.CheckConstraint(connectionHandler, obj);
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.UpdateAsync(obj, method, parameters) > 0;

        }

        public virtual async Task<List<TDataStructure>> WhereAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, bool>> expression, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.WhereAsync(expression, simpleload);

        }

        public virtual async Task<bool> AnyAsync(IConnectionHandler connectionHandler)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.AnyAsync();

        }
        public virtual async Task<bool> AnyAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, bool>> expression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.AnyAsync(expression);

        }

        public virtual async Task<TResult> MaxAsync<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.MaxAsync(expression);

        }
        public virtual async Task<double> AverageAsync<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.AverageAsync(expression);

        }
        public virtual async Task<TResult> MinAsync<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.MinAsync(expression);

        }
        public virtual async Task<TResult> SumAsync<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SumAsync(expression);

        }
        public virtual async Task<TResult> MaxAsync<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.MaxAsync(expression, conditionsexpression);

        }
        public virtual async Task<double> AverageAsync<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.AverageAsync(expression, conditionsexpression);

        }
        public virtual async Task<TResult> MinAsync<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.MinAsync(expression, conditionsexpression);

        }
        public virtual async Task<TResult> SumAsync<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SumAsync(expression, conditionsexpression);

        }

        public virtual async Task<int> CountAsync(IConnectionHandler connectionHandler)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.CountAsync();

        }
        public virtual async Task<int> CountAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, bool>> expression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.CountAsync(expression);

        }
        public virtual async Task<int> CountAsync<TColumn>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TColumn>> expression, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.CountAsync(expression, distinct);

        }
        public virtual async Task<int> CountAsync<TColumn>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TColumn>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.CountAsync(expression, conditionsexpression, distinct);

        }



        public virtual async Task<TDataStructure> FirstOrDefaultAsync(IConnectionHandler connectionHandler, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.FirstOrDefaultAsync(simpleload);

        }
        public virtual async Task<TDataStructure> FirstOrDefaultAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, bool>> expression, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.FirstOrDefaultAsync(expression, simpleload);

        }
        public virtual async Task<TDataStructure> FirstOrDefaultWithOrderByAsync(IConnectionHandler connectionHandler, OrderByModel<TDataStructure>[] orderByexpression, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.FirstOrDefaultWithOrderByAsync(orderByexpression, simpleload);

        }
        public virtual async Task<TDataStructure> FirstOrDefaultWithOrderByAsync(IConnectionHandler connectionHandler, OrderByModel<TDataStructure>[] orderByexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.FirstOrDefaultWithOrderByAsync(orderByexpression, conditionsexpression, simpleload);

        }
        public virtual async Task<TDataStructure> FirstOrDefaultWithOrderByAsync<TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.FirstOrDefaultWithOrderByAsync(orderByexpression, simpleload);

        }
        public virtual async Task<TDataStructure> FirstOrDefaultWithOrderByDescendingAsync<TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.FirstOrDefaultWithOrderByDescendingAsync(orderByexpression, simpleload);

        }
        public virtual async Task<TDataStructure> FirstOrDefaultWithOrderByAsync<TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.FirstOrDefaultWithOrderByAsync(orderByexpression, conditionsexpression, simpleload);

        }
        public virtual async Task<TDataStructure> FirstOrDefaultWithOrderByDescendingAsync<TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.FirstOrDefaultWithOrderByDescendingAsync(orderByexpression, conditionsexpression, simpleload);

        }


        public virtual async Task<TDataStructure> SingleOrDefaultAsync(IConnectionHandler connectionHandler, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SingleOrDefaultAsync();

        }
        public virtual async Task<TDataStructure> SingleOrDefaultAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, bool>> expression, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SingleOrDefaultAsync(expression);

        }




        public virtual async Task<TDataStructure> IncludeFirstOrDefaultAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] Includeexpression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.IncludeFirstOrDefaultAsync(Includeexpression);

        }
        public virtual async Task<TDataStructure> IncludeFirstOrDefaultAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] Includeexpression, OrderByModel<TDataStructure> orderByModel)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.IncludeFirstOrDefaultAsync(Includeexpression, orderByModel);

        }
        public virtual async Task<TDataStructure> IncludeFirstOrDefaultAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] Includeexpression, OrderByModel<TDataStructure>[] orderByModels)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.IncludeFirstOrDefaultAsync(Includeexpression, orderByModels);

        }
        public virtual async Task<TDataStructure> IncludeFirstOrDefaultAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.IncludeFirstOrDefaultAsync(Includeexpression, conditionsexpression);


        }
        public virtual async Task<TDataStructure> IncludeFirstOrDefaultAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.IncludeFirstOrDefaultAsync(Includeexpression, conditionsexpression, orderByModel);

        }
        public virtual async Task<TDataStructure> IncludeFirstOrDefaultAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModels)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.IncludeFirstOrDefaultAsync(Includeexpression, conditionsexpression, orderByModels);

        }



        public virtual async Task<List<TDataStructure>> IncludeTopAsync(IConnectionHandler connectionHandler, int topcount, Expression<Func<TDataStructure, Object>>[] Includeexpression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.IncludeTopAsync(topcount, Includeexpression);

        }
        public virtual async Task<List<TDataStructure>> IncludeTopAsync(IConnectionHandler connectionHandler, int topcount, Expression<Func<TDataStructure, Object>>[] Includeexpression, OrderByModel<TDataStructure> orderByModel)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.IncludeTopAsync(topcount, Includeexpression, orderByModel);

        }
        public virtual async Task<List<TDataStructure>> IncludeTopAsync(IConnectionHandler connectionHandler, int topcount, Expression<Func<TDataStructure, Object>>[] Includeexpression, OrderByModel<TDataStructure>[] orderByModels)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.IncludeTopAsync(topcount, Includeexpression, orderByModels);

        }
        public virtual async Task<List<TDataStructure>> IncludeTopAsync(IConnectionHandler connectionHandler, int topcount, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.IncludeTopAsync(topcount, Includeexpression, conditionsexpression);


        }
        public virtual async Task<List<TDataStructure>> IncludeTopAsync(IConnectionHandler connectionHandler, int topcount, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.IncludeTopAsync(topcount, Includeexpression, conditionsexpression, orderByModel);

        }
        public virtual async Task<List<TDataStructure>> IncludeTopAsync(IConnectionHandler connectionHandler, int topcount, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModels)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.IncludeTopAsync(topcount, Includeexpression, conditionsexpression, orderByModels);

        }



        public virtual async Task<List<TDataStructure>> IncludePagedListAsync(IConnectionHandler connectionHandler, int pageIndex, int pagesize, Expression<Func<TDataStructure, Object>>[] Includeexpression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.IncludePagedListAsync(pageIndex, pagesize, Includeexpression);

        }
        public virtual async Task<List<TDataStructure>> IncludePagedListAsync(IConnectionHandler connectionHandler, int pageIndex, int pagesize, Expression<Func<TDataStructure, Object>>[] Includeexpression, OrderByModel<TDataStructure> orderByModel)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.IncludePagedListAsync(pageIndex, pagesize, Includeexpression, orderByModel);

        }
        public virtual async Task<List<TDataStructure>> IncludePagedListAsync(IConnectionHandler connectionHandler, int pageIndex, int pagesize, Expression<Func<TDataStructure, Object>>[] Includeexpression, OrderByModel<TDataStructure>[] orderByModels)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.IncludePagedListAsync(pageIndex, pagesize, Includeexpression, orderByModels);

        }
        public virtual async Task<List<TDataStructure>> IncludePagedListAsync(IConnectionHandler connectionHandler, int pageIndex, int pagesize, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.IncludePagedListAsync(pageIndex, pagesize, Includeexpression, conditionsexpression);


        }
        public virtual async Task<List<TDataStructure>> IncludePagedListAsync(IConnectionHandler connectionHandler, int pageIndex, int pagesize, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.IncludePagedListAsync(pageIndex, pagesize, Includeexpression, conditionsexpression, orderByModel);

        }
        public virtual async Task<List<TDataStructure>> IncludePagedListAsync(IConnectionHandler connectionHandler, int pageIndex, int pagesize, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModels)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.IncludePagedListAsync(pageIndex, pagesize, Includeexpression, conditionsexpression, orderByModels);

        }




        public virtual async Task<List<TDataStructure>> IncludeAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] Includeexpression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.IncludeAsync(Includeexpression);

        }
        public virtual async Task<List<TDataStructure>> IncludeAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] Includeexpression, OrderByModel<TDataStructure> orderByModel)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.IncludeAsync(Includeexpression, orderByModel);

        }
        public virtual async Task<List<TDataStructure>> IncludeAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] Includeexpression, OrderByModel<TDataStructure>[] orderByModels)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.IncludeAsync(Includeexpression, orderByModels);

        }
        public virtual async Task<List<TDataStructure>> IncludeAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.IncludeAsync(Includeexpression, conditionsexpression);


        }
        public virtual async Task<List<TDataStructure>> IncludeAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.IncludeAsync(Includeexpression, conditionsexpression, orderByModel);

        }
        public virtual async Task<List<TDataStructure>> IncludeAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModels)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.IncludeAsync(Includeexpression, conditionsexpression, orderByModels);

        }


        public virtual async Task<List<TDataStructure>> PagedListAsync(IConnectionHandler connectionHandler, int pageIndex, int pagesize, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.PagedListAsync(pageIndex, pagesize, simpleload);

        }
        public virtual async Task<List<TDataStructure>> PagedListAsync(IConnectionHandler connectionHandler, int pageIndex, int pagesize, OrderByModel<TDataStructure> orderByModel, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.PagedListAsync(pageIndex, pagesize, orderByModel, simpleload);

        }
        public virtual async Task<List<TDataStructure>> PagedListAsync(IConnectionHandler connectionHandler, int pageIndex, int pagesize, OrderByModel<TDataStructure>[] orderByModels, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.PagedListAsync(pageIndex, pagesize, orderByModels, simpleload);

        }
        public virtual async Task<List<TDataStructure>> PagedListAsync(IConnectionHandler connectionHandler, int pageIndex, int pagesize, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.PagedListAsync(pageIndex, pagesize, conditionsexpression, simpleload);


        }
        public virtual async Task<List<TDataStructure>> PagedListAsync(IConnectionHandler connectionHandler, int pageIndex, int pagesize, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.PagedListAsync(pageIndex, pagesize, conditionsexpression, orderByModel, simpleload);

        }
        public virtual async Task<List<TDataStructure>> PagedListAsync(IConnectionHandler connectionHandler, int pageIndex, int pagesize, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModels, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.PagedListAsync(pageIndex, pagesize, conditionsexpression, orderByModels, simpleload);

        }




        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectKeyValuePairAsync(DataValueField, DataTextField, distinct);
        }
        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectKeyValuePairAsync(DataValueField, DataTextField, conditionsexpression, distinct);

        }
        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectKeyValuePairAsync(DataValueField, DataTextField, conditionsexpression, orderByModel, distinct);

        }
        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectKeyValuePairAsync(DataValueField, DataTextField, conditionsexpression, orderbymodels, distinct);
        }
        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, OrderByModel<TDataStructure> orderByModel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectKeyValuePairAsync(DataValueField, DataTextField, orderByModel, distinct);
        }
        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, OrderByModel<TDataStructure>[] orderbymodels, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectKeyValuePairAsync(DataValueField, DataTextField, orderbymodels, distinct);
        }



        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, string culture, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectKeyValuePairAsync(DataValueField, DataTextField, culture, distinct);
        }
        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, string culture, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectKeyValuePairAsync(DataValueField, DataTextField, culture, conditionsexpression, distinct);

        }
        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, string culture, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectKeyValuePairAsync(DataValueField, DataTextField, culture, conditionsexpression, orderByModel, distinct);

        }
        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, string culture, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectKeyValuePairAsync(DataValueField, DataTextField, culture, conditionsexpression, orderbymodels, distinct);
        }
        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, string culture, OrderByModel<TDataStructure> orderByModel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectKeyValuePairAsync(DataValueField, DataTextField, culture, orderByModel, distinct);
        }
        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, string culture, OrderByModel<TDataStructure>[] orderbymodels, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectKeyValuePairAsync(DataValueField, DataTextField, culture, orderbymodels, distinct);
        }



        public virtual async Task<List<TDataStructure>> SelectTopAsync(IConnectionHandler connectionHandler, int topcount, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectTopAsync(topcount, simpleload);
        }
        public virtual async Task<List<TDataStructure>> SelectTopAsync(IConnectionHandler connectionHandler, int topcount, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectTopAsync(topcount, conditionsexpression, simpleload);

        }
        public virtual async Task<List<TDataStructure>> SelectTopAsync(IConnectionHandler connectionHandler, int topcount, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectTopAsync(topcount, conditionsexpression, orderByModel, simpleload);

        }
        public virtual async Task<List<TDataStructure>> SelectTopAsync(IConnectionHandler connectionHandler, int topcount, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectTopAsync(topcount, conditionsexpression, orderbymodels, simpleload);
        }
        public virtual async Task<List<TDataStructure>> SelectTopAsync(IConnectionHandler connectionHandler, int topcount, OrderByModel<TDataStructure> orderByModel, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectTopAsync(topcount, orderByModel, simpleload);
        }
        public virtual async Task<List<TDataStructure>> SelectTopAsync(IConnectionHandler connectionHandler, int topcount, OrderByModel<TDataStructure>[] orderbymodels, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectTopAsync(topcount, orderbymodels, simpleload);
        }




        public virtual async Task<TResult> SelectFirstOrDefaultAsync<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectFirstOrDefaultAsync(expression);
        }
        public virtual async Task<TResult> SelectFirstOrDefaultAsync<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectFirstOrDefaultAsync(expression, conditionsexpression);
        }
        public virtual async Task<TResult> SelectFirstOrDefaultAsync<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, OrderByModel<TDataStructure> orderByModel)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectFirstOrDefaultAsync(expression, orderByModel);
        }
        public virtual async Task<TResult> SelectFirstOrDefaultAsync<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectFirstOrDefaultAsync(expression, conditionsexpression, orderByModel);
        }
        public virtual async Task<TResult> SelectFirstOrDefaultAsync<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, OrderByModel<TDataStructure>[] orderByModels)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectFirstOrDefaultAsync(expression, orderByModels);
        }
        public virtual async Task<TResult> SelectFirstOrDefaultAsync<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModels)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectFirstOrDefaultAsync(expression, conditionsexpression, orderByModels);
        }


        public virtual async Task<List<TResult>> SelectAsync<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectAsync(expression, distinct);
        }
        public virtual async Task<List<TResult>> SelectAsync<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectAsync(expression, conditionsexpression, distinct);
        }
        public virtual async Task<List<TResult>> SelectAsync<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, OrderByModel<TDataStructure> orderByModel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectAsync(expression, orderByModel, distinct);
        }
        public virtual async Task<List<TResult>> SelectAsync<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectAsync(expression, conditionsexpression, orderByModel, distinct);
        }
        public virtual async Task<List<TResult>> SelectAsync<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, OrderByModel<TDataStructure>[] orderByModels, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectAsync(expression, orderByModels, distinct);
        }
        public virtual async Task<List<TResult>> SelectAsync<TResult>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModels, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectAsync(expression, conditionsexpression, orderByModels, distinct);
        }





        public virtual async Task<dynamic> SelectFirstOrDefaultAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectFirstOrDefaultAsync(expression);
        }
        public virtual async Task<dynamic> SelectFirstOrDefaultAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectFirstOrDefaultAsync(expression, conditionsexpression);

        }

        public virtual async Task<dynamic> SelectFirstOrDefaultAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, OrderByModel<TDataStructure> orderbymodel)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectFirstOrDefaultAsync(expression, orderbymodel);
        }
        public virtual async Task<dynamic> SelectFirstOrDefaultAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderbymodel)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectFirstOrDefaultAsync(expression, conditionsexpression, orderbymodel);

        }


        public virtual async Task<dynamic> SelectFirstOrDefaultAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, OrderByModel<TDataStructure>[] orderbymodels)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectFirstOrDefaultAsync(expression, orderbymodels);
        }
        public virtual async Task<dynamic> SelectFirstOrDefaultAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectFirstOrDefaultAsync(expression, conditionsexpression, orderbymodels);

        }




        public virtual async Task<List<dynamic>> SelectAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectAsync(expression, distinct);
        }
        public virtual async Task<List<dynamic>> SelectAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectAsync(expression, conditionsexpression, distinct);

        }

        public virtual async Task<List<dynamic>> SelectAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectAsync(expression, orderbymodel, distinct);
        }
        public virtual async Task<List<dynamic>> SelectAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectAsync(expression, conditionsexpression, orderbymodel, distinct);

        }


        public virtual async Task<List<dynamic>> SelectAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, OrderByModel<TDataStructure>[] orderbymodels, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectAsync(expression, orderbymodels, distinct);
        }
        public virtual async Task<List<dynamic>> SelectAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.SelectAsync(expression, conditionsexpression, orderbymodels, distinct);

        }







        public virtual async Task<List<TDataStructure>> OrderByAsync<TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.OrderByAsync(orderByexpression, simpleload);

        }
        public virtual async Task<List<TDataStructure>> OrderByAsync(IConnectionHandler connectionHandler, OrderByModel<TDataStructure>[] orderByexpression, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.OrderByAsync(orderByexpression, simpleload);
        }
        public virtual async Task<List<TDataStructure>> OrderByAsync(IConnectionHandler connectionHandler, OrderByModel<TDataStructure>[] orderByexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.OrderByAsync(orderByexpression, conditionsexpression, simpleload);
        }
        public virtual async Task<List<TDataStructure>> OrderByDescendingAsync<TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.OrderByDescendingAsync(orderByexpression, simpleload);

        }
        public virtual async Task<List<TDataStructure>> OrderByAsync<TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.OrderByAsync(orderByexpression, conditionsexpression, simpleload);

        }
        public virtual async Task<List<TDataStructure>> OrderByDescendingAsync<TOrderProperty>(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.OrderByDescendingAsync(orderByexpression, conditionsexpression, simpleload);

        }



        public virtual async Task<List<dynamic>> GroupByAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, bool distinct = false)
        {

            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.GroupByAsync(groupexpression, distinct);
        }
        public virtual async Task<List<dynamic>> GroupByAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.GroupByAsync(groupexpression, conditionsexpression, distinct);
        }
        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.GroupByWithHavingAsync(groupexpression, havingconditionsexpression, distinct);
        }
        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.GroupByWithHavingAsync(groupexpression, conditionsexpression, havingconditionsexpression, distinct);
        }


        public virtual async Task<List<dynamic>> GroupByAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {

            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.GroupByAsync(groupexpression, orderbymodel, distinct);
        }
        public virtual async Task<List<dynamic>> GroupByAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {

            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.GroupByAsync(groupexpression, orderbymodel, distinct);
        }
        public virtual async Task<List<dynamic>> GroupByAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.GroupByAsync(groupexpression, conditionsexpression, orderbymodel, distinct);
        }
        public virtual async Task<List<dynamic>> GroupByAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.GroupByAsync(groupexpression, conditionsexpression, orderbymodel, distinct);
        }

        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.GroupByWithHavingAsync(groupexpression, havingconditionsexpression, orderbymodel, distinct);
        }
        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.GroupByWithHavingAsync(groupexpression, havingconditionsexpression, orderbymodel, distinct);
        }
        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.GroupByWithHavingAsync(groupexpression, conditionsexpression, havingconditionsexpression, orderbymodel, distinct);
        }
        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.GroupByWithHavingAsync(groupexpression, conditionsexpression, havingconditionsexpression, orderbymodel, distinct);
        }


        public virtual async Task<List<dynamic>> GroupByAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, bool distinct = false)
        {

            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.GroupByAsync(groupexpression, aggrigateexpression, distinct);
        }
        public virtual async Task<List<dynamic>> GroupByAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.GroupByAsync(groupexpression, aggrigateexpression, conditionsexpression, distinct);
        }
        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.GroupByWithHavingAsync(groupexpression, aggrigateexpression, havingconditionsexpression, distinct);
        }
        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.GroupByWithHavingAsync(groupexpression, aggrigateexpression, conditionsexpression, havingconditionsexpression, distinct);
        }


        public virtual async Task<List<dynamic>> GroupByAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {

            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.GroupByAsync(groupexpression, aggrigateexpression, orderbymodel, distinct);
        }
        public virtual async Task<List<dynamic>> GroupByAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.GroupByAsync(groupexpression, aggrigateexpression, conditionsexpression, orderbymodel, distinct);
        }
        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.GroupByWithHavingAsync(groupexpression, aggrigateexpression, havingconditionsexpression, orderbymodel, distinct);
        }
        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.GroupByWithHavingAsync(groupexpression, aggrigateexpression, conditionsexpression, havingconditionsexpression, orderbymodel, distinct);
        }

        public virtual async Task<List<dynamic>> GroupByAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {

            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.GroupByAsync(groupexpression, aggrigateexpression, orderbymodel, distinct);
        }
        public virtual async Task<List<dynamic>> GroupByAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.GroupByAsync(groupexpression, aggrigateexpression, conditionsexpression, orderbymodel, distinct);
        }
        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.GroupByWithHavingAsync(groupexpression, aggrigateexpression, havingconditionsexpression, orderbymodel, distinct);
        }
        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(IConnectionHandler connectionHandler, Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, Expression<Func<TDataStructure, bool>> havingconditionsexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            var dal = new DALBase<TDataStructure>(connectionHandler);
            return await dal.GroupByWithHavingAsync(groupexpression, aggrigateexpression, conditionsexpression, havingconditionsexpression, orderbymodel, distinct);
        }


        #endregion
    }
}