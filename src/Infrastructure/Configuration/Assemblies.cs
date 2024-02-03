using System.Reflection;
using App.Application.Contracts;

namespace App.Infrastructure.Configuration;

internal static class Assemblies
{
    public static readonly Assembly Application = typeof(CommandBase).Assembly;

    public static readonly Assembly Infrastructure = typeof(Gateway).Assembly;
}
