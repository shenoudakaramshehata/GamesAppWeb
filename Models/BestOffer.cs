﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Gameapp.Models
{
    [Index(nameof(ItemId), Name = "IX_BestOffer_ItemId")]
    [Index(nameof(ShopId), Name = "IX_BestOffer_ShopId")]
    public partial class BestOffer
    {
        [Key]
        public int BestOfferId { get; set; }
        public int? ItemId { get; set; }
        [StringLength(50)]
        public string ItemName { get; set; }
        public string ItemPhoto { get; set; }
        [StringLength(50)]
        public string ItemDescription { get; set; }
        public int? ItemPrice { get; set; }
        [StringLength(50)]
        public string ShopName { get; set; }
        public int? Disscount { get; set; }
        public int? Discount { get; set; }
        public int? ShopId { get; set; }
        public string ShopPhoto { get; set; }
        public int? Subcategoryid { get; set; }
        [Column("categoryid")]
        public int? Categoryid { get; set; }
        public bool? IsFavourite { get; set; }

        [ForeignKey(nameof(ShopId))]
        [InverseProperty("BestOffer")]
        public virtual Shop Shop { get; set; }
        public Items Item { get; set; }
        public Category MyProperty { get; set; }
    }
}