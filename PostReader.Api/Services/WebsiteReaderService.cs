using AutoMapper;
using Newtonsoft.Json;
using PostReader.Api.Common.CommonModels.Settings;
using PostReader.Api.Common.Interfaces;
using PostReader.Api.Infrastructure.Services.JsonModels;
using PostReader.Api.Models;
using PostReader.Api.Services.Interfaces;
using System.Text;
using System.Text.RegularExpressions;

namespace PostReader.Api.Services
{
    public class WebsiteReaderService : IWebsitesReaderService
    {
        private readonly IRequestWebsiteService _requestWebsiteService;
        private readonly IMapper _mapper;
        private readonly EuropePmcSettings _euroPmcSettings;

        public WebsiteReaderService(IRequestWebsiteService requestWebsiteService, IMapper mapper, EuropePmcSettings euroPmcSettings)
        {
            _requestWebsiteService = requestWebsiteService;
            _euroPmcSettings = euroPmcSettings;
            _mapper = mapper;
        }

        public async Task<List<PostWebsite>> GetPosts(string word, CancellationToken cancellationToken)
        {
            List<Result> result = new();
            string path = _euroPmcSettings.BaseUrl + _euroPmcSettings.UrlBasePart + word;
            await _requestWebsiteService.MakeGetRequestAsync(path, cancellationToken);
            path = CreatePath(word , "*");
            string content = await _requestWebsiteService.MakeGetRequestAsync(path, cancellationToken, true);
            List<Result> deserialized = GetDeserializedJson(content);
            result.AddRange(deserialized);
            string allPostsString = SearchQuantity(content, _euroPmcSettings.RegexQuntity);
            int quantityAllPosts = ConvertToInt(allPostsString);

            if (quantityAllPosts == 0)
                return new List<PostWebsite>();

            if (quantityAllPosts > _euroPmcSettings.DefaultListSize)
            {
                int pagesTotal = (int)Math.Ceiling((decimal)(quantityAllPosts - 25) / _euroPmcSettings.MaxRequstQuantity);

                for (int i = 0; i < pagesTotal; i++)
                {
                    string nextCursor = SearchQuantity(content, _euroPmcSettings.RegexNext);
                    nextCursor = nextCursor.Replace("/", "%2F").Replace("=", "%3D");
                    string pathNext = CreatePath(word, nextCursor, _euroPmcSettings.MaxRequstQuantity);
                    content = await _requestWebsiteService.MakeGetRequestAsync(pathNext, cancellationToken, true);
                    List<Result> deserializedNext = GetDeserializedJson(content);
                    result.AddRange(deserializedNext);
                }
            }

            return _mapper.Map<List<PostWebsite>>(result);
        }

        private static List<Result> GetDeserializedJson(string content)
        {
            ResultObj? deserialized = JsonConvert.DeserializeObject<ResultObj>(content);

            if (deserialized == null 
                || deserialized.ResultList == null || deserialized.ResultList.Result == null)
                return new List<Result>();

            return deserialized.ResultList.Result;
        }

        private string SearchQuantity(string firstContent, string regexPattern)
        {
            Regex regex = new Regex(regexPattern);
            Match matched = regex.Match(firstContent);

            return matched.Groups["values"]?.Value?.Trim() ?? string.Empty;
        }

        private int ConvertToInt(string result)
        {
            bool succses = int.TryParse(result, out int quantityTotal);

            if (succses)
                return quantityTotal;

            return 0;
        }

        private string CreatePath(string word, string nextCursor, int size = 25)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(_euroPmcSettings.BaseUrl).Append(_euroPmcSettings.UrlAjaxFirst)
                .Append(word).Append(_euroPmcSettings.UrlAjaxCursor).Append(nextCursor)
                .Append(_euroPmcSettings.UrlAjaxMiddle).Append(size)
                .Append(_euroPmcSettings.UrlAjaxEnd);

            return builder.ToString();
        }
    }
}
