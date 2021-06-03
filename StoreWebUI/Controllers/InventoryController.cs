using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreBL;
using StoreModels;
using StoreWebUI.Models;

namespace StoreWebUI.Controllers
{
    public class InventoryController : Controller
    {
        public IInventoryBL _inventoryBL;
        public ILocationBL _locationBL;
        public IProductBL _productBL;

        public InventoryController(IInventoryBL inventoryBL, ILocationBL locationBL, IProductBL productBL)
        {
            _inventoryBL = inventoryBL;
            _productBL = productBL;
            _locationBL = locationBL;
        }

        private void IDToNameConverter()
        {
            var locations = _locationBL.GetAllLocations();
            var products = _productBL.GetAllProducts();
            foreach (var location in locations)
            {
                var propName = "location" + location.LocationID;
                ViewData.Add(propName, location.StoreName);
            }

            foreach (var item in products)
            {
                var propName = "product" + item.ProductID;
                ViewData.Add(propName, item.ItemName);
            }
        }

        // GET: Inventory
        public ActionResult Index()
        {
            IDToNameConverter();

            return View(_inventoryBL.GetAllInventories().Select(inventory => new InventoryVM(inventory)).ToList());
        }

        // GET: Inventory/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Inventory/Create
        public ActionResult Create()
        {
            var locations = _locationBL.GetAllLocations();
            var products = _productBL.GetAllProducts();
            var storeNames = new List<string>();
            var itemNames = new List<string>();
            foreach (var location in locations) storeNames.Add(location.StoreName);
            ViewData.Add("locations", storeNames);
            foreach (var item in products) itemNames.Add(item.ItemName);
            ViewData.Add("products", itemNames);

            return View();
        }

        // POST: Inventory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InventoryVM inventoryVM)
        {
            try
            {
                var locations = _locationBL.GetAllLocations();
                var products = _productBL.GetAllProducts();
                var storeNames = new List<string>();
                var itemNames = new List<string>();
                foreach (var location in locations) storeNames.Add(location.StoreName);
                ViewData.Add("locations", storeNames);
                foreach (var item in products) itemNames.Add(item.ItemName);
                ViewData.Add("products", itemNames);
                if (ModelState.IsValid)
                {
                    _inventoryBL.AddInventory(new Inventory
                        {
                            LocationID = inventoryVM.LocationID,
                            ProductID = inventoryVM.ProductID,
                            Quantity = inventoryVM.Quantity
                        }
                    );
                    return RedirectToAction(nameof(Index));
                }

                return View(inventoryVM);
            }
            catch
            {
                return View(inventoryVM);
            }
        }

        // GET: Inventory/Edit/5
        public ActionResult Edit(int id)
        {
            IDToNameConverter();
            try
            {
                var i = 0;
                var products = _productBL.GetAllProducts();
                var storeInventory = _inventoryBL
                    .GetStoreInventoryByLocation(int.Parse(TempData["locationID"].ToString()))
                    .Select(inven => new InventoryVM(inven)).ToList();
                foreach (var product in products)
                {
                    var itemName = "itemName" + i;
                    ViewData.Add(itemName, product.ItemName);
                    i++;
                }

                var locationId = TempData["locationID"].ToString();
                TempData["locationID"] = locationId;
                return View(storeInventory);
            }
            catch
            {
                return View();
            }
        }

        // POST: Inventory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(InventoryVM inventoryVM, int id, IFormCollection collection)
        {
            var i = 0;
            try
            {
                var editInventory =
                    _inventoryBL.GetStoreInventoryByLocation(int.Parse(TempData["locationID"].ToString()));
                var products = _productBL.GetAllProducts();
                var itemNames = new List<string>();
                foreach (var item in products) itemNames.Add(item.ItemName);
                foreach (var inventory in editInventory)
                {
                    inventory.Quantity = int.Parse(collection[itemNames[i]]);
                    _inventoryBL.EditInventory(inventory);
                    i++;
                }

                return RedirectToAction("Index", "Location");
            }
            catch
            {
                return View();
            }
        }

        // GET: Inventory/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
    }
}