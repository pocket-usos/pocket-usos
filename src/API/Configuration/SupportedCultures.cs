namespace App.API.Configuration
{
    internal static class SupportedCultures
    {
        public static string[] All => ["pl", "en"];

        public static string Default => All.First();

        public static bool Check(string? language) => All.Any(x => x == language);
    }
}
