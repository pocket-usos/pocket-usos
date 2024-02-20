namespace App.Domain.Institutions;

public class Institution
{
    public InstitutionId Id { get; private set; }

    public InstitutionName Name { get; private set; }

    public string BaseUrl { get; private set; }

    public bool IsEnabled { get; private set; }

    public static Institution Create(InstitutionName name, string baseUrl)
    {
        return new Institution(name, baseUrl);
    }


    private Institution(InstitutionName name, string baseUrl)
    {
        Id = new InstitutionId(Guid.NewGuid());
        Name = name;
        BaseUrl = baseUrl;
        IsEnabled = false;
    }

    private Institution() {}
}
