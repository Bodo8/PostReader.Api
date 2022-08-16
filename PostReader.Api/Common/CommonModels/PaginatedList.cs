
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostReader.Api.Common.CommonModels
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; }
        public int PageIndex { get; }
        public int TotalPages { get; }
        public int TotalCount { get; }
        public string NextCursor { get; set; }
        public string Word { get; set; }

        public PaginatedList(List<T> items, int count, int pageIndex, string word, int pageSize, string nextCursor)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            Items = items;
            NextCursor = nextCursor;
            Word = word;
        }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        public static PaginatedList<T> Create(IEnumerable<T> source, int pageIndex, int pageSize, string word, int totalPagesOnline, string nextCursor)
        {
            var count = totalPagesOnline;
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return new PaginatedList<T>(items, count, pageIndex, word, pageSize, nextCursor);
        }
    }
}
