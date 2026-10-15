
using CRUD.Controllers;
using CRUD.CustomerService;
using CRUD.Models;
using CRUD.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xunit;
using ICustomerService = CRUD.Services.ICustomerService;

namespace CRUD.Tests
{
    public  class CustomerControllerTests
    {
        public List<Customer> SimpleCustomers()
        {

            return  new List<Customer>
            {
                new Customer{Id = 1, Name = "Mg Mg", Email= "mgmg@gmail.com",Phone= "09786543213"},
                new Customer{Id = 2, Name = "Aung Aung", Email= "aungaung@gmail.com",Phone= "09420134567"},
                new Customer{Id = 3, Name = "Tun Tun", Email= "tuntun@gmail.com",Phone= "09256432167"}
            };
        }

        [Fact]
        public void Index()
        {
            //Arrange
            var customers = SimpleCustomers();
            var mockService = new Mock<ICustomerService>();
            mockService.Setup(s => s.GetAllCustomers())
                .Returns(customers);
            var controller = new CustomerController(mockService.Object);

            //Act
            var result = controller.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result); 
            var model = Assert.IsAssignableFrom<List<CustomerList>>(viewResult.Model);
            Assert.Equal(customers.Count, model.Count);
            Assert.Equal(1, model[0].Id);
            Assert.Equal("Mg Mg", model[0].Name);
            Assert.Equal("mgmg@gmail.com", model[0].Email);
            Assert.Equal("09786543213", model[0].Phone);

            mockService.Verify(s => s.GetAllCustomers(), Times.Once);
        }
        [Fact]
        public void Create_Get_ReturnCustomerCreate()
        {
            //Arrange
            var mockService = new Mock<ICustomerService>();
            var controller = new CustomerController(mockService.Object);

            //Act
            var result = controller.Create();

            //Assert
            var viewResult =  Assert.IsType<ViewResult>(result);
            Assert.IsType<CustomerCreate>(viewResult.Model);
            
            
        }

        [Fact]
        public void Create_ValidCall_RedirectToIndex()
        {
            //Arrange
            var mockService = new Mock<ICustomerService>();
            var customerCreate = new CustomerCreate
            {
                Name = "Mya Mya",
                Email = "myamya@gmail.com",
                Phone = "01243256789"

            };

            var controller = new CustomerController(mockService.Object);
            
            //Act
            var result = controller.Create(customerCreate);

            //Assert
            var rediretResult = Assert.IsType<RedirectToRouteResult>(result);
             
            Assert.Equal("Index", rediretResult.RouteValues["action"]);

            mockService.Verify(s => s.AddCustomer(It.Is<Customer>(c =>
            c.Name == customerCreate.Name &&
            c.Email == customerCreate.Email &&
            c.Phone == customerCreate.Phone)),
            Times.Once);



        }

        [Theory]
        [InlineData("","example@gmail.com","09789123456","Name")]
        [InlineData("aa","","09789123455","Email")]
        [InlineData("bb","bb@gmail.com","","Phone")]
        public void Create_InvalidCall_ReutrnView(string name, string email, string phone,string param)
        {
            //Arrange
            var mockService = new Mock<ICustomerService>();
            var customerCreate = new CustomerCreate
            {
                Name = name,
                Email = email,
                Phone = phone
            };
            var controller = new CustomerController(mockService.Object);
            controller.ModelState.AddModelError(param, $"{param} is required");
           
            //Act
            var result = controller.Create(customerCreate);

            //Assert
           var viewResult =  Assert.IsType<ViewResult>(result);
           Assert.Equal(customerCreate, viewResult.Model);
           mockService.Verify(s => s.AddCustomer(It.IsAny<Customer>()), Times.Never);

        }


        [Fact]
        public void Edit_Get_ReturnCustomerEdit()
        {
            //Arrange
            var customers = SimpleCustomers();
            var mockService = new Mock<ICustomerService>();
            mockService.Setup(s => s.GetCustomerById(customers[0].Id))
                .Returns(customers[0]);
            var controller = new CustomerController(mockService.Object);

            //Act
            var result = controller.Edit(customers[0].Id);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model =  Assert.IsType<CustomerEdit>(viewResult.Model);
            Assert.Equal(customers[0].Id, model.Id);
            Assert.Equal(customers[0].Name, model.Name);
            Assert.Equal(customers[0].Email, model.Email);
            Assert.Equal(customers[0].Phone, model.Phone);

        }

