using System;
using System.Collections.Generic;
using System.Linq;
using WebApp.Entities;
using WebApp.QueryParameter;

namespace WebApp.Repositories
{
    public interface ICustomerRepository
    {
        void Add(Customer item);
        void Delete(Guid id);
        List<Customer> GetAll(CustomerQueryParameter customerQueryParameter);
        Customer GetSingle(Guid id);
        bool Save();
        int Count();
        void Update(Customer item);
    }
}