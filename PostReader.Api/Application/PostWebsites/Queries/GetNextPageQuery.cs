using MediatR;
using PostReader.Api.Common.CommonModels;
using PostReader.Api.Services.Interfaces;

namespace PostReader.Api.Application.PostWebsites.Queries
{
    public class GetNextPageQuery : IRequest<PaginatedList<PostWebsiteDto>>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; } = 25;
    }

    public class GetNextPageQueryHandler : IRequestHandler<GetNextPageQuery, PaginatedList<PostWebsiteDto>>
    {
        private readonly IPaginationListService _paginationListService;

        public GetNextPageQueryHandler(IPaginationListService paginationListService)
        {
            _paginationListService = paginationListService;
        }

        public async Task<PaginatedList<PostWebsiteDto>> Handle(GetNextPageQuery request, CancellationToken cancellationToken)
        {
            return _paginationListService.GetPaginatedListFromSource(request.PageIndex, request.PageSize);
        }
    }
}
