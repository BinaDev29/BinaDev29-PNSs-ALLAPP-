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
        public bool IsHtml { get; set; } = true; // ይህንን ጨምር
    }
}