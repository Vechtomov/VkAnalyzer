using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VkAnalyzer.BE;
using VkAnalyzer.Interfaces;

namespace VkAnalyzer.BL
{
    public class FileDataSaver 
    {
        private readonly string path;

        public FileDataSaver(string path = "")
        {
            this.path = path;
        }

        public void SaveData(IEnumerable<UserOnlineInfo> infos)
        {
            throw new NotImplementedException();
        }

        public async Task SaveDataAsync(IEnumerable<UserOnlineInfo> infos)
        {
            foreach (var info in infos)
            {
                await WriteInFile(info, path);
            }
        }

        private static async Task WriteInFile(UserOnlineInfo info, string path = "")
        {
            string directory = $"{path}\\{info.Id}\\{info.DateTime.Year}\\{info.DateTime.Month}";
            string name = info.DateTime.Day.ToString();
            await FileWriter.WriteInFileAsync(directory, name, $"{info.DateTime.ToString("HH:mm:ss")} : {info.OnlineInfo}{Environment.NewLine}");
        }
    }
}
