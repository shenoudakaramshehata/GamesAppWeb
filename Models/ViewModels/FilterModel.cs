using Gameapp.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gameapp.Models
{
    public class FilterModel
    {
        public DateTime? FromDate { set; get; }
        public DateTime? ToDate { set; get; }
        public DateTime? OnDay { set; get; }
        public int? ShopId { get; set; }
        public int? ShippingAddressId { get; set; }
        public int? CustomterId { get; set; }

        public string radiobtn { get; set; }
        
    }
}
