// File Path: Application.Response.BaseCommandResponse.cs
using System;
using System.Collections.Generic;

namespace Application.Responses
{
    public class BaseCommandResponse
    {
        public Guid Id { get; set; } = Guid.Empty; // Non-nullable warning ለመከላከል
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty; // Non-nullable warning ለመከላከል
        public List<string> Errors { get; set; } = new List<string>(); // Non-nullable warning ለመከላከል
        public string Status { get; set; } = string.Empty; // Non-nullable warning ለመከላከል
    }
}