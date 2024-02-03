namespace App.Domain.Students;

public record StudentStatus(string Value)
{
    public static StudentStatus Active = new(nameof(Active));

    public static StudentStatus Inactive = new(nameof(Inactive));

    public static StudentStatus Unknown = new(nameof(Unknown));

    public static StudentStatus From(int? value)
    {
        switch (value)
        {
            case 1:
                return Inactive;
            case 2:
                return Active;
            default:
                return Unknown;
        }
    }
}
