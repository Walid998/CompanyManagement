using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyManagement.Models
{
    public class OrderViewModel
    {
        public Order Order { get; set; }
        public OrderDetail OrderDetail { get; set; }
        public List<Product> Products { get; set; }
        public string [] OrderType { get; set; }
        public List<Customer>Customers { get; set; }


    }
}