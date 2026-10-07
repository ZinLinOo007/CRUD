
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ServiceModel;
using CRUD.CustomerService;

namespace CRUD.Controllers
{
    public class CustomerController : Controller
    {
        private CustomerServiceClient client;
        public CustomerController()
        {
            this.client = new CustomerServiceClient();
        }
        public ActionResult Index()
        {
            var customer = client.GetCustomers();
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
                client.AddCustomer(customer);
                return RedirectToAction("Index");
            }
            return View(customer);
         
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var customer = client.GetCustomerById(id);

            if (customer == null)
                return HttpNotFound();
            return View(customer);

        }

        [HttpPost]
        public ActionResult Edit(Customer customer)
        {
             
            if (ModelState.IsValid) 
            {
                client.UpdateCustomer(customer);
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            var customer = client.GetCustomerById(id);
            if (customer == null)
                return HttpNotFound();
            return View(customer);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            
            bool isDeleted =  client.DeleteCustomer(id);
            return Json(new { success = isDeleted });
        }
    }
}