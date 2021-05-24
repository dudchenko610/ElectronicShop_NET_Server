using ElectronicShopDataAccessLayer.Core.BaseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicShopDataAccessLayer.Models
{
    public class Category : Entity
    {
        
        public string Name { get; set; }

        public List<Product> Products { get; set; }
        public List<Characteristic> Characteristics { get; set; }

    }
}
