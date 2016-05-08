using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Business.Interface
{
    public interface IQueueMessageService
    {
        void QueueMessage<TMessage>(string queueReference, TMessage message, bool serializeMessage = false);
    }
}
