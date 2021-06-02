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
    public class InventoryController : Controller
    {
        public IInventoryBL _inventoryBL;
        public IProductBL _productBL;
        public ILocationBL _locationBL;
        public InventoryController(IInventoryBL inventoryBL, ILocationBL locationBL, IProductBL productBL)
        {
            _inventoryBL = inventoryBL;
            _productBL = productBL;
            _locationBL = locationBL;
        }

        private void IDToNameConverter()
        {
            List<Location> locations = _locationBL.GetAllLocations();
            List<Product> products = _productBL.GetAllProducts();
            foreach (Location location in locations)
            {
                string propName = "location" + location.LocationID.ToString();
                ViewData.Add(propName, location.StoreName);
            }
            foreach (Product item in products)
            {
                string propName = "product" + item.ProductID.ToString();
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
            List<Location> locations = _locationBL.GetAllLocations();
            List<Product> products = _productBL.GetAllProducts();
            List<string> storeNames = new List<string>();
            List<string> itemNames = new List<string>();
            foreach (Location location in locations)
            {
                storeNames.Add(location.StoreName);
            }
            ViewData.Add("locations", storeNames);
            foreach (Product item in products)
            {
                itemNames.Add(item.ItemName);
            }
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
                List<Location> locations = _locationBL.GetAllLocations();
                List<Product> products = _productBL.GetAllProducts();
                List<string> storeNames = new List<string>();
                List<string> itemNames = new List<string>();
                foreach (Location location in locations)
                {
                    storeNames.Add(location.StoreName);
                }
                ViewData.Add("locations", storeNames);
                foreach (Product item in products)
                {
                    itemNames.Add(item.ItemName);
                }
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
                int i = 0;
                List<Product> products = _productBL.GetAllProducts();
                List<InventoryVM> storeInventory = _inventoryBL.GetStoreInventoryByLocation(Int32.Parse(TempData["locationID"].ToString())).Select(inven => new InventoryVM(inven)).ToList();
                foreach (Product product in products)
                {
                    string itemName = "itemName" + i;
                    ViewData.Add(itemName, product.ItemName);
                    i++;
                }
                string locationId = TempData["locationID"].ToString();
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
            int i = 0;
            try
            {
                List<Inventory> editInventory = _inventoryBL.GetStoreInventoryByLocation(Int32.Parse(TempData["locationID"].ToString()));
                List<Product> products = _productBL.GetAllProducts();
                List<string> itemNames = new List<string>();
                foreach (Product item in products)
                {
                    itemNames.Add(item.ItemName);
                }
                foreach (Inventory inventory in editInventory)
                {
                    inventory.Quantity = Int32.Parse(collection[itemNames[i]]);
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