using System.Collections.Generic;
using System.Threading.Tasks;

namespace collNotes.DeviceServices.Email
{
    public interface IEmailService
    {
        Task SendEmail(string subject, string body, List<string> recipients, string filepath);
    }
}
