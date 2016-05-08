using Cotillo_ShoppingCart_Services.Business.Implementation;
using Cotillo_ShoppingCart_Services.Domain.Model.Message;
using Microsoft.Azure.Jobs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure_WebJob
{
    class Program
    {
        public static void Main()
        {
            JobHost host = new JobHost(new JobHostConfiguration(ConfigurationManager.AppSettings["AzureWebJobsDashboard"]));
            host.RunAndBlock();
        }

        public static void ProcessQueueMessage([QueueTrigger("EmailQueue")] EmailEntity queuedEmail, TextWriter logger)
        {
            logger.WriteLine("About to send an email");

            //Create message service instance
            MessageService messageService = new MessageService(new AzureQueueMessageService(), new SendGridEmailProvider());

            //Send the email if there is a failure, retry 5 times
            int retry = 5;

            while (retry > 0)
            {
                try
                {
                    messageService.SendEmail(queuedEmail);
                    break;
                }
                catch (Exception ex)
                {
                    logger.WriteLine($"Email send failed, exception: {ex.Message}");
                    retry--;

                    if(retry == 0)
                    {
                        //Enqueue the message again so it can be sent at some point again
                        messageService.QueueEmail(queuedEmail);
                    }
                }
            }
        }
    }
}