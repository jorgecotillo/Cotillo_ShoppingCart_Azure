//using Cotillo_ShoppingCart_Services.Business.Implementation;
//using Cotillo_ShoppingCart_Services.Domain.Model.Message;

using Cotillo_ShoppingCart_Services.Business.Implementation;
using Cotillo_ShoppingCart_Services.Domain.Model.Message;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Email_WebJob
{
    public class Program
    {
        public static void Main()
        {
            //JobHost host = new JobHost(new JobHostConfiguration(ConfigurationManager.AppSettings["AzureWebJobsDashboard"]));
            //host.RunAndBlock();

            // DEMO Configure Queue default behavior
            JobHostConfiguration config = new JobHostConfiguration();

            // Maximum number of messages to be processed in parallel
            // Default is 16 messages
            config.Queues.BatchSize = 6;

            // Maximum times to retry a message before moving it to the poison message queue
            // Default is 5 retries
            config.Queues.MaxDequeueCount = 3;

            config.UseCore();

            // Maximum time to wait before polling the queue when it is empty
            // Default is 1 minute
            // config.Queues.MaxPollingInterval = TimeSpan.FromSeconds(15);

            // Setup the JobHost with the custom configuration. 
            var host = new JobHost(config);

            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();
        }

        public static void ProcessQueueMessage([QueueTrigger("emailqueue")] string queuedEmail, TextWriter logger)
        {
            logger.WriteLine("About to send an email");

            EmailEntity emailEntity = JsonConvert.DeserializeObject<EmailEntity>(queuedEmail);

            //Create message service instance
            MessageService messageService = new MessageService(new AzureQueueMessageService(), new SendGridEmailProvider());

            //Send the email if there is a failure, retry 5 times
            int retry = 5;

            while (retry > 0)
            {
                try
                {
                    messageService.SendEmail(emailEntity);
                    break;
                }
                catch (Exception ex)
                {
                    logger.WriteLine($"Email send failed, exception: {ex.Message}");
                    retry--;

                    if (retry == 0)
                    {
                        //Enqueue the message again so it can be sent at some point again
                        messageService.QueueEmail(emailEntity);
                    }
                }
            }
        }
    }
}