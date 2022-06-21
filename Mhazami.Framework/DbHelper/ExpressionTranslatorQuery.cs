using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Mhazami.Framework;
using Mhazami.Utility;
using Convert = System.Convert;

namespace Mhazami.Framework.DbHelper
{

    internal sealed class ExpressionTranslatorQuery : ExpressionVisitor
    {
        private QueryBuilder QueryBuilder;
        private DbCommand DbCommand;
        private FetchDataHelper FetchDataHelper;
        public KeyValuePair<string, string>[] TranslateSelectMultiColumnsExpression<TDataStructure>(DbCommand dbCommand, FetchDataHelper model, Expression<Func<TDataStructure, object>>[] columeExpressions, bool CheckIsNullInAggrigateColumn = false)
        {

            var selectcolumn = new List<KeyValuePair<string, string>>();
            foreach (var colum in columeExpressions)
            {
                var expression = this.TranslateSelectColumnExpression(dbCommand, model, colum, CheckIsNullInAggrigateColumn);
                selectcolumn.Add(expression);
            }
            return selectcolumn.ToArray();
        }
        internal string TranslateConditionExpression(DbCommand dbCommand, FetchDataHelper fetchDataHelper, Expression expression)
        {
            return this.TranslateExpression(dbCommand, fetchDataHelper, expression, false);
        }
        internal KeyValuePair<string, string> TranslateSelectColumnExpression(DbCommand dbCommand, FetchDataHelper fetchDataHelper, Expression expression, bool CheckIsNullInAggrigateColumn = false)
        {
            return new KeyValuePair<string, string>(this.TranslateExpression(dbCommand, fetchDataHelper, expression, true, CheckIsNullInAggrigateColumn), String.Join("And", QueryBuilder.ColumnNames));
        }

        private string TranslateExpression(DbCommand dbCommand, FetchDataHelper fetchDataHelper, Expression expression, bool selectcolumn, bool CheckIsNullInAggrigateColumn = false)
        {
            FetchDataHelper = fetchDataHelper;
            DbCommand = dbCommand;
            this.QueryBuilder = new QueryBuilder { Selectcolumn = selectcolumn };
            if (selectcolumn) this.QueryBuilder.CheckIsNullInAggrigateColumn = CheckIsNullInAggrigateColumn;
            this.Visit(expression);
            return this.QueryBuilder.QueryTranslate.ToString();

        }
        public override Expression Visit(Expression node)
        {
            try
            {
                var visit = base.Visit(node);
                QueryBuilder.HasEqualBinaryExpression = false;
                QueryBuilder.HasLikeExpression = false;
                return visit;
            }
            catch (KnownException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new KnownException("Error In Translate Expression in Radyn Framework ");
            }

        }



        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var expr = (node.Object is ParameterExpression)
            ? Expression.Default(node.Object.Type)
            : node.Object;
            try
            {
                InvokeDynamicMethod(node, expr);
            }
            catch (Exception exception)
            {

                InvokeImplementMethods(node, expr);
            }
            return node;

        }

