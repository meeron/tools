namespace PaginationProvider
{
    using System.Collections.Generic;
    using Infrastructure;

    [SingletonService]
    public class PaginationProvider : IPaginationProvider
    {
        public IPagination<T> For<T>(IEnumerable<T> collection) =>
            new DefaultPagination<T>(collection);
    }
}
