namespace App.Domain.Users;

public class Profile
{
    public string Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Status { get; set; }

    public string Email { get; set; }

    public string Pesel { get; set; }

    public DateOnly BirthDate { get; set; }

    public string Sex { get; set; }

    public string[] PhoneNumbers { get; set; }

    public string[] MobileNumbers { get; set; }

    public Citizenship Citizenship { get; set; }

    public string StudentNumber { get; set; }

    public string PhotoUrl { get; set; }

    public Address[] Addresses { get; set; }
}
