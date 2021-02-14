using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompanyManagement.Models;
using System.Net;
namespace CompanyManagement.Controllers
{
    public class InventoryController : Controller
    {
        CompanyManagmentEntities db = new CompanyManagmentEntities();
        public ActionResult AllProductsStock()
        {
            var products = db.Products.ToList();
            Dictionary<string, List<ProductDetail>> allProducts = new Dictionary<string, List<ProductDetail>>();
            foreach (var item in products)
            {
                var assocOrders = db.ProductDetails.Where(o => o.product_id == item.id).OrderBy(o => o.date_of_buy);
                if (assocOrders.Count() != 0)
                {
                    allProducts[item.name] = assocOrders.ToList();
                }
            }
            return View(allProducts);
        }

        public ActionResult ProductTransactions(int? id)
        {
            if (id != null)
            {
                var ProductTransactions = db.Transactions.Where(p => p.product_id == id).ToList();
                if(ProductTransactions == null)
                {
                    return HttpNotFound();
                }
                return View(ProductTransactions);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public PartialViewResult SearchProducts(string ProductName,DateTime? From_date,DateTime? To_date) 
        {
            var products = db.Products.Where(p => p.name.Contains(ProductName)).ToList();
            Dictionary<string, List<ProductDetail>> allProducts = new Dictionary<string, List<ProductDetail>>();
            foreach (var item in products)
            {
                IOrderedQueryable<ProductDetail>assocOrders = null;
                if (From_date != null && To_date != null)
                    assocOrders = db.ProductDetails.Where(o => o.product_id == item.id && o.date_of_buy > From_date && o.date_of_buy < To_date).OrderBy(o => o.date_of_buy);
                else
                    assocOrders = db.ProductDetails.Where(o => o.product_id == item.id).OrderBy(o => o.date_of_buy);
                
                if (assocOrders.Count() != 0)
                {
                    allProducts[item.name] = assocOrders.ToList();
                }
            }
            return PartialView("_Inventory",allProducts);
        }
        public PartialViewResult SearchTransactions(int? id, DateTime? From_date, DateTime? To_date)
        {
            List <Transaction> ProductTransactions = null; 
            if (From_date != null && To_date != null)
            {
                ProductTransactions = db.Transactions.Where(p => p.product_id == id && p.move_date > From_date && p.move_date < To_date).ToList();
            }
            else
            {
                ProductTransactions = db.Transactions.Where(p => p.product_id == id).ToList();
            }
            return PartialView("_Transactions", ProductTransactions);
        }
    }
}