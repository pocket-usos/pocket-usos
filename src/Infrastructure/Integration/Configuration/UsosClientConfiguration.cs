namespace App.Infrastructure.Integration.Configuration;

internal class UsosClientConfiguration
{
    public required IDictionary<string, InstitutionConfiguration> Institutions { get; set; }

    public required string CallbackUrl { get; set; }
}

internal class InstitutionConfiguration
{
    public required string ConsumerKey { get; set; }

    public required string ConsumerSecret { get; set; }
}
