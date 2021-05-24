using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ViewModels.Product.Filter
{
    public class ProductFilter
    {
        public int CategoryId { get; set; } = -1;
        public int MinPrice { get; set; } = -1;
        public int MaxPrice { get; set; } = 9999999;
        public List<int> ManufacturerIds { get; set; } = new List<int>();
        public List<int> CharacteristicValuesIds { get; set; } = new List<int>();

        public ProductFilter()
        {

        }

        public ProductFilter(ProductFilter productFilter)
        {
            // adjust to correct filter
            CategoryId = productFilter.CategoryId;
            MinPrice = productFilter.MinPrice;
            MaxPrice = productFilter.MaxPrice;
            ManufacturerIds = productFilter.ManufacturerIds;
            CharacteristicValuesIds = productFilter.CharacteristicValuesIds;

        }

    }
}
