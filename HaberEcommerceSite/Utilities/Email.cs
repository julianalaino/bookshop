using HaberEcommerceSite.ViewModels;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace HaberEcommerceSite
{
    public class Email
    {
        private MailMessage message;

        private SmtpClient SMTP;

        public Email(SmtpConfig smtpConfig)
        {
            
            this.message = new MailMessage
            {
                From = new MailAddress(smtpConfig.User, "Contacto Web"),

                IsBodyHtml = false,

                Priority = MailPriority.Normal
            };

            SMTP = new SmtpClient
            {
                Host = smtpConfig.Server,

                Port = smtpConfig.Port,

                Credentials = new NetworkCredential(smtpConfig.User, smtpConfig.Pass),

                EnableSsl = smtpConfig.SSL,
                
                
            };
        }
        
        public void Attach(string path)
        {
            if (File.Exists(path))
            {
                Attachment attachment = new Attachment(path);

                this.message.Attachments.Add(attachment);
            }
        }

        public void AddTo(string address)
        {
            this.message.To.Add(new MailAddress(address));
        }

        public void AddBcc(string address)
        {
            this.message.Bcc.Add(new MailAddress(address));
        }

        public void AddCc(string address)
        {
            this.message.CC.Add(new MailAddress(address));
        }

        public string Subject {

            get { return this.message.Subject; }

            set { this.message.Subject = value; }

        }

        public string Body {

            get { return this.message.Body; }

            set { this.message.Body = value; }

        }

        public bool Send(out string message)
        {
            try
            {
                this.SMTP.Send(this.message);

                message = "OK";

                return true;
            }
            catch (Exception exception)
            {
                message = "Error: " + exception.Message;

                return false;
            }
        }

        void Dispose()
        {
            this.message.Dispose();

            this.SMTP.Dispose();
        }
    }
}