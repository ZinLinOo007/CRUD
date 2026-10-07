using CRUD.CustomerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUD.Services
{
    public class CustomerServices
    {
        private CustomerServiceClient _client;
        public CustomerServices()
        {
            _client = new CustomerServiceClient();
        }

        public List<Customer> GetAllCustomers()
        {
            return _client.GetCustomers().ToList(); 
        }

        public Customer GetCustomerById(int id) {
            return _client.GetCustomerById(id);
        }

        public bool AddCustomer(Customer customer)
        {
            return _client.AddCustomer(customer);
        }

        public bool UpdateCustomer(Customer customer)
        {
            return _client.UpdateCustomer(customer);
        }

        public bool DeleteCustomer(int id)
        {
            return _client.DeleteCustomer(id);
        }   
    }
}