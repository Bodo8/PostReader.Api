using PostReader.Api.Application.PostWebsites.Queries;
using PostReader.Api.Common.CommonModels;

namespace PostReader.Api.Services.Interfaces
{
    public interface IPaginationListService
    {
        PaginatedList<PostWebsiteDto> GetPaginatedList(List<PostWebsiteDto> source, int pageIndex, int pageSize);
        PaginatedList<PostWebsiteDto> GetPaginatedListFromSource(int pageIndex, int pageSize);
    }
}
