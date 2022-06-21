using Framework;
using Mhazami.Framework.CacheManager;
using Mhazami.Framework.DbHelper;
using Mhazami.Framework.Difinition;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace Mhazami.Framework
{
    public abstract class BaseFacade<T> where T : class
    {
        protected BaseFacade()
        {

        }

        protected BaseFacade(IConnectionHandler connectionHandler)
        {

            this._connectionHandler = new ConnectionHandler(connectionHandler);
            this._connectionHandler.OpenConnection();
            this.ControlConnection("OpenConnection");
        }
        protected BaseFacade(IConnectionHandler connectionHandler, bool externalConnection = true)
        {

            this._connectionHandler = new ConnectionHandler(connectionHandler, externalConnection);
            this._connectionHandler.OpenConnection();
            this.ControlConnection("OpenConnection");
        }

        ~BaseFacade()
        {
            this.ControlConnection("CloseConnection");
        }

        private void ControlConnection(string method)
        {

            foreach (PropertyInfo propertyInfo in this.GetType().GetTypeProperties().Where(x => x.PropertyType == typeof(IConnectionHandler)))
            {
                var methodInfo = propertyInfo.PropertyType.GetMethod(method);
                if (methodInfo == null) continue;

                var connectionHandlerPropertyValue = propertyInfo.GetValue(this, null);
                if (connectionHandlerPropertyValue == null) continue;
                try
                {
                    methodInfo.Invoke(connectionHandlerPropertyValue, new object[] { });
                }
                catch (Exception ex)
                {

                }
            }

        }



        private IConnectionHandler _connectionHandler;
        protected IConnectionHandler ConnectionHandler
        {
            get { return this._connectionHandler ?? (this._connectionHandler = new ConnectionHandler()); }
            set { this._connectionHandler = value; }
        }
      
        #region Manual

        public virtual T Get(params object[] keys)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Get(this._connectionHandler, keys);
            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual T SimpleGet(params object[] keys)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.SimpleGet(this._connectionHandler, keys);
            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual void GetLanuageContent(string culture, T obj)
        {
            try
            {
                dynamic bo = this.BoLayer();
                bo.GetLanuageContent(this._connectionHandler, culture, obj);
            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public virtual void GetLanuageContent(string culture, List<T> obj)
        {
            try
            {
                dynamic bo = this.BoLayer();
                bo.GetLanuageContent(this._connectionHandler, culture, obj);
            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual T GetLanuageContent(string culture, params object[] keys)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.GetLanuageContent(this._connectionHandler, culture, keys);
            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual T GetLanuageContentsimple(string culture, params object[] keys)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.GetLanuageContentsimple(this._connectionHandler, culture, keys);
            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public virtual List<T> GetAll(bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.GetAll(this._connectionHandler, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }


        public virtual bool Insert(T obj, Delegates.LogMethod method, params object[] parameters)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Insert(this._connectionHandler, obj, method, parameters);

            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.SavingDataFailed, ex);
            }

        }
        public virtual bool Insert(T obj)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Insert(this._connectionHandler, obj);

            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.SavingDataFailed, ex);
            }

        }

        public virtual bool Update(T obj, Delegates.LogMethod method, params object[] parameters)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Update(this._connectionHandler, obj, method, parameters);

            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.SavingDataFailed, ex);
            }

        }
        public virtual bool Update(T obj)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Update(this._connectionHandler, obj);

            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.SavingDataFailed, ex);
            }

        }

        public virtual bool Delete(params object[] keys)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Delete(this._connectionHandler, keys);

            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
               
                throw new KnownException(Messages.SavingDataFailed, ex);
            }
            
        }
        public virtual bool Delete(T obj)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Delete(this._connectionHandler, obj);

            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                
                throw new KnownException(Messages.SavingDataFailed, ex);
            }
          
        }
        public virtual bool Delete(T obj, Delegates.LogMethod method, params object[] parameters)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Delete(this._connectionHandler, obj, method, parameters);

            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                
                throw new KnownException(Messages.SavingDataFailed, ex);
            }
           
        }




        public virtual List<T> Where(Expression<Func<T, bool>> expression, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Where(this._connectionHandler, expression, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public virtual bool Any()
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Any(this._connectionHandler);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual bool Any(Expression<Func<T, bool>> expression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Any(this._connectionHandler, expression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public virtual TResult Sum<TResult>(Expression<Func<T, TResult>> expression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Sum(this._connectionHandler, expression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual TResult Max<TResult>(Expression<Func<T, TResult>> expression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Max(this._connectionHandler, expression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual TResult Min<TResult>(Expression<Func<T, TResult>> expression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Min(this._connectionHandler, expression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual double Average<TResult>(Expression<Func<T, TResult>> expression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Average(this._connectionHandler, expression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual TResult Sum<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Sum(this._connectionHandler, expression, conditionsexpression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual TResult Max<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Max(this._connectionHandler, expression, conditionsexpression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual TResult Min<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Min(this._connectionHandler, expression, conditionsexpression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual double Average<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Average(this._connectionHandler, expression, conditionsexpression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public virtual int Count()
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Count(this._connectionHandler);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual int Count(Expression<Func<T, bool>> expression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Count(this._connectionHandler, expression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual int Count<TColumn>(Expression<Func<T, TColumn>> expression, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Count(this._connectionHandler, expression, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual int Count<TColumn>(Expression<Func<T, TColumn>> expression, Expression<Func<T, bool>> conditionsexpression, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Count(this._connectionHandler, expression, conditionsexpression, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }





        public virtual T FirstOrDefault(bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.FirstOrDefault(this._connectionHandler, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual T FirstOrDefault(Expression<Func<T, bool>> expression, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.FirstOrDefault(this._connectionHandler, expression, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual T FirstOrDefaultWithOrderBy(OrderByModel<T>[] orderByexpression, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.FirstOrDefaultWithOrderBy(this._connectionHandler, orderByexpression, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual T FirstOrDefaultWithOrderBy(OrderByModel<T>[] orderByexpression, Expression<Func<T, bool>> conditionsexpression, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.FirstOrDefaultWithOrderBy(this._connectionHandler, orderByexpression, conditionsexpression, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual T FirstOrDefaultWithOrderBy<TOrderProperty>(Expression<Func<T, TOrderProperty>> orderByexpression, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.FirstOrDefaultWithOrderBy(this._connectionHandler, orderByexpression, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual T FirstOrDefaultWithOrderBy<TOrderProperty>(Expression<Func<T, TOrderProperty>> orderByexpression, Expression<Func<T, bool>> conditionsexpression, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.FirstOrDefaultWithOrderBy(this._connectionHandler, orderByexpression, conditionsexpression, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual T FirstOrDefaultWithOrderByDescending<TOrderProperty>(Expression<Func<T, TOrderProperty>> orderByexpression, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.FirstOrDefaultWithOrderByDescending(this._connectionHandler, orderByexpression, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual T FirstOrDefaultWithOrderByDescending<TOrderProperty>(Expression<Func<T, TOrderProperty>> orderByexpression, Expression<Func<T, bool>> conditionsexpression, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.FirstOrDefaultWithOrderByDescending(this._connectionHandler, orderByexpression, conditionsexpression, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public virtual T SingleOrDefault(bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.SingleOrDefault(this._connectionHandler);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual T SingleOrDefault(Expression<Func<T, bool>> expression, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.SingleOrDefault(this._connectionHandler, expression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }



        public virtual T IncludeFirstOrDefault(Expression<Func<T, Object>>[] Includeexpression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.IncludeFirstOrDefault(this._connectionHandler, Includeexpression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual T IncludeFirstOrDefault(Expression<Func<T, Object>>[] Includeexpression, OrderByModel<T> orderByModel)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.IncludeFirstOrDefault(this._connectionHandler, Includeexpression, orderByModel);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual T IncludeFirstOrDefault(Expression<Func<T, Object>>[] Includeexpression, OrderByModel<T>[] orderByModels)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.IncludeFirstOrDefault(this._connectionHandler, Includeexpression, orderByModels);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual T IncludeFirstOrDefault(Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.IncludeFirstOrDefault(this._connectionHandler, Includeexpression, conditionsexpression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }


        }
        public virtual T IncludeFirstOrDefault(Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderByModel)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.IncludeFirstOrDefault(this._connectionHandler, Includeexpression, conditionsexpression, orderByModel);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual T IncludeFirstOrDefault(Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderByModels)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.IncludeFirstOrDefault(this._connectionHandler, Includeexpression, conditionsexpression, orderByModels);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }




        public virtual List<T> IncludeTop(int topcount, Expression<Func<T, Object>>[] Includeexpression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.IncludeTop(this._connectionHandler, topcount, Includeexpression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual List<T> IncludeTop(int topcount, Expression<Func<T, Object>>[] Includeexpression, OrderByModel<T> orderByModel)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.IncludeTop(this._connectionHandler, topcount, Includeexpression, orderByModel);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual List<T> IncludeTop(int topcount, Expression<Func<T, Object>>[] Includeexpression, OrderByModel<T>[] orderByModels)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.IncludeTop(this._connectionHandler, topcount, Includeexpression, orderByModels);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual List<T> IncludeTop(int topcount, Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.IncludeTop(this._connectionHandler, topcount, Includeexpression, conditionsexpression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }


        }
        public virtual List<T> IncludeTop(int topcount, Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderByModel)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.IncludeTop(this._connectionHandler, topcount, Includeexpression, conditionsexpression, orderByModel);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual List<T> IncludeTop(int topcount, Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderByModels)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.IncludeTop(this._connectionHandler, topcount, Includeexpression, conditionsexpression, orderByModels);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }




        public virtual List<T> IncludePagedList(int pageIndex, int pagesize, Expression<Func<T, Object>>[] Includeexpression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.IncludePagedList(this._connectionHandler, pageIndex, pagesize, Includeexpression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual List<T> IncludePagedList(int pageIndex, int pagesize, Expression<Func<T, Object>>[] Includeexpression, OrderByModel<T> orderByModel)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.IncludePagedList(this._connectionHandler, pageIndex, pagesize, Includeexpression, orderByModel);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual List<T> IncludePagedList(int pageIndex, int pagesize, Expression<Func<T, Object>>[] Includeexpression, OrderByModel<T>[] orderByModels)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.IncludePagedList(this._connectionHandler, pageIndex, pagesize, Includeexpression, orderByModels);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual List<T> IncludePagedList(int pageIndex, int pagesize, Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.IncludePagedList(this._connectionHandler, pageIndex, pagesize, Includeexpression, conditionsexpression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }


        }
        public virtual List<T> IncludePagedList(int pageIndex, int pagesize, Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderByModel)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.IncludePagedList(this._connectionHandler, pageIndex, pagesize, Includeexpression, conditionsexpression, orderByModel);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual List<T> IncludePagedList(int pageIndex, int pagesize, Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderByModels)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.IncludePagedList(this._connectionHandler, pageIndex, pagesize, Includeexpression, conditionsexpression, orderByModels);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }




        public virtual List<T> Include(Expression<Func<T, Object>>[] Includeexpression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Include(this._connectionHandler, Includeexpression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual List<T> Include(Expression<Func<T, Object>>[] Includeexpression, OrderByModel<T> orderByModel)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Include(this._connectionHandler, Includeexpression, orderByModel);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual List<T> Include(Expression<Func<T, Object>>[] Includeexpression, OrderByModel<T>[] orderByModels)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Include(this._connectionHandler, Includeexpression, orderByModels);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual List<T> Include(Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Include(this._connectionHandler, Includeexpression, conditionsexpression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }


        }
        public virtual List<T> Include(Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderByModel)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Include(this._connectionHandler, Includeexpression, conditionsexpression, orderByModel);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual List<T> Include(Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderByModels)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Include(this._connectionHandler, Includeexpression, conditionsexpression, orderByModels);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }



        public virtual List<T> PagedList(int pageIndex, int pagesize, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.PagedList(this._connectionHandler, pageIndex, pagesize, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual List<T> PagedList(int pageIndex, int pagesize, OrderByModel<T> orderByModel, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.PagedList(this._connectionHandler, pageIndex, pagesize, orderByModel, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual List<T> PagedList(int pageIndex, int pagesize, OrderByModel<T>[] orderByModels, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.PagedList(this._connectionHandler, pageIndex, pagesize, orderByModels, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual List<T> PagedList(int pageIndex, int pagesize, Expression<Func<T, bool>> conditionsexpression, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.PagedList(this._connectionHandler, pageIndex, pagesize, conditionsexpression, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }


        }
        public virtual List<T> PagedList(int pageIndex, int pagesize, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderByModel, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.PagedList(this._connectionHandler, pageIndex, pagesize, conditionsexpression, orderByModel, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual List<T> PagedList(int pageIndex, int pagesize, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderByModels, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.PagedList(this._connectionHandler, pageIndex, pagesize, conditionsexpression, orderByModels, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }



        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, bool distinct = false)
        {

            try
            {
                dynamic bo = this.BoLayer();
                return bo.SelectKeyValuePair(this._connectionHandler, DataValueField, DataTextField, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }


        }
        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, Expression<Func<T, bool>> conditionsexpression, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.SelectKeyValuePair(this._connectionHandler, DataValueField, DataTextField, conditionsexpression, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderByModel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.SelectKeyValuePair(this._connectionHandler, DataValueField, DataTextField, conditionsexpression, orderByModel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderbymodels, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.SelectKeyValuePair(this._connectionHandler, DataValueField, DataTextField, conditionsexpression, orderbymodels, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, OrderByModel<T> orderByModel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.SelectKeyValuePair(this._connectionHandler, DataValueField, DataTextField, orderByModel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, OrderByModel<T>[] orderbymodels, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.SelectKeyValuePair(this._connectionHandler, DataValueField, DataTextField, orderbymodels, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }





        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, string culture, bool distinct = false)
        {

            try
            {
                dynamic bo = this.BoLayer();
                return bo.SelectKeyValuePair(this._connectionHandler, DataValueField, DataTextField, culture, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }


        }
        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, string culture, Expression<Func<T, bool>> conditionsexpression, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.SelectKeyValuePair(this._connectionHandler, DataValueField, DataTextField, culture, conditionsexpression, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, string culture, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderByModel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.SelectKeyValuePair(this._connectionHandler, DataValueField, DataTextField, culture, conditionsexpression, orderByModel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, string culture, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderbymodels, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.SelectKeyValuePair(this._connectionHandler, DataValueField, DataTextField, culture, conditionsexpression, orderbymodels, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, string culture, OrderByModel<T> orderByModel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.SelectKeyValuePair(this._connectionHandler, DataValueField, DataTextField, culture, orderByModel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, string culture, OrderByModel<T>[] orderbymodels, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.SelectKeyValuePair(this._connectionHandler, DataValueField, DataTextField, culture, orderbymodels, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }



        public virtual List<T> SelectTop(int topcount, bool simpleload = false)
        {

            try
            {
                dynamic bo = this.BoLayer();
                return bo.SelectTop(this._connectionHandler, topcount, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }


        }
        public virtual List<T> SelectTop(int topcount, Expression<Func<T, bool>> conditionsexpression, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.SelectTop(this._connectionHandler, topcount, conditionsexpression, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual List<T> SelectTop(int topcount, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderByModel, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.SelectTop(this._connectionHandler, topcount, conditionsexpression, orderByModel, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual List<T> SelectTop(int topcount, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderbymodels, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.SelectTop(this._connectionHandler, topcount, conditionsexpression, orderbymodels, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual List<T> SelectTop(int topcount, OrderByModel<T> orderByModel, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.SelectTop(this._connectionHandler, topcount, orderByModel, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual List<T> SelectTop(int topcount, OrderByModel<T>[] orderbymodels, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.SelectTop(this._connectionHandler, topcount, orderbymodels, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public virtual TResult SelectFirstOrDefault<TResult>(Expression<Func<T, TResult>> expression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.SelectFirstOrDefault(this._connectionHandler, expression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual TResult SelectFirstOrDefault<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.SelectFirstOrDefault(this._connectionHandler, expression, conditionsexpression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public virtual TResult SelectFirstOrDefault<TResult>(Expression<Func<T, TResult>> expression, OrderByModel<T> orderByModel)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.SelectFirstOrDefault(this._connectionHandler, expression, orderByModel);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual TResult SelectFirstOrDefault<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderByModel)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.SelectFirstOrDefault(this._connectionHandler, expression, conditionsexpression, orderByModel);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public virtual TResult SelectFirstOrDefault<TResult>(Expression<Func<T, TResult>> expression, OrderByModel<T>[] orderByModels)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.SelectFirstOrDefault(this._connectionHandler, expression, orderByModels);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual TResult SelectFirstOrDefault<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderByModels)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.SelectFirstOrDefault(this._connectionHandler, expression, conditionsexpression, orderByModels);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }




        public virtual List<TResult> Select<TResult>(Expression<Func<T, TResult>> expression, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Select(this._connectionHandler, expression, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual List<TResult> Select<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Select(this._connectionHandler, expression, conditionsexpression, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public virtual List<TResult> Select<TResult>(Expression<Func<T, TResult>> expression, OrderByModel<T> orderByModel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Select(this._connectionHandler, expression, orderByModel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual List<TResult> Select<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderByModel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Select(this._connectionHandler, expression, conditionsexpression, orderByModel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public virtual List<TResult> Select<TResult>(Expression<Func<T, TResult>> expression, OrderByModel<T>[] orderByModels, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Select(this._connectionHandler, expression, orderByModels, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual List<TResult> Select<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderByModels, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Select(this._connectionHandler, expression, conditionsexpression, orderByModels, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }



        public virtual dynamic SelectFirstOrDefault(Expression<Func<T, Object>>[] expression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.SelectFirstOrDefault(this._connectionHandler, expression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual dynamic SelectFirstOrDefault(Expression<Func<T, Object>>[] expression, Expression<Func<T, bool>> conditionsexpression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.SelectFirstOrDefault(this._connectionHandler, expression, conditionsexpression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public virtual dynamic SelectFirstOrDefault(Expression<Func<T, Object>>[] expression, OrderByModel<T> orderbymodel)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.SelectFirstOrDefault(this._connectionHandler, expression, orderbymodel);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual dynamic SelectFirstOrDefault(Expression<Func<T, Object>>[] expression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderbymodel)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.SelectFirstOrDefault(this._connectionHandler, expression, conditionsexpression, orderbymodel);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public virtual dynamic SelectFirstOrDefault(Expression<Func<T, Object>>[] expression, OrderByModel<T>[] orderbymodels)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.SelectFirstOrDefault(this._connectionHandler, expression, orderbymodels);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual dynamic SelectFirstOrDefault(Expression<Func<T, Object>>[] expression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderbymodels)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.SelectFirstOrDefault(this._connectionHandler, expression, conditionsexpression, orderbymodels);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }



        public virtual List<dynamic> Select(Expression<Func<T, Object>>[] expression, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Select(this._connectionHandler, expression, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual List<dynamic> Select(Expression<Func<T, Object>>[] expression, Expression<Func<T, bool>> conditionsexpression, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Select(this._connectionHandler, expression, conditionsexpression, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public virtual List<dynamic> Select(Expression<Func<T, Object>>[] expression, OrderByModel<T> orderbymodel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Select(this._connectionHandler, expression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual List<dynamic> Select(Expression<Func<T, Object>>[] expression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderbymodel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Select(this._connectionHandler, expression, conditionsexpression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public virtual List<dynamic> Select(Expression<Func<T, Object>>[] expression, OrderByModel<T>[] orderbymodels, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Select(this._connectionHandler, expression, orderbymodels, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual List<dynamic> Select(Expression<Func<T, Object>>[] expression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderbymodels, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.Select(this._connectionHandler, expression, conditionsexpression, orderbymodels, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }




        public virtual List<T> OrderBy(OrderByModel<T>[] expression, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.OrderBy(this._connectionHandler, expression, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual List<T> OrderBy(OrderByModel<T>[] expression, Expression<Func<T, bool>> conditionsexpression, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.OrderBy(this._connectionHandler, expression, conditionsexpression, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual List<T> OrderBy<TOrderProperty>(Expression<Func<T, TOrderProperty>> expression, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.OrderBy(this._connectionHandler, expression, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual List<T> OrderByDescending<TOrderProperty>(Expression<Func<T, TOrderProperty>> expression, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.OrderByDescending(this._connectionHandler, expression, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual List<T> OrderBy<TResult>(Expression<Func<T, TResult>> orderByexpression, Expression<Func<T, bool>> conditionsexpression, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.OrderBy(this._connectionHandler, orderByexpression, conditionsexpression, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual List<T> OrderByDescending<TResult>(Expression<Func<T, TResult>> orderByexpression, Expression<Func<T, bool>> conditionsexpression, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.OrderByDescending(this._connectionHandler, orderByexpression, conditionsexpression, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }





        public virtual List<dynamic> GroupBy(Expression<Func<T, Object>>[] groupexpression, bool distinct = false)
        {

            try
            {
                dynamic bo = this.BoLayer();
                return bo.GroupBy(this._connectionHandler, groupexpression, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual List<dynamic> GroupBy(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> conditionsexpression, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.GroupBy(this._connectionHandler, groupexpression, conditionsexpression, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual List<dynamic> GroupByWithHaving(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> havingconditionsexpression, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.GroupByWithHaving(this._connectionHandler, groupexpression, havingconditionsexpression, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual List<dynamic> GroupByWithHaving(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> conditionsexpression, Expression<Func<T, bool>> havingconditionsexpression, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.GroupByWithHaving(this._connectionHandler, groupexpression, conditionsexpression, havingconditionsexpression, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }


        public virtual List<dynamic> GroupBy(Expression<Func<T, Object>>[] groupexpression, OrderByModel<T> orderbymodel, bool distinct = false)
        {

            try
            {
                dynamic bo = this.BoLayer();
                return bo.GroupBy(this._connectionHandler, groupexpression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual List<dynamic> GroupBy(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderbymodel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.GroupBy(this._connectionHandler, groupexpression, conditionsexpression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual List<dynamic> GroupByWithHaving(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> havingconditionsexpression, OrderByModel<T> orderbymodel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.GroupByWithHaving(this._connectionHandler, groupexpression, havingconditionsexpression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual List<dynamic> GroupByWithHaving(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> conditionsexpression, Expression<Func<T, bool>> havingconditionsexpression, OrderByModel<T> orderbymodel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.GroupByWithHaving(this._connectionHandler, groupexpression, conditionsexpression, havingconditionsexpression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }


        public virtual List<dynamic> GroupBy(Expression<Func<T, Object>>[] groupexpression, OrderByModel<T>[] orderbymodel, bool distinct = false)
        {

            try
            {
                dynamic bo = this.BoLayer();
                return bo.GroupBy(this._connectionHandler, groupexpression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual List<dynamic> GroupBy(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderbymodel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.GroupBy(this._connectionHandler, groupexpression, conditionsexpression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual List<dynamic> GroupByWithHaving(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> havingconditionsexpression, OrderByModel<T>[] orderbymodel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.GroupByWithHaving(this._connectionHandler, groupexpression, havingconditionsexpression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual List<dynamic> GroupByWithHaving(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> conditionsexpression, Expression<Func<T, bool>> havingconditionsexpression, OrderByModel<T>[] orderbymodel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.GroupByWithHaving(this._connectionHandler, groupexpression, conditionsexpression, havingconditionsexpression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public virtual List<dynamic> GroupBy(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, bool distinct = false)
        {

            try
            {
                dynamic bo = this.BoLayer();
                return bo.GroupBy(this._connectionHandler, groupexpression, aggrigateexpression, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual List<dynamic> GroupBy(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> conditionsexpression, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.GroupBy(this._connectionHandler, groupexpression, aggrigateexpression, conditionsexpression, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual List<dynamic> GroupByWithHaving(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> havingconditionsexpression, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.GroupByWithHaving(this._connectionHandler, groupexpression, aggrigateexpression, havingconditionsexpression, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual List<dynamic> GroupByWithHaving(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> conditionsexpression, Expression<Func<T, bool>> havingconditionsexpression, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.GroupByWithHaving(this._connectionHandler, groupexpression, aggrigateexpression, conditionsexpression, havingconditionsexpression, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }


        public virtual List<dynamic> GroupBy(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, OrderByModel<T> orderbymodel, bool distinct = false)
        {

            try
            {
                dynamic bo = this.BoLayer();
                return bo.GroupBy(this._connectionHandler, groupexpression, aggrigateexpression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual List<dynamic> GroupBy(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderbymodel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.GroupBy(this._connectionHandler, groupexpression, aggrigateexpression, conditionsexpression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual List<dynamic> GroupByWithHaving(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> havingconditionsexpression, OrderByModel<T> orderbymodel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.GroupByWithHaving(this._connectionHandler, groupexpression, aggrigateexpression, havingconditionsexpression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual List<dynamic> GroupByWithHaving(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> conditionsexpression, Expression<Func<T, bool>> havingconditionsexpression, OrderByModel<T> orderbymodel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.GroupByWithHaving(this._connectionHandler, groupexpression, aggrigateexpression, conditionsexpression, havingconditionsexpression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }


        public virtual List<dynamic> GroupBy(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, OrderByModel<T>[] orderbymodel, bool distinct = false)
        {

            try
            {
                dynamic bo = this.BoLayer();
                return bo.GroupBy(this._connectionHandler, groupexpression, aggrigateexpression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual List<dynamic> GroupBy(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderbymodel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.GroupBy(this._connectionHandler, groupexpression, aggrigateexpression, conditionsexpression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual List<dynamic> GroupByWithHaving(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> havingconditionsexpression, OrderByModel<T>[] orderbymodel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.GroupByWithHaving(this._connectionHandler, groupexpression, aggrigateexpression, havingconditionsexpression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual List<dynamic> GroupByWithHaving(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> conditionsexpression, Expression<Func<T, bool>> havingconditionsexpression, OrderByModel<T>[] orderbymodel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return bo.GroupByWithHaving(this._connectionHandler, groupexpression, aggrigateexpression, conditionsexpression, havingconditionsexpression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        #endregion



        #region Async


        public virtual async Task<T> GetAsync(params object[] keys)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.GetAsync(this._connectionHandler, keys);
            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<T> SimpleGetAsync(params object[] keys)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SimpleGetAsync(this._connectionHandler, keys);
            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task GetLanuageContentAsync(string culture, T obj)
        {
            try
            {
                dynamic bo = this.BoLayer();
                await bo.GetLanuageContentAsync(this._connectionHandler, culture, obj);
            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public virtual async Task GetLanuageContentAsync(string culture, List<T> obj)
        {
            try
            {
                dynamic bo = this.BoLayer();
                await bo.GetLanuageContentAsync(this._connectionHandler, culture, obj);
            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<T> GetLanuageContentAsync(string culture, params object[] keys)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.GetLanuageContentAsync(this._connectionHandler, culture, keys);
            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<T> GetLanuageContentsimpleAsync(string culture, params object[] keys)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.GetLanuageContentsimpleAsync(this._connectionHandler, culture, keys);
            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public virtual async Task<List<T>> GetAllAsync(bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.GetAllAsync(this._connectionHandler, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }


        public virtual async Task<bool> InsertAsync(T obj, Delegates.LogMethod method, params object[] parameters)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await await bo.InsertAsync(this._connectionHandler, obj, method, parameters);

            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.SavingDataFailed, ex);
            }

        }
        public virtual async Task<bool> InsertAsync(T obj)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.InsertAsync(this._connectionHandler, obj);

            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.SavingDataFailed, ex);
            }

        }

        public virtual async Task<bool> UpdateAsync(T obj, Delegates.LogMethod method, params object[] parameters)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.UpdateAsync(this._connectionHandler, obj, method, parameters);

            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.SavingDataFailed, ex);
            }

        }
        public virtual async Task<bool> UpdateAsync(T obj)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.UpdateAsync(this._connectionHandler, obj);

            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.SavingDataFailed, ex);
            }

        }

        public virtual async Task<bool> DeleteAsync(params object[] keys)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.DeleteAsync(this._connectionHandler, keys);

            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                
                throw new KnownException(Messages.SavingDataFailed, ex);
            }
            
        }
        public virtual async Task<bool> DeleteAsync(T obj)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.DeleteAsync(this._connectionHandler, obj);

            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                
                throw new KnownException(Messages.SavingDataFailed, ex);
            }
            
        }
        public virtual async Task<bool> DeleteAsync(T obj, Delegates.LogMethod method, params object[] parameters)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.DeleteAsync(this._connectionHandler, obj, method, parameters);

            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                
                throw new KnownException(Messages.SavingDataFailed, ex);
            }
            
        }




        public virtual async Task<List<T>> WhereAsync(Expression<Func<T, bool>> expression, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.WhereAsync(this._connectionHandler, expression, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public virtual async Task<bool> AnyAsync()
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.AnyAsync(this._connectionHandler);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.AnyAsync(this._connectionHandler, expression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public virtual async Task<TResult> SumAsync<TResult>(Expression<Func<T, TResult>> expression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SumAsync(this._connectionHandler, expression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<TResult> MaxAsync<TResult>(Expression<Func<T, TResult>> expression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.MaxAsync(this._connectionHandler, expression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<TResult> MinAsync<TResult>(Expression<Func<T, TResult>> expression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.MinAsync(this._connectionHandler, expression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<double> AverageAsync<TResult>(Expression<Func<T, TResult>> expression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.AverageAsync(this._connectionHandler, expression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<TResult> SumAsync<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SumAsync(this._connectionHandler, expression, conditionsexpression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<TResult> MaxAsync<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.MaxAsync(this._connectionHandler, expression, conditionsexpression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<TResult> MinAsync<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.MinAsync(this._connectionHandler, expression, conditionsexpression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<double> AverageAsync<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.AverageAsync(this._connectionHandler, expression, conditionsexpression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public virtual async Task<int> CountAsync()
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.CountAsync(this._connectionHandler);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> expression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.CountAsync(this._connectionHandler, expression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<int> CountAsync<TColumn>(Expression<Func<T, TColumn>> expression, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.CountAsync(this._connectionHandler, expression, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<int> CountAsync<TColumn>(Expression<Func<T, TColumn>> expression, Expression<Func<T, bool>> conditionsexpression, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.CountAsync(this._connectionHandler, expression, conditionsexpression, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }





        public virtual async Task<T> FirstOrDefaultAsync(bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.FirstOrDefaultAsync(this._connectionHandler, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.FirstOrDefaultAsync(this._connectionHandler, expression, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<T> FirstOrDefaultWithOrderByAsync(OrderByModel<T>[] orderByexpression, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.FirstOrDefaultWithOrderByAsync(this._connectionHandler, orderByexpression, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<T> FirstOrDefaultWithOrderByAsync(OrderByModel<T>[] orderByexpression, Expression<Func<T, bool>> conditionsexpression, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.FirstOrDefaultWithOrderByAsync(this._connectionHandler, orderByexpression, conditionsexpression, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<T> FirstOrDefaultWithOrderByAsync<TOrderProperty>(Expression<Func<T, TOrderProperty>> orderByexpression, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.FirstOrDefaultWithOrderByAsync(this._connectionHandler, orderByexpression, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<T> FirstOrDefaultWithOrderByAsync<TOrderProperty>(Expression<Func<T, TOrderProperty>> orderByexpression, Expression<Func<T, bool>> conditionsexpression, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.FirstOrDefaultWithOrderByAsync(this._connectionHandler, orderByexpression, conditionsexpression, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<T> FirstOrDefaultWithOrderByDescendingAsync<TOrderProperty>(Expression<Func<T, TOrderProperty>> orderByexpression, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.FirstOrDefaultWithOrderByDescendingAsync(this._connectionHandler, orderByexpression, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<T> FirstOrDefaultWithOrderByDescendingAsync<TOrderProperty>(Expression<Func<T, TOrderProperty>> orderByexpression, Expression<Func<T, bool>> conditionsexpression, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.FirstOrDefaultWithOrderByDescendingAsync(this._connectionHandler, orderByexpression, conditionsexpression, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public virtual async Task<T> SingleOrDefaultAsync(bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SingleOrDefaultAsync(this._connectionHandler);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> expression, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SingleOrDefaultAsync(this._connectionHandler, expression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }



        public virtual async Task<T> IncludeFirstOrDefaultAsync(Expression<Func<T, Object>>[] Includeexpression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.IncludeFirstOrDefaultAsync(this._connectionHandler, Includeexpression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual async Task<T> IncludeFirstOrDefaultAsync(Expression<Func<T, Object>>[] Includeexpression, OrderByModel<T> orderByModel)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.IncludeFirstOrDefaultAsync(this._connectionHandler, Includeexpression, orderByModel);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual async Task<T> IncludeFirstOrDefaultAsync(Expression<Func<T, Object>>[] Includeexpression, OrderByModel<T>[] orderByModels)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.IncludeFirstOrDefaultAsync(this._connectionHandler, Includeexpression, orderByModels);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual async Task<T> IncludeFirstOrDefaultAsync(Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.IncludeFirstOrDefaultAsync(this._connectionHandler, Includeexpression, conditionsexpression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }


        }
        public virtual async Task<T> IncludeFirstOrDefaultAsync(Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderByModel)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.IncludeFirstOrDefaultAsync(this._connectionHandler, Includeexpression, conditionsexpression, orderByModel);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual async Task<T> IncludeFirstOrDefaultAsync(Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderByModels)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.IncludeFirstOrDefaultAsync(this._connectionHandler, Includeexpression, conditionsexpression, orderByModels);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }




        public virtual async Task<List<T>> IncludeTopAsync(int topcount, Expression<Func<T, Object>>[] Includeexpression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.IncludeTopAsync(this._connectionHandler, topcount, Includeexpression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual async Task<List<T>> IncludeTopAsync(int topcount, Expression<Func<T, Object>>[] Includeexpression, OrderByModel<T> orderByModel)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.IncludeTopAsync(this._connectionHandler, topcount, Includeexpression, orderByModel);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual async Task<List<T>> IncludeTopAsync(int topcount, Expression<Func<T, Object>>[] Includeexpression, OrderByModel<T>[] orderByModels)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.IncludeTopAsync(this._connectionHandler, topcount, Includeexpression, orderByModels);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual async Task<List<T>> IncludeTopAsync(int topcount, Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.IncludeTopAsync(this._connectionHandler, topcount, Includeexpression, conditionsexpression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }


        }
        public virtual async Task<List<T>> IncludeTopAsync(int topcount, Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderByModel)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.IncludeTopAsync(this._connectionHandler, topcount, Includeexpression, conditionsexpression, orderByModel);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual async Task<List<T>> IncludeTopAsync(int topcount, Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderByModels)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.IncludeTopAsync(this._connectionHandler, topcount, Includeexpression, conditionsexpression, orderByModels);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }




        public virtual async Task<List<T>> IncludePagedListAsync(int pageIndex, int pagesize, Expression<Func<T, Object>>[] Includeexpression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.IncludePagedListAsync(this._connectionHandler, pageIndex, pagesize, Includeexpression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual async Task<List<T>> IncludePagedListAsync(int pageIndex, int pagesize, Expression<Func<T, Object>>[] Includeexpression, OrderByModel<T> orderByModel)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.IncludePagedListAsync(this._connectionHandler, pageIndex, pagesize, Includeexpression, orderByModel);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual async Task<List<T>> IncludePagedListAsync(int pageIndex, int pagesize, Expression<Func<T, Object>>[] Includeexpression, OrderByModel<T>[] orderByModels)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.IncludePagedListAsync(this._connectionHandler, pageIndex, pagesize, Includeexpression, orderByModels);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual async Task<List<T>> IncludePagedListAsync(int pageIndex, int pagesize, Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.IncludePagedListAsync(this._connectionHandler, pageIndex, pagesize, Includeexpression, conditionsexpression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }


        }
        public virtual async Task<List<T>> IncludePagedListAsync(int pageIndex, int pagesize, Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderByModel)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.IncludePagedListAsync(this._connectionHandler, pageIndex, pagesize, Includeexpression, conditionsexpression, orderByModel);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual async Task<List<T>> IncludePagedListAsync(int pageIndex, int pagesize, Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderByModels)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.IncludePagedListAsync(this._connectionHandler, pageIndex, pagesize, Includeexpression, conditionsexpression, orderByModels);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }




        public virtual async Task<List<T>> IncludeAsync(Expression<Func<T, Object>>[] Includeexpression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.IncludeAsync(this._connectionHandler, Includeexpression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual async Task<List<T>> IncludeAsync(Expression<Func<T, Object>>[] Includeexpression, OrderByModel<T> orderByModel)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.IncludeAsync(this._connectionHandler, Includeexpression, orderByModel);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual async Task<List<T>> IncludeAsync(Expression<Func<T, Object>>[] Includeexpression, OrderByModel<T>[] orderByModels)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.IncludeAsync(this._connectionHandler, Includeexpression, orderByModels);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual async Task<List<T>> IncludeAsync(Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.IncludeAsync(this._connectionHandler, Includeexpression, conditionsexpression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }


        }
        public virtual async Task<List<T>> IncludeAsync(Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderByModel)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.IncludeAsync(this._connectionHandler, Includeexpression, conditionsexpression, orderByModel);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual async Task<List<T>> IncludeAsync(Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderByModels)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.IncludeAsync(this._connectionHandler, Includeexpression, conditionsexpression, orderByModels);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }



        public virtual async Task<List<T>> PagedListAsync(int pageIndex, int pagesize, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.PagedListAsync(this._connectionHandler, pageIndex, pagesize, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual async Task<List<T>> PagedListAsync(int pageIndex, int pagesize, OrderByModel<T> orderByModel, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.PagedListAsync(this._connectionHandler, pageIndex, pagesize, orderByModel, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual async Task<List<T>> PagedListAsync(int pageIndex, int pagesize, OrderByModel<T>[] orderByModels, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.PagedListAsync(this._connectionHandler, pageIndex, pagesize, orderByModels, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual async Task<List<T>> PagedListAsync(int pageIndex, int pagesize, Expression<Func<T, bool>> conditionsexpression, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.PagedListAsync(this._connectionHandler, pageIndex, pagesize, conditionsexpression, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }


        }
        public virtual async Task<List<T>> PagedListAsync(int pageIndex, int pagesize, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderByModel, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.PagedListAsync(this._connectionHandler, pageIndex, pagesize, conditionsexpression, orderByModel, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual async Task<List<T>> PagedListAsync(int pageIndex, int pagesize, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderByModels, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.PagedListAsync(this._connectionHandler, pageIndex, pagesize, conditionsexpression, orderByModels, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }



        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, bool distinct = false)
        {

            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectKeyValuePairAsync(this._connectionHandler, DataValueField, DataTextField, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }


        }
        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, Expression<Func<T, bool>> conditionsexpression, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectKeyValuePairAsync(this._connectionHandler, DataValueField, DataTextField, conditionsexpression, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderByModel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectKeyValuePairAsync(this._connectionHandler, DataValueField, DataTextField, conditionsexpression, orderByModel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderbymodels, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectKeyValuePairAsync(this._connectionHandler, DataValueField, DataTextField, conditionsexpression, orderbymodels, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, OrderByModel<T> orderByModel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectKeyValuePairAsync(this._connectionHandler, DataValueField, DataTextField, orderByModel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, OrderByModel<T>[] orderbymodels, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectKeyValuePairAsync(this._connectionHandler, DataValueField, DataTextField, orderbymodels, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }





        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, string culture, bool distinct = false)
        {

            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectKeyValuePairAsync(this._connectionHandler, DataValueField, DataTextField, culture, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }


        }
        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, string culture, Expression<Func<T, bool>> conditionsexpression, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectKeyValuePairAsync(this._connectionHandler, DataValueField, DataTextField, culture, conditionsexpression, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, string culture, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderByModel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectKeyValuePairAsync(this._connectionHandler, DataValueField, DataTextField, culture, conditionsexpression, orderByModel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, string culture, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderbymodels, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectKeyValuePairAsync(this._connectionHandler, DataValueField, DataTextField, culture, conditionsexpression, orderbymodels, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, string culture, OrderByModel<T> orderByModel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectKeyValuePairAsync(this._connectionHandler, DataValueField, DataTextField, culture, orderByModel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, string culture, OrderByModel<T>[] orderbymodels, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectKeyValuePairAsync(this._connectionHandler, DataValueField, DataTextField, culture, orderbymodels, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }



        public virtual async Task<List<T>> SelectTopAsync(int topcount, bool simpleload = false)
        {

            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectTopAsync(this._connectionHandler, topcount, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }


        }
        public virtual async Task<List<T>> SelectTopAsync(int topcount, Expression<Func<T, bool>> conditionsexpression, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectTopAsync(this._connectionHandler, topcount, conditionsexpression, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual async Task<List<T>> SelectTopAsync(int topcount, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderByModel, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectTopAsync(this._connectionHandler, topcount, conditionsexpression, orderByModel, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }

        }
        public virtual async Task<List<T>> SelectTopAsync(int topcount, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderbymodels, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectTopAsync(this._connectionHandler, topcount, conditionsexpression, orderbymodels, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<List<T>> SelectTopAsync(int topcount, OrderByModel<T> orderByModel, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectTopAsync(this._connectionHandler, topcount, orderByModel, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<List<T>> SelectTopAsync(int topcount, OrderByModel<T>[] orderbymodels, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectTopAsync(this._connectionHandler, topcount, orderbymodels, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public virtual async Task<TResult> SelectFirstOrDefaultAsync<TResult>(Expression<Func<T, TResult>> expression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectFirstOrDefaultAsync(this._connectionHandler, expression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<TResult> SelectFirstOrDefaultAsync<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectFirstOrDefaultAsync(this._connectionHandler, expression, conditionsexpression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public virtual async Task<TResult> SelectFirstOrDefaultAsync<TResult>(Expression<Func<T, TResult>> expression, OrderByModel<T> orderByModel)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectFirstOrDefaultAsync(this._connectionHandler, expression, orderByModel);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<TResult> SelectFirstOrDefaultAsync<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderByModel)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectFirstOrDefaultAsync(this._connectionHandler, expression, conditionsexpression, orderByModel);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public virtual async Task<TResult> SelectFirstOrDefaultAsync<TResult>(Expression<Func<T, TResult>> expression, OrderByModel<T>[] orderByModels)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectFirstOrDefaultAsync(this._connectionHandler, expression, orderByModels);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<TResult> SelectFirstOrDefaultAsync<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderByModels)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectFirstOrDefaultAsync(this._connectionHandler, expression, conditionsexpression, orderByModels);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }




        public virtual async Task<List<TResult>> SelectAsync<TResult>(Expression<Func<T, TResult>> expression, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectAsync(this._connectionHandler, expression, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<List<TResult>> SelectAsync<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectAsync(this._connectionHandler, expression, conditionsexpression, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public virtual async Task<List<TResult>> SelectAsync<TResult>(Expression<Func<T, TResult>> expression, OrderByModel<T> orderByModel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectAsync(this._connectionHandler, expression, orderByModel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<List<TResult>> SelectAsync<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderByModel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectAsync(this._connectionHandler, expression, conditionsexpression, orderByModel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public virtual async Task<List<TResult>> SelectAsync<TResult>(Expression<Func<T, TResult>> expression, OrderByModel<T>[] orderByModels, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectAsync(this._connectionHandler, expression, orderByModels, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<List<TResult>> SelectAsync<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderByModels, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectAsync(this._connectionHandler, expression, conditionsexpression, orderByModels, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }



        public virtual async Task<dynamic> SelectFirstOrDefaultAsync(Expression<Func<T, Object>>[] expression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectFirstOrDefaultAsync(this._connectionHandler, expression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<dynamic> SelectFirstOrDefaultAsync(Expression<Func<T, Object>>[] expression, Expression<Func<T, bool>> conditionsexpression)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectFirstOrDefaultAsync(this._connectionHandler, expression, conditionsexpression);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public virtual async Task<dynamic> SelectFirstOrDefaultAsync(Expression<Func<T, Object>>[] expression, OrderByModel<T> orderbymodel)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectFirstOrDefaultAsync(this._connectionHandler, expression, orderbymodel);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<dynamic> SelectFirstOrDefaultAsync(Expression<Func<T, Object>>[] expression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderbymodel)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectFirstOrDefaultAsync(this._connectionHandler, expression, conditionsexpression, orderbymodel);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public virtual async Task<dynamic> SelectFirstOrDefaultAsync(Expression<Func<T, Object>>[] expression, OrderByModel<T>[] orderbymodels)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectFirstOrDefaultAsync(this._connectionHandler, expression, orderbymodels);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<dynamic> SelectFirstOrDefaultAsync(Expression<Func<T, Object>>[] expression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderbymodels)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectFirstOrDefaultAsync(this._connectionHandler, expression, conditionsexpression, orderbymodels);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }



        public virtual async Task<List<dynamic>> SelectAsync(Expression<Func<T, Object>>[] expression, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectAsync(this._connectionHandler, expression, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<List<dynamic>> SelectAsync(Expression<Func<T, Object>>[] expression, Expression<Func<T, bool>> conditionsexpression, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectAsync(this._connectionHandler, expression, conditionsexpression, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public virtual async Task<List<dynamic>> SelectAsync(Expression<Func<T, Object>>[] expression, OrderByModel<T> orderbymodel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectAsync(this._connectionHandler, expression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<List<dynamic>> SelectAsync(Expression<Func<T, Object>>[] expression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderbymodel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectAsync(this._connectionHandler, expression, conditionsexpression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public virtual async Task<List<dynamic>> SelectAsync(Expression<Func<T, Object>>[] expression, OrderByModel<T>[] orderbymodels, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectAsync(this._connectionHandler, expression, orderbymodels, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<List<dynamic>> SelectAsync(Expression<Func<T, Object>>[] expression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderbymodels, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.SelectAsync(this._connectionHandler, expression, conditionsexpression, orderbymodels, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }




        public virtual async Task<List<T>> OrderByAsync(OrderByModel<T>[] expression, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.OrderByAsync(this._connectionHandler, expression, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<List<T>> OrderByAsync(OrderByModel<T>[] expression, Expression<Func<T, bool>> conditionsexpression, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.OrderByAsync(this._connectionHandler, expression, conditionsexpression, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<List<T>> OrderByAsync<TOrderProperty>(Expression<Func<T, TOrderProperty>> expression, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.OrderByAsync(this._connectionHandler, expression, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<List<T>> OrderByDescendingAsync<TOrderProperty>(Expression<Func<T, TOrderProperty>> expression, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.OrderByDescendingAsync(this._connectionHandler, expression, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<List<T>> OrderByAsync<TResult>(Expression<Func<T, TResult>> orderByexpression, Expression<Func<T, bool>> conditionsexpression, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.OrderByAsync(this._connectionHandler, orderByexpression, conditionsexpression, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<List<T>> OrderByDescendingAsync<TResult>(Expression<Func<T, TResult>> orderByexpression, Expression<Func<T, bool>> conditionsexpression, bool simpleload = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.OrderByDescendingAsync(this._connectionHandler, orderByexpression, conditionsexpression, simpleload);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }





        public virtual async Task<List<dynamic>> GroupByAsync(Expression<Func<T, Object>>[] groupexpression, bool distinct = false)
        {

            try
            {
                dynamic bo = this.BoLayer();
                return await bo.GroupByAsync(this._connectionHandler, groupexpression, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<List<dynamic>> GroupByAsync(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> conditionsexpression, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.GroupByAsync(this._connectionHandler, groupexpression, conditionsexpression, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> havingconditionsexpression, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.GroupByWithHavingAsync(this._connectionHandler, groupexpression, havingconditionsexpression, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> conditionsexpression, Expression<Func<T, bool>> havingconditionsexpression, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.GroupByWithHavingAsync(this._connectionHandler, groupexpression, conditionsexpression, havingconditionsexpression, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }


        public virtual async Task<List<dynamic>> GroupByAsync(Expression<Func<T, Object>>[] groupexpression, OrderByModel<T> orderbymodel, bool distinct = false)
        {

            try
            {
                dynamic bo = this.BoLayer();
                return await bo.GroupByAsync(this._connectionHandler, groupexpression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<List<dynamic>> GroupByAsync(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderbymodel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.GroupByAsync(this._connectionHandler, groupexpression, conditionsexpression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> havingconditionsexpression, OrderByModel<T> orderbymodel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.GroupByWithHavingAsync(this._connectionHandler, groupexpression, havingconditionsexpression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> conditionsexpression, Expression<Func<T, bool>> havingconditionsexpression, OrderByModel<T> orderbymodel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.GroupByWithHavingAsync(this._connectionHandler, groupexpression, conditionsexpression, havingconditionsexpression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }


        public virtual async Task<List<dynamic>> GroupByAsync(Expression<Func<T, Object>>[] groupexpression, OrderByModel<T>[] orderbymodel, bool distinct = false)
        {

            try
            {
                dynamic bo = this.BoLayer();
                return await bo.GroupByAsync(this._connectionHandler, groupexpression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<List<dynamic>> GroupByAsync(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderbymodel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.GroupByAsync(this._connectionHandler, groupexpression, conditionsexpression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> havingconditionsexpression, OrderByModel<T>[] orderbymodel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.GroupByWithHavingAsync(this._connectionHandler, groupexpression, havingconditionsexpression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> conditionsexpression, Expression<Func<T, bool>> havingconditionsexpression, OrderByModel<T>[] orderbymodel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.GroupByWithHavingAsync(this._connectionHandler, groupexpression, conditionsexpression, havingconditionsexpression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }

        public virtual async Task<List<dynamic>> GroupByAsync(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, bool distinct = false)
        {

            try
            {
                dynamic bo = this.BoLayer();
                return await bo.GroupByAsync(this._connectionHandler, groupexpression, aggrigateexpression, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<List<dynamic>> GroupByAsync(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> conditionsexpression, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.GroupByAsync(this._connectionHandler, groupexpression, aggrigateexpression, conditionsexpression, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> havingconditionsexpression, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.GroupByWithHavingAsync(this._connectionHandler, groupexpression, aggrigateexpression, havingconditionsexpression, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> conditionsexpression, Expression<Func<T, bool>> havingconditionsexpression, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.GroupByWithHavingAsync(this._connectionHandler, groupexpression, aggrigateexpression, conditionsexpression, havingconditionsexpression, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }


        public virtual async Task<List<dynamic>> GroupByAsync(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, OrderByModel<T> orderbymodel, bool distinct = false)
        {

            try
            {
                dynamic bo = this.BoLayer();
                return await bo.GroupByAsync(this._connectionHandler, groupexpression, aggrigateexpression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<List<dynamic>> GroupByAsync(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderbymodel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.GroupByAsync(this._connectionHandler, groupexpression, aggrigateexpression, conditionsexpression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> havingconditionsexpression, OrderByModel<T> orderbymodel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.GroupByWithHavingAsync(this._connectionHandler, groupexpression, aggrigateexpression, havingconditionsexpression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> conditionsexpression, Expression<Func<T, bool>> havingconditionsexpression, OrderByModel<T> orderbymodel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.GroupByWithHavingAsync(this._connectionHandler, groupexpression, aggrigateexpression, conditionsexpression, havingconditionsexpression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }


        public virtual async Task<List<dynamic>> GroupByAsync(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, OrderByModel<T>[] orderbymodel, bool distinct = false)
        {

            try
            {
                dynamic bo = this.BoLayer();
                return await bo.GroupByAsync(this._connectionHandler, groupexpression, aggrigateexpression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<List<dynamic>> GroupByAsync(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderbymodel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.GroupByAsync(this._connectionHandler, groupexpression, aggrigateexpression, conditionsexpression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> havingconditionsexpression, OrderByModel<T>[] orderbymodel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.GroupByWithHavingAsync(this._connectionHandler, groupexpression, aggrigateexpression, havingconditionsexpression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }
        public virtual async Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> conditionsexpression, Expression<Func<T, bool>> havingconditionsexpression, OrderByModel<T>[] orderbymodel, bool distinct = false)
        {
            try
            {
                dynamic bo = this.BoLayer();
                return await bo.GroupByWithHavingAsync(this._connectionHandler, groupexpression, aggrigateexpression, conditionsexpression, havingconditionsexpression, orderbymodel, distinct);


            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KnownException(Messages.DataFetchFailed, ex);
            }
        }


        #endregion


        internal object BoLayer()
        {
            var fullName = typeof(T).FullName;
            var obj = TypeBOClassessCache.StorageCache[fullName];
            if (obj != null) return obj;
            var data = GetBoLayer();
            TypeBOClassessCache.StorageCache.Add(fullName, data);
            return data;


        }



        private object GetBoLayer()
        {
            var ass = this.GetType().Assembly;
            var types = ass.GetTypes().Where(x => x.Name.Equals(typeof(T).Name + "BO")).ToList();
            var type = types.Count >= 1 ? types[0] : null;
            if (types.Count > 1)
            {
                var dic = types.ToDictionary(t => t, t => 0);
                var fullName = typeof(T).FullName;
                var fparts = fullName.Split('.');
                foreach (var t in types)
                {
                    var tparts = t.FullName.Split('.');
                    for (var j = fparts.Length - 1; j >= 0; j--)
                    {
                        for (var k = tparts.Length - 1; k >= 0; k--)
                        {
                            if (fparts[j].Equals(tparts[k]))
                                dic[t]++;
                        }
                    }
                }
                type = dic.OrderByDescending(x => x.Value).Select(x => x.Key).ToList()[0];
            }

            if (type == null)
                return null;
            var constructorInfo = type.GetConstructor(new Type[] { });
            return constructorInfo.Invoke(new object[] { });
        }
    }
}
