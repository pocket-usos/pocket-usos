using Microsoft.AspNetCore.Mvc;

namespace App.API.Configuration.UsosAuthorize;

internal class UsosAuthorizeAttribute : TypeFilterAttribute
{
    public UsosAuthorizeAttribute() : base(typeof(UsosAuthorizeFilter))
    { }
}
