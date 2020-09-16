using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using collNotes.Services.Data;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace collNotes.DeviceServices.Email
{
    public class EmailService : IEmailService
    {
        public enum Result
        {
            Success,
            Fail,
            NotSupported
        };

        private readonly IExceptionRecordService exceptionRecordService =
            DependencyService.Get<IExceptionRecordService>(DependencyFetchTarget.NewInstance);

        public EmailService() { }

        public async Task<Result> SendEmail(string subject, string body, List<string> recipients, string filepath)
        {
            Result result;

            try
            {
                ExperimentalFeatures.Enable("EmailAttachments_Experimental"); // required to allow attachment in email
                List<EmailAttachment> emailAttachments = new List<EmailAttachment>();
                EmailAttachment attachment = new EmailAttachment(filepath);
                emailAttachments.Add(attachment);

                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = recipients,
                    Attachments = emailAttachments
                };
                await Xamarin.Essentials.Email.ComposeAsync(message);

                result = Result.Success;
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email is not supported on this device
                await exceptionRecordService.CreateExceptionRecord(fbsEx);
                result = Result.NotSupported;
            }
            catch (Exception ex)
            {
                await exceptionRecordService.CreateExceptionRecord(ex);
                result = Result.Fail;
            }
            return result;
        }
    }
}
