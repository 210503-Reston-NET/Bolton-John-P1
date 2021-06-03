using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreBL;
using StoreModels;
using StoreWebUI.Models;

namespace StoreWebUI.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerBL _customerBL;
        private readonly ILocationBL _locationBL;
        private readonly IOrderBL _orderBL;

        public CustomerController(ICustomerBL customerBL, IOrderBL orderBL, ILocationBL locationBL)
        {
            _customerBL = customerBL;
            _orderBL = orderBL;
            _locationBL = locationBL;
        }

        public ActionResult List(CustomerVM customerVM)
        {
            var customerList = new List<CustomerVM>();
            try
            {
                var customerModel =
                    _customerBL.SearchCustomer(TempData["firstName"].ToString(), TempData["lastName"].ToString());
                var customer = new CustomerVM(customerModel);
                customerList.Add(customer);
                return View(customerList);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string firstName, string lastName)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TempData["firstName"] = firstName;
                    TempData["lastName"] = lastName;
                    return RedirectToAction(nameof(List));
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerVM customerVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _customerBL.AddCustomer(new Customer
                        {
                            FirstName = customerVM.FirstName,
                            LastName = customerVM.LastName,
                            PhoneNumber = customerVM.PhoneNumber,
                            Email = customerVM.Email
                        }
                    );
                    TempData["firstName"] = customerVM.FirstName;
                    TempData["lastName"] = customerVM.LastName;
                    return RedirectToAction(nameof(List));
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            return View(new CustomerVM(_customerBL.SearchCustomer(id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomerVM customerVM, int id, IFormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var editCustomer = _customerBL.SearchCustomer(id);
                    editCustomer.FirstName = customerVM.FirstName;
                    editCustomer.LastName = customerVM.LastName;
                    editCustomer.Email = customerVM.Email;
                    editCustomer.PhoneNumber = customerVM.PhoneNumber;
                    _customerBL.EditCustomer(editCustomer);
                    TempData["firstName"] = customerVM.FirstName;
                    TempData["lastName"] = customerVM.LastName;
                    return RedirectToAction(nameof(List));
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View(new CustomerVM(_customerBL.SearchCustomer(id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                _customerBL.DeleteCustomer(_customerBL.SearchCustomer(id));
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult ViewOrders(int id)
        {
            try
            {
                var i = 0;
                var orders = _orderBL.GetCustomerOrders(id).Select(ord => new OrderVM(ord)).ToList();
                foreach (var order in orders)
                {
                    var customerSelector = "customer" + i;
                    var customer = _customerBL.SearchCustomer(order.CustomerID);
                    var customerName = customer.FirstName + " " + customer.LastName;
                    ViewData.Add(customerSelector, customerName);

                    var storeSelector = "location" + i;
                    var location = _locationBL.GetLocationById(order.LocationID);
                    ViewData.Add(storeSelector, location.StoreName);

                    i++;
                }

                return View(orders);
            }
            catch
            {
                return RedirectToAction(nameof(List));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ViewOrders(int id, string sort)
        {
            if (ModelState.IsValid && !string.IsNullOrWhiteSpace(sort))
                try
                {
                    var sortedOrders = new List<OrderVM>();
                    var orders = _orderBL.GetCustomerOrders(id).Select(ord => new OrderVM(ord)).ToList();
                    switch (sort)
                    {
                        case "Sort By Cost":
                            sortedOrders = orders.OrderBy(ord => ord.Total).ToList();
                            SetListSelectors(id);
                            return View(sortedOrders);

                        case "Sort By Date Ascending":
                            sortedOrders = orders.OrderByDescending(ord => ord.OrderDate).ToList();
                            SetListSelectors(id);
                            return View(sortedOrders);

                        case "Sort By Date Descending":
                            sortedOrders = orders.OrderBy(ord => ord.OrderDate).ToList();
                            SetListSelectors(id);
                            return View(sortedOrders);
                    }
                }
                catch
                {
                    return View();
                }

            return View();
        }

        private void SetListSelectors(int id)
        {
            var i = 0;
            var orders = _orderBL.GetCustomerOrders(id).Select(ord => new OrderVM(ord)).ToList();
            foreach (var order in orders)
            {
                var customerSelector = "customer" + i;
                var customer = _customerBL.SearchCustomer(order.CustomerID);
                var customerName = customer.FirstName + " " + customer.LastName;
                ViewData.Add(customerSelector, customerName);

                var storeSelector = "location" + i;
                var location = _locationBL.GetLocationById(order.LocationID);
                ViewData.Add(storeSelector, location.StoreName);

                i++;
            }
        }
    }
}