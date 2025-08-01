// File Path: Application/Contracts/Services/IEmailService.cs
using Application.Dto.EmailSender; // EmailSenderRequestDto እዚህ ካለህ
using System.Threading.Tasks;

namespace Application.Contracts.Services // ትክክለኛው namespace ይህ መሆኑን አረጋግጥ
{
    public interface IEmailService
    {
        // method ስምህ SendEmailAsync መሆኑን አረጋግጥ
        Task<bool> SendEmailAsync(EmailSenderRequestDto emailRequest);
    }
}