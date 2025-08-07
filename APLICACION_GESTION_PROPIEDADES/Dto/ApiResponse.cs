﻿namespace APLICACION_GESTION_PROPIEDADES.Dto
{
	public class ApiResponse<T>
	{
		public bool Success { get; set; }
		public string? Message { get; set; }
		public T? Data { get; set; }

		public static ApiResponse<T> Ok(T data, string message = "")
		{
			return new ApiResponse<T> { Success = true, Data = data, Message = message };
		}

		public static ApiResponse<T> Fail(string message)
		{
			return new ApiResponse<T> { Success = false, Message = message };
		}
	}

}
