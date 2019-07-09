using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Parser.Core
{
    class HtmlLoader
    {
        readonly HttpClient client;
        readonly string url;

        public HtmlLoader(Parser parser)
        {
            client = new HttpClient();
            url = $"{parser.BaseUrl}/{parser.Prefix}/";
        }

        public async Task<string> GetSourceByPageId(string word)
        {
            var currentUrl = url.Replace("{CurrentWord}", word);
            var response = await client.GetAsync(currentUrl);
            string source = null;

            if(response != null && response.StatusCode == HttpStatusCode.OK)
                source = await response.Content.ReadAsStringAsync();

            return source;
        }
    }
}
