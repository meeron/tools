namespace PaginationProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Models;

    internal class DefaultPagination<T> : IPagination<T>
    {
        private const int DefaultPage = 1;

        private const int MinItemsPerPage = 5;

        private IEnumerable<T> _collection;

        internal DefaultPagination(IEnumerable<T> collection)
        {
            _collection = collection;
        }

        public IPagination<T> WithDefaultSort<TKey>(Func<T, TKey> keySelector)
        {
            _collection = _collection.OrderBy(keySelector);
            return this;
        }

        public PaginationResult<T> Apply(Pagination pagination)
        {
            if (pagination == null)
            {
                return new PaginationResult<T> { Items = _collection };
            }

            var itemsPerPage = Math.Max(pagination.ItemsPerPage, MinItemsPerPage);
            var page = Math.Max(pagination.Page, DefaultPage);

            return new PaginationResult<T>
            {
                TotalItems = _collection.Count(),
                Items = OrderBy(_collection, pagination.Sort, pagination.SortDir)
                    .Skip(itemsPerPage * (page - 1))
                    .Take(itemsPerPage)
            };
        }

        private static IEnumerable<T> OrderBy(IEnumerable<T> collection, string propertyName, string sortDirection)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                return collection;
            }

            var keySelector = ToLambda(propertyName)?.Compile();
            if (keySelector != null)
            {
                return sortDirection?.Equals("desc", StringComparison.OrdinalIgnoreCase) ?? false
                    ? collection.OrderByDescending(keySelector)
                    : collection.OrderBy(keySelector);
            }

            return collection;
        }

        private static Expression<Func<T, object>> ToLambda(string propertyName)
        {
            if (typeof(T).GetProperty(propertyName) == null)
            {
                return null;
            }

            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
        }
    }
}
