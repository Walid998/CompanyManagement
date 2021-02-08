$(document).ready(function () {
    $('form').submit(function (e) {
        e.preventDefault();
    });
    // check if order number duplicated
    $("#Order_custom_order_id").on("keyup", function () {
        var order_id = $("#Order_custom_order_id").val();
        IsOrderIdFounded(order_id);
    });
    // Get Product Price , Uom and Vat
    $('#product_id').on('change', function () {
        var productId = $("#product_id").val();
        GetPriceOfProduct(productId);
    });
    // Calculate Total SubOrder
    $("#OrderDetail_quantity").on("keyup keypress", function (e) {
        var keyCode = e.keyCode || e.which;
        if (keyCode === 13) {
    e.preventDefault();
            return false;
        }

        var avaliable_quan = parseFloat($("#available_quantity_value").val());
        var required_qaun = parseFloat($("#OrderDetail_quantity").val());
        if ($("#order_type").val() == "بيع" && required_qaun > avaliable_quan) {
            $("#quantity_validation").text("الكمية المطلوبة اكبر من الكمية المتاحة");
            $("#AddItems").attr("disabled", "disabled");
        }
        else if (required_qaun <= 0)
        {
            $("#quantity_validation").text("لايمكن ادخال كمية اقل من او تساوى 0");
            $("#AddItems").attr("disabled", "disabled");
        }
        else {
            var order_type = $("#OrderType").val();
            var total = TotalSubOrder(order_type);
            if (total == "NaN")
                $("#OrderDetail_total_price").val("");
            else
                $("#OrderDetail_total_price").val(total);
            $("#quantity_validation").text("");
            $("#AddItems").removeAttr("disabled");
        }

    });

    $("#Order_payment_amount").on("keyup", function () {
    CalcRest();
    })

});

function TotalSubOrder(order_type) {
    if (order_type == "بيع") {
        var sale_price = parseFloat($("#OrderDetail_sale_price").val());
        var quantity = parseFloat($("#OrderDetail_quantity").val());
        return parseFloat(sale_price * quantity).toFixed(2);
    } else {
        var cost_price = parseFloat($("#OrderDetail_cost_price").val());
        var quantity = parseFloat($("#OrderDetail_quantity").val());
        return parseFloat(cost_price * quantity).toFixed(2);
    }
}

function TotalSubOrder_OnVat() {
    var product_vat = parseFloat($("#Product_Vat").val());
    var total = parseFloat($("#OrderDetail_total_price").val());
    return parseFloat(total + (total * (product_vat / 100))).toFixed(2);
}

