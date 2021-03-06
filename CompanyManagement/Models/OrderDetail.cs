//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CompanyManagement.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderDetail
    {
        public int order_details_id { get; set; }
        public string order_id { get; set; }
        public int product_id { get; set; }
        public Nullable<double> discount { get; set; }
        public Nullable<decimal> total_price { get; set; }
        public Nullable<int> uom_id { get; set; }
        public Nullable<double> quantity { get; set; }
        public Nullable<System.DateTime> order_date { get; set; }
        public Nullable<decimal> sale_price { get; set; }
        public Nullable<decimal> cost_price { get; set; }
        public Nullable<double> vat { get; set; }
    
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
        public virtual Unit_of_Measures Unit_of_Measures { get; set; }
    }
}
