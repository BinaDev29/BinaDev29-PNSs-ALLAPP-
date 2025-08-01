// File Path: Infrastructure/Services/EmailService.cs
using Application.Contracts.Services; // IEmailServiceን ለመጠቀም
using Application.Dto.EmailSender; // EmailSenderRequestDtoን ለመጠቀም
using System.Threading.Tasks;
using System; // ለ Console.WriteLine

namespace Infrastructure.Services // ትክክለኛው namespace ይህ መሆኑን አረጋግጥ
{
    // EmailService IEmailServiceን implement ማድረጉን እርግጠኛ ሁን
    public class EmailService : IEmailService
    {
        public async Task<bool> SendEmailAsync(EmailSenderRequestDto emailRequest)
        {
            // የኢሜይል የመላክ አመክንዮ እዚህ ይመጣል
            // ለጊዜው ስህተት እንዳይሆን
            Console.WriteLine($"Sending email to: {string.Join(", ", emailRequest.To)}");
            Console.WriteLine($"Subject: {emailRequest.Subject}");
            Console.WriteLine($"Body: {emailRequest.Body}");
            Console.WriteLine($"Is HTML: {emailRequest.IsHtml}");
            return await Task.FromResult(true);
        }
    }
}