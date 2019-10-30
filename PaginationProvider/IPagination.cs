namespace PaginationProvider
{
    using System;
    using Models;

    public interface IPagination<T>
    {
        IPagination<T> WithDefaultSort<TKey>(Func<T, TKey> keySelector);

        PaginationResult<T> Apply(Pagination pagination);
    }
}
