using System.Threading.Tasks;
using Taxi.Common.Model;

namespace Taxi.Common.Services
{
    public interface IApiService
    {
        Task<bool> CheckConnectionAsync(string url);
        Task<Response> GetTaxiAsync(string plaque, string urlBase, string servicePrefix, string controller);

    }
}
