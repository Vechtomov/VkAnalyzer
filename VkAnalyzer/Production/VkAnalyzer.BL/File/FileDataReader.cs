using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkAnalyzer.BE;

namespace VkAnalyzer.BL.File
{
    public class FileDataReader 
    {
        private readonly string _directoryPath;
        public FileDataReader(string directoryPath = "")
        {
            _directoryPath = directoryPath;
        }

        public UserOnlineData ReadData(long id, DateTime from, DateTime to)
        {
            var result = new List<DateOnline>();

            for (var i = from; i < to; i = i.AddDays(1))
            {
                var directory = $"{_directoryPath}\\{id}\\{i.Year}\\{i.Month}";
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
            var directory = $"{_directoryPath}\\{id}\\{day.Year}\\{day.Month}";
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
                var directory = $"{_directoryPath}\\{id}\\{i.Year}\\{i.Month}";
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
            var directory = $"{_directoryPath}\\{id}\\{day.Year}\\{day.Month}";
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