        private void InvokeImplementMethods(MethodCallExpression node, Expression expr)
        {

            var names = Enum.GetNames(typeof(ExpressionMethods));
            if (!names.Contains(node.Method.Name))
                throw new KnownException(" the Method Format not supported by Radyn Framework !!!");

            if (expr != null && expr.Type == typeof(bool))
                QueryBuilder.HasEqualBinaryExpression = true;
            var arguments = node.Arguments;
            var expressionMethods = node.Method.Name.ToEnum<ExpressionMethods>();
            switch (expressionMethods)
            {
                case ExpressionMethods.Contains:
                case ExpressionMethods.StartsWith:
                case ExpressionMethods.EndsWith:
                    {
                        if (!arguments.Any())
                            throw new KnownException(" Please Enter Method Arguments.. ");
                        dynamic expression = arguments[0];
                        QueryBuilder.Append("(");
                        this.Visit(expr);
                        QueryBuilder.Append(string.Format(" like N'{0}",
                            (expressionMethods != ExpressionMethods.EndsWith ? "%" : "")));
                        QueryBuilder.HasLikeExpression = true;
                        this.Visit(expression);
                        QueryBuilder.Append(string.Format("{0}'",
                            (expressionMethods != ExpressionMethods.StartsWith ? "%" : "")));
                        QueryBuilder.Append(")");
                        return;
                    }
                case ExpressionMethods.Equals:
                    {
                        if (!arguments.Any())
                            throw new KnownException(" Please Enter Method Arguments.. ");
                        dynamic nodes = arguments[0];
                        var binaryExpression = System.Linq.Expressions.Expression.MakeBinary(ExpressionType.Equal, expr ?? node, nodes);
                        this.Visit(binaryExpression);
                        return;
                    }
                case ExpressionMethods.CompareTo:
                    {
                        if (!arguments.Any())
                            throw new KnownException(" Please Enter Method Arguments.. ");
                        dynamic expression = arguments[0];
                        this.Visit(expr);
                        QueryBuilder.CompareExpertion = expression;
                        return;
                    }
                case ExpressionMethods.Compare:
                    {
                        if (!arguments.Any())
                            throw new KnownException(" Please Enter Method Arguments.. ");
                        if (arguments.Count != 2)
                            throw new KnownException(" the format not supported by radyn framework !!!");
                        dynamic expression = arguments[0];
                        this.Visit(expression);
                        dynamic expression2 = arguments[1];
                        QueryBuilder.CompareExpertion = expression2;
                        return;
                    }
                case ExpressionMethods.IsNullOrEmpty:
                    {
                        if (!arguments.Any())
                            throw new KnownException(" Please Enter Method Arguments.. ");
                        dynamic expression = arguments[0];
                        var binaryExpression = System.Linq.Expressions.Expression.MakeBinary(ExpressionType.Equal, expression,
                            System.Linq.Expressions.Expression.Constant(string.Empty));
                        this.Visit(binaryExpression);
                        return;
                    }
                case ExpressionMethods.ToLower:
                    {
                        QueryBuilder.Append("lower(");
                        this.Visit(expr);
                        QueryBuilder.Append(")");
                        return;
                    }
                case ExpressionMethods.ToInt:
                    {
                        if (!arguments.Any())
                            throw new KnownException(" Please Enter Method Arguments.. ");
                        if (arguments.Count != 1)
                            throw new KnownException(" the format not supported by radyn framework !!!");
                        QueryBuilder.Append("CONVERT(Int,");
                        this.Visit(arguments[0]);
                        QueryBuilder.Append(")");
                        return;
                    }
                case ExpressionMethods.ToLong:
                    {
                        if (!arguments.Any())
                            throw new KnownException(" Please Enter Method Arguments.. ");
                        if (arguments.Count != 1)
                            throw new KnownException(" the format not supported by radyn framework !!!");
                        QueryBuilder.Append("CONVERT(bigint,");
                        this.Visit(arguments[0]);
                        QueryBuilder.Append(")");
                        return;
                    }
                case ExpressionMethods.ToShort:
                    {
                        if (!arguments.Any())
                            throw new KnownException(" Please Enter Method Arguments.. ");
                        if (arguments.Count != 1)
                            throw new KnownException(" the format not supported by radyn framework !!!");
                        QueryBuilder.Append("CONVERT(smallint,");
                        this.Visit(arguments[0]);
                        QueryBuilder.Append(")");
                        return;
                    }
                case ExpressionMethods.ToByte:
                    {
                        if (!arguments.Any())
                            throw new KnownException(" Please Enter Method Arguments.. ");
                        if (arguments.Count != 1)
                            throw new KnownException(" the format not supported by radyn framework !!!");
                        QueryBuilder.Append("CONVERT(tinyint,");
                        this.Visit(arguments[0]);
                        QueryBuilder.Append(")");

                        return;
                    }
                case ExpressionMethods.ToDouble:
                    {
                        if (!arguments.Any())
                            throw new KnownException(" Please Enter Method Arguments.. ");
                        if (arguments.Count != 1)
                            throw new KnownException(" the format not supported by radyn framework !!!");
                        QueryBuilder.Append("CONVERT(float,");
                        this.Visit(arguments[0]);
                        QueryBuilder.Append(")");

                        return;
                    }
                case ExpressionMethods.ToDecimal:
                    {
                        if (!arguments.Any())
                            throw new KnownException(" Please Enter Method Arguments.. ");
                        if (arguments.Count != 1)
                            throw new KnownException(" the format not supported by radyn framework !!!");
                        QueryBuilder.Append("CONVERT(decimal,");
                        this.Visit(arguments[0]);
                        QueryBuilder.Append(")");

                        return;
                    }
                case ExpressionMethods.ToFloat:
                    {
                        if (!arguments.Any())
                            throw new KnownException(" Please Enter Method Arguments.. ");
                        if (arguments.Count != 1)
                            throw new KnownException(" the format not supported by radyn framework !!!");
                        QueryBuilder.Append("CONVERT(float,");
                        this.Visit(arguments[0]);
                        QueryBuilder.Append(")");

                        return;
                    }
                case ExpressionMethods.ToBool:
                    {
                        if (!arguments.Any())
                            throw new KnownException(" Please Enter Method Arguments.. ");
                        if (arguments.Count != 1)
                            throw new KnownException(" the format not supported by radyn framework !!!");
                        QueryBuilder.Append("CONVERT(bit,");
                        this.Visit(arguments[0]);
                        QueryBuilder.Append(")");

                        return;
                    }
                case ExpressionMethods.ToGuid:
                    {
                        if (!arguments.Any())
                            throw new KnownException(" Please Enter Method Arguments.. ");
                        if (arguments.Count != 1)
                            throw new KnownException(" the format not supported by radyn framework !!!");
                        QueryBuilder.Append("CONVERT(uniqueidentifier,");
                        this.Visit(arguments[0]);
                        QueryBuilder.Append(")");

                        return;
                    }
                case ExpressionMethods.ToDateTime:
                    {
                        if (!arguments.Any())
                            throw new KnownException(" Please Enter Method Arguments.. ");
                        if (arguments.Count != 1)
                            throw new KnownException(" the format not supported by radyn framework !!!");
                        QueryBuilder.Append("CONVERT(datetime,");
                        this.Visit(arguments[0]);
                        QueryBuilder.Append(")");

                        return;
                    }
                case ExpressionMethods.ToUpper:
                    {
                        QueryBuilder.Append("Upper(");
                        this.Visit(expr);
                        QueryBuilder.Append(")");
                        return;
                    }
                case ExpressionMethods.Year:
                    {
                        if (!arguments.Any())
                            throw new KnownException(" Please Enter Method Arguments.. ");
                        if (arguments.Count != 1)
                            throw new KnownException(" the format not supported by radyn framework !!!");
                        QueryBuilder.Append("year(");
                        this.Visit(arguments[0]);
                        QueryBuilder.Append(")");
                        return;
                    }
                case ExpressionMethods.Month:
                    {
                        if (!arguments.Any())
                            throw new KnownException(" Please Enter Method Arguments.. ");
                        if (arguments.Count != 1)
                            throw new KnownException(" the format not supported by radyn framework !!!");
                        QueryBuilder.Append("Month(");
                        this.Visit(arguments[0]);
                        QueryBuilder.Append(")");
                        return;
                    }
                case ExpressionMethods.Day:
                    {
                        if (!arguments.Any())
                            throw new KnownException(" Please Enter Method Arguments.. ");
                        if (arguments.Count != 1)
                            throw new KnownException(" the format not supported by radyn framework !!!");
                        QueryBuilder.Append("Day(");
                        this.Visit(arguments[0]);
                        QueryBuilder.Append(")");
                        return;
                    }
                case ExpressionMethods.Trim:
                    {
                        QueryBuilder.Append("RTRIM(");
                        this.Visit(expr);
                        QueryBuilder.Append(")");
                        return;
                    }
                case ExpressionMethods.ToString:
                    {
                        QueryBuilder.Append("cast(");
                        this.Visit(expr);
                        QueryBuilder.Append(" as nvarchar(4000))");
                        return;
                    }
                case ExpressionMethods.ToShortDateString:
                    {
                        QueryBuilder.Append("cast(");
                        this.Visit(expr);
                        QueryBuilder.Append(" as Date)");
                        return;
                    }

                case ExpressionMethods.Length:
                    {
                        if (!arguments.Any())
                            throw new KnownException(" Please Enter Method Arguments.. ");
                        if (arguments.Count != 1)
                            throw new KnownException(" the format not supported by radyn framework !!!");
                        QueryBuilder.Append("LEN(");
                        this.Visit(arguments[0]);
                        QueryBuilder.Append(")");
                        return;
                    }

                case ExpressionMethods.IndexOf:
                    {
                        if (!arguments.Any())
                            throw new KnownException(" Please Enter Method Arguments.. ");
                        if (arguments.Count != 1)
                            throw new KnownException(" the format not supported by radyn framework !!!");
                        QueryBuilder.Append("CHARINDEX(");
                        this.Visit(arguments[0]);
                        QueryBuilder.Append(",");
                        this.Visit(expr);
                        QueryBuilder.Append(")");
                        return;
                    }
                case ExpressionMethods.Substring:
                    {
                        if (!arguments.Any())
                            throw new KnownException(" Please Enter Method Arguments.. ");
                        if (arguments.Count != 2)
                            throw new KnownException(" the format not supported by radyn framework !!!");
                        QueryBuilder.Append("SUBSTRING(");
                        this.Visit(expr);
                        QueryBuilder.Append(",");
                        var argument0 = DynamicInvoke(arguments[0]);
                        QueryBuilder.Append(argument0.ToString().ToInt() == 0 ? 1 : argument0.ToString().ToInt() + 1);
                        QueryBuilder.Append(",");
                        QueryBuilder.Append(DynamicInvoke(arguments[1]));
                        QueryBuilder.Append(")");
                        return;
                    }
                case ExpressionMethods.In:
                case ExpressionMethods.NotIn:
                    {
                        if (!arguments.Any())
                            throw new KnownException(" Please Enter Method Arguments.. ");
                        if (arguments.Count != 2)
                            throw new KnownException(" the format not supported by radyn framework !!!");
                        dynamic expression = arguments[0];
                        dynamic itemsexpression = arguments[1];
                        QueryBuilder.Append("(");
                        this.Visit(expression);
                        QueryBuilder.Append(string.Format("  {0} IN (",
                            expressionMethods == ExpressionMethods.NotIn ? " NOT " : ""));
                        this.Visit(itemsexpression);
                        QueryBuilder.Append(")");
                        QueryBuilder.Append(")");
                        return;
                    }

            }


        }

