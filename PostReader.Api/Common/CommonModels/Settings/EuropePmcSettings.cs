namespace PostReader.Api.Common.CommonModels.Settings
{
    public class EuropePmcSettings
    {
        public string BaseUrl { get; set; }
        public string UrlBasePart { get; set; }
        public string UrlAjaxFirst { get; set; }
        public string UrlAjaxMiddle { get; set; }
        public string UrlAjaxEnd { get; set; }
        public int DefaultListSize { get; set; }
        public string RegexQuntity { get; set; }
    }
}
