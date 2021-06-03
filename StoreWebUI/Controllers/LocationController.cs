using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using StoreBL;
using StoreModels;
using StoreWebUI.Models;

namespace StoreWebUI.Controllers
{
    public class LocationController : Controller
    {
        private readonly ICustomerBL _customerBL;
        private readonly IInventoryBL _inventoryBL;
        private readonly ILocationBL _locationBL;
        private readonly IOrderBL _orderBL;
        private readonly IProductBL _productBL;

        public LocationController(ILocationBL locationBL, IInventoryBL inventoryBL, IProductBL productBL,
            IOrderBL orderBL, ICustomerBL customerBL)
        {
            _locationBL = locationBL;
            _inventoryBL = inventoryBL;
            _productBL = productBL;
            _orderBL = orderBL;
            _customerBL = customerBL;
        }

        // GET: Location
        public ActionResult Index()
        {
            return View(_locationBL.GetAllLocations().Select(location => new LocationVM(location)).ToList());
        }

        // GET: Location/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Location/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Location/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LocationVM locationVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _locationBL.AddLocation(new Location
                    {
                        StoreName = locationVM.StoreName,
                        City = locationVM.City,
                        State = locationVM.State,
                        Address = locationVM.Address
                    });

                    var products = _productBL.GetAllProducts();
                    var location = _locationBL.GetLocation(locationVM.StoreName);
                    foreach (var item in products)
                        _inventoryBL.AddInventory(new Inventory
                        {
                            LocationID = location.LocationID,
                            ProductID = item.ProductID,
                            Quantity = 0
                        });

                    return RedirectToAction(nameof(Index));
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Location/Edit/5
        public ActionResult Edit(int id)
        {
            return View(new LocationVM(_locationBL.GetLocationById(id)));
        }

        // POST: Location/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LocationVM locationVM, int id, IFormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var editLocation = _locationBL.GetLocationById(id);
                    editLocation.StoreName = locationVM.StoreName;
                    editLocation.City = locationVM.City;
                    editLocation.State = locationVM.State;
                    editLocation.Address = locationVM.Address;
                    _locationBL.EditLocation(editLocation);
                    return RedirectToAction(nameof(Index));
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Location/Delete/5
        public ActionResult Delete(int id)
        {
            return View(new LocationVM(_locationBL.GetLocationById(id)));
        }

        // POST: Location/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                _locationBL.DeleteLocation(_locationBL.GetLocationById(id));
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // Get: View Inventory
        public ActionResult Inventory(int id)
        {
            TempData["locationID"] = id;
            return RedirectToAction("Edit", "Inventory");
        }

        // Get
        public ActionResult ViewOrders(int id)
        {
            try
            {
                var orders = _orderBL.GetLocationOrders(id).Select(ord => new OrderVM(ord)).ToList();
                SetListSelectors(id);
                return View(orders);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ViewOrders(int id, string sort)
        {
            if (ModelState.IsValid && !string.IsNullOrWhiteSpace(sort))
                try
                {
                    var sortedOrders = new List<OrderVM>();
                    var orders = _orderBL.GetLocationOrders(id).Select(ord => new OrderVM(ord)).ToList();
                    switch (sort)
                    {
                        case "Sort By Cost":
                            Log.Information("Sort by Cost Seleceted");
                            sortedOrders = orders.OrderBy(ord => ord.Total).ToList();
                            SetListSelectors(id);
                            return View(sortedOrders);

                        case "Sort By Date Ascending":
                            Log.Information("Sort by Date Ascending Selected");
                            sortedOrders = orders.OrderByDescending(ord => ord.OrderDate).ToList();
                            SetListSelectors(id);
                            return View(sortedOrders);

                        case "Sort By Date Descending":
                            Log.Information("Sort by Date Descending Selected");
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
            var orders = _orderBL.GetLocationOrders(id).Select(ord => new OrderVM(ord)).ToList();
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