        protected override Expression VisitUnary(UnaryExpression u)
        {
            switch (u.NodeType)
            {
                case ExpressionType.Not:
                    QueryBuilder.Append(" NOT ");
                    this.Visit(u.Operand);
                    break;
                case ExpressionType.Convert:
                    this.Visit(u.Operand);
                    break;
                default:
                    throw new KnownException(string.Format("The unary operator '{0}' is not supported", u.NodeType));
            }
            return u;
        }



        protected override Expression VisitBinary(BinaryExpression binaryExpression)
        {
            QueryBuilder.HasEqualBinaryExpression = binaryExpression.NodeType == ExpressionType.Equal ||
                                                    binaryExpression.NodeType == ExpressionType.NotEqual;
            QueryBuilder.Append("(");
            this.Visit(binaryExpression.Left);
            switch (binaryExpression.NodeType)
            {
                case ExpressionType.Coalesce:
                    QueryBuilder.Append(")");
                    return binaryExpression;
                case ExpressionType.Add:
                    QueryBuilder.Append(" + ");
                    break;
                case ExpressionType.Subtract:
                    QueryBuilder.Append(" - ");
                    break;
                case ExpressionType.Multiply:
                    QueryBuilder.Append(" * ");
                    break;
                case ExpressionType.Divide:
                    QueryBuilder.Append(" / ");
                    break;
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    QueryBuilder.Append(" AND ");
                    break;
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    QueryBuilder.Append(" OR ");
                    break;
                case ExpressionType.Equal:
                    QueryBuilder.Append(IsNullConstant(binaryExpression.Right) ? " IS " : " = ");
                    break;
                case ExpressionType.NotEqual:
                    QueryBuilder.Append(IsNullConstant(binaryExpression.Right) ? " IS NOT " : " <> ");
                    break;
                case ExpressionType.LessThan:
                    QueryBuilder.Append(" < ");
                    break;
                case ExpressionType.LessThanOrEqual:
                    QueryBuilder.Append(" <= ");
                    break;
                case ExpressionType.GreaterThan:
                    QueryBuilder.Append(" > ");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    QueryBuilder.Append(" >= ");
                    break;
                default:
                    throw new KnownException(string.Format("The binary operator '{0}' is not supported", binaryExpression.NodeType));

            }
            var compareExpertion = QueryBuilder.CompareExpertion;
            QueryBuilder.CompareExpertion = null;
            this.Visit(compareExpertion ?? binaryExpression.Right);
            QueryBuilder.Append(")");
            return binaryExpression;
        }



