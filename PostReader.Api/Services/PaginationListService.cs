using PostReader.Api.Application.PostWebsites.Queries;
using PostReader.Api.Common.CommonModels;
using PostReader.Api.Infrastructure.Services.JsonModels;
using PostReader.Api.Services.Interfaces;

namespace PostReader.Api.Services
{
    public class PaginationListService : IPaginationListService
    {
        private static List<PostWebsiteDto> _source;
        private static int _totalPages;

        public bool HasNextPage(int pageIndex)
        { 
            if(_totalPages > 0)
               return pageIndex <= _totalPages;

            return false;
        }

        public PaginatedList<PostWebsiteDto> GetPaginatedList
            (
            List<PostWebsiteDto> source,
            int pageIndex, int pageSize,
            string nextCursor,
            int totalResultsOnline,
            string word
            )
        {
            _source = source.Select(x => x).ToList();
            SetPaginatedServiceParameters(pageSize);

            return PaginatedList<PostWebsiteDto>.Create(_source, pageIndex, pageSize, word, totalResultsOnline, nextCursor);
        }

        public PaginatedList<PostWebsiteDto> GetPaginatedListFromSource
            (
            int pageIndex,
            string word,
            int pageSize,
            int totalResultsOnline,
            string nextCursor
            )
        {
            if (_source == null)
                return new PaginatedList<PostWebsiteDto>(new List<PostWebsiteDto>(), 0, 1, word, 1, nextCursor);
  
            return PaginatedList<PostWebsiteDto>.Create(_source, pageIndex, pageSize, word, totalResultsOnline, nextCursor);
        }

        public PaginatedList<PostWebsiteDto> GetNextPaginatedListFromSource
            (
            List<PostWebsiteDto> nextSource,
            int pageIndex,
            string word,
            int pageSize,
            int totalResultsOnline,
            string nextCursor
            )
        {
            var unigue = nextSource.Except(_source).ToList();
            _source.AddRange(unigue);
            SetPaginatedServiceParameters(pageSize);

            return PaginatedList<PostWebsiteDto>.Create
                (
                _source,
                pageIndex,
                pageSize,
                word,
                totalResultsOnline,
                nextCursor
                );
        }

        private static void SetPaginatedServiceParameters(int pageSize)
        {
            _totalPages = (int)Math.Ceiling(_source.Count / (double)pageSize);
        }
    }
}
