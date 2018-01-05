using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Entities;

namespace WebApp.Services
{
    public class SeedDataService : ISeedDataService
    {
        private MyDBContext _mydbcontext;
        public SeedDataService(MyDBContext mydbcontext)
        {
            _mydbcontext = mydbcontext;
        }
        public async Task EnsureSeedData()
        {
            _mydbcontext.Database.EnsureCreated();

            _mydbcontext.Customers.RemoveRange(_mydbcontext.Customers);
            _mydbcontext.SaveChanges();

            Customer customer = new Customer();
            customer.Firstname = "Tina";
            customer.Lastname = "Soltanian";
            customer.Age = 36;
            customer.Id = Guid.NewGuid();

            _mydbcontext.Customers.Add(customer);
            _mydbcontext.SaveChanges();

            Customer customer2 = new Customer();
            customer2.Firstname = "John";
            customer2.Lastname = "nash";
            customer2.Age = 60;
            customer2.Id = Guid.NewGuid();

            _mydbcontext.Customers.Add(customer2);
            _mydbcontext.SaveChanges();

            Customer customer3 = new Customer();
            customer3.Firstname = "Donald";
            customer3.Lastname = "Trump";
            customer3.Age = 60;
            customer3.Id = Guid.NewGuid();

            _mydbcontext.Customers.Add(customer3);
            _mydbcontext.SaveChanges();

            for (int i = 1; i < 6; i++)
            {
                Customer c = new Customer();
                c.Firstname = "Customer" + i.ToString();
                c.Lastname = "Customer" + i.ToString();
                c.Age = i + 30;
                c.Id = Guid.NewGuid();

                _mydbcontext.Customers.Add(c);
                _mydbcontext.SaveChanges();
            }

            await _mydbcontext.SaveChangesAsync();
        }
    }
}
