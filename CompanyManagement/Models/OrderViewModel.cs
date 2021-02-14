using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyManagement.Models
{
    public class OrderViewModel
    {
        CompanyManagmentEntities db;
        public Order Order { get; set; }
        public OrderDetail OrderDetail { get; set; }
        public IEnumerable<OrderDetail> OrderDetails { get; set; }
        public List<Product> Products { get; set; }
        public string [] OrderType { get; set; }
        public List<Customer>Customers { get; set; }

        public void AddOrder(Order order)
        {
            db = new CompanyManagmentEntities();
            //var date_time_now = DateTime.Now;
            Order newOrder = new Order();
            newOrder.custom_order_id = order.custom_order_id;
            newOrder.order_type = order.order_type;
            newOrder.customer_id = order.customer_id;
            newOrder.total_payment = order.total_payment;
            newOrder.total_vat = order.total_vat;
            //newOrder.payment_amount = order.payment_amount;
            //newOrder.rest_amount = order.rest_amount;
            newOrder.order_date = order.order_date;
            newOrder.notes = order.notes;
            db.Orders.Add(newOrder);
            db.SaveChanges();
            string order_id = newOrder.custom_order_id;
            foreach (var item in order.OrderDetails)
            {
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.order_date = item.order_date;
                orderDetail.order_id = order_id;
                orderDetail.product_id = item.product_id;
                orderDetail.sale_price = item.sale_price;
                orderDetail.cost_price = item.cost_price;
                orderDetail.quantity = item.quantity;
                orderDetail.uom_id = item.uom_id;
                orderDetail.discount = item.discount;
                orderDetail.vat = item.vat;
                orderDetail.total_price = item.total_price;
                db.OrderDetails.Add(orderDetail);
                db.SaveChanges();
                double? income =0;
                double? outcome=0;
                decimal? unit_price = 0;
                var order_uom = db.Unit_of_Measures.Find(item.uom_id);
                double? realQuantity = 0;
                if(order_uom.unit_type == "bigger")
                {
                    realQuantity = item.quantity * order_uom.ratio;
                    item.sale_price = item.sale_price /(decimal) order_uom.ratio;
                }
                else if(order_uom.unit_type == "smaller")
                {
                    realQuantity = item.quantity / order_uom.ratio;
                }
                else
                {
                    realQuantity = item.quantity;
                }
                if (order.order_type == "شراء")
                {
                    item.quantity = realQuantity;
                    Purchase(db, item);
                    income = item.quantity;
                    unit_price = item.cost_price;
                }
                else
                {
                    Sale(db, item.product_id, item.quantity);
                    outcome = item.quantity;
                    unit_price = item.sale_price;
                }
                // ============ 
                var pros = db.ProductDetails.Where(p => p.product_id == item.product_id);
                double? count = 0;
                foreach(var item2 in pros)
                {
                    count += item2.quantity;
                }
                Transaction Transaction = new Transaction() {
                    product_id = item.product_id,
                    balance = count,
                    income = income,
                    outcome = outcome,
                    sale_price = unit_price,
                    move_date = item.order_date,
                    order_id = order_id
                };
                db.Transactions.Add(Transaction);
                db.SaveChanges();
            }
        }
        private void Purchase(CompanyManagmentEntities db,OrderDetail orderDetail)
        {
            ProductDetail newItem = new ProductDetail();
            newItem.product_id = orderDetail.product_id;
            newItem.sale_price = orderDetail.sale_price;
            newItem.cost_price = orderDetail.cost_price;
            newItem.quantity = orderDetail.quantity;
            newItem.total_price = orderDetail.total_price;
            newItem.date_of_buy = orderDetail.order_date;
            db.ProductDetails.Add(newItem);
            db.SaveChanges();
        }
        private bool Sale(CompanyManagmentEntities db,int productId,double? quantity)
        {
            try
            {
                var product = db.ProductDetails.OrderBy(s => s.date_of_buy).First(s => s.product_id == productId && s.quantity > 0);
                if (product != null && quantity <= product.quantity)
                {
                    product.quantity -= quantity;
                    db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        
    }
}