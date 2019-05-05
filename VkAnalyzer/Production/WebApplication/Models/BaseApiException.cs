using System;

namespace WebApplication.Models
{
	/// <inheritdoc />
	/// <summary>
	/// Класс стандартного исключения API, обрабатывается глобальным 
	/// обработчиком ошибок, который возвращает 200 код.
	/// Необработанные исключения, не отнаследованные от него, будут возвращаться с 500 кодом.
	/// </summary>
	public class BaseApiException : Exception
	{
	}
}
