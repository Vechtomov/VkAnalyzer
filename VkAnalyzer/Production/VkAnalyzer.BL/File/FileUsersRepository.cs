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

		public IEnumerable<User> GetUsers()
		{
			return System.IO.File.ReadAllLines(_fileRepositorySettings.FileUsersRepositoryPath).Select(User.Parse);
		}

		public async Task<IEnumerable<User>> GetUsersAsync()
		{
			var lines = await System.IO.File.ReadAllLinesAsync(_fileRepositorySettings.FileUsersRepositoryPath);
			return lines.AsParallel().Select(User.Parse);
		}

		public void AddUser(User user)
		{
			FileWriter.WriteInFile(_fileRepositorySettings.FileUsersRepositoryPath, user + Environment.NewLine);
		}

		public async Task AddUserAsync(User user)
		{
			await FileWriter.WriteInFileAsync(_fileRepositorySettings.FileUsersRepositoryPath, user + Environment.NewLine);
		}
	}
}
