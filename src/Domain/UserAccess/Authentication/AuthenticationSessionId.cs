using App.Domain.BuildingBlocks;

namespace App.Domain.UserAccess.Authentication;

public class AuthenticationSessionId(Guid value) : TypedIdValueBase(value);
