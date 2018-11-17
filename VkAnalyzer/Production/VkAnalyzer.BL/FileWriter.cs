using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace VkAnalyzer.BL
{
    public static class FileWriter
    {
        public static void WriteInFile(string directory, string name, string text,
            FileMode fileMode = FileMode.Append, FileAccess fileAccess = FileAccess.Write)
        {
            DirectoryInfo dir = new DirectoryInfo(directory);
            if (!dir.Exists)
                dir.Create();
            byte[] buff = Encoding.UTF8.GetBytes(text);
            using (FileStream fileStream = new FileStream($"{directory}\\{name}.txt", fileMode, fileAccess))
            {
                fileStream.Write(buff, 0, buff.Length);
            }
        }

        public static async Task WriteInFileAsync(string directory, string name, string text,
            FileMode fileMode = FileMode.Append, FileAccess fileAccess = FileAccess.Write)
        {
            DirectoryInfo dir = new DirectoryInfo(directory);
            if (!dir.Exists)
                dir.Create();
            byte[] buff = Encoding.UTF8.GetBytes(text);
            using (FileStream fileStream = new FileStream($"{directory}\\{name}.txt", fileMode, fileAccess))
            {
                await fileStream.WriteAsync(buff, 0, buff.Length);
            }
        }
    }
}
