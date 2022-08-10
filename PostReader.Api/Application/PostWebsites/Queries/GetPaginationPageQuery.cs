using MediatR;
using PostReader.Api.Common.CommonModels;
using PostReader.Api.Services.Interfaces;

namespace PostReader.Api.Application.PostWebsites.Queries
{
    public class GetPaginationPageQuery : IRequest<PaginatedList<PostWebsiteDto>>
    {
        public int PageIndex { get; set; }
        public int? PageSize { get; set; } = 25;
    }

    public class GetPaginationPageQueryHandler : IRequestHandler<GetPaginationPageQuery, PaginatedList<PostWebsiteDto>>
    {
        private readonly IPaginationListService _paginationListService;

        public GetPaginationPageQueryHandler(IPaginationListService paginationListService)
        {
            _paginationListService = paginationListService;
        }

        public async Task<PaginatedList<PostWebsiteDto>> Handle(GetPaginationPageQuery request, CancellationToken cancellationToken)
        {
            return _paginationListService.GetPaginatedListFromSource(request.PageIndex, (int)request.PageSize);
        }
    }
}
