
using PostReader.Api.Models;

namespace PostReader.Api.Services.Interfaces
{
    public interface IWebsitesReaderService
    {
        Task<List<PostWebsite>> GetPosts(string word, CancellationToken cancellationToken);
    }
}
