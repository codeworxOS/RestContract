namespace Codeworx.Rest.Client
{
    public class DefaultRestOptions<TContract> : RestOptions<TContract>
        where TContract : class
    {
        public DefaultRestOptions(RestOptions options)
            : base(() => options.GetHttpClient())
        {
        }
    }
}