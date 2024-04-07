namespace App.Infrastructure.Integration.Requests.Types;

internal class GetRequest(string path) : Request(HttpMethod.Get, path);
