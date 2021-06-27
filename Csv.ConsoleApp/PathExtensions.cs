using System;
using System.IO;

namespace Csv.ConsoleApp
{
    public static class PathExtensions
    {
        public static string GetRelativePath(this string filename)
        {
            var workingDirectory = Environment.CurrentDirectory;
            var projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            return Path.Combine(projectDirectory, filename);
        }

        public static bool IsValidPath(this string path)
        {
            if (!Path.IsPathRooted(path))
                return File.Exists(GetRelativePath(path));
            else
                return File.Exists(path);
        }

        public static string GetFilepath(this string path)
        {
            if (!Path.IsPathRooted(path))
                return GetRelativePath(path);
            else
                return path;
        }

        public static bool HasValidDirectory(this string path)
        {
            var directoryName = Path.GetDirectoryName(path);
            return Directory.Exists(directoryName);
        }
    }
}
