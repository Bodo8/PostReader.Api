using PostReader.Api.Common.Mappings;
using PostReader.Api.Models;

namespace PostReader.Api.Application.PostWebsites.Queries
{
    public class PostWebsiteDto : IMapFrom<PostWebsite>, IEquatable<PostWebsiteDto>
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime FirstPublicationDate { get; set; }

        public bool Equals(PostWebsiteDto? other)
        {
            if (other is null)
                return false;

            return this.Title == other.Title && this.Author == other.Author
                && this.FirstPublicationDate == other.FirstPublicationDate;
        }

        public override bool Equals(object obj) => Equals(obj as PostWebsiteDto);

        public override int GetHashCode()
        {
            return HashCode.Combine(Title, Author, FirstPublicationDate);
        }
    }
}