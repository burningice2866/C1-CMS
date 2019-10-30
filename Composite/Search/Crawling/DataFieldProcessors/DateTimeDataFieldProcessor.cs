using System;
using System.Globalization;
using System.Reflection;

namespace Composite.Search.Crawling.DataFieldProcessors
{
    /// <summary>
    /// The default field processor for <see cref="DateTime"/> fields.
    /// </summary>
    public class DateTimeDataFieldProcessor: DefaultDataFieldProcessor
    {
        /// <exclude />
        public override object GetIndexValue(object fieldValue)
        {
            return ((DateTime?) fieldValue)?.ToString("s");
        }

        /// <exclude />
        protected override DocumentFieldPreview.ValuePreviewDelegate GetPreviewFunction(PropertyInfo propertyInfo)
        {
            return value =>
            {
                if (value == null) return null;
                if (DateTime.TryParseExact((string)value, "s", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                {
                    return date.ToString("yyyy MMM d");
                }
                return (string)value;
            };  
        }

        /// <exclude />
        public override string[] GetFacetValues(object value)
        {
            var str = ((DateTime?) value)?.ToString("yyyy MM");

            return str != null ? new [] {str} : null;
        }

        /// <exclude />
        protected override DocumentFieldFacet.FacetValuePreviewDelegate GetFacetValuePreviewFunction(PropertyInfo propertyInfo)
        {
            return value =>
            {
                if (value == null) return null;
                var parts = value.Split(' ');

                int month = int.Parse(parts[1]);

                var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
                return $"{parts[0]} {monthName}";
            };
        }
    }
}
