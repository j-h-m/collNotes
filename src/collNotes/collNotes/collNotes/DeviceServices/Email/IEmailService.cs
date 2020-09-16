using System.Collections.Generic;
using System.Threading.Tasks;
using static collNotes.DeviceServices.Email.EmailService;

namespace collNotes.DeviceServices.Email
{
    public interface IEmailService
    {
        Task<Result> SendEmail(string subject, string body, List<string> recipients, string filepath);
    }
}
