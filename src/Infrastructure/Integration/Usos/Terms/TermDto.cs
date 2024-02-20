namespace App.Infrastructure.Integration.Usos.Terms;

public class TermsDto
{
    public required TermDto[] Terms { get; set; }
}

public class TermDto
{
    public required string Id { get; set; }

    public required IDictionary<string, string> Name { get; set; }

    public required string StartDate { get; set; }

    public required string EndDate { get; set; }
}
