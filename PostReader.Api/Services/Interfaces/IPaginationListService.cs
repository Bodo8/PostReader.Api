using PostReader.Api.Application.PostWebsites.Queries;
using PostReader.Api.Common.CommonModels;

namespace PostReader.Api.Services.Interfaces
{
    public interface IPaginationListService
    {
        bool HasNextPage(int pageIndex);

        PaginatedList<PostWebsiteDto> GetPaginatedList
            (
            List<PostWebsiteDto> source,
            int pageIndex,
            int pageSize, 
            string nextCursor,
            int totalResults,
            string word
            );
        PaginatedList<PostWebsiteDto> GetPaginatedListFromSource
            (
            int pageIndex,
            string word,
            int pageSize,
            int totalResultsOnline,
            string nextCursor
            );
        PaginatedList<PostWebsiteDto> GetNextPaginatedListFromSource
            (List<PostWebsiteDto> nextSource,
            int pageIndex,
            string word,
            int pageSize,
            int totalResultsOnline,
            string nextCursor
            );
    }
}
