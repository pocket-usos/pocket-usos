using System.Net.Mime;
using System.Text;
using System.Text.Json;
using App.Infrastructure.Integration.Json;

namespace App.Infrastructure.Integration.Requests;
internal static class UsosContent
{
    public static HttpContent Serialize(object content)
    {
        return new StringContent(
            JsonSerializer.Serialize(new Dictionary<string, object> { { "data", content } }, new JsonSerializerOptions
            {
                PropertyNamingPolicy = new SnakeCaseNamingPolicy()
            }),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);
    }
}
