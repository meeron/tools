namespace PaginationProvider
{
    using System.Collections.Generic;

    public interface IPaginationProvider
    {
        IPagination<T> For<T>(IEnumerable<T> collection);
    }
}