        protected override Expression VisitConstant(ConstantExpression c)
        {
            WriteInquery(new SetValueModel() { Value = c.Value, Type = c.Type });
            return c;
        }

        protected override Expression VisitMember(MemberExpression m)
        {
            var expression = m.Expression ?? m;
            if (expression.NodeType == ExpressionType.Parameter)
            {
                this.VisitParameterExpression(m);
                return m;
            }
            try
            {
              
                var result = DynamicInvoke(expression);
                var memeberVal = GetMemeberVal(m.Member, result);
                this.WriteInquery(result == null ? null : new SetValueModel() { Value = memeberVal, Type = m.Type });
            }
            catch
            {
                this.VisitParameterExpression(m);
            }
            return m;



        }

        #region Utility

        private void InvokeDynamicMethod(MethodCallExpression node, Expression expr)
        {

            var invokeobj = DynamicInvoke(expr ?? node);
            if (invokeobj == null) return;
            var readOnlyCollection = node.Arguments;
            var objects = new List<Object>();
            foreach (var expression in readOnlyCollection)
            {
                dynamic readOnly = expression;
                var result = DynamicInvoke(readOnly);
                if (result != null)
                    objects.Add(result);
            }
            var obj = node.Method.Invoke(invokeobj, objects.ToArray());
            if (obj != null)
                WriteInquery(new SetValueModel() { Value = obj, Type = obj.GetType() });


        }

