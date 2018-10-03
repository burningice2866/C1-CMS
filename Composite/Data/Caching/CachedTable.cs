﻿using System.Collections.Generic;
using System.Linq;

namespace Composite.Data.Caching
{
    /// <summary>
    /// Cached table
    /// </summary>
    internal abstract class CachedTable
    {
        /// <summary>
        /// The queryable data
        /// </summary>
        public abstract IQueryable Queryable { get; }

        internal abstract bool Add(IEnumerable<IData> dataset);

        internal abstract bool Update(IEnumerable<IData> dataset);

        internal abstract bool Remove(IEnumerable<IData> dataset);

        /// <summary>
        /// Row by key table
        /// </summary>
        public abstract IReadOnlyDictionary<object, IEnumerable<IData>> RowsByKey { get; }
    }

    internal class CachedTable<T> : CachedTable
    {
        private IReadOnlyCollection<T> _items;
        private IReadOnlyDictionary<object, IEnumerable<IData>> _rowsByKey;

        public CachedTable(IReadOnlyCollection<T> items)
        {
            _items = items;
        }

        public override IQueryable Queryable => _items.AsQueryable();


        internal override bool Add(IEnumerable<IData> dataset)
        {
            var toAdd = dataset.Cast<T>().ToList();

            lock (this)
            {
                var existingRows = _items;

                var newTable = new List<T>(existingRows.Count + toAdd.Count);
                newTable.AddRange(existingRows);
                newTable.AddRange(toAdd);

                _items = newTable;

                return true;
            }
        }


        internal override bool Update(IEnumerable<IData> dataset)
        {
            var toUpdate = dataset.ToDictionary(_ => _.DataSourceId);

            lock (this)
            {
                var existingRows = _items;


                var updated = new List<T>(existingRows.Count);

                int updatedTotal = 0;
                foreach (var data in existingRows)
                {
                    bool matched = toUpdate.TryGetValue(((IData)data).DataSourceId, out IData updatedDataItem);

                    if (matched)
                    {
                        updatedTotal++;
                    }

                    updated.Add(matched ? (T)updatedDataItem : data);
                }

                _items = updated.AsReadOnly();
                _rowsByKey = null; // Can be optimized as well

                return updatedTotal == toUpdate.Count;
            }
        }


        internal override bool Remove(IEnumerable<IData> dataset)
        {
            var toRemove = new HashSet<DataSourceId>(dataset.Select(_ => _.DataSourceId));

            lock (this)
            {
                var existingRows = _items;

                if (existingRows.Count < toRemove.Count) return false;

                var newTable = new List<T>(existingRows.Count - toRemove.Count);

                int removed = 0;
                foreach (var item in existingRows)
                {
                    if (toRemove.Contains(((IData)item).DataSourceId))
                    {
                        removed++;
                        continue;
                    }
                    newTable.Add(item);
                }
                _items = newTable;
                _rowsByKey = null; // Can be optimized as well

                return removed == toRemove.Count;
            }
        }


        public override IReadOnlyDictionary<object, IEnumerable<IData>> RowsByKey
        {
            get
            {
                var result = _rowsByKey;
                if (result != null) return result;

                lock (this)
                {
                    result = _rowsByKey;
                    if (result != null) return result;

                    var keyPropertyInfo = typeof(T).GetKeyProperties().Single();

                    result = _items
                        .GroupBy(data => keyPropertyInfo.GetValue(data, null))
                        .ToDictionary(group => group.Key, group => group.ToArray() as IEnumerable<IData>);

                    return _rowsByKey = result;
                }
            }
        }
    }
}
