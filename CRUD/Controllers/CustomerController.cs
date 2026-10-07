
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ServiceModel;
using CRUD.CustomerService;
using CRUD.Services;

namespace CRUD.Controllers
{
    public class CustomerController : Controller
    {
        private CustomerServices _customerService;
        public CustomerController()
        {
            this._customerService = new CustomerServices();
        }
        public ActionResult Index()
        {
            var customer = _customerService.GetAllCustomers();
            return View(customer);
            
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new Customer());
        }

        [HttpPost]
        public ActionResult Create(Customer customer) {

            
            if (ModelState.IsValid)
            {
                _customerService.AddCustomer(customer);
                return RedirectToAction("Index");
            }
            return View(customer);
         
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var customer = _customerService.GetCustomerById(id);

            if (customer == null)
                return HttpNotFound();
            return View(customer);

        }

        [HttpPost]
        public ActionResult Edit(Customer customer)
        {
             
            if (ModelState.IsValid) 
            {
                _customerService.UpdateCustomer(customer);
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            var customer = _customerService.GetCustomerById(id);
            if (customer == null)
                return HttpNotFound();
            return View(customer);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            
            bool isDeleted = _customerService.DeleteCustomer(id);
            return Json(new { success = isDeleted });
        }
    }
}