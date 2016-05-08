using Cotillo_ShoppingCart_Services.Business.Interface;
using System;
using Cotillo_ShoppingCart_Services.Domain.Model.Message;
using System.Net;
using System.Net.Mail;
using SendGrid;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Business.Implementation
{
    public class SendGridEmailProvider : IEmailProvider
    {
        public void SendEmail(EmailEntity emailEntity)
        {
            try
            {
                // Create the email object first, then add the properties.
                var myMessage = new SendGridMessage();

                // Add the message properties.
                myMessage.From = new MailAddress("john@example.com");

                // Add multiple addresses to the To field.
                List<String> recipients = new List<String>
                {
                    @"Jeff Smith <jeff@example.com>",
                    @"Anna Lidman <anna@example.com>",
                    @"Peter Saddow <peter@example.com>"
                };

                myMessage.AddTo(recipients);

                myMessage.Subject = "Testing the SendGrid Library";

                //Add the HTML and Text bodies
                myMessage.Html = "<p>Hello World!</p>";
                myMessage.Text = "Hello World plain text!";

                // Create network credentials to access your SendGrid account
                var username = "your_sendgrid_username";
                var pswd = "your_sendgrid_password";

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
                // Create the email object first, then add the properties.
                var myMessage = new SendGridMessage();

                // Add the message properties.
                myMessage.From = new MailAddress("john@example.com");

                // Add multiple addresses to the To field.
                List<String> recipients = new List<String>
                {
                    @"Jeff Smith <jeff@example.com>",
                    @"Anna Lidman <anna@example.com>",
                    @"Peter Saddow <peter@example.com>"
                };

                myMessage.AddTo(recipients);

                myMessage.Subject = "Testing the SendGrid Library";

                //Add the HTML and Text bodies
                myMessage.Html = "<p>Hello World!</p>";
                myMessage.Text = "Hello World plain text!";

                // Create network credentials to access your SendGrid account
                var username = "your_sendgrid_username";
                var pswd = "your_sendgrid_password";

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
