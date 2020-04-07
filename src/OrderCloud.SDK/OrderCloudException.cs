﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Flurl.Http;

namespace OrderCloud.SDK
{
	public class OrderCloudException : Exception
	{
		public HttpStatusCode? HttpStatus { get; }
		public ApiError[] Errors { get; }

		internal OrderCloudException(HttpCall call, ApiError[] errors) : base(BuildMessage(call, errors), call.Exception) {
			HttpStatus = call.HttpStatus;
			Errors = errors;
		}

		private static string BuildMessage(HttpCall call, ApiError[] errors) {
			var code = errors?.FirstOrDefault()?.ErrorCode;
			var msg = errors?.FirstOrDefault()?.Message;
			if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(msg))
				return $"{code}: {msg}";

			return new[] { code, msg, call?.Exception?.Message, "An unknown error occurred." }
				.FirstOrDefault(x => !string.IsNullOrEmpty(x));
		}
	}

	internal class ApiErrorResponse
	{
		public ApiError[] Errors { get; set; }
	}

	internal class AuthErrorResponse
	{
		public string error { get; set; }
		public string error_description { get; set; }
	}

	public class ApiError
	{
		public string ErrorCode { get; set; }
		public string Message { get; set; }
		public object Data { get; set; }
	}
}
