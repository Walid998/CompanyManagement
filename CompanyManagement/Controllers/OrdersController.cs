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
            Product Product = null;
            IQueryable<Unit_of_Measures> product_uom;
            List<Object[]> uom = null;
            try
            {
                Product = db.Products.Find(productId);
                product_uom = db.Unit_of_Measures.Where(u => u.category_id == Product.Unit_of_Measures.category_id);
                uom = new List<object[]>();
                foreach (var unit in product_uom)
                {
                    uom.Add(new Object[] { unit.id, unit.unit_name });
                }
            }
            catch (NullReferenceException e) { }
            // GetUom
            ProductDetail product;
            Object[] product_data = null;
            try
            {
                product = db.ProductDetails.OrderBy(s => s.date_of_buy).First(s => s.product_id == productId && s.quantity > 0);
                if(product != null)
                    product_data = new Object[] {product.sale_price,product.quantity };
            }
            catch{ }
            return Json(new
            {
                prop1 = product_data,
                prop2 = uom,
                prop3 = Product.vat
            }
                , JsonRequestBehavior.AllowGet);
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
        public ActionResult CreatePurchaseOrder()
        {
            OrderViewModel order = new OrderViewModel() {
                OrderType = new string[] { "بيع", "شراء" },
                Customers = db.Customers.ToList(),
                Products = db.Products.ToList()
            };
            
            return View(order);
        }

        public ActionResult CreateSalesOrder()
        {
            OrderViewModel order = new OrderViewModel()
            {
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