using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Entities;
using WebApp.QueryParameter;
using System.Linq.Dynamic.Core;

namespace WebApp.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private MyDBContext _context;
        public CustomerRepository(MyDBContext context)
        {
            _context = context;
        }

        public List<Customer> GetAll(CustomerQueryParameter customerQueryParameter)
        {
            List<Customer> customers = _context.Customers.OrderBy(customerQueryParameter.OrderBy, customerQueryParameter.Descending).ToList();

            if (customerQueryParameter.HasQuery)
            {
                customers = customers.Where(a => a.Firstname.ToLowerInvariant().Contains(customerQueryParameter.Query.ToLowerInvariant())
               || (a.Lastname.ToLowerInvariant().Contains(customerQueryParameter.Query.ToLowerInvariant()))
                ).ToList();
            }

            return customers
                .ToList().Skip(customerQueryParameter.PageCount * (customerQueryParameter.Page - 1))
                .Take(customerQueryParameter.PageCount).ToList();
        }
        public Customer GetSingle(Guid id)
        {
            return _context.Customers.FirstOrDefault(x => x.Id == id);
        }

        public void Add(Customer item)
        {
            _context.Customers.Add(item);
        }

        public void Delete(Guid id)
        {
            Customer customer = GetSingle(id);
            _context.Customers.Remove(customer);
        }

        public void Update(Customer item)
        {
            _context.Customers.Update(item);
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }

        public int Count()
        {
            return _context.Customers.Count();
        }
    }
}
