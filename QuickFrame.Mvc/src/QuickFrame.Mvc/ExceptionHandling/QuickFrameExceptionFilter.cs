using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace QuickFrame.Mvc.ExceptionHandling
{
	public class QuickFrameExceptionFilter : IExceptionFilter {
		public void OnException(ExceptionContext context) {
			var response = context.HttpContext.Response;
			var exceptionType = context.Exception.GetType();
			response.ContentType = "application/json";
			if(exceptionType == typeof(ArgumentException)) {
				response.StatusCode = (int)HttpStatusCode.BadRequest;
			} else {
				response.StatusCode = (int)HttpStatusCode.InternalServerError;
			}
			response.WriteAsync(context.Exception.Message);
		}
	}
}
