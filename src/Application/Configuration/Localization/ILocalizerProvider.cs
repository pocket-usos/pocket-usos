using Microsoft.Extensions.Localization;

namespace App.Application.Configuration.Localization;

public interface ILocalizerProvider
{
    IStringLocalizer GetLocalizer();
}