        private void WriteInquery(SetValueModel model)
        {

            if (model.IsParamets)
            {
                QueryBuilder.Append((QueryBuilder.CheckIsNullInAggrigateColumn&& QueryBuilder.Selectcolumn) ? string.Format(" ISNULL({0}, 0) ", model.Value) : model.Value);
                return;
            }
            var value = model.Value;
            var queryable = value as IQueryable;
            if (queryable != null) return;
            if (value == null)
            {
                QueryBuilder.Append("NULL");
                return;
            }
            var type = model.Type.GetTypeValidValue();
            if (Utils.IsValueType(type))
            {
                if (type.IsEnum)
                {
                    try
                    {
                        var enumvalue = Convert.ToInt32((Enum.Parse(type, value.ToString())));
                        QueryBuilder.Append(DbCommand.GenerateNewParameter(type, enumvalue));
                    }
                    catch
                    {
                    }

                    return;
                }
                if (type == typeof(Guid))
                {
                    QueryBuilder.Append(DbCommand.GenerateNewParameter(type, (Guid)value));
                    return;
                }
                switch (Type.GetTypeCode(type))
                {

                    case TypeCode.String:
                        var val = value.ToString();
                        if (QueryBuilder.HasLikeExpression)
                        {

                            QueryBuilder.Append("'+" + DbCommand.GenerateNewParameter(type, val) + "+'");
                            return;
                        }
                        QueryBuilder.Append(DbCommand.GenerateNewParameter(type, val));
                        return;
                    default:
                        QueryBuilder.Append(DbCommand.GenerateNewParameter(type, value));
                        return;


                }

            }

            var enumerable = (value as IEnumerable);
            if (enumerable != null)
            {
                var substring = "";
                var array = ((IEnumerable)value).Cast<Object>().ToArray();
                if (array.Any())
                {

                    foreach (var obj in array)
                    {
                        if (!string.IsNullOrEmpty(substring)) substring += ",";
                        var objval = obj;
                        var enumType = obj.GetType().GetTypeValidValue();
                        if (enumType.IsEnum)
                        {
                            try
                            {
                                objval = Convert.ToInt32((Enum.Parse(enumType, obj.ToString()))).ToString();

                            }
                            catch
                            {


                            }
                        }

                        substring += "N'" + objval + "'";
                    }


                }
                else
                    substring += " null ";

                QueryBuilder.Append(substring);

            }



        }

