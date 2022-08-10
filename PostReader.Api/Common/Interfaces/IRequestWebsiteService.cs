namespace PostReader.Api.Common.Interfaces
{
    public interface IRequestWebsiteService
    {
        Task<string> MakeGetRequestAsync(string path, CancellationToken cancellationToken, bool isAjaxRequest = false);
    }
}
