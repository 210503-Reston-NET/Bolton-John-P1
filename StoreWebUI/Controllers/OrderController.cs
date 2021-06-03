using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreBL;
using StoreModels;
using StoreWebUI.Models;

namespace StoreWebUI.Controllers
{
    public class OrderController : Controller
    {
        public ICustomerBL _customerBL;
        public ILineItemBL _lineItemBL;
        public ILocationBL _locationBL;
        public IOrderBL _orderBL;
        public IProductBL _productBL;

        public OrderController(IOrderBL orderBL, ICustomerBL customerBL, ILocationBL locationBL, IProductBL productBL,
            ILineItemBL lineItemBL)
        {
            _orderBL = orderBL;
            _customerBL = customerBL;
            _locationBL = locationBL;
            _productBL = productBL;
            _lineItemBL = lineItemBL;
        }

        // GET: Order
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
                    if (_customerBL.SearchCustomer(firstName, lastName) != null)
                    {
                        TempData["firstName"] = firstName;
                        TempData["lastName"] = lastName;
                        return RedirectToAction(nameof(Location));
                    }

                return View();
            }
            catch
            {
                return View();
            }
        }

        // Get
        public ActionResult Location()
        {
            var firstName = TempData["firstName"].ToString();
            TempData["firstName"] = firstName;
            var lastName = TempData["lastName"].ToString();
            TempData["lastName"] = lastName;
            return View();
        }

        // Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Location(string storeName)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var customer = _customerBL.SearchCustomer(TempData["firstName"].ToString(),
                        TempData["lastName"].ToString());
                    var location = _locationBL.GetLocation(storeName);
                    var orderID = 0;
                    if (location == null) return View();
                    var newOrder = new Order(customer.CustomerID, location.LocationID, 0, DateTime.Now.ToString());
                    _orderBL.AddOrder(newOrder, location, customer);
                    var orders = _orderBL.GetAllOrders();
                    // Retrieves latest orderID
                    foreach (var order in orders) orderID = order.OrderID;
                    TempData["OrderID"] = orderID;
                    return RedirectToAction(nameof(LineItems));
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        // Get
        public ActionResult LineItems()
        {
            try
            {
                var i = 0;
                var products = _productBL.GetAllProducts().Select(prod => new ProductVM(prod)).ToList();
                foreach (var item in products)
                {
                    var itemName = "itemName" + i;
                    ViewData.Add(itemName, item.ItemName);
                    i++;
                }

                var orderId = TempData["OrderID"].ToString();
                TempData["OrderID"] = orderId;
                return View(products);
            }
            catch
            {
                return View();
            }
        }

        // Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LineItems(IFormCollection collection)
        {
            try
            {
                var products = _productBL.GetAllProducts();
                var quantity = new List<int>();
                foreach (var item in products)
                {
                    var newLineItem = new LineItem(item.ProductID, int.Parse(collection[item.ItemName]),
                        int.Parse(TempData["OrderID"].ToString()));
                    quantity.Add(int.Parse(collection[item.ItemName]));
                    _lineItemBL.AddLineItem(newLineItem, item);
                }

                var orderTotal = _productBL.GetTotal(quantity);
                TempData["OrderTotal"] = orderTotal.ToString();
                var orderId = TempData["OrderID"].ToString();
                TempData["OrderID"] = orderId;
                return RedirectToAction(nameof(OrderConfirmation));
            }
            catch
            {
                return View();
            }
        }

        // Get
        public ActionResult OrderConfirmation()
        {
            var order = _orderBL.ViewOrder(int.Parse(TempData["OrderID"].ToString()));
            var customer = _customerBL.SearchCustomer(order.CustomerID);
            var customerName = customer.FirstName + " " + customer.LastName;
            var location = _locationBL.GetLocationById(order.LocationID);
            ViewData["Total"] = TempData["OrderTotal"];
            ViewData["Customer"] = customerName;
            ViewData["Location"] = location.StoreName;
            ViewData["OrderID"] = TempData["OrderID"];
            var orderTotal = TempData["OrderTotal"].ToString();
            TempData["OrderTotal"] = orderTotal;
            var orderId = TempData["OrderID"].ToString();
            TempData["OrderID"] = orderId;
            var orderVM = new OrderVM(order);
            return View(orderVM);
        }

        // Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OrderConfirmation(string confirm)
        {
            if (ModelState.IsValid && !string.IsNullOrWhiteSpace(confirm))
                try
                {
                    switch (confirm)
                    {
                        case "Place Order":
                            var order = _orderBL.ViewOrder(int.Parse(TempData["OrderID"].ToString()));
                            order.Total = double.Parse(TempData["OrderTotal"].ToString());
                            var customer = _customerBL.SearchCustomer(order.CustomerID);
                            var location = _locationBL.GetLocationById(order.LocationID);
                            _orderBL.UpdateOrder(order, location, customer);
                            return RedirectToAction("Index", "Home");

                        case "Cancel Order":
                            var cancelOrder = _orderBL.ViewOrder(int.Parse(TempData["OrderID"].ToString()));
                            _orderBL.DeleteOrder(cancelOrder);
                            return RedirectToAction("Index", "Home");
                    }
                }
                catch
                {
                    return View();
                }

            return View();
        }

        // Get
        public ActionResult Search()
        {
            return View();
        }

        // Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(int orderId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TempData["OrderID"] = orderId;
                    return RedirectToAction(nameof(ViewOrder));
                }
            }
            catch
            {
                return View();
            }

            return View();
        }

        // Get
        public ActionResult ViewOrder(OrderVM orderVM)
        {
            try
            {
                var customerOrder = new List<OrderVM>();
                var order = _orderBL.ViewOrder(int.Parse(TempData["OrderID"].ToString()));
                var customer = _customerBL.SearchCustomer(order.CustomerID);
                var location = _locationBL.GetLocationById(order.LocationID);
                ViewData["Customer"] = customer.FirstName + " " + customer.LastName;
                ViewData["Location"] = location.StoreName;
                customerOrder.Add(new OrderVM(order));
                return View(customerOrder);
            }
            catch
            {
                return RedirectToAction(nameof(Search));
            }
        }
    }
}