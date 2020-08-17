using System.Threading.Tasks;

namespace MainServer.strategy
{
    public interface IHttpRequest
    {
        Task<Response> Apply(string[] urls, string[] paths);
    }
}