        private object DynamicInvoke(Expression m)
        {
            return Expression.Lambda(m).Compile().DynamicInvoke();
        }
        private object GetMemeberVal(MemberInfo m, object invoke)
        {

            if (m is FieldInfo)
                return ((FieldInfo)m).GetValue(invoke);
            if (m is PropertyInfo)
                return ((PropertyInfo)m).GetValue(invoke, null);
            return null;

        }
        private void VisitParameterExpression(MemberExpression m)
        {

            if (VisitFrameworkMethodExpression(m)) return;
            if (QueryBuilder.HasEqualBinaryExpression)
            {
                this.VisitParameter(m);
                return;
            }
            if (((m.Member.Name.ToLower() == "hasvalue") && m.Member.DeclaringType != null &&
                 m.Member.DeclaringType.FullName.ToLower().Contains("system.nullable")))
            {
                var binaryExpression = Expression.MakeBinary(ExpressionType.NotEqual, m.Expression, Expression.Constant(null));
                this.Visit(binaryExpression);
                return;
            }
            if (m.Type == typeof(bool) & !QueryBuilder.Selectcolumn)
            {
                var binaryExpression = Expression.Equal(m, Expression.Constant(true));
                this.Visit(binaryExpression);
                return;
            }
            this.VisitParameter(m);
        }




        private void VisitParameter(MemberExpression m)
        {
            var proccessExceptions = new PrepareExpressionModel();
            PrepareProccessMemberAccessList(m, proccessExceptions);
            var key = GenerateKeyFromPrepareProccessList(proccessExceptions);
            this.WriteInquery(new SetValueModel() { Value = key, IsParamets = true });
        }

