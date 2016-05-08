using Cotillo_ShoppingCart_Services.Business.Interface;
using System;
using Cotillo_ShoppingCart_Services.Domain.Model.Message;
using System.Net;
using System.Net.Mail;
using SendGrid;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Configuration;

namespace Cotillo_ShoppingCart_Services.Business.Implementation
{
    public class SendGridEmailProvider : IEmailProvider
    {
        public void SendEmail(EmailEntity emailEntity)
        {
            try
            {
                //Checking invariants
                if (emailEntity == null)
                    throw new NullReferenceException("Email entity cannot be null");

                if (String.IsNullOrWhiteSpace(emailEntity.From))
                    throw new ArgumentException("From cannot be empty");

                if (String.IsNullOrWhiteSpace(emailEntity.To))
                    throw new ArgumentException("To cannot be empty");

                if (String.IsNullOrWhiteSpace(emailEntity.Body))
                    throw new ArgumentException("Body cannot be empty");

                // Create the email object first, then add the properties.
                var myMessage = new SendGridMessage();

                // Add the message properties.
                myMessage.From = new MailAddress(emailEntity.From);

                // Add multiple addresses to the To field.
                List<String> recipients = new List<String>
                {
                    emailEntity.To
                };
                myMessage.AddTo(recipients);

                myMessage.Subject = emailEntity.Subject;

                //Add the HTML and Text bodies
                myMessage.Html = emailEntity.Body;
                //myMessage.Text = "Hello World plain text!";

                // Create network credentials to access your SendGrid account
                var username = ConfigurationManager.AppSettings["SendGridUser"];
                var pswd = ConfigurationManager.AppSettings["SendGridPwd"];

                var credentials = new NetworkCredential(username, pswd);
                // Create an Web transport for sending email.
                var transportWeb = new Web(credentials);

                // Send the email, which returns an awaitable task.
                transportWeb.DeliverAsync(myMessage).Wait();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SendEmailAsync(EmailEntity emailEntity)
        {
            try
            {
                //Checking invariants
                if (emailEntity == null)
                    throw new NullReferenceException("Email entity cannot be null");

                if (String.IsNullOrWhiteSpace(emailEntity.From))
                    throw new ArgumentException("From cannot be empty");

                if (String.IsNullOrWhiteSpace(emailEntity.To))
                    throw new ArgumentException("To cannot be empty");

                if (String.IsNullOrWhiteSpace(emailEntity.Body))
                    throw new ArgumentException("Body cannot be empty");

                // Create the email object first, then add the properties.
                var myMessage = new SendGridMessage();

                // Add the message properties.
                myMessage.From = new MailAddress(emailEntity.From);

                // Add multiple addresses to the To field.
                List<String> recipients = new List<String>
                {
                    emailEntity.To
                };
                myMessage.AddTo(recipients);

                myMessage.Subject = emailEntity.Subject;

                //Add the HTML and Text bodies
                myMessage.Html = emailEntity.Body;
                //myMessage.Text = "Hello World plain text!";

                // Create network credentials to access your SendGrid account
                var username = ConfigurationManager.AppSettings["SendGridUser"];
                var pswd = ConfigurationManager.AppSettings["SendGridPwd"];

                var credentials = new NetworkCredential(username, pswd);
                // Create an Web transport for sending email.
                var transportWeb = new Web(credentials);

                // Send the email, which returns an awaitable task.
                await transportWeb.DeliverAsync(myMessage);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
