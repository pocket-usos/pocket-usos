using App.Domain.BuildingBlocks;

namespace App.Domain.Institutions;

public class InstitutionId(Guid value) : TypedIdValueBase(value)
{
    public string ToBase64String()
    {
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Value.ToString()));
    }
}
