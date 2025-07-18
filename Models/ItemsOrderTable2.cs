﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Gameapp.Models
{
    [Index(nameof(ItemId), Name = "IX_ItemsOrderTable2_ItemId")]
    public partial class ItemsOrderTable2
    {
        //[Key]
        public int OrderId { get; set; }
        //[Key]
        public int ItemId { get; set; }
        public int Quantity { get; set; }

        [ForeignKey(nameof(ItemId))]
        [InverseProperty(nameof(Items.ItemsOrderTable2))]
        public virtual Items Item { get; set; }
        [ForeignKey(nameof(OrderId))]
        [InverseProperty("ItemsOrderTable2")]
        public virtual Order Order { get; set; }
    }
}