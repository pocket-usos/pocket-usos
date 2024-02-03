using App.Infrastructure.Integration.Requests;
using App.Infrastructure.Integration.Responses;

namespace App.Infrastructure.Integration.Client;

internal interface IUsosHttpClient
{
    public Task<Response> SendAsync(Request request);
}