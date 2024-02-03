namespace App.Domain.Users;

public record UserStatus(string Value)
{
    public static UserStatus Active = new(nameof(Active));

    public static UserStatus Inactive = new(nameof(Inactive));

    public static UserStatus Unknown = new(nameof(Unknown));

    public static UserStatus From(int? value)
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
