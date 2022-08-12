using AutoMapper;
using MediatR;
using PostReader.Api.Common.CommonModels;
using PostReader.Api.Models;
using PostReader.Api.Services.Interfaces;

namespace PostReader.Api.Application.PostWebsites.Queries
{
    public class GetPostsQuery : IRequest<PaginatedList<PostWebsiteDto>>
    {
        public string Sentence { get; set; }
    }

    public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, PaginatedList<PostWebsiteDto>>
    {
        private readonly IWebsitesReaderService _websitesReaderService;
        private readonly IMapper _mapper;
        private readonly IPaginationListService _paginationListService;

        public GetPostsQueryHandler(
            IWebsitesReaderService websitesReaderService,
            IMapper mapper,
            IPaginationListService paginationListService)
        {
            _websitesReaderService = websitesReaderService;
            _mapper = mapper;
            _paginationListService = paginationListService;
        }

        public async Task<PaginatedList<PostWebsiteDto>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            string word = request.Sentence.Replace(" ", "%20");
            List<PostWebsite> posts = await _websitesReaderService.GetPosts(word, cancellationToken);
            List<PostWebsiteDto> postsDto = _mapper.Map<List<PostWebsiteDto>>(posts);

            return _paginationListService.GetPaginatedList(postsDto, 1, 25);
        }
    }
}
