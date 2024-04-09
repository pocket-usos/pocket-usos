namespace App.Infrastructure.Integration.Requests.Types;

internal class PostRequest(string path) : Request(HttpMethod.Post, path)
{
    public Request WithContent(object content)
    {
        Content = UsosContent.Serialize(content);

        return this;
    }
}
