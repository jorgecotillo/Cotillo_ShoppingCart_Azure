using Cotillo_ShoppingCart_Services.Domain.Model.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Business.Interface
{
    public interface IEmailProvider
    {
        void SendEmail(EmailEntity emailEntity);
        Task SendEmailAsync(EmailEntity emailEntity);
    }
}
