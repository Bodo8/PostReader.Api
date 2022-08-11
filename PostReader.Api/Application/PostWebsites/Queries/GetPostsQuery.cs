using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using PostReader.Api.Common.CommonModels;
using PostReader.Api.Common.Mappings;
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
            //List<PostWebsite> posts = await _websitesReaderService.GetPosts(word, cancellationToken);
           // List<PostWebsiteDto> postsDto = _mapper.Map<List<PostWebsiteDto>>(posts);

           // return _paginationListService.GetPaginatedList(postsDto, 1, 25);

            return GetItemList(); // !!!!
        }


        //test !!!!
        private PaginatedList<PostWebsiteDto> GetItemList()
        {
            List<PostWebsiteDto> posts = new();

            for (int i = 0; i < 62; i++)
            {
                posts.Add(new PostWebsiteDto()
                {
                    Title = "For displaying the records, an HTML Table is used. A loop will be executed over the Customers property ",
                    Author = "Name of the Controller",
                    FirstPublicationDate = new DateTime(2022, 08, 10)
                });
            }

            return _paginationListService.GetPaginatedList(posts, 1, 25);
        }
    }
}
