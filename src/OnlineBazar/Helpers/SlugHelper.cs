using System.Globalization;
using System.Text.RegularExpressions;

public static class SlugHelper
{
    public static string Slugify(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return "item";
        var slug = value.ToLowerInvariant().Trim();

        // remove invalid chars
        slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
        // convert multiple spaces to single
        slug = Regex.Replace(slug, @"\s+", " ");
        // replace spaces with hyphens
        slug = Regex.Replace(slug, @"\s", "-");
        // collapse duplicate hyphens
        slug = Regex.Replace(slug, "-+", "-");
        // trim hyphens
        slug = slug.Trim('-');

        return string.IsNullOrWhiteSpace(slug) ? "item" : slug;
    }
}
