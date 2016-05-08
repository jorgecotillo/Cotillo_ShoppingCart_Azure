using Cotillo_ShoppingCart_Services.Business.Implementation;
using Cotillo_ShoppingCart_Services.Business.Interface;
using Cotillo_ShoppingCart_Services.Caching.Implementation;
using Cotillo_ShoppingCart_Services.Caching.Interface;
using Cotillo_ShoppingCart_Services.Integration.Implementation.EF;
using Cotillo_ShoppingCart_Services.Integration.Interfaces.EF;
using SimpleInjector;

namespace Cotillo_ShoppingCart_Services.IoCContainer
{
    /// <summary>
    /// 
    /// </summary>
    public class IoCServiceRegistration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        public void Register(IoCServiceContainer container, IoCLifestyleScope scope = IoCLifestyleScope.None)
        {
            if(container == null) { container = new IoCServiceContainer(); }

            switch (scope)
            {
                case IoCLifestyleScope.None:
                    break;
                case IoCLifestyleScope.WebRequest:
                    container.UseWebRequestLifestyle();
                    break;
                case IoCLifestyleScope.WebApi:
                    container.UseWebApiRequestLifestyle();
                    break;
                default:
                    break;
            }

            container.Register<IDbContext>(() => new EFContext(), Lifestyle.Scoped);
            container.RegisterOpenGeneric(typeof(IRepository<>), typeof(EFRepository<>), Lifestyle.Scoped);

            //Register Services here
            container.Register<IProductService, ProductService>(Lifestyle.Scoped);

            container.Register<IImageService, AzureBlobImageService>(Lifestyle.Scoped);

            container.Register<IQueueMessageService, AzureQueueMessageService>(Lifestyle.Scoped);

            container.Register<IEmailProvider, SendGridEmailProvider>(Lifestyle.Scoped);

            container.Register<IMessageService, MessageService>(Lifestyle.Scoped);
            
            //Enable Redis Cache
            container.Register<ICacheManager>(() => 
                new RedisCacheManager(
                    redisServerURI: "jcshoppingcart.redis.cache.windows.net", 
                    port: 6380, 
                    ssl: true, 
                    password: "FGYbVlp0YFTdHFVWEV0FSW04pG6qCt2IqpMApJ+fkLM="), Lifestyle.Scoped);
        }
    }
}
