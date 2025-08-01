// File Path: Application.Response.AuthenticationCommandResponse.cs
using System;
using System.Collections.Generic;

namespace Application.Responses
{
    public class AuthenticationCommandResponse
    {
        public Guid UserId { get; set; } = Guid.Empty; // Non-nullable warning ለመከላከል
        public string UserName { get; set; } = string.Empty; // Non-nullable warning ለመከላከል
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty; // Non-nullable warning ለመከላከል
        public List<string> Errors { get; set; } = new List<string>(); // Non-nullable warning ለመከላከል
        public string Token { get; set; } = string.Empty; // Non-nullable warning ለመከላከል
        public string Appid { get; set; } = string.Empty; // Non-nullable warning ለመከላከል
        public double ExpireIn { get; set; } = 0.0; // Non-nullable warning ለመከላከል
    }
}