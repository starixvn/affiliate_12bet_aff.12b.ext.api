namespace Fanex.Aff12B.Gateway.Core.Handlers
{
    public class ApiKeyHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = base.SendAsync(request, cancellationToken);

            return response;
        }
    }
}
