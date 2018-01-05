using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Dtos;
using WebApp.Entities;
using WebApp.QueryParameter;
using WebApp.Repositories;

namespace WebApp.Controllers
{
    //[ApiVersion("2.0")]
    [Route("api/[controller]")]
   // [Authorize(Policy = "resourcesAdmin")]
    public class Customer2Controller : Controller
    {
        private ICustomerRepository _customerRepository;
        ILogger<CustomerController> _logger;
        public Customer2Controller(ICustomerRepository customerRepository, ILogger<CustomerController> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
            logger.LogInformation("customercontroler has started!");
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Customer>), 200)]
        public IActionResult GetAllCustomer(CustomerQueryParameter customerQueryParameter)
        {

            _logger.LogInformation("------> GetAllCustomers version 1");
            var allcustomers = _customerRepository.GetAll(customerQueryParameter).ToList();

            var allCustomersDto = allcustomers.Select(x => Mapper.Map<CustomerDto>(x));

            Response.Headers.Add("x-Paging", JsonConvert.SerializeObject(new { totalCount = _customerRepository.Count() }));

            return Ok(allCustomersDto);
        }

    }
}
