using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Composite.Data.Caching;
using Composite.Core.Extensions;
using Composite.Core.Linq;


namespace Composite.Data.Foundation
{
    internal sealed class DataFacadeQueryable<T> : IQueryable<T>, IOrderedQueryable<T>, IDataFacadeQueryable, IQueryProvider
    {
        private readonly List<IQueryable> _sources;
        private readonly Expression _currentExpression;
        private readonly Expression _initialExpression;



        public DataFacadeQueryable(IEnumerable<IQueryable<T>> sources)
        {
            Verify.ArgumentNotNull(sources, "sources");

            _sources = new List<IQueryable>();
            foreach (IQueryable<T> source in sources)
            {
                _sources.Add(source);
            }

            _initialExpression = Expression.Constant(this);
            _currentExpression = _initialExpression;
        }


        /// <summary>
        /// Invoked via reflection
        /// </summary>
        public DataFacadeQueryable(List<IQueryable> sources, Expression currentExpression)
        {
            _sources = sources;
            _currentExpression = currentExpression;
        }



        public IQueryable<S> CreateQuery<S>(Expression expression)
        {
            Verify.ArgumentNotNull(expression, "expression");
            Verify.ArgumentCondition(typeof(IQueryable<S>).IsAssignableFrom(expression.Type), "expression", "Incorrect expression type");

            return new DataFacadeQueryable<S>(_sources, expression);
        }



        public S Execute<S>(Expression expression)
        {
            bool pullIntoMemory = ShouldBePulledIntoMemory(expression);

            var handleInProviderVisitor = new DataFacadeQueryableExpressionVisitor(pullIntoMemory);

            Expression newExpression = handleInProviderVisitor.Visit(expression);

            IQueryable source = handleInProviderVisitor.Queryable;

            return (S)source.Provider.Execute(newExpression);
        }



        public IEnumerator<T> GetEnumerator()
        {
            if(object.Equals(_currentExpression, _initialExpression))
            {
                if(_sources.Count == 1 && _sources[0] is IEnumerable<T>)
                {
                    return (_sources[0] as IEnumerable<T>).GetEnumerator();
                }

                if (_sources.Count == 0)
                {
                    return new List<T>().GetEnumerator();
                }
            }

            bool pullIntoMemory = ShouldBePulledIntoMemory(_currentExpression);
            var handleInProviderVisitor = new DataFacadeQueryableExpressionVisitor(pullIntoMemory);

            Expression newExpression = handleInProviderVisitor.Visit(_currentExpression);

            // Checking if the source contains queries both from SQL and Composite.Data.Caching.CachingQueryable in-memory queries. 
            // In this case, we can replace CachingQueryable instances with sql queries which allows building correct sql statements.
            var analyzer = new QueryableAnalyzerVisitor();
            analyzer.Visit(newExpression);

            if(analyzer.CachedSqlQueries > 1
               || (analyzer.SqlQueries > 0 && analyzer.CachedSqlQueries > 0))
            {
                newExpression = new CachedQueryExtractorVisitor().Visit(newExpression);
                newExpression = handleInProviderVisitor.Visit(newExpression);
            }

            // Executing the query
            IQueryable source = handleInProviderVisitor.Queryable;
            IQueryable<T> queryable = (IQueryable<T>)source.Provider.CreateQuery(newExpression);

            return queryable.GetEnumerator();
        }



        IEnumerator IEnumerable.GetEnumerator()
        {
            MethodInfo methodInfo = DataFacadeReflectionCache.GetDataFacadeQueryableGetEnumeratorMethodInfo(typeof(T));         

            return (IEnumerator)methodInfo.Invoke(this, null);
        }



        public IQueryable CreateQuery(Expression expression)
        {
            if (_currentExpression == expression) return this;

            Type elementType = TypeHelpers.FindElementType(expression);

            Type multibleSourceQueryableType = typeof(DataFacadeQueryable<>).MakeGenericType(new Type[] { elementType });

            return Activator.CreateInstance(
                multibleSourceQueryableType,
                new object[] { _sources, expression }) as IQueryable;
        }



        public Type ElementType
        {
            get { return typeof(T); }
        }



        public object Execute(Expression expression)
        {
            MethodInfo methodInfo = DataFacadeReflectionCache.GetDataFacadeQueryableExecuteMethodInfo(typeof(T), expression.Type);

            return methodInfo.Invoke(this, new object[] { expression });
        }



        public Expression Expression
        {
            get
            {
                if (_currentExpression != null)
                {
                    return _currentExpression;
                }
                
                return Expression.Constant(this);
            }
        }



        public IEnumerable<IQueryable> Sources
        {
            get
            {
                foreach (IQueryable queryable in _sources)
                {
                    yield return queryable;
                }
            }
        }


        private bool ShouldBePulledIntoMemory(Expression expression)
        {
            var analyzer = new QueryableAnalyzerVisitor();
            analyzer.Visit(expression);

            return analyzer.MethodsNotSupportedBySql > 0;
        }


        public bool IsEnumerableQuery
        {
            get
            {
                return _sources.Count == 1
                       && (_sources[0] as IQueryable<T>).IsEnumerableQuery();
            }
        }

        public IQueryProvider Provider
        {
            get { return this; }
        }


        private class QueryableAnalyzerVisitor : ExpressionVisitor
        {
            public int SqlQueries { get; private set; }
            public int CachedSqlQueries { get; private set; }
            public int InMemoryQueries { get; private set; }
            public int MethodsNotSupportedBySql { get; private set; }
            public int Other { get; private set; }

            protected override Expression VisitConstant(ConstantExpression node)
            {
                var value = node.Value;
                if (value is IQueryable)
                {
                    if (value is ITable)
                    {
                        SqlQueries++;
                    }
                    else if (value is ICachedQuery)
                    {
                        CachedSqlQueries++;
                    }
                    else if (value is EnumerableQuery)
                    {
                        InMemoryQueries++;
                    }
                    else
                    {
                        Other++;
                    }
                }

                return base.VisitConstant(node);
            }

            protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
            {
                // Checking for C# anonymous functions
                if(methodCallExpression.Method.ReflectedType.FullName.Contains("+<>"))
                {
                    MethodsNotSupportedBySql++;
                }

                return base.VisitMethodCall(methodCallExpression);
            }
        }

        private class CachedQueryExtractorVisitor : ExpressionVisitor
        {
            protected override Expression VisitConstant(ConstantExpression node)
            {
                object value = node.Value;
                if(value is ICachedQuery)
                {
                    IQueryable originalQuery = (value as ICachedQuery).GetOriginalQuery();

                    return Expression.Constant(originalQuery);
                }

                return base.VisitConstant(node);
            }
        }
    }
}
