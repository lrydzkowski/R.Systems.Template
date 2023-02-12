using System.Reflection;

namespace R.Systems.Template.Tests.Api.AzureFunctions.Integration.Common;

internal static class EmbeddedFilesReader
{
    public static string GetContent(string path)
    {
        Assembly assembly = typeof(EmbeddedFilesReader).Assembly;
        path = path.TransformPath(assembly);
        if (!path.Exists(assembly))
        {
            throw new InvalidOperationException($"Embedded file doesn't exist. Path: '{path}'.");
        }

        using Stream? stream = assembly.GetManifestResourceStream(path);
        using StreamReader reader = new(stream!);

        return reader.ReadToEnd();
    }

    private static string TransformPath(this string path, Assembly assembly)
    {
        string transformedPath = path.Replace('/', '.').Replace('\\', '.');

        return $"{assembly.GetName().Name}.{transformedPath}";
    }

    private static bool Exists(this string path, Assembly assembly)
    {
        return assembly.GetManifestResourceNames().Any(x => x == path);
    }
}
