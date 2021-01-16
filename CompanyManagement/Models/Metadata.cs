using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CompanyManagement.Models
{
    public class ProductMetadata
    {
        [StringLength(50,ErrorMessage ="يجب ألا يزيد اسم المنتج عن 50 حرف")]
        [Display(Name ="اسم المنتج")]
        public string name { get; set; }
        [Display(Name = "القيمة المضافة")]
        public Nullable<double> vat { get; set; }
        [StringLength(50, ErrorMessage = "يجب ألا يزيد كود المنتج عن 50 حرف")]
        [Display(Name = "كود المنتج")]
        public string code { get; set; }
    }
    public class ProductDetailsMetadata
    {
        [Display(Name = "اسم المنتج")]
        public int product_id { get; set; }
        [Display(Name = "السعر")]
        public Nullable<decimal> unit_price { get; set; }
        [Display(Name = "الكمية")]
        public Nullable<int> quantity { get; set; }
        [Display(Name = "الاجمالى")]
        public Nullable<decimal> total_price { get; set; }
    }
    public class OrderMetadata
    {
        [Display(Name = "رقم الفاتورة")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "هذا الحقل مطلوب")]
        public string custom_order_id { get; set; }
        [Display(Name = "العميل")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "يجب اختيار عميل")]
        public int customer_id { get; set; }
        [Display(Name = "نوع الفاتورة")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "يجب تحديد نوع الفاتورة")]
        public string order_type { get; set; }
        [Display(Name = "الاجمالى")]
        public Nullable<decimal> total_payment { get; set; }
        [Display(Name = "المدفوع")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "هذا الحقل مطلوب")]
        public Nullable<decimal> payment_amount { get; set; }
        [Display(Name = "المتبقى")]
        public Nullable<decimal> rest_amount { get; set; }
        [Display(Name = "التاريخ")]
        public Nullable<System.DateTime> order_date { get; set; }
    }
    public class OrderDetailsMetadata
    {
        [Display(Name ="رقم الفاتورة")]
        public string order_id { get; set; }
        [Display(Name = "اسم المنتج")]
        public int product_id { get; set; }
        [Display(Name = "السعر")]
        public Nullable<decimal> unit_price { get; set; }
        [Display(Name = "الكمية")]
        public Nullable<int> quantity { get; set; }
        [Display(Name = "discount")]
        public Nullable<double> discount { get; set; }
        [Display(Name = "الاجمالى")]
        public Nullable<decimal> total_price { get; set; }
        [Display(Name = "التاريخ")]
        public Nullable<System.DateTime> order_data { get; set; }
    }
    public class CustomerMetadata
    {
        [Display(Name = "اسم العميل")]
        public string name { get; set; }
        [Display(Name = "الهاتف")]
        public string phone { get; set; }
        [Display(Name = "البريد الالكترونى")]
        public string email { get; set; }
        [Display(Name = "نوع العميل")]
        public string type { get; set; }
    }

    public class TransactionsMetadata
    {
        [Display(Name ="رقم الحركة")]
        public int id { get; set; }
        [Display(Name = "المنتج")]
        public Nullable<int> product_id { get; set; }
        [Display(Name = "الرصيد")]
        public Nullable<int> balance { get; set; }
        [Display(Name = "الوارد")]
        public Nullable<int> income { get; set; }
        [Display(Name = "الصادر")]
        public Nullable<int> outcome { get; set; }
        [Display(Name = "التكلفة")]
        public Nullable<decimal> unit_cost { get; set; }
        [Display(Name = "التاريخ")]
        public Nullable<System.DateTime> move_date { get; set; }
        [Display(Name = "البيان")]
        public string order_id { get; set; }
    }
}