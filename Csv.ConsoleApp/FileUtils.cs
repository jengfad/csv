using System.IO;

namespace Csv.ConsoleApp
{
    public static class FileUtils
    {
        public static void ClearFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
