using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gameapp.Models.ViewModels
{
    public class ItemImagesAndItemVm
    {
        public int ItemId { get; set; }
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public int? ShopId { get; set; }
        public string ItemName { get; set; }
        public string ItemImage { get; set; }
        public string ItemDescription { get; set; }
        public decimal? ItemPrice { get; set; }
        public bool? IsActive { get; set; }
        public int? OrderIndex { get; set; }
        public int? CustomerId { get; set; }
        public bool? IsFavourite { get; set; }
        public IEnumerable<ProductImages> ProductImages { get; set; }
    }
}
