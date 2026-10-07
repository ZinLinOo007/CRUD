
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRUD.Models;
using CRUD.CustomerService;

namespace CRUD.Controllers
{
    public class CustomerController : Controller
    {
        private Services.CustomerService _customerService;
        public CustomerController()
        {
            this._customerService = new Services.CustomerService();
        }
        public ActionResult Index()
        {
            var customer = _customerService.GetAllCustomers();
            var customerList = customer.Select(c => new CustomerList
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                Phone = c.Phone
            }).ToList();
            return View(customerList);
            
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new CustomerCreate());
        }

        [HttpPost]
        public ActionResult Create(CustomerCreate customerCreate) {

            
            if (ModelState.IsValid)
            {
                var customer = new Customer
                {
                    Name = customerCreate.Name,
                    Email = customerCreate.Email,
                    Phone = customerCreate.Phone
                };
                _customerService.AddCustomer(customer);
                return RedirectToAction("Index");
            }
            return View(customerCreate);
         
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var customer = _customerService.GetCustomerById(id);

            if (customer == null)
                return HttpNotFound();
            var customerEdit = new CustomerEdit
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone
            };
            return View(customerEdit);

        }

        [HttpPost]
        public ActionResult Edit(CustomerEdit customerEdit)
        {
             
            if (ModelState.IsValid) 
            {
                var customer = new Customer
                {
                    Id = customerEdit.Id,
                    Name = customerEdit.Name,
                    Email = customerEdit.Email,
                    Phone = customerEdit.Phone
                };
                _customerService.UpdateCustomer(customer);
                return RedirectToAction("Index");
            }
            return View(customerEdit);
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            var customer = _customerService.GetCustomerById(id);
            if (customer == null)
                return HttpNotFound();
            var customerDetail = new CustomerDetail
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone
            };
            return View(customerDetail);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            
            bool isDeleted = _customerService.DeleteCustomer(id);
            return Json(new { success = isDeleted });
        }
    }
}