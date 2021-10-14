using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchangeTags.Models;

namespace StackExchangeTags.Services
{
    public class ApiCallService : IApiCallService
    {
        private static readonly string API_ADDRESS =
            "https://api.stackexchange.com/2.3";

        private static readonly string TAGS_ENDPOINT = "/tags";

        private async Task<StackExchangeTagList<StackExchangeTag>> GetTagListUsingApiCall(int pageNumber, int pageSize, string sortDirection, string sortType, string site)
        {
            StackExchangeTagList<StackExchangeTag> tagList = new StackExchangeTagList<StackExchangeTag>();
            using (var httpClient = new HttpClient())
            {
                string url = $"{API_ADDRESS}{TAGS_ENDPOINT}?page={pageNumber}&pagesize={pageSize}&order={sortDirection}&sort={sortType}&site={site}";
                using (var response = await httpClient.GetAsync(url))
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var gzipStream = new GZipStream(responseStream, CompressionMode.Decompress);
                    var streamReader = new StreamReader(gzipStream);
                    var jsonTextReader = new JsonTextReader(streamReader);
                    var serializer = new JsonSerializer();
                    tagList = serializer.Deserialize<StackExchangeTagList<StackExchangeTag>>(jsonTextReader);
                }
            }

            return tagList;
        }
        
        public async Task<PageResult<StackExchangeTagList<StackExchangeTag>>> GetTags(int pageNumber, int pageSize, string sortDirection, string sortType, string site)
        {
            StackExchangeTagList<StackExchangeTag> tagList = new StackExchangeTagList<StackExchangeTag>();
            tagList.Items = new List<StackExchangeTag>();
            if (pageSize <= 100)
            {
                tagList = await GetTagListUsingApiCall(pageNumber, pageSize, sortDirection, sortType,
                    site);
            }
            else
            {
                int loopCount = pageSize / 100;
                int reminderFromDividing = pageSize % 100;
                for (int i = 0; i < loopCount; i++)
                {
                    var temp = await GetTagListUsingApiCall(i + 1, 100, sortDirection, sortType, site);
                    tagList.Items.AddRange(temp.Items);
                }

                if (reminderFromDividing > 0)
                {
                    var temp = await GetTagListUsingApiCall(loopCount + 1, reminderFromDividing, sortDirection, sortType, site);
                    tagList.Items.AddRange(temp.Items);
                }
            }

            return new PageResult<StackExchangeTagList<StackExchangeTag>>(tagList, 1000, pageSize, pageNumber);
        }
    }
}