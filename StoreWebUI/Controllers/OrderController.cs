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
    public class OrderController : Controller
    {
        public IOrderBL _orderBL;
        public ICustomerBL _customerBL;
        public ILocationBL _locationBL;
        public IProductBL _productBL;
        public ILineItemBL _lineItemBL;

        public OrderController(IOrderBL orderBL, ICustomerBL customerBL, ILocationBL locationBL, IProductBL productBL, ILineItemBL lineItemBL)
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
                {
                    if (_customerBL.SearchCustomer(firstName, lastName) != null)
                    {
                        TempData["firstName"] = firstName;
                        TempData["lastName"] = lastName;
                        return RedirectToAction(nameof(Location));
                    }
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
            string firstName = TempData["firstName"].ToString();
            TempData["firstName"] = firstName;
            string lastName = TempData["lastName"].ToString();
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
                    Customer customer = _customerBL.SearchCustomer(TempData["firstName"].ToString(), TempData["lastName"].ToString());
                    Location location = _locationBL.GetLocation(storeName);
                    int orderID = 0;
                    if (location == null)
                    {
                        return View();
                    }
                    Order newOrder = new Order(customer.CustomerID, location.LocationID, 0, DateTime.Now.ToString());
                    _orderBL.AddOrder(newOrder, location, customer);
                    List<Order> orders = _orderBL.GetAllOrders();
                    // Retrieves latest orderID
                    foreach (Order order in orders)
                    {
                        orderID = order.OrderID;
                    }
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
                int i = 0;
                List<ProductVM> products = _productBL.GetAllProducts().Select(prod => new ProductVM(prod)).ToList();
                foreach (ProductVM item in products)
                {
                    string itemName = "itemName" + i;
                    ViewData.Add(itemName, item.ItemName);
                    i++;
                }
                string orderId = TempData["OrderID"].ToString();
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
                List<Product> products = _productBL.GetAllProducts();
                List<int> quantity = new List<int>();
                foreach (Product item in products)
                {
                    LineItem newLineItem = new LineItem(item.ProductID, Int32.Parse(collection[item.ItemName]), Int32.Parse(TempData["OrderID"].ToString()));
                    quantity.Add(Int32.Parse(collection[item.ItemName]));
                    _lineItemBL.AddLineItem(newLineItem, item);
                }
                double orderTotal = _productBL.GetTotal(quantity);
                TempData["OrderTotal"] = orderTotal.ToString();
                string orderId = TempData["OrderID"].ToString();
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
            Order order = _orderBL.ViewOrder(Int32.Parse(TempData["OrderID"].ToString()));
            Customer customer = _customerBL.SearchCustomer(order.CustomerID);
            string customerName = customer.FirstName + " " + customer.LastName;
            Location location = _locationBL.GetLocationById(order.LocationID);
            ViewData["Total"] = TempData["OrderTotal"];
            ViewData["Customer"] = customerName;
            ViewData["Location"] = location.StoreName;
            ViewData["OrderID"] = TempData["OrderID"];
            string orderTotal = TempData["OrderTotal"].ToString();
            TempData["OrderTotal"] = orderTotal;
            string orderId = TempData["OrderID"].ToString();
            TempData["OrderID"] = orderId;
            OrderVM orderVM = new OrderVM(order);
            return View(orderVM);
        }

        // Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OrderConfirmation(string confirm)
        {
            if (ModelState.IsValid && !String.IsNullOrWhiteSpace(confirm))
            {
                try
                {
                    switch (confirm)
                    {
                        case "Place Order":
                            Order order = _orderBL.ViewOrder(Int32.Parse(TempData["OrderID"].ToString()));
                            order.Total = Double.Parse(TempData["OrderTotal"].ToString());
                            Customer customer = _customerBL.SearchCustomer(order.CustomerID);
                            Location location = _locationBL.GetLocationById(order.LocationID);
                            _orderBL.UpdateOrder(order, location, customer);
                            return RedirectToAction("Index", "Home");

                        case "Cancel Order":
                            Order cancelOrder = _orderBL.ViewOrder(Int32.Parse(TempData["OrderID"].ToString()));
                            _orderBL.DeleteOrder(cancelOrder);
                            return RedirectToAction("Index", "Home");
                    }
                }
                catch
                {
                    return View();
                }
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
                List<OrderVM> customerOrder = new List<OrderVM>();
                Order order = _orderBL.ViewOrder(Int32.Parse(TempData["OrderID"].ToString()));
                Customer customer = _customerBL.SearchCustomer(order.CustomerID);
                Location location = _locationBL.GetLocationById(order.LocationID);
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