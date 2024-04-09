namespace App.Infrastructure.Integration.Requests.Types;

internal class DeleteRequest(string path) : Request(HttpMethod.Delete, path);
