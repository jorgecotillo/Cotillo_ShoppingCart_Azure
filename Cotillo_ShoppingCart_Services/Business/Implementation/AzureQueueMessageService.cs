using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using Cotillo_ShoppingCart_Services.Business.Interface;
using System.Configuration;

namespace Cotillo_ShoppingCart_Services.Business.Implementation
{
    public class AzureQueueMessageService : IQueueMessageService
    {
        public void QueueMessage<TMessage>(string queueReference, TMessage message, bool serializeMessage = false)
        {
            try
            {
                // Retrieve storage account from connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount
                    .Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

                // Create the queue client.
                CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

                // Retrieve a reference to a queue.
                CloudQueue queue = queueClient.GetQueueReference(queueReference.ToLower());

                // Create the queue if it doesn't already exist.
                queue.CreateIfNotExists();

                // Create a message and add it to the queue.
                string messageToQueue = string.Empty;
                if (serializeMessage)
                    messageToQueue = JsonConvert.SerializeObject(message);
                else
                    messageToQueue = message.ToString();

                CloudQueueMessage queueMessage = new CloudQueueMessage(messageToQueue);
                queue.AddMessage(queueMessage);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
