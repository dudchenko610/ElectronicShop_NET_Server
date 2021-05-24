using ElectronicShopDataAccessLayer.Core.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ElectronicShopDataAccessLayer.Models
{
    public class Product : ConcurrentModelSql
    {
        public string Name { get; set; }


        public int ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }


        public int CategoryId { get; set; }
        public Category Category { get; set; }

        
        
        public int ProductMainPhotoId { get; set; }

        [NotMapped]
        public ProductPhoto ProductMainPhoto { get; set; }
        [NotMapped]
        public ProductLike ProductLike { get; set; }

        public List<ProductPhoto> ProductPhotos { get; set; }

        public List<ProductComment> ProductComments { get; set; }

        public List<ProductLike> ProductLikes { get; set; }

        public List<N_N_Product_CharacteristicValue> N_N_Product_Characteristics { get; set; }


        public uint Price { get; set; }
        

        public string Description { get; set; }

 
        public int LikeCount { get; set; }

        public int DislikeCount { get; set; }


        public int BoughtCount { get; set; }
        public int ReviewCount { get; set; }

        public int Count { get; set; }


    }
}