        private string GenerateKeyFromPrepareProccessList(PrepareExpressionModel prepareModel)
        {

            var prop = prepareModel.ProccessExceptions.OrderBy(x => x.Index).FirstOrDefault();
            if (prop == null || prop.PropertyInfo == null) return String.Empty;
            var tableNameKey = "";
            Type parentType = null;
            var proccessExceptions = prepareModel.ProccessExceptions.OrderByDescending(x => x.Index).ToList();
            foreach (var proccessException in proccessExceptions)
            {
                if (parentType != null && proccessException.PropertyInfo != null)
                {
                    var assosiation = proccessException.PropertyInfo.GetPropertyAttributes().Assosiation;
                    if (assosiation != null)
                    {
                        if (string.IsNullOrEmpty(assosiation.PropName) || !assosiation.FillData)
                            throw new KnownException(String.Format(" propery:  {0} is Disable Select Assosiation ",
                                proccessException.PropertyInfo.Name));
                        tableNameKey = parentType.GetTableKey(tableNameKey, proccessException.PropertyInfo);
                    }
                }
                else
                    tableNameKey = proccessException.Type.GetTableKey();

                parentType = proccessException.Type;
                if (FetchDataHelper.FilterAssosation && !FetchDataHelper.SelectedAssosiation.Contains(tableNameKey))
                    FetchDataHelper.SelectedAssosiation.Add(tableNameKey);
            }
            var columnfullname = "";
            var hasmultilang = prop.PropertyInfo.GetPropertyAttributes().MultiLanguage;
            if (hasmultilang != null)
            {
                var type = typeof(LanguageContent);
                var tableKey = type.GetTableKey(tableNameKey, prop.PropertyInfo);
                columnfullname = String.Format("[{0}].[Value]", tableKey);
                if (!FetchDataHelper.SelectedMultiLangAssosiation.ContainsKey(tableKey))
                    FetchDataHelper.SelectedMultiLangAssosiation.Add(tableKey, hasmultilang.FillAnyLanguage);

            }
            else
                columnfullname = String.Format("[{0}].[{1}]", tableNameKey, prop.PropertyInfo.Name);
            if (QueryBuilder.Selectcolumn && !QueryBuilder.ColumnNames.Contains(prop.PropertyInfo.Name))
                QueryBuilder.ColumnNames.Add(prop.PropertyInfo.Name);
            return columnfullname;
        }
        private void PrepareProccessMemberAccessList(Expression memberExpression, PrepareExpressionModel dictionary, int? counter = null)
        {
            counter = counter ?? 1;
            if (memberExpression == null) return;
            var ismemebreaccess = memberExpression.NodeType == ExpressionType.MemberAccess;
            var proccessException = new ProccessException() { Type = memberExpression.Type, Index = (int)counter };
            if (ismemebreaccess)
            {
                dynamic membertype = memberExpression;
                var member = membertype.Member;
                if (member != null && member is PropertyInfo)
                    proccessException.PropertyInfo = member;
            }
            if (ValidateProccessException(proccessException))
            {
                dictionary.ProccessExceptions.Add(proccessException);
                counter++;
            }
            if (ismemebreaccess)
            {
                dynamic ex = memberExpression;
                PrepareProccessMemberAccessList(ex.Expression, dictionary, counter);
            }


        }

        private bool ValidateProccessException(ProccessException proccessException)
        {
            if (proccessException.PropertyInfo == null || proccessException.PropertyInfo.DeclaringType == null)
                return true;
            if ((proccessException.PropertyInfo.Name.ToLower() == "value" ||
                 proccessException.PropertyInfo.Name.ToLower() == "hasvalue") &&
                proccessException.PropertyInfo.DeclaringType.FullName.ToLower().Contains("system.nullable"))
                return false;
            return proccessException.PropertyInfo.DeclaringType != typeof(DateTime) &&
                   proccessException.PropertyInfo.DeclaringType != typeof(string);
        }

