using System;

namespace VkAnalyzer.WebApp.Models
{
	/// <inheritdoc />
	/// <summary>
	/// Класс стандартного исключения API, обрабатывается глобальным 
	/// обработчиком ошибок, который возвращает 200 код.
	/// Необработанные исключения, не отнаследованные от него, будут возвращаться с 500 кодом.
	/// </summary>
	public class BaseApiException : Exception
	{
		public BaseApiException()
		{
		}

		public BaseApiException(string message) : base(message)
		{
		}


		public BaseApiException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
