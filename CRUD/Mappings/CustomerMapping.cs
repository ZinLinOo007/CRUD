using CRUD.CustomerService;
using CRUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUD.Mappings
{
    public class CustomerMapping
    {
        public static List<CustomerList> MapToCustomerList(List<Customer> customers)
        {
            return customers.Select(c => new CustomerList
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                Phone = c.Phone
            }).ToList();
        }

        public static Customer MapToCustomer(CustomerCreate customerCreate)
        {
            return new Customer
            {
                Name = customerCreate.Name,
                Email = customerCreate.Email,
                Phone = customerCreate.Phone,
            };
        }

        public static CustomerEdit MapToCustomerEdit(Customer customer)
        {
            return new CustomerEdit
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone,
            };
        }

        public static Customer MapToCustomer(CustomerEdit customerEdit)
        {
            return new Customer
            {
                Id = customerEdit.Id,
                Name = customerEdit.Name,
                Email = customerEdit.Email,
                Phone = customerEdit.Phone,

            };
        }


        public static CustomerDetail MapToCustomerDetail(Customer customer)
        {
            return new CustomerDetail
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone,
            };
        }
    }
}