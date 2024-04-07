namespace App.Infrastructure.Integration.Requests.Types;

internal class PatchRequest(string path) : Request(HttpMethod.Patch, path)
{
    public Request WithContent(object content)
    {
        Content = UsosContent.Serialize(content);

        return this;
    }
}
