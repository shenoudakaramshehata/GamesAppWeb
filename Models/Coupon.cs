﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Gameapp.Models
{
    [Index(nameof(CouponTypeId), Name = "IX_Coupon_CouponTypeId")]
    public partial class Coupon
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Serial { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime IssueDate { get; set; }

        [Required]
        public double? Amount { get; set; }
        public int CouponTypeId { get; set; }
        public bool Used { get; set; }

        [ForeignKey(nameof(CouponTypeId))]
        [InverseProperty("Coupon")]
        public virtual CouponType CouponType { get; set; }
    }
}