using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace TrusteeApp.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public MailJetSettings? _mailJetSettings { get; set; }

        public EmailSender(IConfiguration configuration) => _configuration = configuration;

        public Task SendEmailAsync(string email, string subject, string htmlMessage) => Execute(email, subject, htmlMessage);

        public async Task Execute(string email, string subject, string body)
        {
            _mailJetSettings = _configuration.GetSection("MailJet").Get<MailJetSettings>()!;

            MailjetClient client = new MailjetClient(_mailJetSettings.ApiKey, _mailJetSettings.SecretKey);

            MailjetRequest request = new MailjetRequest { Resource = Send.Resource }
           .Property(Send.FromEmail, WebConstants.EmailAdmin)
           .Property(Send.FromName, "Trustees")
           .Property(Send.Subject, subject)
           .Property(Send.TextPart, "")
           .Property(Send.HtmlPart, body)
           .Property(Send.Recipients, new JArray {
                new JObject {
                 {"Email", email}
                 }
               });
            MailjetResponse response = await client.PostAsync(request);
        }
    }
}
