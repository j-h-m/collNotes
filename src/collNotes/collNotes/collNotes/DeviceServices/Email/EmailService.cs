﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using collNotes.Ef.Context;
using collNotes.Services.Data;
using Xamarin.Essentials;

namespace collNotes.DeviceServices.Email
{
    public class EmailService : IEmailService
    {
        private readonly IExceptionRecordService exceptionRecordService;

        public EmailService(CollNotesContext context)
        {
            this.exceptionRecordService = new ExceptionRecordService(context);
        }

        public async Task SendEmail(string subject, string body, List<string> recipients, string filepath)
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
                await Xamarin.Essentials.Email.ComposeAsync(message);
            }
            catch (Exception ex)
            {
                await exceptionRecordService.CreateExceptionRecord(ex);
            }
        }
    }
}
