// File Path: Application/Dto/EmailSender/EmailSenderRequestDto.cs
// Namespace: Application.Dto.EmailSender
using System.Collections.Generic;

namespace Application.Dto.EmailSender
{
    public class EmailSenderRequestDto
    {
        public List<string> To { get; set; } = new List<string>();
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string? From { get; set; } // አማራጭ ነው
        public bool IsHtml { get; set; } = true; // ኢሜይሉ HTML መሆኑን ያሳያል
        // public string? LogoBase64 { get; set; } // ይህ መስመር ተወግዷል
    }
}