        protected bool IsNullConstant(Expression exp)
        {
            if (exp.NodeType == ExpressionType.Constant)
                return ((ConstantExpression)exp).Value == null;
            try
            {
                return DynamicInvoke(exp) == null;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        #region FrameworkMethod

        private bool VisitFrameworkMethodExpression(MemberExpression m)
        {
            if (((m.Member.Name.ToLower() == ExpressionMethods.Length.ToString().ToLower()) && m.Member.DeclaringType != null &&
                 m.Member.DeclaringType == typeof(string)))
            {
                var newMethod = GetType()
                    .GetMethod(ExpressionMethods.Length.ToString(), BindingFlags.Static | BindingFlags.Public);
                if (newMethod == null || m.Expression == null)
                    throw new KnownException(String.Format(" Method {0} not found in Assembly", m.Member.Name));
                var newExpression = Expression.Call(newMethod, m.Expression);
                this.Visit(newExpression);
                return true;
            }
            if (((m.Member.Name.ToLower() == ExpressionMethods.Year.ToString().ToLower()) && m.Member.DeclaringType != null &&
                  m.Member.DeclaringType == typeof(DateTime)))
            {
                var newMethod = GetType()
                    .GetMethod(ExpressionMethods.Year.ToString(), BindingFlags.Static | BindingFlags.Public);
                if (newMethod == null || m.Expression == null)
                    throw new KnownException(String.Format(" Method {0} not found in Assembly", m.Member.Name));
                var newExpression = Expression.Call(newMethod, m.Expression);
                this.Visit(newExpression);
                return true;
            }
            if (((m.Member.Name.ToLower() == ExpressionMethods.Month.ToString().ToLower()) && m.Member.DeclaringType != null &&
                 m.Member.DeclaringType == typeof(DateTime)))
            {
                var newMethod = GetType()
                    .GetMethod(ExpressionMethods.Month.ToString(), BindingFlags.Static | BindingFlags.Public);
                if (newMethod == null || m.Expression == null)
                    throw new KnownException(String.Format(" Method {0} not found in Assembly", m.Member.Name));
                var newExpression = Expression.Call(newMethod, m.Expression);
                this.Visit(newExpression);
                return true;
            }
            if (((m.Member.Name.ToLower() == ExpressionMethods.Day.ToString().ToLower()) && m.Member.DeclaringType != null &&
                 m.Member.DeclaringType == typeof(DateTime)))
            {
                var newMethod = GetType().GetMethod(ExpressionMethods.Day.ToString(), BindingFlags.Static | BindingFlags.Public);
                if (newMethod == null || m.Expression == null)
                    throw new KnownException(String.Format(" Method {0} not found in Assembly", m.Member.Name));
                var newExpression = Expression.Call(newMethod, m.Expression);
                this.Visit(newExpression);
                return true;
            }
            return false;
        }
        public static int Year(DateTime dateTime)
        {
            return dateTime.Year;
        }
        public static int Month(DateTime dateTime)
        {
            return dateTime.Month;
        }
        public static int Day(DateTime dateTime)
        {
            return dateTime.Day;
        }
        public static int Length(string obj)
        {
            return obj.Length;
        }
        #endregion
    }

    internal class PrepareExpressionModel
    {
        private List<ProccessException> _proccessExceptions;
        internal List<ProccessException> ProccessExceptions
        {
            get
            {
                return this._proccessExceptions ?? (this._proccessExceptions = new List<ProccessException>());
            }
        }
    }
    class ProccessException
    {
        internal int Index { get; set; }
        internal PropertyInfo PropertyInfo { get; set; }
        internal Type Type { get; set; }

    }
    class SetValueModel
    {

        internal bool IsParamets { get; set; }
        internal object Value { get; set; }
        internal Type Type { get; set; }




    }
    class QueryBuilder
    {
        internal QueryBuilder()
        {
            QueryTranslate = new StringBuilder();
            ColumnNames = new List<string>();
        }


        internal Expression CompareExpertion { get; set; }
        internal StringBuilder QueryTranslate { get; set; }
        internal List<string> ColumnNames { get; set; }
        internal bool HasEqualBinaryExpression { get; set; }
        internal bool Selectcolumn { get; set; }
        internal bool CheckIsNullInAggrigateColumn { get; set; }
        internal bool HasLikeExpression { get; set; }



        internal void Append(object Value)
        {
            this.QueryTranslate.Append(Value);
        }



    }

}
