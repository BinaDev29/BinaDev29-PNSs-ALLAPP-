// File Path: Application/Contracts/Services/ISmsSenderService.cs
// Namespace: Application.Contracts.Services
using System.Threading.Tasks;
using Application.Dto.SmsSender; // ለ SmsSenderRequestDto

namespace Application.Contracts.Services
{
    public interface ISmsSenderService
    {
        Task<bool> SendSms(SmsSenderRequestDto smsRequest);
    }
}