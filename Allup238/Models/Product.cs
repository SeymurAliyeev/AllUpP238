﻿using Humanizer.Localisation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AllUpMVC.Models
{
    public class Product : BaseEntity
    {
        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(350)]
        public string Desc { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public double CostPrice { get; set; }
        public double SalePrice { get; set; }
        public double DiscountPercent { get; set; }
        public bool IsNew { get; set; }
        public bool IsBestProduct { get; set; }
        public bool IsAvailable { get; set; }
        public int StockCount { get; set; }
        public string ProductCode { get; set; }
        public bool IsFeatured { get; set; }

        public List<ProductImage>? ProductImages { get; set; }
        [NotMapped]
        public IFormFile? PosterImageFile { get; set; }
        [NotMapped]
        public IFormFile? HoverImageFile { get; set; }
        [NotMapped]
        public List<IFormFile>? ImageFiles { get; set; }

        [NotMapped]
        public List<int>? ProductImageIds { get; set; }
    }
}
