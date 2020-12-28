using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompanyManagement.Models;
namespace CompanyManagement.Controllers
{
    public class OrderDetailsController : Controller
    {
        CompanyManagmentEntities db = new CompanyManagmentEntities();
        // GET: OrderDetails
        public PartialViewResult Index()
        {
            ViewBag.Products = new SelectList(db.Products, "id", "name");
            return PartialView("_Index");
        }
    }
}