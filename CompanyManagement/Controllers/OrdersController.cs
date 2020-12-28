using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompanyManagement.Models;
namespace CompanyManagement.Controllers
{
    public class OrdersController : Controller
    {
        CompanyManagmentEntities db = new CompanyManagmentEntities();
        // GET: Orders
        public ActionResult Index()
        {
            return View(db.Orders.ToList());
        }
        // GET: SearchOrders
        public PartialViewResult SearchOrder(string OrderID)
        {
            var Orders = db.Orders.Where(o => o.custom_order_id.StartsWith(OrderID));
            return PartialView("_Orders",Orders);
        }
        public JsonResult GetProductPrice(int productId)
        {
            var unit_price = db.Stocks.OrderBy(s => s.date_of_buy).Single(s => s.product_id == productId).unit_price;
            return Json(unit_price,JsonRequestBehavior.AllowGet);
        }

        // GET: Create
        public ActionResult Create()
        {
            OrderViewModel order = new OrderViewModel() {
                OrderType = new string[] { "بيع", "شراء" },
                Customers = db.Customers.ToList(),
                Products = db.Products.ToList()
            };
            
            return View(order);
        }

        // GET: Details
        public ActionResult Details(int? id)
        {
            return View();
        }

        // GET: Edit
        public ActionResult Edit(int? id)
        {
            return View();
        }

        // GET: Delete
        public ActionResult Delete(int? id)
        {
            return View();
        }
    }
}