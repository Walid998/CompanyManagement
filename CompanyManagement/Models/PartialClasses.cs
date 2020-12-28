using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CompanyManagement.Models
{
    [MetadataType(typeof(ProductMetadata))]
    public partial class Product
    {
    }
    [MetadataType(typeof(OrderMetadata))]
    public partial class Order
    {
    }
    [MetadataType(typeof(OrderDetailsMetadata))]
    public partial class OrderDetail
    {
    }
    [MetadataType(typeof(StockMetadata))]
    public partial class Stock
    {
    }
    [MetadataType(typeof(CustomerMetadata))]
    public partial class Customer
    {
    }
}