function CalcFinalTotal() {
    $("#TotalBeforVat").text("0.00");
    $("#TotalVat").text("0.00");
    $("#Order_total_payment").text("0.00");
    
    var total = 0.00;
    var vat = 0.00;
    $("#ItemListOfSubOrders").find("tr:gt(0)").each(function () {
        total += parseFloat($(this).find("td:eq(7)").text());
        vat += parseFloat($(this).find("td:eq(7)").text()) * ( parseFloat($(this).find("td:eq(6)").text()) / 100);
    })
    var FinalTotal = total + vat ;
    $("#TotalBeforVat").text(total.toFixed(2));
    $("#TotalVat").text(vat.toFixed(2));
    $("#Order_total_payment").text(FinalTotal.toFixed(2));
}
function CalcRest() {

    var total = parseFloat($("#Order_total_payment").val());
    var amount = parseFloat($("#Order_payment_amount").val());
    var rest = amount - total;
    if ($("#Order_total_payment").val() == "") {
    $("#Order_rest_amount").val("0.00");
    } else {
    $("#Order_rest_amount").val(rest.toFixed(2));
    }
}
function ResetSubOrder() {
    $("#product_id").val(0);
    $("#OrderDetail_sale_price").val('');
    $("#OrderDetail_cost_price").val('');
    $("#OrderDetail_quantity").val('');
    $("#OrderDetail_discount").val('');
    $("#Product_Vat").val('');
    $("#OrderDetail_total_price").val('');
    $("#available_quantity_txt").text('');
    $("#product_uom").find("option").each(function () {
    $(this).remove();
    })
}
function ResetOrder() {
    $("#Order_custom_order_id").val("");
    $("#customer_id").val(0);
    $("#order_type").val(0);
    RemoveSubOrder();
    ClearTableOfSubOrders();
    $("#TotalBeforVat").text("0.00");
    $("#TotalVat").text("0.00");
    $("#Order_total_payment").text("0.00");
}
function ClearTableOfSubOrders() {
    $("#ItemListOfSubOrders").find("tr:gt(0)").each(function () {
        $(this).remove();
    })
}
function RemoveSubOrder(ItemId) {
    var totalsuborder = parseFloat($(ItemId).closest('tr').find("td:eq(7)").text());
    var vat_ratio = parseFloat($(ItemId).closest('tr').find("td:eq(6)").text());
    var order_vat = totalsuborder * (vat_ratio / 100);
    var old_vat = parseFloat($("#TotalVat").text());
    var final_total = parseFloat($("#TotalBeforVat").text());
    var new_vat = old_vat - order_vat;
    var new_final_total = final_total - totalsuborder;
    var new_global_total = new_final_total + new_vat;
    $("#TotalBeforVat").text(new_final_total.toFixed(2));
    $("#TotalVat").text(new_vat.toFixed(2));
    $("#Order_total_payment").text(new_global_total.toFixed(2));
    $(ItemId).closest('tr').remove();
}
function PreventEnterKeyPress(e) {
    var keyCode = e.keyCode || e.which;
    if (keyCode === 13) {
    e.preventDefault();
        return false;
    }
}

function IsTableEmpty() {
    var i = 0;
    $("#ItemListOfSubOrders").find("tr:gt(0)").each(function () {
    i = i + 1;
    })
    return i;
}

// Add Customer
function AddCustomer() {
    $('#AddCustomerModal').appendTo("body").modal('show');
}
// Add Product
function AddProduct() {
    $('#AddProductModal').appendTo("body").modal('show');
}
// Add Order
function OrderPlaced() {
    $('#OrderPlaced').appendTo("body").modal('show');
}
// Ajax Calls
function IsOrderIdFounded(orderId) {
    $.ajax({
        async: true,
        type: "Get",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        url: "/Orders/IsOrderIdFounded",
        data: { orderId: orderId },
        success: function (data) {
            if (!data) {
                $("#order_id_validation").text("");
                $("#Order_custom_order_id").attr("style", "color:green;font-weight:bold");
                $("#CreateBill").removeAttr("disabled");
            }
            else {
                $("#Order_custom_order_id").attr("style", "color:red;font-weight:bold");
                $("#order_id_validation").text("هذا الرقم مكرر !!");
                $("#CreateBill").attr("disabled", "disabled");
            }
        },
        error: function () {
            console.log("erorr!!!");
        }

    })
}

