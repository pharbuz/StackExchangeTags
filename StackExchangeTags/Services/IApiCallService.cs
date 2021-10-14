using System.Threading.Tasks;
using StackExchangeTags.Models;

namespace StackExchangeTags.Services
{
    public interface IApiCallService
    {
        Task<PageResult<StackExchangeTagList<StackExchangeTag>>> GetTags(int pageNumber, int pageSize, string sortDirection, string sortType, string site);
    }
}