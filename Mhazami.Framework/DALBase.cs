using Mhazami.Framework.DbHelper;
using Mhazami.Framework.Difinition;
using System.Data;
using System.Linq.Expressions;


namespace Mhazami.Framework
{
    public class DALBase<TDataStructure> where TDataStructure : class
    {
        public DALBase(IConnectionHandler connectionHandler)
        {
            this.ConnectionHandler = connectionHandler;
        }

        public IConnectionHandler ConnectionHandler { get; set; }

        #region Manual

        public virtual int Insert(TDataStructure obj)
        {
            return DBManager.Insert(this.ConnectionHandler, obj);
        }
        public virtual int Update(TDataStructure obj)
        {
            return DBManager.Update(this.ConnectionHandler, obj);
        }
        public virtual int Delete(params object[] keys)
        {
            return DBManager.Delete<TDataStructure>(this.ConnectionHandler, keys);
        }
        public virtual int Delete(TDataStructure obj)
        {
            return DBManager.Delete(this.ConnectionHandler, obj);
        }
        public virtual int Insert(TDataStructure obj, Delegates.LogMethod method, params object[] parameters)
        {
            return DBManager.Insert(this.ConnectionHandler, obj, method, parameters);
        }
        public virtual int Update(TDataStructure obj, Delegates.LogMethod method, params object[] parameters)
        {
            return DBManager.Update(this.ConnectionHandler, obj, method, parameters);
        }
        public virtual int Delete(TDataStructure obj, Delegates.LogMethod method, params object[] parameters)
        {
            return DBManager.Delete(this.ConnectionHandler, obj, method, parameters);
        }


        public virtual TDataStructure Get(params object[] keys)
        {
            return DBManager.GetObject<TDataStructure>(this.ConnectionHandler, keys);
        }
        public virtual TDataStructure SimpleGet(params object[] keys)
        {
            return DBManager.SimpleGetObject<TDataStructure>(this.ConnectionHandler, keys);
        }
        public virtual void GetLanuageContent(string culture, TDataStructure obj)
        {
            DBManager.GetLanuageContent(this.ConnectionHandler, culture, obj);
        }
        public virtual void GetLanuageContent(string culture, List<TDataStructure> obj)
        {
            DBManager.GetLanuageContent(this.ConnectionHandler, culture, obj);
        }
        public virtual TDataStructure GetLanuageContent(string culture, params object[] keys)
        {
            return DBManager.GetLanuageContent<TDataStructure>(this.ConnectionHandler, culture, keys);
        }
        public virtual TDataStructure GetLanuageContentsimple(string culture, params object[] keys)
        {
            return DBManager.GetLanuageContentsimple<TDataStructure>(this.ConnectionHandler, culture, keys);
        }

        public virtual List<TDataStructure> GetAll(bool simpleload = false)
        {
            return DBManager.GetCollection<TDataStructure>(this.ConnectionHandler, simpleload);
        }


        public virtual List<TDataStructure> Where(Expression<Func<TDataStructure, bool>> expression, bool simpleload = false)
        {
            return DBManager.Where(this.ConnectionHandler, expression, simpleload);
        }
        public virtual bool Any()
        {
            return DBManager.Any<TDataStructure>(this.ConnectionHandler);
        }
        public virtual bool Any(Expression<Func<TDataStructure, bool>> expression)
        {
            return DBManager.Any(this.ConnectionHandler, expression);
        }

        public virtual TResult Max<TResult>(Expression<Func<TDataStructure, TResult>> expression)
        {
            return DBManager.Max(this.ConnectionHandler, expression);
        }
        public virtual double Average<TResult>(Expression<Func<TDataStructure, TResult>> expression)
        {
            return DBManager.Average(this.ConnectionHandler, expression);
        }
        public virtual TResult Min<TResult>(Expression<Func<TDataStructure, TResult>> expression)
        {
            return DBManager.Min(this.ConnectionHandler, expression);
        }
        public virtual TResult Sum<TResult>(Expression<Func<TDataStructure, TResult>> expression)
        {
            return DBManager.Sum(this.ConnectionHandler, expression);
        }
        public virtual TResult Max<TResult>(Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            return DBManager.Max(this.ConnectionHandler, expression, conditionsexpression);
        }
        public virtual double Average<TResult>(Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            return DBManager.Average(this.ConnectionHandler, expression, conditionsexpression);
        }
        public virtual TResult Min<TResult>(Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            return DBManager.Min(this.ConnectionHandler, expression, conditionsexpression);
        }
        public virtual TResult Sum<TResult>(Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            return DBManager.Sum(this.ConnectionHandler, expression, conditionsexpression);
        }

