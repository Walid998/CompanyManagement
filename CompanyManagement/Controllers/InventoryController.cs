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

        public ActionResult ProductCard(int? id)
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
    }
}