function PlaceAnOrder(InvoiceType) {
    if ($("#Order_custom_order_id").val() == "") {
    alert("يجب ادخال رقم الفاتورة");
        $("#Order_custom_order_id").focus();
        return;
    }
    if ($("#customer_id").val() == 0) {
    alert("يجب اختيار عميل");
        $("#customer_id").focus();
        return;
    }
    if (IsTableEmpty() == 0) {
    alert("لا يمكن حفظ فاتورة فارغة");
        $("#product_id").focus();
        return;
    }
    if (parseFloat($("#Order_rest_amount").val()) < 0) {
    alert("المبلغ المدفوع اقل من قيمة اجمالى الفاتورة");
        $("#Order_payment_amount").focus();
        return;
    }
    if ($("#Order_payment_amount").val() == "") {
    alert("يجب ادخال المبلغ المدفوع");
        $("#Order_payment_amount").focus();
        return;
    }
    if (parseFloat($("#Order_payment_amount").val()) < parseFloat($("#Order_total_payment").val())){
    alert("المبلغ المدفوع اقل من قيمة اجمالى الفاتورة");
        $("#Order_payment_amount").focus();
        return;
    }

    var Order = { };
    var OrderDetails = new Array();
    Order.custom_order_id = $("#Order_custom_order_id").val();
    Order.customer_id = $("#customer_id").val();
    Order.order_type = InvoiceType;
    Order.total_payment = parseFloat($("#Order_total_payment").text());
    Order.total_vat = parseFloat($("#TotalVat").text());
    Order.order_date = $("#order_date").val();
    Order.notes = $("#notes").text();
    $("#ItemListOfSubOrders").find("tr:gt(0)").each(function () {
        var SingleOrderDetails = { };
        SingleOrderDetails.order_id = $("#Order_custom_order_id").val();
        SingleOrderDetails.product_id = parseInt($(this).find("td:eq(0)").text());
        SingleOrderDetails.quantity = parseInt($(this).find("td:eq(2)").text());
        SingleOrderDetails.sale_price = parseFloat($(this).find("td:eq(4)").text());
        if (InvoiceType == "بيع")
            SingleOrderDetails.discount = parseFloat($(this).find("td:eq(5)").text());
        else
            SingleOrderDetails.cost_price = parseFloat($(this).find("td:eq(5)").text());
        SingleOrderDetails.total_price = parseFloat($(this).find("td:eq(7)").text());
        SingleOrderDetails.uom_id = parseInt($(this).find("td:eq(9)").text());
        SingleOrderDetails.order_date = $("#order_date").val();
        OrderDetails.push(SingleOrderDetails);
    });
    Order.OrderDetails = OrderDetails;

    $.ajax({
    async: true,
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        url: "/Orders/CreateOrder",
        data: JSON.stringify(Order),
        success: function (data) {
    OrderPlaced();
            ResetOrder();
        },
        error: function () {
    alert("حدث مشكلة اثناء حفظ الفاتورة");
        }
    });
}

function CreateCustomer() {
    Customer = {};
    Customer.name = $("#CustomerModal_Name").val();
    Customer.phone = $("#CustomerModal_Phone").val();
    Customer.email = $("#CustomerModal_Email").val();
    Customer.type = $("#CustomerModal_Type").val();
    $.ajax({
    async: true,
        type: "post",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        url: "/Customers/CreateCustomer",
        data: JSON.stringify(Customer),
        success: function (data) {
    $("#CloseModal_cust").click();
            alert("تم إضافة العميل: " + data.name);
            resetCreateCustomerForm();
            var new_customer = "<option value='" + data.id + "'>" + data.name + "</option>";
            $("#customer_id").append(new_customer);
        },
        error: function () {

}
    });
}
function resetCreateCustomerForm() {
    $("#CustomerModal_Type").val(0);
    $("#CustomerModal_Name").val('');
    $("#CustomerModal_Phone").val('');
    $("#CustomerModal_Email").val('');
}

function CreateProduct() {
    Product = {};
    Product.name = $("#ProductModal_Name").val();
    Product.code = $("#ProductModal_Code").val();
    Product.vat = $("#ProductModal_Vat").val();
    $.ajax({
    async: true,
        type: "post",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        url: "/Products/CreateProduct",
        data: JSON.stringify(Product),
        success: function (data) {
    $("#CloseModal_pro").click();
            alert("تم إضافة المنتج: " + data.name);
            resetCreateProductForm();
            var new_product = "<option value='" + data.id + "'>" + data.name + "</option>";
            $("#product_id").append(new_product);
        },
        error: function () {

}
    });
}
function resetCreateProductForm() {
    $("#ProductModal_Name").val('');
    $("#ProductModal_Code").val('');
    $("#ProductModal_Vat").val('');
}
