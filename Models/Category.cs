﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Gameapp.Models
{
    public partial class Category
    {
        public Category()
        {
            Items = new HashSet<Items>();
            SubCategory = new HashSet<SubCategory>();
        }

        [Key]
        public int CategoryId { get; set; }
        [Column("CategoryTLAR")]
        [StringLength(50)]
        [Required]
        public string CategoryTlar { get; set; }
        [Column("CategoryTLEN")]
        [StringLength(50)]
        [Required]
        public string CategoryTlen { get; set; }
        public string CategoryPic { get; set; }
        public string CategoryIcon { get; set; }

        public bool? IsActive { get; set; }
        public int? OrderIndex { get; set; }

        [InverseProperty("Category")]
        public virtual ICollection<Items> Items { get; set; }
        [InverseProperty("Category")]
        public virtual ICollection<SubCategory> SubCategory { get; set; }
    }
}