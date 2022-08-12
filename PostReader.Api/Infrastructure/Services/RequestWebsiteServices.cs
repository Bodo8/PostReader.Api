using PostReader.Api.Common.Interfaces;
using System.Net;
using System.Text;
using System.Web;

namespace PostReader.Api.Infrastructure.Services
{
    public class RequestWebsiteServices : IRequestWebsiteService
    {
        public async Task<string> MakeGetRequestAsync(string path, CancellationToken cancellationToken, bool isAjaxRequest = false)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", GetUserAgent());

                if (isAjaxRequest)
                    client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");

                var response = await client.GetAsync(path).Result.Content.ReadAsStreamAsync(cancellationToken);

                using (StreamReader reader = new StreamReader(response, Encoding.UTF8))
                {
                    var result = reader.ReadToEnd();

                    return HttpUtility.HtmlDecode(result);
                };
            };
        }

        private string GetUserAgent()
        {
            return "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:81.0) Gecko/20100101 Firefox/81.0";
        }
    }
}
