namespace App.Infrastructure.Integration.Requests.Types;

internal class PutRequest(string path) : Request(HttpMethod.Put, path)
{
    public Request WithContent(object content)
    {
        Content = UsosContent.Serialize(content);

        return this;
    }
}
