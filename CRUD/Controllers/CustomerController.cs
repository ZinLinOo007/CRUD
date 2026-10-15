
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRUD.Models;
using CRUD.Mappings;
using CRUD.Services;

namespace CRUD.Controllers
{
    public class CustomerController : Controller
    {
        private ICustomerService _customerService;

        public CustomerController()
        {
            _customerService = new Services.CustomerService();
        }
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        public ActionResult Index()
        {
            var customers = _customerService.GetAllCustomers();
            var customerList =  CustomerMapping.MapToCustomerList(customers);
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
                var customer = CustomerMapping.MapToCustomer(customerCreate);
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
            var customerEdit = CustomerMapping.MapToCustomerEdit(customer);
            return View(customerEdit);

        }

        [HttpPost]
        public ActionResult Edit(CustomerEdit customerEdit)
        {
             
            if (ModelState.IsValid) 
            {

                var customer = CustomerMapping.MapToCustomer(customerEdit);
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
           
            var customerDetail = CustomerMapping.MapToCustomerDetail(customer);
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