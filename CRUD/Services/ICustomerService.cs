using CRUD.CustomerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Services
{
    public interface ICustomerService
    {
        List<Customer>  GetAllCustomers();
        Customer GetCustomerById(int id);

        bool AddCustomer(Customer customer);
        bool UpdateCustomer(Customer customer);
        bool DeleteCustomer(int id);

    }
}
