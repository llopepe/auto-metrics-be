using System.Reflection;

namespace Core.Framework.Aplication.Common.Extensions
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class BuildInfoExtension
    {
        public static string GetBuildDate()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var filePath = assembly.Location;
            var buildDate = File.GetLastWriteTime(filePath);

            return buildDate.ToString("yyyy-MM-dd HH:mm:ss");

        }
    }
}
