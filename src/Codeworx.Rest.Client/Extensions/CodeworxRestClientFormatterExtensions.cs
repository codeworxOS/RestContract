using System.Net.Http;
using System.Threading.Tasks;

namespace Codeworx.Rest.Client
{
    public static class CodeworxRestClientFormatterExtensions
    {
        public static async Task<TResult> DeserializeAsync<TResult>(this IContentFormatter formatter, HttpResponseMessage response)
        {
            return (TResult)await formatter.DeserializeAsync(typeof(TResult), response);
        }
    }
}