// File Path: Application/Dto/SmsSender/SmsSenderRequestDto.cs
// Namespace: Application.Dto.SmsSender

namespace Application.Dto.SmsSender
{
    public class SmsSenderRequestDto
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}