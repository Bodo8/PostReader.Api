using PostReader.Api.Common.Mappings;
using PostReader.Api.Models;

namespace PostReader.Api.Application.PostWebsites.Queries
{
    public class PostWebsiteDto : IMapFrom<PostWebsite>
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime FirstPublicationDate { get; set; }
    }
}