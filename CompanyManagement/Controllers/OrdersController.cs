using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            return View(db.Orders.OrderByDescending(o => o.order_date).ToList());
        }
        // GET: SearchOrders
        public PartialViewResult SearchOrder(string OrderID)
        {
            var Orders = db.Orders.Where(o => o.custom_order_id.StartsWith(OrderID));
            return PartialView("_Orders",Orders);
        }
        public JsonResult GetProductPrice(int productId)
        {
            try
            {
                var product = db.ProductDetails.OrderBy(s => s.date_of_buy).First(s => s.product_id == productId && s.quantity > 0);
                Object[] product_data = new Object[] {product.unit_price,product.quantity };
                return Json(product_data, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return null;
            } 
        }
        public JsonResult IsOrderIdFounded(string orderId)
        {
            bool founded = false;
            try
            {
                var order = db.Orders.First(o => o.custom_order_id == orderId);
                if (order != null)
                    founded = true;
            }
            catch
            {
                
            }
            
            return Json(founded,JsonRequestBehavior.AllowGet);
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
        [HttpPost]
        public JsonResult CreateOrder(Order order)
        {
            OrderViewModel orderViewModel = new OrderViewModel();
            orderViewModel.AddOrder(order);
            return Json("تم حفظ الفاتورة بنجاح !!", JsonRequestBehavior.AllowGet);
        }
        // GET: Details
        public ActionResult Details(string id)
        {
            var order = db.Orders.Find(id);
            if (order != null)
            {
                OrderViewModel orderView = new OrderViewModel()
                {
                    Order = order,
                    OrderDetails = db.OrderDetails.Where(o => o.order_id == id).ToList(),
                };
                return View(orderView);
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); 
        }
        
    }
}