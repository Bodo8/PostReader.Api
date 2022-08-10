using AutoMapper;
using PostReader.Api.Infrastructure.Services.JsonModels;
using PostReader.Api.Models;

namespace PostReader.Api.Application.PostWebsites.Mappings
{
    public class PostsWebsiteProfile : Profile
    {
        public PostsWebsiteProfile()
        {
            CreateMap<Result, PostWebsite>().ForMember(d => d.Author,
                opt => opt.MapFrom(s => s.AuthorString));
        }
    }
}
