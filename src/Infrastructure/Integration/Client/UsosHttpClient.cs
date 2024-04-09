using App.Infrastructure.Integration.Requests;
using App.Infrastructure.Integration.Responses;

namespace App.Infrastructure.Integration.Client;

internal class UsosHttpClient(HttpClient httpClient) : IUsosHttpClient
{
    public async Task<Response> SendAsync(Request request)
    {
        var httpRequestMessage = new HttpRequestMessage(request.Method, request.GetFullUrl())
        {
            Content = request.Content
        };

        foreach (var header in request.Headers)
        {
            httpRequestMessage.Headers.Add(header.Key, header.Value);
        }

        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
        return await Response.From(httpResponseMessage);
    }
}
