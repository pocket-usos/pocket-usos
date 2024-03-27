namespace App.Domain.Institutions;

public class Institution
{
    public const string DefaultLogoPath = "/images/institutions-logos/default-logo.png";

    public InstitutionId Id { get; private set; }

    public InstitutionName Name { get; private set; }

    public string BaseUrl { get; private set; }

    public string? LogoPath { get; private set; }

    public bool IsBeta { get; private set; }

    public bool IsEnabled { get; private set; }

    public static Institution Create(InstitutionName name, string baseUrl, string? logoPath)
    {
        return new Institution(name, baseUrl, logoPath);
    }


    private Institution(InstitutionName name, string baseUrl, string? logoPath)
    {
        Id = new InstitutionId(Guid.NewGuid());
        Name = name;
        BaseUrl = baseUrl;
        LogoPath = logoPath;
        IsEnabled = false;
        IsBeta = true;
    }

    private Institution() {}
}