        public virtual int Count()
        {
            return DBManager.Count<TDataStructure>(this.ConnectionHandler);
        }
        public virtual int Count(Expression<Func<TDataStructure, bool>> expression)
        {
            return DBManager.Count(this.ConnectionHandler, expression);
        }
        public virtual int Count<TColumn>(Expression<Func<TDataStructure, TColumn>> expression, bool distinct = false)
        {
            return DBManager.Count(this.ConnectionHandler, expression, distinct);
        }
        public virtual int Count<TColumn>(Expression<Func<TDataStructure, TColumn>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {
            return DBManager.Count(this.ConnectionHandler, expression, conditionsexpression, distinct);
        }



        public virtual TDataStructure FirstOrDefault(bool simpleload = false)
        {
            return DBManager.FirstOrDefault<TDataStructure>(this.ConnectionHandler, simpleload);
        }
        public virtual TDataStructure FirstOrDefault(Expression<Func<TDataStructure, bool>> expression, bool simpleload = false)
        {
            return DBManager.FirstOrDefault(this.ConnectionHandler, expression, simpleload);
        }
        public virtual TDataStructure FirstOrDefaultWithOrderBy<TOrderProperty>(Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, bool simpleload = false)
        {
            return DBManager.FirstOrDefaultWithOrderBy(this.ConnectionHandler, orderByexpression, simpleload);
        }
        public virtual TDataStructure FirstOrDefaultWithOrderBy(OrderByModel<TDataStructure>[] orderByexpression, bool simpleload = false)
        {
            return DBManager.FirstOrDefaultWithOrderBy(this.ConnectionHandler, orderByexpression, simpleload);
        }
        public virtual TDataStructure FirstOrDefaultWithOrderBy(OrderByModel<TDataStructure>[] orderByexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            return DBManager.FirstOrDefaultWithOrderBy(this.ConnectionHandler, orderByexpression, conditionsexpression, simpleload);
        }
        public virtual TDataStructure FirstOrDefaultWithOrderByDescending<TOrderProperty>(Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, bool simpleload = false)
        {
            return DBManager.FirstOrDefaultWithOrderByDescending(this.ConnectionHandler, orderByexpression, simpleload);
        }
        public virtual TDataStructure FirstOrDefaultWithOrderBy<TOrderProperty>(Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            return DBManager.FirstOrDefaultWithOrderBy(this.ConnectionHandler, orderByexpression, conditionsexpression, simpleload);
        }
        public virtual TDataStructure FirstOrDefaultWithOrderByDescending<TOrderProperty>(Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            return DBManager.FirstOrDefaultWithOrderByDescending(this.ConnectionHandler, orderByexpression, conditionsexpression, simpleload);
        }



        public virtual TDataStructure SingleOrDefault(bool simpleload = false)
        {
            return DBManager.SingleOrDefault<TDataStructure>(this.ConnectionHandler, simpleload);
        }
        public virtual TDataStructure SingleOrDefault(Expression<Func<TDataStructure, bool>> expression, bool simpleload = false)
        {
            return DBManager.SingleOrDefault(this.ConnectionHandler, expression, simpleload);
        }


        public virtual TDataStructure IncludeFirstOrDefault(Expression<Func<TDataStructure, Object>>[] Includeexpression)
        {

            return DBManager.IncludeFirstOrDefault(this.ConnectionHandler, Includeexpression);

        }
        public virtual TDataStructure IncludeFirstOrDefault(Expression<Func<TDataStructure, Object>>[] Includeexpression, OrderByModel<TDataStructure> orderByModel)
        {

            return DBManager.IncludeFirstOrDefault(this.ConnectionHandler, Includeexpression, new[] { orderByModel });

        }
        public virtual TDataStructure IncludeFirstOrDefault(Expression<Func<TDataStructure, Object>>[] Includeexpression, OrderByModel<TDataStructure>[] orderByModels)
        {

            return DBManager.IncludeFirstOrDefault(this.ConnectionHandler, Includeexpression, orderByModels);

        }
        public virtual TDataStructure IncludeFirstOrDefault(Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {

            return DBManager.IncludeFirstOrDefault(this.ConnectionHandler, Includeexpression, conditionsexpression);

        }
        public virtual TDataStructure IncludeFirstOrDefault(Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel)
        {

            return DBManager.IncludeFirstOrDefault(this.ConnectionHandler, Includeexpression, conditionsexpression, new[] { orderByModel });

        }
        public virtual TDataStructure IncludeFirstOrDefault(Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModels)
        {

            return DBManager.IncludeFirstOrDefault(this.ConnectionHandler, Includeexpression, conditionsexpression, orderByModels);

        }




        public virtual List<TDataStructure> IncludeTop(int topcount, Expression<Func<TDataStructure, Object>>[] Includeexpression)
        {

            return DBManager.IncludeTop(this.ConnectionHandler, topcount, Includeexpression);

        }
        public virtual List<TDataStructure> IncludeTop(int topcount, Expression<Func<TDataStructure, Object>>[] Includeexpression, OrderByModel<TDataStructure> orderByModel)
        {

            return DBManager.IncludeTop(this.ConnectionHandler, topcount, Includeexpression, new[] { orderByModel });

        }
        public virtual List<TDataStructure> IncludeTop(int topcount, Expression<Func<TDataStructure, Object>>[] Includeexpression, OrderByModel<TDataStructure>[] orderByModels)
        {

            return DBManager.IncludeTop(this.ConnectionHandler, topcount, Includeexpression, orderByModels);

        }
        public virtual List<TDataStructure> IncludeTop(int topcount, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {

            return DBManager.IncludeTop(this.ConnectionHandler, topcount, Includeexpression, conditionsexpression);

        }
        public virtual List<TDataStructure> IncludeTop(int topcount, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel)
        {

            return DBManager.IncludeTop(this.ConnectionHandler, topcount, Includeexpression, conditionsexpression, new[] { orderByModel });

        }
        public virtual List<TDataStructure> IncludeTop(int topcount, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModels)
        {

            return DBManager.IncludeTop(this.ConnectionHandler, topcount, Includeexpression, conditionsexpression, orderByModels);

        }




        public virtual List<TDataStructure> IncludePagedList(int pageIndex, int pagesize, Expression<Func<TDataStructure, Object>>[] Includeexpression)
        {

            return DBManager.IncludePagedList(this.ConnectionHandler, pageIndex, pagesize, Includeexpression);

        }
        public virtual List<TDataStructure> IncludePagedList(int pageIndex, int pagesize, Expression<Func<TDataStructure, Object>>[] Includeexpression, OrderByModel<TDataStructure> orderByModel)
        {

            return DBManager.IncludePagedList(this.ConnectionHandler, pageIndex, pagesize, Includeexpression, new[] { orderByModel });

        }
        public virtual List<TDataStructure> IncludePagedList(int pageIndex, int pagesize, Expression<Func<TDataStructure, Object>>[] Includeexpression, OrderByModel<TDataStructure>[] orderByModels)
        {

            return DBManager.IncludePagedList(this.ConnectionHandler, pageIndex, pagesize, Includeexpression, orderByModels);

        }
        public virtual List<TDataStructure> IncludePagedList(int pageIndex, int pagesize, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {

            return DBManager.IncludePagedList(this.ConnectionHandler, pageIndex, pagesize, Includeexpression, conditionsexpression);

        }
        public virtual List<TDataStructure> IncludePagedList(int pageIndex, int pagesize, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel)
        {

            return DBManager.IncludePagedList(this.ConnectionHandler, pageIndex, pagesize, Includeexpression, conditionsexpression, new[] { orderByModel });

        }
        public virtual List<TDataStructure> IncludePagedList(int pageIndex, int pagesize, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModels)
        {

            return DBManager.IncludePagedList(this.ConnectionHandler, pageIndex, pagesize, Includeexpression, conditionsexpression, orderByModels);

        }



        public virtual List<TDataStructure> Include(Expression<Func<TDataStructure, Object>>[] Includeexpression)
        {

            return DBManager.Include(this.ConnectionHandler, Includeexpression);

        }
        public virtual List<TDataStructure> Include(Expression<Func<TDataStructure, Object>>[] Includeexpression, OrderByModel<TDataStructure> orderByModel)
        {

            return DBManager.Include(this.ConnectionHandler, Includeexpression, new[] { orderByModel });

        }
        public virtual List<TDataStructure> Include(Expression<Func<TDataStructure, Object>>[] Includeexpression, OrderByModel<TDataStructure>[] orderByModels)
        {

            return DBManager.Include(this.ConnectionHandler, Includeexpression, orderByModels);

        }
        public virtual List<TDataStructure> Include(Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {

            return DBManager.Include(this.ConnectionHandler, Includeexpression, conditionsexpression);

        }
        public virtual List<TDataStructure> Include(Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel)
        {

            return DBManager.Include(this.ConnectionHandler, Includeexpression, conditionsexpression, new[] { orderByModel });

        }
        public virtual List<TDataStructure> Include(Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModels)
        {

            return DBManager.Include(this.ConnectionHandler, Includeexpression, conditionsexpression, orderByModels);

        }



        public virtual List<TDataStructure> PagedList(int pageIndex, int pagesize, bool simpleload = false)
        {

            return DBManager.PagedList<TDataStructure>(this.ConnectionHandler, pageIndex, pagesize, simpleload);

        }
        public virtual List<TDataStructure> PagedList(int pageIndex, int pagesize, OrderByModel<TDataStructure> orderByModel, bool simpleload = false)
        {

            return DBManager.PagedList(this.ConnectionHandler, pageIndex, pagesize, new[] { orderByModel }, simpleload);

        }
        public virtual List<TDataStructure> PagedList(int pageIndex, int pagesize, OrderByModel<TDataStructure>[] orderByModels, bool simpleload = false)
        {

            return DBManager.PagedList(this.ConnectionHandler, pageIndex, pagesize, orderByModels, simpleload);

        }
        public virtual List<TDataStructure> PagedList(int pageIndex, int pagesize, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {

            return DBManager.PagedList(this.ConnectionHandler, pageIndex, pagesize, conditionsexpression, simpleload);

        }
        public virtual List<TDataStructure> PagedList(int pageIndex, int pagesize, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel, bool simpleload = false)
        {

            return DBManager.PagedList(this.ConnectionHandler, pageIndex, pagesize, conditionsexpression, new[] { orderByModel }, simpleload);

        }
        public virtual List<TDataStructure> PagedList(int pageIndex, int pagesize, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModels, bool simpleload = false)
        {

            return DBManager.PagedList(this.ConnectionHandler, pageIndex, pagesize, conditionsexpression, orderByModels, simpleload);

        }







        public virtual dynamic SelectFirstOrDefault(Expression<Func<TDataStructure, Object>>[] expression)
        {
            return DBManager.SelectFirstOrDefault(this.ConnectionHandler, expression);
        }
        public virtual dynamic SelectFirstOrDefault(Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            return DBManager.SelectFirstOrDefault(this.ConnectionHandler, expression, conditionsexpression);
        }

        public virtual dynamic SelectFirstOrDefault(Expression<Func<TDataStructure, Object>>[] expression, OrderByModel<TDataStructure> orderbymodel)
        {
            return DBManager.SelectFirstOrDefault(this.ConnectionHandler, expression, new[] { orderbymodel });
        }
        public virtual dynamic SelectFirstOrDefault(Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderbymodel)
        {
            return DBManager.SelectFirstOrDefault(this.ConnectionHandler, expression, conditionsexpression, new[] { orderbymodel });
        }

        public virtual dynamic SelectFirstOrDefault(Expression<Func<TDataStructure, Object>>[] expression, OrderByModel<TDataStructure>[] orderbymodels)
        {
            return DBManager.SelectFirstOrDefault(this.ConnectionHandler, expression, orderbymodels);
        }
        public virtual dynamic SelectFirstOrDefault(Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels)
        {
            return DBManager.SelectFirstOrDefault(this.ConnectionHandler, expression, conditionsexpression, orderbymodels);
        }


        public virtual List<dynamic> Select(Expression<Func<TDataStructure, Object>>[] expression, bool distinct = false)
        {
            return DBManager.Select(this.ConnectionHandler, expression, distinct);
        }
        public virtual List<dynamic> Select(Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {
            return DBManager.Select(this.ConnectionHandler, expression, conditionsexpression, distinct);
        }

        public virtual List<dynamic> Select(Expression<Func<TDataStructure, Object>>[] expression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            return DBManager.Select(this.ConnectionHandler, expression, new[] { orderbymodel }, distinct);
        }
        public virtual List<dynamic> Select(Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            return DBManager.Select(this.ConnectionHandler, expression, conditionsexpression, new[] { orderbymodel }, distinct);
        }

        public virtual List<dynamic> Select(Expression<Func<TDataStructure, Object>>[] expression, OrderByModel<TDataStructure>[] orderbymodels, bool distinct = false)
        {
            return DBManager.Select(this.ConnectionHandler, expression, orderbymodels, distinct);
        }
        public virtual List<dynamic> Select(Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels, bool distinct = false)
        {
            return DBManager.Select(this.ConnectionHandler, expression, conditionsexpression, orderbymodels, distinct);
        }





        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, bool distinct = false)
        {
            return DBManager.SelectKeyValuePair<TDataStructure>(this.ConnectionHandler, DataValueField, DataTextField, distinct);
        }
        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {
            return DBManager.SelectKeyValuePair(this.ConnectionHandler, DataValueField, DataTextField, conditionsexpression, distinct);
        }
        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel, bool distinct = false)
        {
            return DBManager.SelectKeyValuePair(this.ConnectionHandler, DataValueField, DataTextField, conditionsexpression, new[] { orderByModel }, distinct);
        }
        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels, bool distinct = false)
        {
            return DBManager.SelectKeyValuePair(this.ConnectionHandler, DataValueField, DataTextField, conditionsexpression, orderbymodels, distinct);
        }
        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, OrderByModel<TDataStructure> orderByModel, bool distinct = false)
        {
            return DBManager.SelectKeyValuePair(this.ConnectionHandler, DataValueField, DataTextField, new[] { orderByModel }, distinct);
        }
        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, OrderByModel<TDataStructure>[] orderbymodels, bool distinct = false)
        {
            return DBManager.SelectKeyValuePair(this.ConnectionHandler, DataValueField, DataTextField, orderbymodels, distinct);
        }




        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, string culture, bool distinct = false)
        {
            return DBManager.SelectKeyValuePair<TDataStructure>(this.ConnectionHandler, DataValueField, DataTextField, culture, distinct);
        }
        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, string culture, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {
            return DBManager.SelectKeyValuePair(this.ConnectionHandler, DataValueField, DataTextField, culture, conditionsexpression, distinct);
        }
        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, string culture, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel, bool distinct = false)
        {
            return DBManager.SelectKeyValuePair(this.ConnectionHandler, DataValueField, DataTextField, culture, conditionsexpression, new[] { orderByModel }, distinct);
        }
        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, string culture, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels, bool distinct = false)
        {
            return DBManager.SelectKeyValuePair(this.ConnectionHandler, DataValueField, DataTextField, culture, conditionsexpression, orderbymodels, distinct);
        }
        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, string culture, OrderByModel<TDataStructure> orderByModel, bool distinct = false)
        {
            return DBManager.SelectKeyValuePair(this.ConnectionHandler, DataValueField, DataTextField, culture, new[] { orderByModel }, distinct);
        }
        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, string culture, OrderByModel<TDataStructure>[] orderbymodels, bool distinct = false)
        {
            return DBManager.SelectKeyValuePair(this.ConnectionHandler, DataValueField, DataTextField, culture, orderbymodels, distinct);
        }




        public virtual List<TDataStructure> SelectTop(int topcount, bool simpleload = false)
        {
            return DBManager.SelectTop<TDataStructure>(this.ConnectionHandler, topcount, simpleload);
        }
        public virtual List<TDataStructure> SelectTop(int topcount, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            return DBManager.SelectTop(this.ConnectionHandler, topcount, conditionsexpression, simpleload);
        }
        public virtual List<TDataStructure> SelectTop(int topcount, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel, bool simpleload = false)
        {
            return DBManager.SelectTop(this.ConnectionHandler, topcount, conditionsexpression, new[] { orderByModel }, simpleload);
        }
        public virtual List<TDataStructure> SelectTop(int topcount, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels, bool simpleload = false)
        {
            return DBManager.SelectTop(this.ConnectionHandler, topcount, conditionsexpression, orderbymodels, simpleload);
        }
        public virtual List<TDataStructure> SelectTop(int topcount, OrderByModel<TDataStructure> orderByModel, bool simpleload = false)
        {
            return DBManager.SelectTop(this.ConnectionHandler, topcount, new[] { orderByModel }, simpleload);
        }
        public virtual List<TDataStructure> SelectTop(int topcount, OrderByModel<TDataStructure>[] orderbymodels, bool simpleload = false)
        {
            return DBManager.SelectTop(this.ConnectionHandler, topcount, orderbymodels, simpleload);
        }



        public virtual TResult SelectFirstOrDefault<TResult>(Expression<Func<TDataStructure, TResult>> expression)
        {
            return DBManager.SelectFirstOrDefault(this.ConnectionHandler, expression);
        }
        public virtual TResult SelectFirstOrDefault<TResult>(Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            return DBManager.SelectFirstOrDefault(this.ConnectionHandler, expression, conditionsexpression);
        }

        public virtual TResult SelectFirstOrDefault<TResult>(Expression<Func<TDataStructure, TResult>> expression, OrderByModel<TDataStructure> orderByModel)
        {
            return DBManager.SelectFirstOrDefault(this.ConnectionHandler, expression, new[] { orderByModel });
        }
        public virtual TResult SelectFirstOrDefault<TResult>(Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel)
        {
            return DBManager.SelectFirstOrDefault(this.ConnectionHandler, expression, conditionsexpression, new[] { orderByModel });
        }

        public virtual TResult SelectFirstOrDefault<TResult>(Expression<Func<TDataStructure, TResult>> expression, OrderByModel<TDataStructure>[] orderByModels)
        {
            return DBManager.SelectFirstOrDefault(this.ConnectionHandler, expression, orderByModels);
        }
        public virtual TResult SelectFirstOrDefault<TResult>(Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModels)
        {
            return DBManager.SelectFirstOrDefault(this.ConnectionHandler, expression, conditionsexpression, orderByModels);
        }



        public virtual List<TResult> Select<TResult>(Expression<Func<TDataStructure, TResult>> expression, bool distinct = false)
        {
            return DBManager.Select(this.ConnectionHandler, expression, distinct);
        }
        public virtual List<TResult> Select<TResult>(Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {
            return DBManager.Select(this.ConnectionHandler, expression, conditionsexpression, distinct);
        }

        public virtual List<TResult> Select<TResult>(Expression<Func<TDataStructure, TResult>> expression, OrderByModel<TDataStructure> orderByModel, bool distinct = false)
        {
            return DBManager.Select(this.ConnectionHandler, expression, new[] { orderByModel }, distinct);
        }
        public virtual List<TResult> Select<TResult>(Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel, bool distinct = false)
        {
            return DBManager.Select(this.ConnectionHandler, expression, conditionsexpression, new[] { orderByModel }, distinct);
        }

        public virtual List<TResult> Select<TResult>(Expression<Func<TDataStructure, TResult>> expression, OrderByModel<TDataStructure>[] orderByModels, bool distinct = false)
        {
            return DBManager.Select(this.ConnectionHandler, expression, orderByModels, distinct);
        }
        public virtual List<TResult> Select<TResult>(Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModels, bool distinct = false)
        {
            return DBManager.Select(this.ConnectionHandler, expression, conditionsexpression, orderByModels, distinct);
        }




        public List<TDataStructure> OrderBy<TOrderProperty>(Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, bool simpleload = false)
        {
            return DBManager.OrderBy(this.ConnectionHandler, orderByexpression, simpleload);
        }
        public List<TDataStructure> OrderBy(OrderByModel<TDataStructure>[] orderByexpression, bool simpleload = false)
        {
            return DBManager.OrderBy(this.ConnectionHandler, orderByexpression, simpleload);
        }
        public List<TDataStructure> OrderBy(OrderByModel<TDataStructure>[] orderByexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            return DBManager.OrderBy(this.ConnectionHandler, orderByexpression, conditionsexpression, simpleload);
        }
        public List<TDataStructure> OrderByDescending<TOrderProperty>(Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, bool simpleload = false)
        {
            return DBManager.OrderByDescending(this.ConnectionHandler, orderByexpression, simpleload);
        }
        public List<TDataStructure> OrderBy<TOrderProperty>(Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            return DBManager.OrderBy(this.ConnectionHandler, orderByexpression, conditionsexpression, simpleload);
        }
        public List<TDataStructure> OrderByDescending<TOrderProperty>(Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            return DBManager.OrderByDescending(this.ConnectionHandler, orderByexpression, conditionsexpression, simpleload);
        }







        public virtual List<dynamic> GroupBy(Expression<Func<TDataStructure, Object>>[] groupexpression, bool distinct = false)
        {
            return DBManager.GroupBy(this.ConnectionHandler, groupexpression, distinct);
        }
        public virtual List<dynamic> GroupBy(Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {
            return DBManager.GroupBy(this.ConnectionHandler, groupexpression, conditionsexpression, distinct);
        }
        public virtual List<dynamic> GroupByWithHaving(Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> Havingconditionsexpression, bool distinct = false)
        {
            return DBManager.GroupByWithHaving(this.ConnectionHandler, groupexpression, Havingconditionsexpression, distinct);
        }
        public virtual List<dynamic> GroupByWithHaving(Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, Expression<Func<TDataStructure, bool>> Havingconditionsexpression, bool distinct = false)
        {
            return DBManager.GroupByWithHaving(this.ConnectionHandler, groupexpression, conditionsexpression, Havingconditionsexpression, distinct);
        }

        public virtual List<dynamic> GroupBy(Expression<Func<TDataStructure, Object>>[] groupexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            return DBManager.GroupBy(this.ConnectionHandler, groupexpression, new[] { orderbymodel }, distinct);
        }
        public virtual List<dynamic> GroupBy(Expression<Func<TDataStructure, Object>>[] groupexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            return DBManager.GroupBy(this.ConnectionHandler, groupexpression, orderbymodel, distinct);
        }

        public virtual List<dynamic> GroupBy(Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            return DBManager.GroupBy(this.ConnectionHandler, groupexpression, conditionsexpression, orderbymodel, distinct);
        }
        public virtual List<dynamic> GroupBy(Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            return DBManager.GroupBy(this.ConnectionHandler, groupexpression, conditionsexpression, new[] { orderbymodel }, distinct);
        }
        public virtual List<dynamic> GroupByWithHaving(Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> Havingconditionsexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            return DBManager.GroupByWithHaving(this.ConnectionHandler, groupexpression, Havingconditionsexpression, new[] { orderbymodel }, distinct);
        }

        public virtual List<dynamic> GroupByWithHaving(Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> Havingconditionsexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            return DBManager.GroupByWithHaving(this.ConnectionHandler, groupexpression, Havingconditionsexpression, orderbymodel, distinct);
        }
        public virtual List<dynamic> GroupByWithHaving(Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, Expression<Func<TDataStructure, bool>> Havingconditionsexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            return DBManager.GroupByWithHaving(this.ConnectionHandler, groupexpression, conditionsexpression, Havingconditionsexpression, new[] { orderbymodel }, distinct);
        }

        public virtual List<dynamic> GroupByWithHaving(Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, Expression<Func<TDataStructure, bool>> Havingconditionsexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            return DBManager.GroupByWithHaving(this.ConnectionHandler, groupexpression, conditionsexpression, Havingconditionsexpression, orderbymodel, distinct);
        }

        public virtual List<dynamic> GroupBy(Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, bool distinct = false)
        {
            return DBManager.GroupBy(this.ConnectionHandler, groupexpression, aggrigateexpression, distinct);
        }
        public virtual List<dynamic> GroupBy(Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {
            return DBManager.GroupBy(this.ConnectionHandler, groupexpression, aggrigateexpression, conditionsexpression, distinct);
        }
        public virtual List<dynamic> GroupByWithHaving(Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> Havingconditionsexpression, bool distinct = false)
        {
            return DBManager.GroupByWithHaving(this.ConnectionHandler, groupexpression, aggrigateexpression, Havingconditionsexpression, distinct);
        }
        public virtual List<dynamic> GroupByWithHaving(Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, Expression<Func<TDataStructure, bool>> Havingconditionsexpression, bool distinct = false)
        {
            return DBManager.GroupByWithHaving(this.ConnectionHandler, groupexpression, aggrigateexpression, conditionsexpression, Havingconditionsexpression, distinct);
        }

        public virtual List<dynamic> GroupBy(Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            return DBManager.GroupBy(this.ConnectionHandler, groupexpression, aggrigateexpression, new[] { orderbymodel }, distinct);
        }
        public virtual List<dynamic> GroupBy(Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            return DBManager.GroupBy(this.ConnectionHandler, groupexpression, aggrigateexpression, orderbymodel, distinct);
        }
        public virtual List<dynamic> GroupBy(Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            return DBManager.GroupBy(this.ConnectionHandler, groupexpression, aggrigateexpression, conditionsexpression, new[] { orderbymodel }, distinct);
        }

        public virtual List<dynamic> GroupBy(Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            return DBManager.GroupBy(this.ConnectionHandler, groupexpression, aggrigateexpression, conditionsexpression, orderbymodel, distinct);
        }
        public virtual List<dynamic> GroupByWithHaving(Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> Havingconditionsexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            return DBManager.GroupByWithHaving(this.ConnectionHandler, groupexpression, aggrigateexpression, Havingconditionsexpression, new[] { orderbymodel }, distinct);
        }
        public virtual List<dynamic> GroupByWithHaving(Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, Expression<Func<TDataStructure, bool>> Havingconditionsexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            return DBManager.GroupByWithHaving(this.ConnectionHandler, groupexpression, aggrigateexpression, conditionsexpression, Havingconditionsexpression, new[] { orderbymodel }, distinct);
        }

        public virtual List<dynamic> GroupByWithHaving(Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> Havingconditionsexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            return DBManager.GroupByWithHaving(this.ConnectionHandler, groupexpression, aggrigateexpression, Havingconditionsexpression, orderbymodel, distinct);
        }
        public virtual List<dynamic> GroupByWithHaving(Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, Expression<Func<TDataStructure, bool>> Havingconditionsexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            return DBManager.GroupByWithHaving(this.ConnectionHandler, groupexpression, aggrigateexpression, conditionsexpression, Havingconditionsexpression, orderbymodel, distinct);
        }
        #endregion


        #region Async




        public virtual async Task<int> InsertAsync(TDataStructure obj)
        {
            return await DBManager.InsertAsync(this.ConnectionHandler, obj);
        }
        public virtual async Task<int> UpdateAsync(TDataStructure obj)
        {
            return await DBManager.UpdateAsync(this.ConnectionHandler, obj);
        }
        public virtual async Task<int> DeleteAsync(params object[] keys)
        {
            return await DBManager.DeleteAsync<TDataStructure>(this.ConnectionHandler, keys);
        }
        public virtual async Task<int> DeleteAsync(TDataStructure obj)
        {
            return await DBManager.DeleteAsync(this.ConnectionHandler, obj);
        }
        public virtual async Task<int> InsertAsync(TDataStructure obj, Delegates.LogMethod method, params object[] parameters)
        {
            return await DBManager.InsertAsync(this.ConnectionHandler, obj, method, parameters);
        }
        public virtual async Task<int> UpdateAsync(TDataStructure obj, Delegates.LogMethod method, params object[] parameters)
        {
            return await DBManager.UpdateAsync(this.ConnectionHandler, obj, method, parameters);
        }
        public virtual async Task<int> DeleteAsync(TDataStructure obj, Delegates.LogMethod method, params object[] parameters)
        {
            return await DBManager.DeleteAsync(this.ConnectionHandler, obj, method, parameters);
        }


        public virtual async Task<TDataStructure> GetAsync(params object[] keys)
        {
            return await DBManager.GetObjectAsync<TDataStructure>(this.ConnectionHandler, keys);
        }
        public virtual async Task<TDataStructure> SimpleGetAsync(params object[] keys)
        {
            return await DBManager.SimpleGetObjectAsync<TDataStructure>(this.ConnectionHandler, keys);
        }
        public virtual async Task GetLanuageContentAsync(string culture, TDataStructure obj)
        {
            await DBManager.GetLanuageContentAsync(this.ConnectionHandler, culture, obj);
        }
        public virtual async Task GetLanuageContentAsync(string culture, List<TDataStructure> obj)
        {
            await DBManager.GetLanuageContentAsync(this.ConnectionHandler, culture, obj);
        }
        public virtual async Task<TDataStructure> GetLanuageContentAsync(string culture, params object[] keys)
        {
            return await DBManager.GetLanuageContentAsync<TDataStructure>(this.ConnectionHandler, culture, keys);
        }
        public virtual async Task<TDataStructure> GetLanuageContentsimpleAsync(string culture, params object[] keys)
        {
            return await DBManager.GetLanuageContentsimpleAsync<TDataStructure>(this.ConnectionHandler, culture, keys);
        }

        public virtual async Task<List<TDataStructure>> GetAllAsync(bool simpleload = false)
        {
            return await DBManager.GetCollectionAsync<TDataStructure>(this.ConnectionHandler, simpleload);
        }


        public virtual async Task<List<TDataStructure>> WhereAsync(Expression<Func<TDataStructure, bool>> expression, bool simpleload = false)
        {
            return await DBManager.WhereAsync(this.ConnectionHandler, expression, simpleload);
        }
        public virtual async Task<bool> AnyAsync()
        {
            return await DBManager.AnyAsync<TDataStructure>(this.ConnectionHandler);
        }
        public virtual async Task<bool> AnyAsync(Expression<Func<TDataStructure, bool>> expression)
        {
            return await DBManager.AnyAsync(this.ConnectionHandler, expression);
        }

        public virtual async Task<TResult> MaxAsync<TResult>(Expression<Func<TDataStructure, TResult>> expression)
        {
            return await DBManager.MaxAsync(this.ConnectionHandler, expression);
        }
        public virtual async Task<double> AverageAsync<TResult>(Expression<Func<TDataStructure, TResult>> expression)
        {
            return await DBManager.AverageAsync(this.ConnectionHandler, expression);
        }
        public virtual async Task<TResult> MinAsync<TResult>(Expression<Func<TDataStructure, TResult>> expression)
        {
            return await DBManager.MinAsync(this.ConnectionHandler, expression);
        }
        public virtual async Task<TResult> SumAsync<TResult>(Expression<Func<TDataStructure, TResult>> expression)
        {
            return await DBManager.SumAsync(this.ConnectionHandler, expression);
        }
        public virtual async Task<TResult> MaxAsync<TResult>(Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            return await DBManager.MaxAsync(this.ConnectionHandler, expression, conditionsexpression);
        }
        public virtual async Task<double> AverageAsync<TResult>(Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            return await DBManager.AverageAsync(this.ConnectionHandler, expression, conditionsexpression);
        }
        public virtual async Task<TResult> MinAsync<TResult>(Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            return await DBManager.MinAsync(this.ConnectionHandler, expression, conditionsexpression);
        }
        public virtual async Task<TResult> SumAsync<TResult>(Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            return await DBManager.SumAsync(this.ConnectionHandler, expression, conditionsexpression);
        }

        public virtual async Task<int> CountAsync()
        {
            return await DBManager.CountAsync<TDataStructure>(this.ConnectionHandler);
        }
        public virtual async Task<int> CountAsync(Expression<Func<TDataStructure, bool>> expression)
        {
            return await DBManager.CountAsync(this.ConnectionHandler, expression);
        }
        public virtual async Task<int> CountAsync<TColumn>(Expression<Func<TDataStructure, TColumn>> expression, bool distinct = false)
        {
            return await DBManager.CountAsync(this.ConnectionHandler, expression, distinct);
        }
        public virtual async Task<int> CountAsync<TColumn>(Expression<Func<TDataStructure, TColumn>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {
            return await DBManager.CountAsync(this.ConnectionHandler, expression, conditionsexpression, distinct);
        }



        public virtual async Task<TDataStructure> FirstOrDefaultAsync(bool simpleload = false)
        {
            return await DBManager.FirstOrDefaultAsync<TDataStructure>(this.ConnectionHandler, simpleload);
        }
        public virtual async Task<TDataStructure> FirstOrDefaultAsync(Expression<Func<TDataStructure, bool>> expression, bool simpleload = false)
        {
            return await DBManager.FirstOrDefaultAsync(this.ConnectionHandler, expression, simpleload);
        }
        public virtual async Task<TDataStructure> FirstOrDefaultWithOrderByAsync<TOrderProperty>(Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, bool simpleload = false)
        {
            return await DBManager.FirstOrDefaultWithOrderByAsync(this.ConnectionHandler, orderByexpression, simpleload);
        }
        public virtual async Task<TDataStructure> FirstOrDefaultWithOrderByAsync(OrderByModel<TDataStructure>[] orderByexpression, bool simpleload = false)
        {
            return await DBManager.FirstOrDefaultWithOrderByAsync(this.ConnectionHandler, orderByexpression, simpleload);
        }
        public virtual async Task<TDataStructure> FirstOrDefaultWithOrderByAsync(OrderByModel<TDataStructure>[] orderByexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            return await DBManager.FirstOrDefaultWithOrderByAsync(this.ConnectionHandler, orderByexpression, conditionsexpression, simpleload);
        }
        public virtual async Task<TDataStructure> FirstOrDefaultWithOrderByDescendingAsync<TOrderProperty>(Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, bool simpleload = false)
        {
            return await DBManager.FirstOrDefaultWithOrderByDescendingAsync(this.ConnectionHandler, orderByexpression, simpleload);
        }
        public virtual async Task<TDataStructure> FirstOrDefaultWithOrderByAsync<TOrderProperty>(Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            return await DBManager.FirstOrDefaultWithOrderByAsync(this.ConnectionHandler, orderByexpression, conditionsexpression, simpleload);
        }
        public virtual async Task<TDataStructure> FirstOrDefaultWithOrderByDescendingAsync<TOrderProperty>(Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            return await DBManager.FirstOrDefaultWithOrderByDescendingAsync(this.ConnectionHandler, orderByexpression, conditionsexpression, simpleload);
        }



        public virtual async Task<TDataStructure> SingleOrDefaultAsync(bool simpleload = false)
        {
            return await DBManager.SingleOrDefaultAsync<TDataStructure>(this.ConnectionHandler, simpleload);
        }
        public virtual async Task<TDataStructure> SingleOrDefaultAsync(Expression<Func<TDataStructure, bool>> expression, bool simpleload = false)
        {
            return await DBManager.SingleOrDefaultAsync(this.ConnectionHandler, expression, simpleload);
        }


        public virtual async Task<TDataStructure> IncludeFirstOrDefaultAsync(Expression<Func<TDataStructure, Object>>[] Includeexpression)
        {

            return await DBManager.IncludeFirstOrDefaultAsync(this.ConnectionHandler, Includeexpression);

        }
        public virtual async Task<TDataStructure> IncludeFirstOrDefaultAsync(Expression<Func<TDataStructure, Object>>[] Includeexpression, OrderByModel<TDataStructure> orderByModel)
        {

            return await DBManager.IncludeFirstOrDefaultAsync(this.ConnectionHandler, Includeexpression, new[] { orderByModel });

        }
        public virtual async Task<TDataStructure> IncludeFirstOrDefaultAsync(Expression<Func<TDataStructure, Object>>[] Includeexpression, OrderByModel<TDataStructure>[] orderByModels)
        {

            return await DBManager.IncludeFirstOrDefaultAsync(this.ConnectionHandler, Includeexpression, orderByModels);

        }
        public virtual async Task<TDataStructure> IncludeFirstOrDefaultAsync(Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {

            return await DBManager.IncludeFirstOrDefaultAsync(this.ConnectionHandler, Includeexpression, conditionsexpression);

        }
        public virtual async Task<TDataStructure> IncludeFirstOrDefaultAsync(Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel)
        {

            return await DBManager.IncludeFirstOrDefaultAsync(this.ConnectionHandler, Includeexpression, conditionsexpression, new[] { orderByModel });

        }
        public virtual async Task<TDataStructure> IncludeFirstOrDefaultAsync(Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModels)
        {

            return await DBManager.IncludeFirstOrDefaultAsync(this.ConnectionHandler, Includeexpression, conditionsexpression, orderByModels);

        }




        public virtual async Task<List<TDataStructure>> IncludeTopAsync(int topcount, Expression<Func<TDataStructure, Object>>[] Includeexpression)
        {

            return await DBManager.IncludeTopAsync(this.ConnectionHandler, topcount, Includeexpression);

        }
        public virtual async Task<List<TDataStructure>> IncludeTopAsync(int topcount, Expression<Func<TDataStructure, Object>>[] Includeexpression, OrderByModel<TDataStructure> orderByModel)
        {

            return await DBManager.IncludeTopAsync(this.ConnectionHandler, topcount, Includeexpression, new[] { orderByModel });

        }
        public virtual async Task<List<TDataStructure>> IncludeTopAsync(int topcount, Expression<Func<TDataStructure, Object>>[] Includeexpression, OrderByModel<TDataStructure>[] orderByModels)
        {

            return await DBManager.IncludeTopAsync(this.ConnectionHandler, topcount, Includeexpression, orderByModels);

        }
        public virtual async Task<List<TDataStructure>> IncludeTopAsync(int topcount, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {

            return await DBManager.IncludeTopAsync(this.ConnectionHandler, topcount, Includeexpression, conditionsexpression);

        }
        public virtual async Task<List<TDataStructure>> IncludeTopAsync(int topcount, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel)
        {

            return await DBManager.IncludeTopAsync(this.ConnectionHandler, topcount, Includeexpression, conditionsexpression, new[] { orderByModel });

        }
        public virtual async Task<List<TDataStructure>> IncludeTopAsync(int topcount, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModels)
        {

            return await DBManager.IncludeTopAsync(this.ConnectionHandler, topcount, Includeexpression, conditionsexpression, orderByModels);

        }




        public virtual async Task<List<TDataStructure>> IncludePagedListAsync(int pageIndex, int pagesize, Expression<Func<TDataStructure, Object>>[] Includeexpression)
        {

            return await DBManager.IncludePagedListAsync(this.ConnectionHandler, pageIndex, pagesize, Includeexpression);

        }
        public virtual async Task<List<TDataStructure>> IncludePagedListAsync(int pageIndex, int pagesize, Expression<Func<TDataStructure, Object>>[] Includeexpression, OrderByModel<TDataStructure> orderByModel)
        {

            return await DBManager.IncludePagedListAsync(this.ConnectionHandler, pageIndex, pagesize, Includeexpression, new[] { orderByModel });

        }
        public virtual async Task<List<TDataStructure>> IncludePagedListAsync(int pageIndex, int pagesize, Expression<Func<TDataStructure, Object>>[] Includeexpression, OrderByModel<TDataStructure>[] orderByModels)
        {

            return await DBManager.IncludePagedListAsync(this.ConnectionHandler, pageIndex, pagesize, Includeexpression, orderByModels);

        }
        public virtual async Task<List<TDataStructure>> IncludePagedListAsync(int pageIndex, int pagesize, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {

            return await DBManager.IncludePagedListAsync(this.ConnectionHandler, pageIndex, pagesize, Includeexpression, conditionsexpression);

        }
        public virtual async Task<List<TDataStructure>> IncludePagedListAsync(int pageIndex, int pagesize, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel)
        {

            return await DBManager.IncludePagedListAsync(this.ConnectionHandler, pageIndex, pagesize, Includeexpression, conditionsexpression, new[] { orderByModel });

        }
        public virtual async Task<List<TDataStructure>> IncludePagedListAsync(int pageIndex, int pagesize, Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModels)
        {

            return await DBManager.IncludePagedListAsync(this.ConnectionHandler, pageIndex, pagesize, Includeexpression, conditionsexpression, orderByModels);

        }



        public virtual async Task<List<TDataStructure>> IncludeAsync(Expression<Func<TDataStructure, Object>>[] Includeexpression)
        {

            return await DBManager.IncludeAsync(this.ConnectionHandler, Includeexpression);

        }
        public virtual async Task<List<TDataStructure>> IncludeAsync(Expression<Func<TDataStructure, Object>>[] Includeexpression, OrderByModel<TDataStructure> orderByModel)
        {

            return await DBManager.IncludeAsync(this.ConnectionHandler, Includeexpression, new[] { orderByModel });

        }
        public virtual async Task<List<TDataStructure>> IncludeAsync(Expression<Func<TDataStructure, Object>>[] Includeexpression, OrderByModel<TDataStructure>[] orderByModels)
        {

            return await DBManager.IncludeAsync(this.ConnectionHandler, Includeexpression, orderByModels);

        }
        public virtual async Task<List<TDataStructure>> IncludeAsync(Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {

            return await DBManager.IncludeAsync(this.ConnectionHandler, Includeexpression, conditionsexpression);

        }
        public virtual async Task<List<TDataStructure>> IncludeAsync(Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel)
        {

            return await DBManager.IncludeAsync(this.ConnectionHandler, Includeexpression, conditionsexpression, new[] { orderByModel });

        }
        public virtual async Task<List<TDataStructure>> IncludeAsync(Expression<Func<TDataStructure, Object>>[] Includeexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModels)
        {

            return await DBManager.IncludeAsync(this.ConnectionHandler, Includeexpression, conditionsexpression, orderByModels);

        }



        public virtual async Task<List<TDataStructure>> PagedListAsync(int pageIndex, int pagesize, bool simpleload = false)
        {

            return await DBManager.PagedListAsync<TDataStructure>(this.ConnectionHandler, pageIndex, pagesize, simpleload);

        }
        public virtual async Task<List<TDataStructure>> PagedListAsync(int pageIndex, int pagesize, OrderByModel<TDataStructure> orderByModel, bool simpleload = false)
        {

            return await DBManager.PagedListAsync(this.ConnectionHandler, pageIndex, pagesize, new[] { orderByModel }, simpleload);

        }
        public virtual async Task<List<TDataStructure>> PagedListAsync(int pageIndex, int pagesize, OrderByModel<TDataStructure>[] orderByModels, bool simpleload = false)
        {

            return await DBManager.PagedListAsync(this.ConnectionHandler, pageIndex, pagesize, orderByModels, simpleload);

        }
        public virtual async Task<List<TDataStructure>> PagedListAsync(int pageIndex, int pagesize, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {

            return await DBManager.PagedListAsync(this.ConnectionHandler, pageIndex, pagesize, conditionsexpression, simpleload);

        }
        public virtual async Task<List<TDataStructure>> PagedListAsync(int pageIndex, int pagesize, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel, bool simpleload = false)
        {

            return await DBManager.PagedListAsync(this.ConnectionHandler, pageIndex, pagesize, conditionsexpression, new[] { orderByModel }, simpleload);

        }
        public virtual async Task<List<TDataStructure>> PagedListAsync(int pageIndex, int pagesize, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModels, bool simpleload = false)
        {

            return await DBManager.PagedListAsync(this.ConnectionHandler, pageIndex, pagesize, conditionsexpression, orderByModels, simpleload);

        }







        public virtual async Task<dynamic> SelectFirstOrDefaultAsync(Expression<Func<TDataStructure, Object>>[] expression)
        {
            return await DBManager.SelectFirstOrDefaultAsync(this.ConnectionHandler, expression);
        }
        public virtual async Task<dynamic> SelectFirstOrDefaultAsync(Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            return await DBManager.SelectFirstOrDefaultAsync(this.ConnectionHandler, expression, conditionsexpression);
        }

        public virtual async Task<dynamic> SelectFirstOrDefaultAsync(Expression<Func<TDataStructure, Object>>[] expression, OrderByModel<TDataStructure> orderbymodel)
        {
            return await DBManager.SelectFirstOrDefaultAsync(this.ConnectionHandler, expression, new[] { orderbymodel });
        }
        public virtual async Task<dynamic> SelectFirstOrDefaultAsync(Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderbymodel)
        {
            return await DBManager.SelectFirstOrDefaultAsync(this.ConnectionHandler, expression, conditionsexpression, new[] { orderbymodel });
        }

        public virtual async Task<dynamic> SelectFirstOrDefaultAsync(Expression<Func<TDataStructure, Object>>[] expression, OrderByModel<TDataStructure>[] orderbymodels)
        {
            return await DBManager.SelectFirstOrDefaultAsync(this.ConnectionHandler, expression, orderbymodels);
        }
        public virtual async Task<dynamic> SelectFirstOrDefaultAsync(Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels)
        {
            return await DBManager.SelectFirstOrDefaultAsync(this.ConnectionHandler, expression, conditionsexpression, orderbymodels);
        }


        public virtual async Task<List<dynamic>> SelectAsync(Expression<Func<TDataStructure, Object>>[] expression, bool distinct = false)
        {
            return await DBManager.SelectAsync(this.ConnectionHandler, expression, distinct);
        }
        public virtual async Task<List<dynamic>> SelectAsync(Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {
            return await DBManager.SelectAsync(this.ConnectionHandler, expression, conditionsexpression, distinct);
        }

        public virtual async Task<List<dynamic>> SelectAsync(Expression<Func<TDataStructure, Object>>[] expression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            return await DBManager.SelectAsync(this.ConnectionHandler, expression, new[] { orderbymodel }, distinct);
        }
        public virtual async Task<List<dynamic>> SelectAsync(Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            return await DBManager.SelectAsync(this.ConnectionHandler, expression, conditionsexpression, new[] { orderbymodel }, distinct);
        }

        public virtual async Task<List<dynamic>> SelectAsync(Expression<Func<TDataStructure, Object>>[] expression, OrderByModel<TDataStructure>[] orderbymodels, bool distinct = false)
        {
            return await DBManager.SelectAsync(this.ConnectionHandler, expression, orderbymodels, distinct);
        }
        public virtual async Task<List<dynamic>> SelectAsync(Expression<Func<TDataStructure, Object>>[] expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels, bool distinct = false)
        {
            return await DBManager.SelectAsync(this.ConnectionHandler, expression, conditionsexpression, orderbymodels, distinct);
        }





        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, bool distinct = false)
        {
            return await DBManager.SelectKeyValuePairAsync<TDataStructure>(this.ConnectionHandler, DataValueField, DataTextField, distinct);
        }
        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {
            return await DBManager.SelectKeyValuePairAsync(this.ConnectionHandler, DataValueField, DataTextField, conditionsexpression, distinct);
        }
        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel, bool distinct = false)
        {
            return await DBManager.SelectKeyValuePairAsync(this.ConnectionHandler, DataValueField, DataTextField, conditionsexpression, new[] { orderByModel }, distinct);
        }
        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels, bool distinct = false)
        {
            return await DBManager.SelectKeyValuePairAsync(this.ConnectionHandler, DataValueField, DataTextField, conditionsexpression, orderbymodels, distinct);
        }
        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, OrderByModel<TDataStructure> orderByModel, bool distinct = false)
        {
            return await DBManager.SelectKeyValuePairAsync(this.ConnectionHandler, DataValueField, DataTextField, new[] { orderByModel }, distinct);
        }
        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, OrderByModel<TDataStructure>[] orderbymodels, bool distinct = false)
        {
            return await DBManager.SelectKeyValuePairAsync(this.ConnectionHandler, DataValueField, DataTextField, orderbymodels, distinct);
        }




        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, string culture, bool distinct = false)
        {
            return await DBManager.SelectKeyValuePairAsync<TDataStructure>(this.ConnectionHandler, DataValueField, DataTextField, culture, distinct);
        }
        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, string culture, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {
            return await DBManager.SelectKeyValuePairAsync(this.ConnectionHandler, DataValueField, DataTextField, culture, conditionsexpression, distinct);
        }
        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, string culture, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel, bool distinct = false)
        {
            return await DBManager.SelectKeyValuePairAsync(this.ConnectionHandler, DataValueField, DataTextField, culture, conditionsexpression, new[] { orderByModel }, distinct);
        }
        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, string culture, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels, bool distinct = false)
        {
            return await DBManager.SelectKeyValuePairAsync(this.ConnectionHandler, DataValueField, DataTextField, culture, conditionsexpression, orderbymodels, distinct);
        }
        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, string culture, OrderByModel<TDataStructure> orderByModel, bool distinct = false)
        {
            return await DBManager.SelectKeyValuePairAsync(this.ConnectionHandler, DataValueField, DataTextField, culture, new[] { orderByModel }, distinct);
        }
        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(Expression<Func<TDataStructure, Object>> DataValueField, Expression<Func<TDataStructure, Object>> DataTextField, string culture, OrderByModel<TDataStructure>[] orderbymodels, bool distinct = false)
        {
            return await DBManager.SelectKeyValuePairAsync(this.ConnectionHandler, DataValueField, DataTextField, culture, orderbymodels, distinct);
        }




        public virtual async Task<List<TDataStructure>> SelectTopAsync(int topcount, bool simpleload = false)
        {
            return await DBManager.SelectTopAsync<TDataStructure>(this.ConnectionHandler, topcount, simpleload);
        }
        public virtual async Task<List<TDataStructure>> SelectTopAsync(int topcount, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            return await DBManager.SelectTopAsync(this.ConnectionHandler, topcount, conditionsexpression, simpleload);
        }
        public virtual async Task<List<TDataStructure>> SelectTopAsync(int topcount, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel, bool simpleload = false)
        {
            return await DBManager.SelectTopAsync(this.ConnectionHandler, topcount, conditionsexpression, new[] { orderByModel }, simpleload);
        }
        public virtual async Task<List<TDataStructure>> SelectTopAsync(int topcount, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodels, bool simpleload = false)
        {
            return await DBManager.SelectTopAsync(this.ConnectionHandler, topcount, conditionsexpression, orderbymodels, simpleload);
        }
        public virtual async Task<List<TDataStructure>> SelectTopAsync(int topcount, OrderByModel<TDataStructure> orderByModel, bool simpleload = false)
        {
            return await DBManager.SelectTopAsync(this.ConnectionHandler, topcount, new[] { orderByModel }, simpleload);
        }
        public virtual async Task<List<TDataStructure>> SelectTopAsync(int topcount, OrderByModel<TDataStructure>[] orderbymodels, bool simpleload = false)
        {
            return await DBManager.SelectTopAsync(this.ConnectionHandler, topcount, orderbymodels, simpleload);
        }



        public virtual async Task<TResult> SelectFirstOrDefaultAsync<TResult>(Expression<Func<TDataStructure, TResult>> expression)
        {
            return await DBManager.SelectFirstOrDefaultAsync(this.ConnectionHandler, expression);
        }
        public virtual async Task<TResult> SelectFirstOrDefaultAsync<TResult>(Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression)
        {
            return await DBManager.SelectFirstOrDefaultAsync(this.ConnectionHandler, expression, conditionsexpression);
        }

        public virtual async Task<TResult> SelectFirstOrDefaultAsync<TResult>(Expression<Func<TDataStructure, TResult>> expression, OrderByModel<TDataStructure> orderByModel)
        {
            return await DBManager.SelectFirstOrDefaultAsync(this.ConnectionHandler, expression, new[] { orderByModel });
        }
        public virtual async Task<TResult> SelectFirstOrDefaultAsync<TResult>(Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel)
        {
            return await DBManager.SelectFirstOrDefaultAsync(this.ConnectionHandler, expression, conditionsexpression, new[] { orderByModel });
        }

        public virtual async Task<TResult> SelectFirstOrDefaultAsync<TResult>(Expression<Func<TDataStructure, TResult>> expression, OrderByModel<TDataStructure>[] orderByModels)
        {
            return await DBManager.SelectFirstOrDefaultAsync(this.ConnectionHandler, expression, orderByModels);
        }
        public virtual async Task<TResult> SelectFirstOrDefaultAsync<TResult>(Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModels)
        {
            return await DBManager.SelectFirstOrDefaultAsync(this.ConnectionHandler, expression, conditionsexpression, orderByModels);
        }



        public virtual async Task<List<TResult>> SelectAsync<TResult>(Expression<Func<TDataStructure, TResult>> expression, bool distinct = false)
        {
            return await DBManager.SelectAsync(this.ConnectionHandler, expression, distinct);
        }
        public virtual async Task<List<TResult>> SelectAsync<TResult>(Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {
            return await DBManager.SelectAsync(this.ConnectionHandler, expression, conditionsexpression, distinct);
        }

        public virtual async Task<List<TResult>> SelectAsync<TResult>(Expression<Func<TDataStructure, TResult>> expression, OrderByModel<TDataStructure> orderByModel, bool distinct = false)
        {
            return await DBManager.SelectAsync(this.ConnectionHandler, expression, new[] { orderByModel }, distinct);
        }
        public virtual async Task<List<TResult>> SelectAsync<TResult>(Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderByModel, bool distinct = false)
        {
            return await DBManager.SelectAsync(this.ConnectionHandler, expression, conditionsexpression, new[] { orderByModel }, distinct);
        }

        public virtual async Task<List<TResult>> SelectAsync<TResult>(Expression<Func<TDataStructure, TResult>> expression, OrderByModel<TDataStructure>[] orderByModels, bool distinct = false)
        {
            return await DBManager.SelectAsync(this.ConnectionHandler, expression, orderByModels, distinct);
        }
        public virtual async Task<List<TResult>> SelectAsync<TResult>(Expression<Func<TDataStructure, TResult>> expression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderByModels, bool distinct = false)
        {
            return await DBManager.SelectAsync(this.ConnectionHandler, expression, conditionsexpression, orderByModels, distinct);
        }




        public async Task<List<TDataStructure>> OrderByAsync<TOrderProperty>(Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, bool simpleload = false)
        {
            return await DBManager.OrderByAsync(this.ConnectionHandler, orderByexpression, simpleload);
        }
        public async Task<List<TDataStructure>> OrderByAsync(OrderByModel<TDataStructure>[] orderByexpression, bool simpleload = false)
        {
            return await DBManager.OrderByAsync(this.ConnectionHandler, orderByexpression, simpleload);
        }
        public async Task<List<TDataStructure>> OrderByAsync(OrderByModel<TDataStructure>[] orderByexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            return await DBManager.OrderByAsync(this.ConnectionHandler, orderByexpression, conditionsexpression, simpleload);
        }
        public async Task<List<TDataStructure>> OrderByDescendingAsync<TOrderProperty>(Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, bool simpleload = false)
        {
            return await DBManager.OrderByDescendingAsync(this.ConnectionHandler, orderByexpression, simpleload);
        }
        public async Task<List<TDataStructure>> OrderByAsync<TOrderProperty>(Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            return await DBManager.OrderByAsync(this.ConnectionHandler, orderByexpression, conditionsexpression, simpleload);
        }
        public async Task<List<TDataStructure>> OrderByDescendingAsync<TOrderProperty>(Expression<Func<TDataStructure, TOrderProperty>> orderByexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool simpleload = false)
        {
            return await DBManager.OrderByDescendingAsync(this.ConnectionHandler, orderByexpression, conditionsexpression, simpleload);
        }







        public virtual async Task<List<dynamic>> GroupByAsync(Expression<Func<TDataStructure, Object>>[] groupexpression, bool distinct = false)
        {
            return await DBManager.GroupByAsync(this.ConnectionHandler, groupexpression, distinct);
        }
        public virtual async Task<List<dynamic>> GroupByAsync(Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {
            return await DBManager.GroupByAsync(this.ConnectionHandler, groupexpression, conditionsexpression, distinct);
        }
        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> Havingconditionsexpression, bool distinct = false)
        {
            return await DBManager.GroupByWithHavingAsync(this.ConnectionHandler, groupexpression, Havingconditionsexpression, distinct);
        }
        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, Expression<Func<TDataStructure, bool>> Havingconditionsexpression, bool distinct = false)
        {
            return await DBManager.GroupByWithHavingAsync(this.ConnectionHandler, groupexpression, conditionsexpression, Havingconditionsexpression, distinct);
        }

        public virtual async Task<List<dynamic>> GroupByAsync(Expression<Func<TDataStructure, Object>>[] groupexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            return await DBManager.GroupByAsync(this.ConnectionHandler, groupexpression, new[] { orderbymodel }, distinct);
        }
        public virtual async Task<List<dynamic>> GroupByAsync(Expression<Func<TDataStructure, Object>>[] groupexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            return await DBManager.GroupByAsync(this.ConnectionHandler, groupexpression, orderbymodel, distinct);
        }

        public virtual async Task<List<dynamic>> GroupByAsync(Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            return await DBManager.GroupByAsync(this.ConnectionHandler, groupexpression, conditionsexpression, orderbymodel, distinct);
        }
        public virtual async Task<List<dynamic>> GroupByAsync(Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            return await DBManager.GroupByAsync(this.ConnectionHandler, groupexpression, conditionsexpression, new[] { orderbymodel }, distinct);
        }
        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> Havingconditionsexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            return await DBManager.GroupByWithHavingAsync(this.ConnectionHandler, groupexpression, Havingconditionsexpression, new[] { orderbymodel }, distinct);
        }

        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> Havingconditionsexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            return await DBManager.GroupByWithHavingAsync(this.ConnectionHandler, groupexpression, Havingconditionsexpression, orderbymodel, distinct);
        }
        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, Expression<Func<TDataStructure, bool>> Havingconditionsexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            return await DBManager.GroupByWithHavingAsync(this.ConnectionHandler, groupexpression, conditionsexpression, Havingconditionsexpression, new[] { orderbymodel }, distinct);
        }

        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<TDataStructure, Object>>[] groupexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, Expression<Func<TDataStructure, bool>> Havingconditionsexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            return await DBManager.GroupByWithHavingAsync(this.ConnectionHandler, groupexpression, conditionsexpression, Havingconditionsexpression, orderbymodel, distinct);
        }

        public virtual async Task<List<dynamic>> GroupByAsync(Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, bool distinct = false)
        {
            return await DBManager.GroupByAsync(this.ConnectionHandler, groupexpression, aggrigateexpression, distinct);
        }
        public virtual async Task<List<dynamic>> GroupByAsync(Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, bool distinct = false)
        {
            return await DBManager.GroupByAsync(this.ConnectionHandler, groupexpression, aggrigateexpression, conditionsexpression, distinct);
        }
        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> Havingconditionsexpression, bool distinct = false)
        {
            return await DBManager.GroupByWithHavingAsync(this.ConnectionHandler, groupexpression, aggrigateexpression, Havingconditionsexpression, distinct);
        }
        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, Expression<Func<TDataStructure, bool>> Havingconditionsexpression, bool distinct = false)
        {
            return await DBManager.GroupByWithHavingAsync(this.ConnectionHandler, groupexpression, aggrigateexpression, conditionsexpression, Havingconditionsexpression, distinct);
        }

        public virtual async Task<List<dynamic>> GroupByAsync(Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            return await DBManager.GroupByAsync(this.ConnectionHandler, groupexpression, aggrigateexpression, new[] { orderbymodel }, distinct);
        }
        public virtual async Task<List<dynamic>> GroupByAsync(Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            return await DBManager.GroupByAsync(this.ConnectionHandler, groupexpression, aggrigateexpression, orderbymodel, distinct);
        }
        public virtual async Task<List<dynamic>> GroupByAsync(Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            return await DBManager.GroupByAsync(this.ConnectionHandler, groupexpression, aggrigateexpression, conditionsexpression, new[] { orderbymodel }, distinct);
        }

        public virtual async Task<List<dynamic>> GroupByAsync(Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            return await DBManager.GroupByAsync(this.ConnectionHandler, groupexpression, aggrigateexpression, conditionsexpression, orderbymodel, distinct);
        }
        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> Havingconditionsexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            return await DBManager.GroupByWithHavingAsync(this.ConnectionHandler, groupexpression, aggrigateexpression, Havingconditionsexpression, new[] { orderbymodel }, distinct);
        }
        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, Expression<Func<TDataStructure, bool>> Havingconditionsexpression, OrderByModel<TDataStructure> orderbymodel, bool distinct = false)
        {
            return await DBManager.GroupByWithHavingAsync(this.ConnectionHandler, groupexpression, aggrigateexpression, conditionsexpression, Havingconditionsexpression, new[] { orderbymodel }, distinct);
        }

        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> Havingconditionsexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            return await DBManager.GroupByWithHavingAsync(this.ConnectionHandler, groupexpression, aggrigateexpression, Havingconditionsexpression, orderbymodel, distinct);
        }
        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<TDataStructure, Object>>[] groupexpression, GroupByModel<TDataStructure>[] aggrigateexpression, Expression<Func<TDataStructure, bool>> conditionsexpression, Expression<Func<TDataStructure, bool>> Havingconditionsexpression, OrderByModel<TDataStructure>[] orderbymodel, bool distinct = false)
        {
            return await DBManager.GroupByWithHavingAsync(this.ConnectionHandler, groupexpression, aggrigateexpression, conditionsexpression, Havingconditionsexpression, orderbymodel, distinct);
        }


        #endregion
    }
}
