using App.Application.Users;

namespace App.Infrastructure.Integration.Usos.Students;

public class UserDto
{
    public required string Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public int? StudentStatus { get; set; }

    public required string Sex { get; set; }

    public string? Email { get; set; }

    public string? Pesel { get; set; }

    public IDictionary<string, string?>? Titles { get; set; }

    public DateOnly? BirthDate { get; set; }

    public CitizenshipDto? Citizenship { get; set; }

    public string? StudentNumber { get; set; }

    public required string[] PhoneNumbers { get; set; }

    public required IDictionary<string, string> PhotoUrls { get; set; }

    public required string[] MobileNumbers { get; set; }

    public required AddressDto[] PostalAddresses { get; set; }

    public Profile ToProfile()
    {
        return new Profile
        {
            Id = Id,
            FirstName = FirstName,
            LastName = LastName,
            Status = UserStatus.From(StudentStatus).Value,
            Email = Email!,
            Pesel = Pesel!,
            BirthDate = BirthDate!.Value,
            Sex = Sex,
            PhoneNumbers = PhoneNumbers,
            MobileNumbers = MobileNumbers,
            Citizenship = new Citizenship(Citizenship!.Id),
            StudentNumber = StudentNumber!,
            PhotoUrl = PhotoUrls.Select(photo => photo.Value).ToArray()[0],
            Addresses = PostalAddresses.Select(a => new Address(a.Type, a.Address)).ToArray()
        };
    }

    public User ToUser()
    {
        return new User
        {
            Id = Id,
            FirstName = FirstName,
            LastName = LastName,
            Email = Email ?? null,
            Status = UserStatus.From(StudentStatus).Value,
            Sex = Sex,
            PhotoUrl = PhotoUrls.Select(photo => photo.Value).ToArray()[0],
            Title = Titles?["before"],
        };
    }
}

public class CitizenshipDto
{
    public required string Id { get; set; }
}

public class AddressDto
{
    public required string Type { get; set; }

    public required string Address { get; set; }
}
