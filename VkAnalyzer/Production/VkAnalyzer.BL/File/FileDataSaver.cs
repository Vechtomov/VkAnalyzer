using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VkAnalyzer.BE;

namespace VkAnalyzer.BL.File
{
    public class FileDataSaver 
    {
        private readonly string _path;

        public FileDataSaver(string path = "")
        {
            _path = path;
        }

        public void SaveData(IEnumerable<UserOnlineInfo> infos)
        {
            throw new NotImplementedException();
        }

        public async Task SaveDataAsync(IEnumerable<UserOnlineInfo> infos)
        {
            foreach (var info in infos)
            {
                await WriteInFile(info, _path);
            }
        }

        private static async Task WriteInFile(UserOnlineInfo info, string path = "")
        {
            var directory = $"{path}\\{info.Id}\\{info.DateTime.Year}\\{info.DateTime.Month}";
            var name = info.DateTime.Day.ToString();
            await FileWriter.WriteInFileAsync(directory, name, $"{info.DateTime:HH:mm:ss} : {info.OnlineInfo}{Environment.NewLine}");
        }
    }
}
