﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Gameapp.Models
{
    [Index(nameof(CategoryId), Name = "IX_Items_CategoryId")]
    [Index(nameof(CustomerId), Name = "IX_Items_CustomerId")]
    [Index(nameof(ShopId), Name = "IX_Items_ShopId")]
    [Index(nameof(SubCategoryId), Name = "IX_Items_SubCategoryId")]
    public partial class Items
    {
        public Items()
        {
            ItemsOrderTable2 = new HashSet<ItemsOrderTable2>();
            ItemsOrderTb = new HashSet<ItemsOrderTb>();
            Order = new HashSet<Order>();
        }

        [Key]
        public int ItemId { get; set; }

        [Required]

        public int? CategoryId { get; set; }

        [Required]

        public int? SubCategoryId { get; set; }
        public int? ShopId { get; set; }
        [MaxLength]
        [Required]
        public string ItemName { get; set; }

        [Required]
        public string ItemNameAr { get; set; }
        public string ItemImage { get; set; }
        [MaxLength]
        [Required]

        public string ItemDescription { get; set; }

        [MaxLength]
        [Required]

        public string ItemDescriptionAr { get; set; }

        [Column(TypeName = "decimal(18, 0)")]
        [Required]

        public decimal? ItemPrice { get; set; }

        public bool? IsActive { get; set; }
        [Required]

        public int? OrderIndex { get; set; }
        public int? CustomerId { get; set; }
        public bool? IsFavourite { get; set; }
        public bool? OutOfStock { get; set; }

        [ForeignKey(nameof(CategoryId))]
        [InverseProperty("Items")]
        public virtual Category Category { get; set; }
        [ForeignKey(nameof(CustomerId))]
        [InverseProperty("Items")]
        public virtual Customer Customer { get; set; }
        [ForeignKey(nameof(ShopId))]
        [InverseProperty("Items")]
        public virtual Shop Shop { get; set; }
        [ForeignKey(nameof(SubCategoryId))]
        [InverseProperty("Items")]
        public virtual SubCategory SubCategory { get; set; }
        [InverseProperty("Item")]
        public virtual ICollection<ItemsOrderTable2> ItemsOrderTable2 { get; set; }
        [InverseProperty("ItemId1Navigation")]
        public virtual ICollection<ItemsOrderTb> ItemsOrderTb { get; set; }
        [InverseProperty("ItemsItem")]
        public virtual ICollection<Order> Order { get; set; }
        
      
        public IEnumerable<ProductImages> ProductImages { get; set; }



        public int? CountryId { get; set; }

        [ForeignKey("CountryId")]
        public Country Country { get; set; }
    }
}