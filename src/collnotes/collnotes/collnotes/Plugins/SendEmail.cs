using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace collnotes.Plugins
{
    public static class SendEmail
    {
        /// <summary>
        /// Uses email to allow the User to export their data.
        /// Automatically adds the file as an attachment.
        /// Only used on iOS.
        /// </summary>
        /// <returns>The email.</returns>
        /// <param name="subject">Subject.</param>
        /// <param name="body">Body.</param>
        /// <param name="recipients">Recipients.</param>
        /// <param name="filepath">Filepath.</param>
        public static async Task CreateAndSend(string subject, string body, List<string> recipients, string filepath)
        {
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
                await Email.ComposeAsync(message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
