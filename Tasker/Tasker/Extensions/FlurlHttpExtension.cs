using Flurl.Http;

namespace Tasker.Extensions
{
    public static class FlurlHttpExtension
    {
        public static IFlurlRequest AddAuthorizationHeader(this IFlurlRequest request)
        {
            request.Headers.Add("Authorization", Settings.CurrentToken);

            return request;
        }
    }
}