using App.Application.Contracts;

namespace App.Application.UserAccess.Authentication.LogOut;
public class LogOutCommand(Guid sessionId) : CommandBase
{
    public Guid SessionId { get; } = sessionId;
}
