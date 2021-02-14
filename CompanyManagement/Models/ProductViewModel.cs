using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyManagement.Models
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public List<Product> Products { get; set; }
        public List<Unit_of_Measures> Uoms { get; set; }
    }
}