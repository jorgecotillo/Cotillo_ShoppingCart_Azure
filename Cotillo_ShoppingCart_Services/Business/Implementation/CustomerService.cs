using Cotillo_ShoppingCart_Services.Business.Interface;
using Cotillo_ShoppingCart_Services.Domain.Model.Customer;
using Cotillo_ShoppingCart_Services.Integration.Interfaces.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Business.Implementation
{
    public class CustomerService : CRUDCommonService<CustomerEntity>, ICustomerService
    {
        #region Fields
        private readonly IRepository<CustomerEntity> customerRepository;
        #endregion

        #region Constructor
        public CustomerService(IRepository<CustomerEntity> customerRepository)
            : base(customerRepository)
        {
            this.customerRepository = customerRepository;
        }
        #endregion

        //public async Task<IList<CustomerEntity>> GetEmployersByName(string employerName)
        //{
        //    try
        //    {
        //        IList<CustomerEntity> employers = await (from employer in customerRepository.Table
        //                                                 where employer.Name.ToLower().Contains(employerName.ToLower())
        //                                                 select employer).ToListAsync();
        //        return employers;
        //    }
        //    catch (AggregateException)
        //    {
        //        throw;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
    }
}
