using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreBL;
using StoreModels;
using StoreWebUI.Models;
using System.Linq;

namespace StoreWebUI.Controllers
{
    public class ProductController : Controller
    {
        public IProductBL _productBL;
        public ProductController(IProductBL productBL)
        {
            _productBL = productBL;
        }

        // GET: Product
        public ActionResult Index()
        {
            return View(_productBL.GetAllProducts().Select(product => new ProductVM(product)).ToList());
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductVM productVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _productBL.AddProduct(new Product
                    {
                        ItemName = productVM.ItemName,
                        Price = productVM.Price,
                        Description = productVM.Description

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

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            return View(new ProductVM(_productBL.GetProductById(id)));
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductVM productVM, int id, IFormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Product editProduct = _productBL.GetProductById(id);
                    editProduct.ItemName = productVM.ItemName;
                    editProduct.Price = productVM.Price;
                    editProduct.Description = productVM.Description;
                    _productBL.EditProduct(editProduct);
                    return RedirectToAction(nameof(Index));
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            return View(new ProductVM(_productBL.GetProductById(id)));
        }

        // POST: Product/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                _productBL.DeleteProduct(_productBL.GetProductById(id));
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}