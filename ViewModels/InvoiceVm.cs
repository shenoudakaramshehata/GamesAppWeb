using Gameapp.ViewModels;
using System;
using System.Collections.Generic;

namespace Gameapp.ViewModels
{
    public class InvoiceVm
    {
        public int OrderId { get; set; }
        public string OrderDate { get; set; }
        public int ItemOrder { get; set; }
        public double NetOrder { get; set; }
        public double OrderTotal { get; set; }
        public string OrderDateWithAnotherFo { get; set; }
        public string CusAddress { get; set; }
        public string CusName { get; set; }
        public string Status { get; set; }
        public string SiteEmail { get; set; }
        public string SitePhone { get; set; }
        public string Country { get; set; }
        public int? CountryId { get; set; }

        public double Discount  { get; set; }
        public double ShippingCost  { get; set; }
        public string ShippingAddress  { get; set; }
        public string ShippingAddressPhone  { get; set; }
        public int InvoiceNumber  { get; set; }
        public string WebSite  { get; set; }
        public string SuppEmail  { get; set; }
        public string ConntactNumber  { get; set; }
        public string PaymentTit  { get; set; }
        public string Address  { get; set; }
        public string OrderTime  { get; set; }
        public string CusArea { get; set; }
        public string CusStreet { get; set; }
        public string CusBuilding { get; set; }
        public string CusFloor { get; set; }
        public string orderNo { get; set; }

    
        public List<OrderItemVm> orderItemVms { get; set; }
        public string? ShippingLabel { get; set; }
        public string? ShippingNo { get; set; }
        public string ItemTitleEn { get; set; }
        public string ItemDis { get; set; }
        public string ItemImage { get; set; }
        public double ItemPrice { get; set; }
        public int ItemQuantity { get; set; }
        public double Total { get; set; }
        public string OrderSerail { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string AdminAddress { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int? currencyId { get; set; }
        public string TwitterLink { get; set; }
        public string WhatsApplink { get; set; }
        public string Instgramlink { get; set; }


        public string currencyName { get; set; }


    }
}
