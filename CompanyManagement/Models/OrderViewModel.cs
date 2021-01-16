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
            var date_time_now = DateTime.Now;
            Order newOrder = new Order();
            newOrder.custom_order_id = order.custom_order_id;
            newOrder.order_type = order.order_type;
            newOrder.customer_id = order.customer_id;
            newOrder.total_payment = order.total_payment;
            newOrder.payment_amount = order.payment_amount;
            newOrder.rest_amount = order.rest_amount;
            newOrder.order_date = date_time_now;
            db.Orders.Add(newOrder);
            db.SaveChanges();
            string order_id = newOrder.custom_order_id;
            foreach (var item in order.OrderDetails)
            {
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.order_data = date_time_now;
                orderDetail.order_id = order_id;
                orderDetail.product_id = item.product_id;
                orderDetail.unit_price = item.unit_price;
                orderDetail.quantity = item.quantity;
                orderDetail.discount = item.discount;
                orderDetail.total_price = item.total_price;
                db.OrderDetails.Add(orderDetail);
                db.SaveChanges();
                int? income =0;
                int? outcome=0;
                if (order.order_type == "شراء")
                {
                    Purchase(db, item, date_time_now);
                    income = item.quantity;
                }
                else
                {
                    Sale(db, item.product_id, item.quantity);
                    outcome = item.quantity;
                }
                // ============ 
                var pros = db.ProductDetails.Where(p => p.product_id == item.product_id);
                int? count = 0;
                foreach(var item2 in pros)
                {
                    count += item2.quantity;
                }
                Transaction Transaction = new Transaction() {
                    product_id = item.product_id,
                    balance = count,
                    income = income,
                    outcome = outcome,
                    unit_cost = item.unit_price,
                    move_date = date_time_now,
                    order_id = order_id
                };
                db.Transactions.Add(Transaction);
                db.SaveChanges();
            }
        }
        private void Purchase(CompanyManagmentEntities db,OrderDetail orderDetail,DateTime dateTime)
        {
            ProductDetail newItem = new ProductDetail();
            newItem.product_id = orderDetail.product_id;
            newItem.unit_price = orderDetail.unit_price;
            newItem.quantity = orderDetail.quantity;
            newItem.total_price = orderDetail.total_price;
            newItem.date_of_buy = dateTime;
            db.ProductDetails.Add(newItem);
            db.SaveChanges();
        }
        private bool Sale(CompanyManagmentEntities db,int productId,int? quantity)
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