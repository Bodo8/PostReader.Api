using Newtonsoft.Json;

namespace PostReader.Api.Infrastructure.Services.JsonModels
{
    public class ResultObj
    {
        public ResultList ResultList { get; set; }
    }

    public class ResultList
    {
        public List<Result> Result { get; set; }
    }

    public class Result
    {
        public string Title { get; set; }
        public string AuthorString { get; set; }
        public string FirstPublicationDate { get; set; }
    }
}