        [Fact]
        public void Edit_Get_HttpNotFound()
        {
            //Arrange 
            var mockService = new Mock<ICustomerService>();
            mockService.Setup(s => s.GetCustomerById(999))
                .Returns((Customer)null);
            var controller = new CustomerController(mockService.Object);

            //Act
            var result = controller.Edit(999);

            //Assert
            Assert.IsType<HttpNotFoundResult>(result);
        }

        [Fact]
        public void Edit_Post_VaildCall_ReturnRedirectToIndex()
        {
            //Arrange
            var customerEdit = new CustomerEdit
            {
                Id = 1,
                Name = "Mg Mg",
                Email = "mm@gmail.com",
                Phone = "091234556789"
            };

            var mockService = new Mock<ICustomerService>();
            var controller = new CustomerController(mockService.Object);
            //Act
            var result = controller.Edit(customerEdit);

            //Assert
            var redirectResult = Assert.IsType<RedirectToRouteResult>(result);
            Assert.Equal("Index", redirectResult.RouteValues["action"]);
            mockService.Verify(s => s.UpdateCustomer(It.Is<Customer>(c =>
            c.Id == customerEdit.Id && 
            c.Name == customerEdit.Name &&
            c.Email == customerEdit.Email &&
            c.Phone == customerEdit.Phone)),
            Times.Once);


        }

        [Theory]
        [InlineData( 1,"", "example@gmail.com", "09789123456", "Name")]
        [InlineData(2,"aa", "", "09789123455", "Email")]
        [InlineData(3,"bb", "bb@gmail.com", "", "Phone")]
        public void Edit_Post_InvalidCall_ReturnView(int id,string name, string email, string phone, string param)
        {
            //Arrange
            var customerEdit = new CustomerEdit
            {
                Id = id,
                Name = name,
                Email = email,
                Phone = phone,

            };
            var mockService = new Mock<ICustomerService>();
            var controller = new CustomerController(mockService.Object);
            controller.ModelState.AddModelError(param, $"{param} is required");
            //Act
            var result = controller.Edit(customerEdit);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result); 
            Assert.Same(customerEdit, viewResult.Model);
            mockService.Verify(s => s.UpdateCustomer(It.IsAny<Customer>()), Times.Never);

        }

        [Fact]
        public void Detail_Get_ReturnView()
        {
            //Arrange
            var customers = SimpleCustomers();
            var mockService = new Mock<ICustomerService>();
            mockService.Setup(s => s.GetCustomerById(customers[0].Id))
                .Returns(customers[0]);
            var controller = new CustomerController(mockService.Object);

            //Act
            var result = controller.Detail(customers[0].Id);
            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model =  Assert.IsType<CustomerDetail>(viewResult.Model);
            Assert.Equal(customers[0].Id, model.Id);
            Assert.Equal(customers[0].Name, model.Name);
            Assert.Equal(customers[0].Email, model.Email);
            Assert.Equal(customers[0].Phone, model.Phone);

        }

        [Fact]
        public void Detail_Get_ReturnHttpNotFound()
        {
            //Arrange
            var mockService = new Mock<ICustomerService>();
            mockService.Setup(s => s.GetCustomerById(999))
                .Returns((Customer)null);
            var controller = new CustomerController(mockService.Object);

            //Act
            var result = controller.Detail(999);
            
            //Assert
            Assert.IsType<HttpNotFoundResult>(result);
        }

        [Fact]
        public void Delete_Post_ReturnTrue()
        {
            //Arrange
            var mockService = new Mock<ICustomerService>();
            mockService.Setup(s=> s.DeleteCustomer(1))
                .Returns(true);
            var controller = new CustomerController(mockService.Object);

            //Act
            var result = controller.Delete(1);

            //Assert
            Assert.IsType<JsonResult>(result);
            
        }


    }
}
