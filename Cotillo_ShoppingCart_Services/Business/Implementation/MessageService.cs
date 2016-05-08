using Cotillo_ShoppingCart_Services.Business.Interface;
using System;

using Cotillo_ShoppingCart_Services.Domain.Model.Message;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Business.Implementation
{
    public class MessageService : IMessageService
    {
        readonly IQueueMessageService _queueMessageService;
        readonly IEmailProvider _emailProvider;
        public MessageService(IQueueMessageService queueMessageService, IEmailProvider emailProvider)
        {
            _queueMessageService = queueMessageService;
            _emailProvider = emailProvider;
        }

        public void QueueEmail(EmailEntity emailEntity)
        {
            try
            {
                _queueMessageService
                    .QueueMessage(queueReference: "EmailQueue", message: emailEntity, serializeMessage: true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SendEmail(EmailEntity emailEntity)
        {
            try
            {
                _emailProvider.SendEmail(emailEntity);
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
                await _emailProvider.SendEmailAsync(emailEntity);   
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
