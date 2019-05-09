using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using VkAnalyzer.WebApp.Models;

namespace VkAnalyzer.WebApp.Middlewares
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;

		public ExceptionMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await _next(httpContext);
			}
			catch (BaseApiException ex)
			{
				await HandleBaseApiExceptionAsync(httpContext, ex);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(httpContext, ex);
			}
		}

		private static Task HandleBaseApiExceptionAsync(HttpContext context, Exception exception)
		{
			context.Response.ContentType = "application/json";

			return context.Response.WriteAsync(JsonConvert.SerializeObject(new BaseErrorResponse
			{
				Error = exception.Message
			}));
		}

		private static Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

			return context.Response.WriteAsync(JsonConvert.SerializeObject(new BaseErrorResponse
			{
				Error = "Произошла непредвиденная ошибка. Пожалуйста, сообщите о" +
					$" ней разработчикам.{Environment.NewLine}Message: {exception.Message}"
			}));
		}
	}
}