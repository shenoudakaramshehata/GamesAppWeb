﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Gameapp.Models
{
    [Index(nameof(CategoryId), Name = "IX_SubCategory_CategoryId")]
    public partial class SubCategory
    {
        public SubCategory()
        {
            Items = new HashSet<Items>();
            Shop = new HashSet<Shop>();
        }

        [Key]
        public int SubCategoryId { get; set; }

        public int CategoryId { get; set; }
        [Column("SubCategoryTLAR")]
        [StringLength(50)]
        [Required]
        public string SubCategoryTlar { get; set; }
        [Column("SubCategoryTLEN")]
        [StringLength(50)]
        [Required]

        public string SubCategoryTlen { get; set; }
        //[Required]

        public string SubCategoryPic { get; set; }

        public bool? IsActive { get; set; }

        [Required]
            
        public int? OrderIndex { get; set; }

        [ForeignKey(nameof(CategoryId))]
        [InverseProperty("SubCategory")]
        public virtual Category Category { get; set; }
        [InverseProperty("SubCategory")]
        public virtual ICollection<Items> Items { get; set; }
        [InverseProperty("SubCategory")]
        public virtual ICollection<Shop> Shop { get; set; }
    }
}