using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicShopDataAccessLayer.ViewModels.Product.Query
{
    public class ProductJoined
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ManufacturerId { get; set; }
        public int CategoryId { get; set; }
        public int ProductMainPhotoId { get; set; }
        public uint Price { get; set; }
        public string FileName { get; set; }
    }
}
