using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

using Composite.Functions;

namespace Composite.Plugins.Functions.FunctionProviders.FileBasedFunctionProvider
{
    /// <exclude />
    public class ParameterWidgets : IEnumerable<KeyValuePair<PropertyInfo, WidgetFunctionProvider>>
    {
        private readonly IDictionary<PropertyInfo, WidgetFunctionProvider> _inner;

        /// <exclude />
        public ParameterWidgets()
        {
            _inner = new Dictionary<PropertyInfo, WidgetFunctionProvider>();
        }

        /// <exclude />
        public void Add(Expression<Func<object>> key, WidgetFunctionProvider value)
        {
            MemberExpression memberExpression;

            switch (key.Body.NodeType)
            {
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    var unaryExpression = (UnaryExpression)key.Body;
                    memberExpression = (MemberExpression)unaryExpression.Operand;

                    break;

                default:
                    memberExpression = key.Body as MemberExpression;

                    break;
            }

            var propInfo = (PropertyInfo)memberExpression.Member;

            Add(propInfo, value);
        }

        /// <exclude />
        public void Add(PropertyInfo key, WidgetFunctionProvider value)
        {
            _inner.Add(key, value);
        }

        /// <exclude />
        public bool ContainsKey(PropertyInfo key)
        {
            return _inner.ContainsKey(key);
        }

        /// <exclude />
        public WidgetFunctionProvider this[PropertyInfo key]
        {
            get { return _inner[key]; }
            set { _inner[key] = value; }
        }

        /// <exclude />
        public IEnumerator<KeyValuePair<PropertyInfo, WidgetFunctionProvider>> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _inner.GetEnumerator();
        }
    }
}
