using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkAnalyzer.BE;
using VkAnalyzer.Interfaces;

namespace VkAnalyzer.BL.File
{
    public class FileUsersRepository : IUsersRepository
    {
        private readonly FileRepositorySettings _fileRepositorySettings;
        public FileUsersRepository(FileRepositorySettings fileRepositorySettings)
        {
            _fileRepositorySettings = fileRepositorySettings ?? throw new ArgumentNullException(nameof(fileRepositorySettings));
        }

        public IEnumerable<long> GetUsers()
        {
            return System.IO.File.ReadAllLines(_fileRepositorySettings.FileUsersRepositoryPath).Select(long.Parse);
        }

        public async Task<IEnumerable<long>> GetUsersAsync()
        {
            var lines = await System.IO.File.ReadAllLinesAsync(_fileRepositorySettings.FileUsersRepositoryPath);
            return lines.Select(long.Parse);
        }

        public void AddUser(long id)
        {
            FileWriter.WriteInFile(_fileRepositorySettings.FileUsersRepositoryPath, id + Environment.NewLine);
        }

        public async Task AddUserAsync(long id)
        {
            await FileWriter.WriteInFileAsync(_fileRepositorySettings.FileUsersRepositoryPath, id + Environment.NewLine);
        }
    }
}
