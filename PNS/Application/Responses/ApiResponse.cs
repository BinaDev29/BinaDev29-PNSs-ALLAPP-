// File Path: Application.Responses.ApiResponse<T>.cs
using System.Collections.Generic;

namespace Application.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty; // Non-nullable warning ለመከላከል
        public T? Data { get; set; }
        public List<string>? Errors { get; set; } // ይህ nullable ስለሆነ ችግር የለውም

        // Success ምላሽ ለመፍጠር
        public ApiResponse(T data, string message = "Request successful.")
        {
            Data = data;
            Message = message;
            Success = true;
            Errors = null; // Success ከሆነ Errors null ነው
        }

        // Error ምላሽ ለመፍጠር
        public ApiResponse(List<string> errors, string message = "An error occurred.")
        {
            Errors = errors;
            Message = message;
            Success = false;
            Data = default; // Data null መሆን አለበት
        }

        // ባዶ ምላሽ ለመፍጠር (message ብቻ)
        public ApiResponse(string message = "Request successful.")
        {
            Message = message;
            Success = true;
            Data = default;
            Errors = null;
        }
    }
}