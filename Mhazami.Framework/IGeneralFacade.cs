using Mhazami.Framework.Difinition;
using System.Linq.Expressions;


namespace Mhazami.Framework
{
    public interface IBaseFacade<T> where T : class
    {
        #region Manual

        List<T> GetAll(bool simpleload = false);
        T Get(params object[] keys);
        T SimpleGet(params object[] keys);

        T GetLanuageContent(string culture, params object[] keys);
        T GetLanuageContentsimple(string culture, params object[] keys);
        void GetLanuageContent(string culture, T obj);
        void GetLanuageContent(string culture, List<T> obj);

        bool Insert(T obj);
        bool Insert(T obj, Delegates.LogMethod method, params object[] parameters);

        bool Update(T obj);
        bool Update(T obj, Delegates.LogMethod method, params object[] parameters);

        bool Delete(params object[] keys);
        bool Delete(T obj);
        bool Delete(T obj, Delegates.LogMethod method, params object[] parameters);


        List<T> Where(Expression<Func<T, bool>> expression, bool simpleload = false);


        bool Any();
        bool Any(Expression<Func<T, bool>> expression);




        TResult Sum<TResult>(Expression<Func<T, TResult>> expression);
        TResult Max<TResult>(Expression<Func<T, TResult>> expression);
        TResult Min<TResult>(Expression<Func<T, TResult>> expression);
        double Average<TResult>(Expression<Func<T, TResult>> expression);
        TResult Sum<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression);
        TResult Max<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression);
        TResult Min<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression);
        double Average<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression);

        int Count();
        int Count(Expression<Func<T, bool>> expression);
        int Count<TColumn>(Expression<Func<T, TColumn>> expression, bool distinct = false);
        int Count<TColumn>(Expression<Func<T, TColumn>> expression, Expression<Func<T, bool>> conditionsexpression, bool distinct = false);


        T FirstOrDefault(bool simpleload = false);
        T FirstOrDefault(Expression<Func<T, bool>> expression, bool simpleload = false);
        T FirstOrDefaultWithOrderBy<TOrderProperty>(Expression<Func<T, TOrderProperty>> orderByexpression, bool simpleload = false);
        T FirstOrDefaultWithOrderBy<TOrderProperty>(Expression<Func<T, TOrderProperty>> orderByexpression, Expression<Func<T, bool>> conditionsexpression, bool simpleload = false);
        T FirstOrDefaultWithOrderByDescending<TOrderProperty>(Expression<Func<T, TOrderProperty>> orderByexpression, bool simpleload = false);
        T FirstOrDefaultWithOrderByDescending<TOrderProperty>(Expression<Func<T, TOrderProperty>> orderByexpression, Expression<Func<T, bool>> conditionsexpression, bool simpleload = false);
        T FirstOrDefaultWithOrderBy(OrderByModel<T>[] orderByexpression, bool simpleload = false);
        T FirstOrDefaultWithOrderBy(OrderByModel<T>[] orderByexpression, Expression<Func<T, bool>> conditionsexpression, bool simpleload = false);

        T SingleOrDefault(bool simpleload = false);
        T SingleOrDefault(Expression<Func<T, bool>> expression, bool simpleload = false);

        T IncludeFirstOrDefault(Expression<Func<T, Object>>[] Includeexpression);
        T IncludeFirstOrDefault(Expression<Func<T, Object>>[] Includeexpression, OrderByModel<T> orderByModel);
        T IncludeFirstOrDefault(Expression<Func<T, Object>>[] Includeexpression, OrderByModel<T>[] orderByModels);
        T IncludeFirstOrDefault(Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression);
        T IncludeFirstOrDefault(Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderByModel);
        T IncludeFirstOrDefault(Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderByModels);





        List<T> Include(Expression<Func<T, Object>>[] Includeexpression);
        List<T> Include(Expression<Func<T, Object>>[] Includeexpression, OrderByModel<T> orderByModel);
        List<T> Include(Expression<Func<T, Object>>[] Includeexpression, OrderByModel<T>[] orderByModels);
        List<T> Include(Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression);
        List<T> Include(Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderByModel);
        List<T> Include(Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderByModels);



        List<T> IncludePagedList(int pageIndex, int pagesize, Expression<Func<T, Object>>[] Includeexpression);
        List<T> IncludePagedList(int pageIndex, int pagesize, Expression<Func<T, Object>>[] Includeexpression, OrderByModel<T> orderByModel);
        List<T> IncludePagedList(int pageIndex, int pagesize, Expression<Func<T, Object>>[] Includeexpression, OrderByModel<T>[] orderByModels);
        List<T> IncludePagedList(int pageIndex, int pagesize, Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression);
        List<T> IncludePagedList(int pageIndex, int pagesize, Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderByModel);
        List<T> IncludePagedList(int pageIndex, int pagesize, Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderByModels);



        List<T> IncludeTop(int topcount, Expression<Func<T, Object>>[] Includeexpression);
        List<T> IncludeTop(int topcount, Expression<Func<T, Object>>[] Includeexpression, OrderByModel<T> orderByModel);
        List<T> IncludeTop(int topcount, Expression<Func<T, Object>>[] Includeexpression, OrderByModel<T>[] orderByModels);
        List<T> IncludeTop(int topcount, Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression);
        List<T> IncludeTop(int topcount, Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderByModel);
        List<T> IncludeTop(int topcount, Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderByModels);


        List<T> PagedList(int pageIndex, int pagesize, bool simpleload = false);
        List<T> PagedList(int pageIndex, int pagesize, OrderByModel<T> orderByModel, bool simpleload = false);
        List<T> PagedList(int pageIndex, int pagesize, OrderByModel<T>[] orderByModels, bool simpleload = false);
        List<T> PagedList(int pageIndex, int pagesize, Expression<Func<T, bool>> conditionsexpression, bool simpleload = false);
        List<T> PagedList(int pageIndex, int pagesize, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderByModel, bool simpleload = false);
        List<T> PagedList(int pageIndex, int pagesize, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderByModels, bool simpleload = false);


        List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, bool distinct = false);
        List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, Expression<Func<T, bool>> conditionsexpression, bool distinct = false);
        List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderbymodel, bool distinct = false);
        List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderbymodels, bool distinct = false);
        List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, OrderByModel<T> orderbymodel, bool distinct = false);
        List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, OrderByModel<T>[] orderbymodels, bool distinct = false);



        List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, string culture, bool distinct = false);
        List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, string culture, Expression<Func<T, bool>> conditionsexpression, bool distinct = false);
        List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, string culture, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderbymodel, bool distinct = false);
        List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, string culture, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderbymodels, bool distinct = false);
        List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, string culture, OrderByModel<T> orderbymodel, bool distinct = false);
        List<KeyValuePair<string, string>> SelectKeyValuePair(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, string culture, OrderByModel<T>[] orderbymodels, bool distinct = false);


        List<T> SelectTop(int topcount, bool simpleload = false);
        List<T> SelectTop(int topcount, Expression<Func<T, bool>> conditionsexpression, bool simpleload = false);
        List<T> SelectTop(int topcount, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderByModel, bool simpleload = false);
        List<T> SelectTop(int topcount, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderbymodels, bool simpleload = false);
        List<T> SelectTop(int topcount, OrderByModel<T> orderByModel, bool simpleload = false);
        List<T> SelectTop(int topcount, OrderByModel<T>[] orderbymodels, bool simpleload = false);


        TResult SelectFirstOrDefault<TResult>(Expression<Func<T, TResult>> expression);
        TResult SelectFirstOrDefault<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression);

        TResult SelectFirstOrDefault<TResult>(Expression<Func<T, TResult>> expression, OrderByModel<T> orderbymodel);
        TResult SelectFirstOrDefault<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderbymodel);

        TResult SelectFirstOrDefault<TResult>(Expression<Func<T, TResult>> expression, OrderByModel<T>[] orderbymodels);
        TResult SelectFirstOrDefault<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderbymodels);



        List<TResult> Select<TResult>(Expression<Func<T, TResult>> expression, bool distinct = false);
        List<TResult> Select<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression, bool distinct = false);

        List<TResult> Select<TResult>(Expression<Func<T, TResult>> expression, OrderByModel<T> orderbymodel, bool distinct = false);
        List<TResult> Select<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderbymodel, bool distinct = false);

        List<TResult> Select<TResult>(Expression<Func<T, TResult>> expression, OrderByModel<T>[] orderbymodels, bool distinct = false);
        List<TResult> Select<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderbymodels, bool distinct = false);



        dynamic SelectFirstOrDefault(Expression<Func<T, Object>>[] expression);
        dynamic SelectFirstOrDefault(Expression<Func<T, Object>>[] expression, Expression<Func<T, bool>> conditionsexpression);

        dynamic SelectFirstOrDefault(Expression<Func<T, Object>>[] expression, OrderByModel<T> orderbymodel);
        dynamic SelectFirstOrDefault(Expression<Func<T, Object>>[] expression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderbymodel);

        dynamic SelectFirstOrDefault(Expression<Func<T, Object>>[] expression, OrderByModel<T>[] orderbymodels);
        dynamic SelectFirstOrDefault(Expression<Func<T, Object>>[] expression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderbymodels);



        List<dynamic> Select(Expression<Func<T, Object>>[] expression, bool distinct = false);
        List<dynamic> Select(Expression<Func<T, Object>>[] expression, Expression<Func<T, bool>> conditionsexpression, bool distinct = false);

        List<dynamic> Select(Expression<Func<T, Object>>[] expression, OrderByModel<T> orderbymodel, bool distinct = false);
        List<dynamic> Select(Expression<Func<T, Object>>[] expression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderbymodel, bool distinct = false);

        List<dynamic> Select(Expression<Func<T, Object>>[] expression, OrderByModel<T>[] orderbymodels, bool distinct = false);
        List<dynamic> Select(Expression<Func<T, Object>>[] expression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderbymodels, bool distinct = false);







        List<T> OrderBy(OrderByModel<T>[] expression, bool simpleload = false);
        List<T> OrderBy(OrderByModel<T>[] expression, Expression<Func<T, bool>> conditionsexpression, bool simpleload = false);
        List<T> OrderBy<TOrderProperty>(Expression<Func<T, TOrderProperty>> orderByexpression, bool simpleload = false);
        List<T> OrderByDescending<TOrderProperty>(Expression<Func<T, TOrderProperty>> orderByexpression, bool simpleload = false);
        List<T> OrderBy<TOrderProperty>(Expression<Func<T, TOrderProperty>> orderByexpression, Expression<Func<T, bool>> conditionsexpression, bool simpleload = false);
        List<T> OrderByDescending<TOrderProperty>(Expression<Func<T, TOrderProperty>> orderByexpression, Expression<Func<T, bool>> conditionsexpression, bool simpleload = false);









        List<dynamic> GroupBy(Expression<Func<T, Object>>[] groupexpression, bool distinct = false);
        List<dynamic> GroupBy(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> conditionsexpression, bool distinct = false);
        List<dynamic> GroupByWithHaving(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> havingconditionsexpression, bool distinct = false);
        List<dynamic> GroupByWithHaving(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> conditionsexpression, Expression<Func<T, bool>> havingconditionsexpression, bool distinct = false);

        List<dynamic> GroupBy(Expression<Func<T, Object>>[] groupexpression, OrderByModel<T> orderbymodel, bool distinct = false);
        List<dynamic> GroupBy(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderbymodel, bool distinct = false);
        List<dynamic> GroupByWithHaving(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> havingconditionsexpression, OrderByModel<T> orderbymodel, bool distinct = false);
        List<dynamic> GroupByWithHaving(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> conditionsexpression, Expression<Func<T, bool>> havingconditionsexpression, OrderByModel<T> orderbymodel, bool distinct = false);


        List<dynamic> GroupBy(Expression<Func<T, Object>>[] groupexpression, OrderByModel<T>[] orderbymodel, bool distinct = false);
        List<dynamic> GroupBy(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderbymodel, bool distinct = false);
        List<dynamic> GroupByWithHaving(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> havingconditionsexpression, OrderByModel<T>[] orderbymodel, bool distinct = false);
        List<dynamic> GroupByWithHaving(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> conditionsexpression, Expression<Func<T, bool>> havingconditionsexpression, OrderByModel<T>[] orderbymodel, bool distinct = false);

        List<dynamic> GroupBy(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, bool distinct = false);
        List<dynamic> GroupBy(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> conditionsexpression, bool distinct = false);
        List<dynamic> GroupByWithHaving(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> havingconditionsexpression, bool distinct = false);
        List<dynamic> GroupByWithHaving(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> conditionsexpression, Expression<Func<T, bool>> havingconditionsexpression, bool distinct = false);

        List<dynamic> GroupBy(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, OrderByModel<T> orderbymodel, bool distinct = false);
        List<dynamic> GroupBy(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderbymodel, bool distinct = false);
        List<dynamic> GroupByWithHaving(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> havingconditionsexpression, OrderByModel<T> orderbymodel, bool distinct = false);
        List<dynamic> GroupByWithHaving(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> conditionsexpression, Expression<Func<T, bool>> havingconditionsexpression, OrderByModel<T> orderbymodel, bool distinct = false);


        List<dynamic> GroupBy(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, OrderByModel<T>[] orderbymodel, bool distinct = false);
        List<dynamic> GroupBy(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderbymodel, bool distinct = false);
        List<dynamic> GroupByWithHaving(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> havingconditionsexpression, OrderByModel<T>[] orderbymodel, bool distinct = false);
        List<dynamic> GroupByWithHaving(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> conditionsexpression, Expression<Func<T, bool>> havingconditionsexpression, OrderByModel<T>[] orderbymodel, bool distinct = false);

        #endregion


        #region Async

        Task<List<T>> GetAllAsync(bool simpleload = false);
        Task<T> GetAsync(params object[] keys);
        Task<T> SimpleGetAsync(params object[] keys);

        Task<T> GetLanuageContentAsync(string culture, params object[] keys);
        Task<T> GetLanuageContentsimpleAsync(string culture, params object[] keys);
        Task GetLanuageContentAsync(string culture, T obj);
        Task GetLanuageContentAsync(string culture, List<T> obj);

        Task<bool> InsertAsync(T obj);
        Task<bool> InsertAsync(T obj, Delegates.LogMethod method, params object[] parameters);

        Task<bool> UpdateAsync(T obj);
        Task<bool> UpdateAsync(T obj, Delegates.LogMethod method, params object[] parameters);

        Task<bool> DeleteAsync(params object[] keys);
        Task<bool> DeleteAsync(T obj);
        Task<bool> DeleteAsync(T obj, Delegates.LogMethod method, params object[] parameters);


        Task<List<T>> WhereAsync(Expression<Func<T, bool>> expression, bool simpleload = false);


        Task<bool> AnyAsync();
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);




        Task<TResult> SumAsync<TResult>(Expression<Func<T, TResult>> expression);
        Task<TResult> MaxAsync<TResult>(Expression<Func<T, TResult>> expression);
        Task<TResult> MinAsync<TResult>(Expression<Func<T, TResult>> expression);
        Task<double> AverageAsync<TResult>(Expression<Func<T, TResult>> expression);
        Task<TResult> SumAsync<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression);
        Task<TResult> MaxAsync<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression);
        Task<TResult> MinAsync<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression);
        Task<double> AverageAsync<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression);

        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> expression);
        Task<int> CountAsync<TColumn>(Expression<Func<T, TColumn>> expression, bool distinct = false);
        Task<int> CountAsync<TColumn>(Expression<Func<T, TColumn>> expression, Expression<Func<T, bool>> conditionsexpression, bool distinct = false);


        Task<T> FirstOrDefaultAsync(bool simpleload = false);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression, bool simpleload = false);
        Task<T> FirstOrDefaultWithOrderByAsync<TOrderProperty>(Expression<Func<T, TOrderProperty>> orderByexpression, bool simpleload = false);
        Task<T> FirstOrDefaultWithOrderByAsync<TOrderProperty>(Expression<Func<T, TOrderProperty>> orderByexpression, Expression<Func<T, bool>> conditionsexpression, bool simpleload = false);
        Task<T> FirstOrDefaultWithOrderByDescendingAsync<TOrderProperty>(Expression<Func<T, TOrderProperty>> orderByexpression, bool simpleload = false);
        Task<T> FirstOrDefaultWithOrderByDescendingAsync<TOrderProperty>(Expression<Func<T, TOrderProperty>> orderByexpression, Expression<Func<T, bool>> conditionsexpression, bool simpleload = false);
        Task<T> FirstOrDefaultWithOrderByAsync(OrderByModel<T>[] orderByexpression, bool simpleload = false);
        Task<T> FirstOrDefaultWithOrderByAsync(OrderByModel<T>[] orderByexpression, Expression<Func<T, bool>> conditionsexpression, bool simpleload = false);

        Task<T> SingleOrDefaultAsync(bool simpleload = false);
        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> expression, bool simpleload = false);

        Task<T> IncludeFirstOrDefaultAsync(Expression<Func<T, Object>>[] Includeexpression);
        Task<T> IncludeFirstOrDefaultAsync(Expression<Func<T, Object>>[] Includeexpression, OrderByModel<T> orderByModel);
        Task<T> IncludeFirstOrDefaultAsync(Expression<Func<T, Object>>[] Includeexpression, OrderByModel<T>[] orderByModels);
        Task<T> IncludeFirstOrDefaultAsync(Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression);
        Task<T> IncludeFirstOrDefaultAsync(Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderByModel);
        Task<T> IncludeFirstOrDefaultAsync(Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderByModels);





        Task<List<T>> IncludeAsync(Expression<Func<T, Object>>[] Includeexpression);
        Task<List<T>> IncludeAsync(Expression<Func<T, Object>>[] Includeexpression, OrderByModel<T> orderByModel);
        Task<List<T>> IncludeAsync(Expression<Func<T, Object>>[] Includeexpression, OrderByModel<T>[] orderByModels);
        Task<List<T>> IncludeAsync(Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression);
        Task<List<T>> IncludeAsync(Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderByModel);
        Task<List<T>> IncludeAsync(Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderByModels);



        Task<List<T>> IncludePagedListAsync(int pageIndex, int pagesize, Expression<Func<T, Object>>[] Includeexpression);
        Task<List<T>> IncludePagedListAsync(int pageIndex, int pagesize, Expression<Func<T, Object>>[] Includeexpression, OrderByModel<T> orderByModel);
        Task<List<T>> IncludePagedListAsync(int pageIndex, int pagesize, Expression<Func<T, Object>>[] Includeexpression, OrderByModel<T>[] orderByModels);
        Task<List<T>> IncludePagedListAsync(int pageIndex, int pagesize, Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression);
        Task<List<T>> IncludePagedListAsync(int pageIndex, int pagesize, Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderByModel);
        Task<List<T>> IncludePagedListAsync(int pageIndex, int pagesize, Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderByModels);



        Task<List<T>> IncludeTopAsync(int topcount, Expression<Func<T, Object>>[] Includeexpression);
        Task<List<T>> IncludeTopAsync(int topcount, Expression<Func<T, Object>>[] Includeexpression, OrderByModel<T> orderByModel);
        Task<List<T>> IncludeTopAsync(int topcount, Expression<Func<T, Object>>[] Includeexpression, OrderByModel<T>[] orderByModels);
        Task<List<T>> IncludeTopAsync(int topcount, Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression);
        Task<List<T>> IncludeTopAsync(int topcount, Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderByModel);
        Task<List<T>> IncludeTopAsync(int topcount, Expression<Func<T, Object>>[] Includeexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderByModels);


        Task<List<T>> PagedListAsync(int pageIndex, int pagesize, bool simpleload = false);
        Task<List<T>> PagedListAsync(int pageIndex, int pagesize, OrderByModel<T> orderByModel, bool simpleload = false);
        Task<List<T>> PagedListAsync(int pageIndex, int pagesize, OrderByModel<T>[] orderByModels, bool simpleload = false);
        Task<List<T>> PagedListAsync(int pageIndex, int pagesize, Expression<Func<T, bool>> conditionsexpression, bool simpleload = false);
        Task<List<T>> PagedListAsync(int pageIndex, int pagesize, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderByModel, bool simpleload = false);
        Task<List<T>> PagedListAsync(int pageIndex, int pagesize, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderByModels, bool simpleload = false);


        Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, Expression<Func<T, bool>> conditionsexpression, bool distinct = false);
        Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderbymodel, bool distinct = false);
        Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderbymodels, bool distinct = false);
        Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, OrderByModel<T> orderbymodel, bool distinct = false);
        Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, OrderByModel<T>[] orderbymodels, bool distinct = false);



        Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, string culture, bool distinct = false);
        Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, string culture, Expression<Func<T, bool>> conditionsexpression, bool distinct = false);
        Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, string culture, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderbymodel, bool distinct = false);
        Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, string culture, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderbymodels, bool distinct = false);
        Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, string culture, OrderByModel<T> orderbymodel, bool distinct = false);
        Task<List<KeyValuePair<string, string>>> SelectKeyValuePairAsync(Expression<Func<T, Object>> DataValueField, Expression<Func<T, Object>> DataTextField, string culture, OrderByModel<T>[] orderbymodels, bool distinct = false);


        Task<List<T>> SelectTopAsync(int topcount, bool simpleload = false);
        Task<List<T>> SelectTopAsync(int topcount, Expression<Func<T, bool>> conditionsexpression, bool simpleload = false);
        Task<List<T>> SelectTopAsync(int topcount, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderByModel, bool simpleload = false);
        Task<List<T>> SelectTopAsync(int topcount, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderbymodels, bool simpleload = false);
        Task<List<T>> SelectTopAsync(int topcount, OrderByModel<T> orderByModel, bool simpleload = false);
        Task<List<T>> SelectTopAsync(int topcount, OrderByModel<T>[] orderbymodels, bool simpleload = false);


        Task<TResult> SelectFirstOrDefaultAsync<TResult>(Expression<Func<T, TResult>> expression);
        Task<TResult> SelectFirstOrDefaultAsync<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression);

        Task<TResult> SelectFirstOrDefaultAsync<TResult>(Expression<Func<T, TResult>> expression, OrderByModel<T> orderbymodel);
        Task<TResult> SelectFirstOrDefaultAsync<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderbymodel);

        Task<TResult> SelectFirstOrDefaultAsync<TResult>(Expression<Func<T, TResult>> expression, OrderByModel<T>[] orderbymodels);
        Task<TResult> SelectFirstOrDefaultAsync<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderbymodels);



        Task<List<TResult>> SelectAsync<TResult>(Expression<Func<T, TResult>> expression, bool distinct = false);
        Task<List<TResult>> SelectAsync<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression, bool distinct = false);
        Task<List<TResult>> SelectAsync<TResult>(Expression<Func<T, TResult>> expression, OrderByModel<T> orderbymodel, bool distinct = false);
        Task<List<TResult>> SelectAsync<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderbymodel, bool distinct = false);
        Task<List<TResult>> SelectAsync<TResult>(Expression<Func<T, TResult>> expression, OrderByModel<T>[] orderbymodels, bool distinct = false);
        Task<List<TResult>> SelectAsync<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderbymodels, bool distinct = false);



        Task<dynamic> SelectFirstOrDefaultAsync(Expression<Func<T, Object>>[] expression);
        Task<dynamic> SelectFirstOrDefaultAsync(Expression<Func<T, Object>>[] expression, Expression<Func<T, bool>> conditionsexpression);
        Task<dynamic> SelectFirstOrDefaultAsync(Expression<Func<T, Object>>[] expression, OrderByModel<T> orderbymodel);
        Task<dynamic> SelectFirstOrDefaultAsync(Expression<Func<T, Object>>[] expression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderbymodel);
        Task<dynamic> SelectFirstOrDefaultAsync(Expression<Func<T, Object>>[] expression, OrderByModel<T>[] orderbymodels);
        Task<dynamic> SelectFirstOrDefaultAsync(Expression<Func<T, Object>>[] expression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderbymodels);



        Task<List<dynamic>> SelectAsync(Expression<Func<T, Object>>[] expression, bool distinct = false);
        Task<List<dynamic>> SelectAsync(Expression<Func<T, Object>>[] expression, Expression<Func<T, bool>> conditionsexpression, bool distinct = false);
        Task<List<dynamic>> SelectAsync(Expression<Func<T, Object>>[] expression, OrderByModel<T> orderbymodel, bool distinct = false);
        Task<List<dynamic>> SelectAsync(Expression<Func<T, Object>>[] expression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderbymodel, bool distinct = false);
        Task<List<dynamic>> SelectAsync(Expression<Func<T, Object>>[] expression, OrderByModel<T>[] orderbymodels, bool distinct = false);
        Task<List<dynamic>> SelectAsync(Expression<Func<T, Object>>[] expression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderbymodels, bool distinct = false);







        Task<List<T>> OrderByAsync(OrderByModel<T>[] expression, bool simpleload = false);
        Task<List<T>> OrderByAsync(OrderByModel<T>[] expression, Expression<Func<T, bool>> conditionsexpression, bool simpleload = false);
        Task<List<T>> OrderByAsync<TOrderProperty>(Expression<Func<T, TOrderProperty>> orderByexpression, bool simpleload = false);
        Task<List<T>> OrderByDescendingAsync<TOrderProperty>(Expression<Func<T, TOrderProperty>> orderByexpression, bool simpleload = false);
        Task<List<T>> OrderByAsync<TOrderProperty>(Expression<Func<T, TOrderProperty>> orderByexpression, Expression<Func<T, bool>> conditionsexpression, bool simpleload = false);
        Task<List<T>> OrderByDescendingAsync<TOrderProperty>(Expression<Func<T, TOrderProperty>> orderByexpression, Expression<Func<T, bool>> conditionsexpression, bool simpleload = false);









        Task<List<dynamic>> GroupByAsync(Expression<Func<T, Object>>[] groupexpression, bool distinct = false);
        Task<List<dynamic>> GroupByAsync(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> conditionsexpression, bool distinct = false);
        Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> havingconditionsexpression, bool distinct = false);
        Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> conditionsexpression, Expression<Func<T, bool>> havingconditionsexpression, bool distinct = false);

        Task<List<dynamic>> GroupByAsync(Expression<Func<T, Object>>[] groupexpression, OrderByModel<T> orderbymodel, bool distinct = false);
        Task<List<dynamic>> GroupByAsync(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderbymodel, bool distinct = false);
        Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> havingconditionsexpression, OrderByModel<T> orderbymodel, bool distinct = false);
        Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> conditionsexpression, Expression<Func<T, bool>> havingconditionsexpression, OrderByModel<T> orderbymodel, bool distinct = false);


        Task<List<dynamic>> GroupByAsync(Expression<Func<T, Object>>[] groupexpression, OrderByModel<T>[] orderbymodel, bool distinct = false);
        Task<List<dynamic>> GroupByAsync(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderbymodel, bool distinct = false);
        Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> havingconditionsexpression, OrderByModel<T>[] orderbymodel, bool distinct = false);
        Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<T, Object>>[] groupexpression, Expression<Func<T, bool>> conditionsexpression, Expression<Func<T, bool>> havingconditionsexpression, OrderByModel<T>[] orderbymodel, bool distinct = false);

        Task<List<dynamic>> GroupByAsync(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, bool distinct = false);
        Task<List<dynamic>> GroupByAsync(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> conditionsexpression, bool distinct = false);
        Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> havingconditionsexpression, bool distinct = false);
        Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> conditionsexpression, Expression<Func<T, bool>> havingconditionsexpression, bool distinct = false);

        Task<List<dynamic>> GroupByAsync(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, OrderByModel<T> orderbymodel, bool distinct = false);
        Task<List<dynamic>> GroupByAsync(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T> orderbymodel, bool distinct = false);
        Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> havingconditionsexpression, OrderByModel<T> orderbymodel, bool distinct = false);
        Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> conditionsexpression, Expression<Func<T, bool>> havingconditionsexpression, OrderByModel<T> orderbymodel, bool distinct = false);


        Task<List<dynamic>> GroupByAsync(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, OrderByModel<T>[] orderbymodel, bool distinct = false);
        Task<List<dynamic>> GroupByAsync(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> conditionsexpression, OrderByModel<T>[] orderbymodel, bool distinct = false);
        Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> havingconditionsexpression, OrderByModel<T>[] orderbymodel, bool distinct = false);
        Task<List<dynamic>> GroupByWithHavingAsync(Expression<Func<T, Object>>[] groupexpression, GroupByModel<T>[] aggrigateexpression, Expression<Func<T, bool>> conditionsexpression, Expression<Func<T, bool>> havingconditionsexpression, OrderByModel<T>[] orderbymodel, bool distinct = false);

        #endregion
    }
}
