using AutoMapper;
using MediatR;
using PostReader.Api.Common.CommonModels;
using PostReader.Api.Models;
using PostReader.Api.Services.Interfaces;

namespace PostReader.Api.Application.PostWebsites.Queries
{
    public class GetPaginationPageQuery : IRequest<PaginatedList<PostWebsiteDto>>
    {
        public int PageIndex { get; set; }
        public string NextCursor { get; set; }
        public string Word { get; set; }
        public int TotalResultsOnline { get; set; }
        public int? PageSize { get; set; } = 25;
    }

    public class GetPaginationPageQueryHandler : IRequestHandler<GetPaginationPageQuery, PaginatedList<PostWebsiteDto>>
    {
        private readonly IPaginationListService _paginationListService;
        private readonly IWebsitesReaderService _websitesReaderService;
        private readonly IMapper _mapper;

        public GetPaginationPageQueryHandler(IPaginationListService paginationListService, IWebsitesReaderService websitesReaderService, IMapper mapper)
        {
            _paginationListService = paginationListService;
            _websitesReaderService = websitesReaderService;
            _mapper = mapper;
        }

        public async Task<PaginatedList<PostWebsiteDto>> Handle(GetPaginationPageQuery request, CancellationToken cancellationToken)
        {
            if (_paginationListService.HasNextPage(request.PageIndex))
                return _paginationListService.GetPaginatedListFromSource(request.PageIndex, request.Word, (int)request.PageSize, request.TotalResultsOnline, request.NextCursor);
            else
            {
                List<PostWebsite> posts = await _websitesReaderService.GetPosts(request.Word, request.NextCursor, cancellationToken, true);
                List<PostWebsiteDto> postsDto = _mapper.Map<List<PostWebsiteDto>>(posts);
                string nextCursor = _websitesReaderService.GetNextCursor();

                return _paginationListService.GetNextPaginatedListFromSource(postsDto, request.PageIndex, request.Word, (int)request.PageSize, request.TotalResultsOnline, nextCursor);
            }
        }
    }
}
