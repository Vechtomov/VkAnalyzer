using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkAnalyzer.BE;
using VkAnalyzer.Interfaces;

namespace VkAnalyzer.BL.File
{
    public class FileRepository : IUserInfoRepository
    {
        private readonly FileRepositorySettings _fileRepositorySettings;
        public FileRepository(FileRepositorySettings fileRepositorySettings)
        {
            _fileRepositorySettings = fileRepositorySettings ?? throw new ArgumentNullException(nameof(fileRepositorySettings));
        }

        public void SaveData(IEnumerable<UserOnlineInfo> infos)
        {
            foreach (var info in infos)
            {
                WriteInFile(info, _fileRepositorySettings.FileDataPath);
            }
        }

        public async Task SaveDataAsync(IEnumerable<UserOnlineInfo> infos)
        {
            foreach (var info in infos)
            {
                await WriteInFileAsync(info, _fileRepositorySettings.FileDataPath);
            }
        }

        private static void WriteInFile(UserOnlineInfo info, string path = "")
        {
            var directory = $"{path}\\{info.Id}\\{info.DateTime.Year}\\{info.DateTime.Month}";
            var name = info.DateTime.Day.ToString();
            FileWriter.WriteInFile(directory, name, $"{info.DateTime:HH:mm:ss} : {info.OnlineInfo}{Environment.NewLine}");
        }

        private static async Task WriteInFileAsync(UserOnlineInfo info, string path = "")
        {
            var directory = $"{path}\\{info.Id}\\{info.DateTime.Year}\\{info.DateTime.Month}";
            var name = info.DateTime.Day.ToString();
            await FileWriter.WriteInFileAsync(directory, name, $"{info.DateTime:HH:mm:ss} : {info.OnlineInfo}{Environment.NewLine}");
        }

        public UserOnlineData ReadData(long id, DateTime from, DateTime to)
        {
            var result = new List<DateOnline>();

            for (var i = from; i < to; i = i.AddDays(1))
            {
                var directory = $"{_fileRepositorySettings.FileDataPath}\\{id}\\{i.Year}\\{i.Month}";
                var path = $"{directory}\\{i.Day}.txt";

                var temp = i;
                result.AddRange(ReadDataFromFile(path)
                    .Select(d => new DateOnline
                    {
                        Date = new DateTime(temp.Year, temp.Month, temp.Day) + d.time,
                        OnlineInfo = d.info
                    }));
            }

            return new UserOnlineData { Id = id, OnlineInfos = result };
        }

        public UserOnlineData ReadDataByDay(long id, DateTime day)
        {
            var directory = $"{_fileRepositorySettings.FileDataPath}\\{id}\\{day.Year}\\{day.Month}";
            var path = $"{directory}\\{day.Day}.txt";

            return new UserOnlineData
            {
                Id = id,
                OnlineInfos = ReadDataFromFile(path)
                    .Select(d => new DateOnline
                    {
                        Date = new DateTime(day.Year, day.Month, day.Day) + d.time,
                        OnlineInfo = d.info
                    })
            };
        }

        public async Task<UserOnlineData> ReadDataAsync(long id, DateTime from, DateTime to)
        {
            var result = new List<DateOnline>();

            for (var i = from; i < to; i = i.AddDays(1))
            {
                var directory = $"{_fileRepositorySettings.FileDataPath}\\{id}\\{i.Year}\\{i.Month}";
                var path = $"{directory}\\{i.Day}.txt";

                var readedData = await ReadDataFromFileAsync(path);

                var temp = i;
                result.AddRange(readedData
                    .Select(d => new DateOnline
                    {
                        Date = new DateTime(temp.Year, temp.Month, temp.Day) + d.time,
                        OnlineInfo = d.info
                    }));
            }

            return new UserOnlineData { Id = id, OnlineInfos = result };
        }

        public async Task<UserOnlineData> ReadDataByDayAsync(long id, DateTime day)
        {
            var directory = $"{_fileRepositorySettings.FileDataPath}\\{id}\\{day.Year}\\{day.Month}";
            var path = $"{directory}\\{day.Day}.txt";

            return new UserOnlineData
            {
                Id = id,
                OnlineInfos = (await ReadDataFromFileAsync(path))
                    .Select(d => new DateOnline
                    {
                        Date = new DateTime(day.Year, day.Month, day.Day) + d.time,
                        OnlineInfo = d.info
                    })
            };
        }

        private static IEnumerable<(TimeSpan time, OnlineInfo info)> ReadDataFromFile(string path)
        {
            return System.IO.File.ReadAllLines(path).Select(ConvertLineToInfo);
        }

        private static async Task<IEnumerable<(TimeSpan time, OnlineInfo info)>> ReadDataFromFileAsync(string path)
        {
            return (await System.IO.File.ReadAllLinesAsync(path)).Select(ConvertLineToInfo);
        }

        private static (TimeSpan, OnlineInfo) ConvertLineToInfo(string line)
        {
            var splittedLine = line.Split(new[] { " : " }, StringSplitOptions.RemoveEmptyEntries);
            var time = TimeSpan.Parse(splittedLine[0]);
            var info = Enum.Parse<OnlineInfo>(splittedLine[1]);

            return (time, info);
        }


    }
}
