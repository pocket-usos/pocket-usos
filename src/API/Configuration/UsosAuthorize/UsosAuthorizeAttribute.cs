using Microsoft.AspNetCore.Mvc;

namespace App.API.Configuration.UsosAuthorize;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
internal class UsosAuthorizeAttribute : TypeFilterAttribute
{
    public UsosAuthorizeAttribute() : base(typeof(UsosAuthorizeFilter))
    { }
}
