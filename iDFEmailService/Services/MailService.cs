using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Excel;
using iDFEmailService.Model;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Search;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EmailService.Services
{
    public class MailService
    {
        private readonly MailSettings _mailSettings;
        private readonly IConfiguration _config;
        public MailService(IConfiguration config)
        {
            _config = config;
            _mailSettings = new MailSettings
            {
                Host = _config.GetSection("MailSettings").GetSection("Host").Value,
                Mail = _config.GetSection("MailSettings").GetSection("Mail").Value,
                Password = _config.GetSection("MailSettings").GetSection("Password").Value,
                IMAPPort = int.Parse(_config.GetSection("MailSettings").GetSection("IMAPPort").Value),
                SMTPPort = int.Parse(_config.GetSection("MailSettings").GetSection("SMTPPort").Value)
            };
        }
        public MailService(IOptions<MailSettings> mailSettings,
             IConfiguration config) : this(config)
        {

        }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(mailRequest.SenderEmail.Mail);
            mailRequest.ToEmail.ForEach(t => email.Bcc.Add(MailboxAddress.Parse(t)));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            //if (mailRequest.Attachments != null)
            //{
            //    byte[] fileBytes;
            //    foreach (var file in mailRequest.Attachments)
            //    {
            //        if (file.Length > 0)
            //        {
            //            using (var ms = new MemoryStream())
            //            {
            //                file.CopyTo(ms);
            //                fileBytes = ms.ToArray();
            //            }
            //            //builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
            //            builder.Attachments.Add(file.FileName, fileBytes);
            //        }
            //    }
            //}
            builder.HtmlBody = "<pre>" + mailRequest.Body.Replace("\r\n", "<br/>") + "</pre>";
            email.Body = builder.ToMessageBody();
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(mailRequest.SenderEmail.Host, mailRequest.SenderEmail.SMTPPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(mailRequest.SenderEmail.Mail, mailRequest.SenderEmail.Password);
            //var secrets = new ClientSecrets
            //{
            //    ClientId = "144417964053-5ce9gr4fia8bsgqdppecusnhmdu3mhk9.apps.googleusercontent.com",
            //    ClientSecret = "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"
            //};
            //var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            //        secrets,
            //        new[] { GmailService.Scope.MailGoogleCom }, _mailSettings.Mail, CancellationToken.None);
            //if (credential.Token.IsExpired(SystemClock.Default))
            //{
            //    await credential.RefreshTokenAsync(CancellationToken.None);
            //}
            //var networkCredentials = new NetworkCredential(_mailSettings.Mail, credential.Token.AccessToken);
            //smtp.Authenticate(networkCredentials);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public async Task<string[]> CheckDeliveryStatus(string msgId, MailRequest mailRequest, List<MailSettings> senders)
        {
            var list = Array.Empty<string>();
            foreach (var sender in senders)
            {
                using var imap = new ImapClient();
                imap.Connect(sender.Host, sender.IMAPPort, SecureSocketOptions.SslOnConnect);
                imap.Authenticate(sender.Mail, sender.Password);
                var inbox = imap.Inbox;
                var sent = imap.GetFolder(SpecialFolder.Sent);
                sent.Open(FolderAccess.ReadOnly);
                var orgemailIds = sent.Search(SearchQuery.BodyContains(msgId));
                if (orgemailIds.Count > 0)
                {
                    SearchQuery query = null;
                    if (orgemailIds.Count > 1)
                    {
                        foreach (var id in orgemailIds)
                        {
                            var org = sent.GetMessage(id);
                            var q = SearchQuery.HeaderContains("In-Reply-To", org.MessageId);

                        }
                        for (int i = 0; i < orgemailIds.Count; i += 2)
                        {
                            var org1 = sent.GetMessage(orgemailIds[i]);
                            var q1 = SearchQuery.HeaderContains("In-Reply-To", org1.MessageId);
                            if (orgemailIds.Count > i + 1)
                            {
                                var org2 = sent.GetMessage(orgemailIds[i + 1]);
                                var q2 = SearchQuery.HeaderContains("In-Reply-To", org2.MessageId);
                                if (query != null)
                                {
                                    query = SearchQuery.Or(query, SearchQuery.Or(q1, q2));
                                }
                                else
                                {
                                    query = SearchQuery.Or(q1, q2);
                                }
                            }
                            else
                            {
                                if (query != null)
                                {
                                    query = SearchQuery.Or(query, q1);
                                }
                                else
                                {
                                    query = q1;
                                }
                            }
                        }
                    }
                    else
                    {
                        var orgemail = sent.GetMessage(orgemailIds[0]);
                        query = SearchQuery.HeaderContains("In-Reply-To", orgemail.MessageId);
                    }
                    inbox.Open(FolderAccess.ReadWrite);
                    var emailIds = await inbox.SearchAsync(SearchQuery.And(query,
                        SearchQuery.Or(SearchQuery.FromContains("mailer-daemon@googlemail.com"), SearchQuery.FromContains("Mail Delivery Subsystem"))));
                    foreach (var id in emailIds)
                    {
                        var msg = inbox.GetMessage(id);
                        list = GetUndeliverableAddresses(msg);
                    }
                }
                imap.Disconnect(true);
            }
            return list;
        }
        public async Task SendWelcomeEmailAsync(WelcomeRequest request)
        {
            string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\WelcomeTemplate.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("[username]", request.UserName).Replace("[email]", request.ToEmail);
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(request.ToEmail));
            email.Subject = $"Welcome {request.UserName}";
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.SMTPPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
        string[] GetUndeliverableAddresses(MimeMessage message)
        {
            var report = message.Body as MultipartReport;

            if (report == null)
                return Array.Empty<string>();// throw new ArgumentException("This is not a multipart/report!");

            MessageDeliveryStatus status = null;

            for (int i = 0; i < report.Count; i++)
            {
                status = report[i] as MessageDeliveryStatus;
                if (status != null)
                    break;
            }

            if (status == null)
                return Array.Empty<string>(); // throw new ArgumentException("Did not contain a message/delivery-status!");

            var undeliverables = new List<string>();
            for (int i = 0; i < status.StatusGroups.Count; i++)
            {
                var recipient = status.StatusGroups[i]["Final-Recipient"];
                if (recipient == null) continue;
                int semicolon = recipient.IndexOf(';');

                var undeliverable = recipient.Substring(semicolon + 1).Trim();
                undeliverables.Add(undeliverable);
            }

            return undeliverables.ToArray();
        }
    }
}
