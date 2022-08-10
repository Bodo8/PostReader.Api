using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PostReader.Api.Common.CommonModels.Settings;
using PostReader.Api.Common.Interfaces;
using PostReader.Api.Infrastructure.Services.JsonModels;
using PostReader.Api.Models;
using PostReader.Api.Services.Interfaces;
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
            string pathFirst = _euroPmcSettings.BaseUrl + _euroPmcSettings.UrlBasePart + word;
            string path = CreatePath(word);
            await _requestWebsiteService.MakeGetRequestAsync(pathFirst, cancellationToken);
            string content = await _requestWebsiteService.MakeGetRequestAsync(path, cancellationToken, true);
            int quantityAllPosts = SearchQuantity(content);

            if (quantityAllPosts == 0)
                return new List<PostWebsite>();

            if (quantityAllPosts > _euroPmcSettings.DefaultListSize)
            {
                path = CreatePath(word, quantityAllPosts);
                content = await _requestWebsiteService.MakeGetRequestAsync(path, cancellationToken, true);
            }
            List<Result> deserialized = GetDeserializedJson(content);

            return _mapper.Map<List<PostWebsite>>(deserialized);
        }

        private static List<Result> GetDeserializedJson(string content)
        {
            ResultObj? deserialized = JsonConvert.DeserializeObject<ResultObj>(content);

            if (deserialized == null)
                return new List<Result>();

            return deserialized.ResultList.Result;
        }

        private int SearchQuantity(string firstContent)
        {
            var regex = new Regex(_euroPmcSettings.RegexQuntity);
            var matched = regex.Match(firstContent);
            string? result = matched.Groups["values"]?.Value.Trim();
            bool succses = int.TryParse(result, out int quantityTotal);

            if(succses)
                return quantityTotal;

            return 0;
        }

        private string CreatePath(string word, int size = 25)
        {
            return _euroPmcSettings.BaseUrl + _euroPmcSettings.UrlAjaxFirst + word + _euroPmcSettings.UrlAjaxMiddle + size + _euroPmcSettings.UrlAjaxEnd;
        }
    }
}
