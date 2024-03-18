namespace App.Infrastructure.Translations;
internal static class UsosTranslateExtensions
{
    public static string Translate(this IDictionary<string, string> translations, string language)
    {
        if (translations.TryGetValue(language, out var translation))
        {
            if (!string.IsNullOrWhiteSpace(translation))
            {
                return translation;
            }
        }

        var firstAvailableTranslation = translations.FirstOrDefault(t => !string.IsNullOrWhiteSpace(t.Value)).Value;
        return firstAvailableTranslation ?? string.Empty;
    }
}
