namespace App.Application.Institutions.Public;

public class InstitutionDto
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }

    public required string LogoUrl { get; init; }

    public required bool IsBeta { get; init; }
}
