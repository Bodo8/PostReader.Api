using PostReader.Api.Application.PostWebsites.Queries;
using PostReader.Api.Common.CommonModels;
using PostReader.Api.Services.Interfaces;

namespace PostReader.Api.Services
{
    public class PaginationListService : IPaginationListService
    {
        private static List<PostWebsiteDto>? _source;

        public PaginatedList<PostWebsiteDto> GetPaginatedList(List<PostWebsiteDto> source, int pageIndex, int pageSize)
        {
            _source = source.Select(x => x).ToList();

            return PaginatedList<PostWebsiteDto>.Create(_source, pageIndex, pageSize);
        }

        public PaginatedList<PostWebsiteDto> GetPaginatedListFromSource(int pageIndex, int pageSize)
        {
            if (_source == null)
                return new PaginatedList<PostWebsiteDto>(new List<PostWebsiteDto>(), 0, 1, 1);

            return PaginatedList<PostWebsiteDto>.Create(_source, pageIndex, pageSize);
        }
    }
}
