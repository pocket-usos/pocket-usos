namespace App.Infrastructure.Integration.Configuration;

internal class UsosClientConfiguration
{
    public required string BaseUrl { get; set; }

    public required string ConsumerKey { get; set; }

    public required string ConsumerSecret { get; set; }
    
    public required string CallbackUrl { get; set; }
}