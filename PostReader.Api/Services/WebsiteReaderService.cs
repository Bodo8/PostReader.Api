using AutoMapper;
using Newtonsoft.Json;
using PostReader.Api.Application.PostWebsites.Queries;
using PostReader.Api.Common.CommonModels;
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
        private int _totalResultsOnline;
        private string _nextCursor;

        public WebsiteReaderService
            (
            IRequestWebsiteService requestWebsiteService,
            IMapper mapper,
            EuropePmcSettings euroPmcSettings
            )
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
            string nextCursor = "*";
            path = CreatePath(word , nextCursor);
            string content = await _requestWebsiteService.MakeGetRequestAsync(path, cancellationToken, true);
            List<Result> deserialized = GetDeserializedJson(content);
            result.AddRange(deserialized);
            string allPostsString = SearchQuantity(content, _euroPmcSettings.RegexQuntity);
            _totalResultsOnline = ConvertToInt(allPostsString);
            

            if (_totalResultsOnline == 0)
                return new List<PostWebsite>();

            if (_totalResultsOnline > _euroPmcSettings.DefaultListSize)
            {
                nextCursor = SearchNextCursor(content);
                List<Result> deserializedNext = await GetResults(word, nextCursor, cancellationToken, true);
                result.AddRange(deserializedNext);
            }

            return _mapper.Map<List<PostWebsite>>(result);
        }

        private string SearchNextCursor(string content)
        {
            string nextCursor = SearchQuantity(content, _euroPmcSettings.RegexNext);
            nextCursor = nextCursor.Replace("/", "%2F").Replace("=", "%3D");

            return nextCursor;
        }

        private async Task<List<Result>> GetResults(string word, string nextCursor, CancellationToken cancellationToken, bool isAjaxRequest)
        {
            string pathNext = CreatePath(word, nextCursor, _euroPmcSettings.MaxRequstQuantity);
            string content = await _requestWebsiteService.MakeGetRequestAsync(pathNext, cancellationToken, isAjaxRequest);
            nextCursor = SearchNextCursor(content);
            _nextCursor = nextCursor;

            return GetDeserializedJson(content);
        }

        public async Task<List<PostWebsite>> GetPosts(string word, string nextCursor, CancellationToken cancellationToken, bool isAjaxRequest)
        {
            List<Result> deserialized = await GetResults(word, nextCursor, cancellationToken, isAjaxRequest);

            return _mapper.Map<List<PostWebsite>>(deserialized);
        }

        public int GetTotalResultsOnline()
        {
            return _totalResultsOnline;
        }

        public string GetNextCursor()
        {
            return _nextCursor;
        }

        private static List<Result> GetDeserializedJson(string content)
        {
            ResultObj? deserialized = new();

            try
            {
                deserialized = JsonConvert.DeserializeObject<ResultObj>(content);
            }
            catch(Exception ex)
            {
                //todo: Add Logger
            }

            return deserialized?.ResultList?.Result ?? new List<Result>();
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
