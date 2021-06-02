using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreBL;
using StoreModels;
using StoreWebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreWebUI.Controllers
{
    public class CustomerController : Controller
    {

        private ICustomerBL _customerBL;
        private IOrderBL _orderBL;
        private ILocationBL _locationBL;


        public CustomerController(ICustomerBL customerBL, IOrderBL orderBL, ILocationBL locationBL)
        {
            _customerBL = customerBL;
            _orderBL = orderBL;
            _locationBL = locationBL;
        }



        public ActionResult List(CustomerVM customerVM)
        {
            List<CustomerVM> customerList = new List<CustomerVM>();
            try
            {
                Customer customerModel = _customerBL.SearchCustomer(TempData["firstName"].ToString(), TempData["lastName"].ToString());
                CustomerVM customer = new CustomerVM(customerModel);
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
                    Customer editCustomer = _customerBL.SearchCustomer(id);
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
                int i = 0;
                List<OrderVM> orders = _orderBL.GetCustomerOrders(id).Select(ord => new OrderVM(ord)).ToList();
                foreach (OrderVM order in orders)
                {
                    string customerSelector = "customer" + i;
                    Customer customer = _customerBL.SearchCustomer(order.CustomerID);
                    string customerName = customer.FirstName + " " + customer.LastName;
                    ViewData.Add(customerSelector, customerName);

                    string storeSelector = "location" + i;
                    Location location = _locationBL.GetLocationById(order.LocationID);
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
            if (ModelState.IsValid && !String.IsNullOrWhiteSpace(sort))
            {
                try
                {
                    List<OrderVM> sortedOrders = new List<OrderVM>();
                    List<OrderVM> orders = _orderBL.GetCustomerOrders(id).Select(ord => new OrderVM(ord)).ToList();
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
            }
            return View();
        }

        private void SetListSelectors(int id)
        {
            int i = 0;
            List<OrderVM> orders = _orderBL.GetCustomerOrders(id).Select(ord => new OrderVM(ord)).ToList();
            foreach (OrderVM order in orders)
            {
                string customerSelector = "customer" + i;
                Customer customer = _customerBL.SearchCustomer(order.CustomerID);
                string customerName = customer.FirstName + " " + customer.LastName;
                ViewData.Add(customerSelector, customerName);

                string storeSelector = "location" + i;
                Location location = _locationBL.GetLocationById(order.LocationID);
                ViewData.Add(storeSelector, location.StoreName);

                i++;
            }
        }
    }
}