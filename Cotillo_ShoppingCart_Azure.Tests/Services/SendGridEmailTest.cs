using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cotillo_ShoppingCart_Services.Business.Implementation;

namespace Cotillo_ShoppingCart_Azure.Tests.Services
{
    [TestClass]
    public class SendGridEmailTest
    {
        [TestMethod]
        public void Send_Email_Sync_No_Exceptions()
        {
            SendGridEmailProvider email = new SendGridEmailProvider();
            email.SendEmail(new Cotillo_ShoppingCart_Services.Domain.Model.Message.EmailEntity()
            {
                Body = "Hola",
                From = "jcotillo@collaborative.com",
                Subject = "Test",
                To = "jorge.cotillo@gmail.com"
            });
        }
    }
}
