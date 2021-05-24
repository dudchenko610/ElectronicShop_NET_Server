using ElectronicShopDataAccessLayer.Core.BaseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicShopDataAccessLayer.Models
{
    public class N_N_Product_CharacteristicValue : Entity
    {

        public int ProductId { get; set; }
        public Product Product { get; set; }


        public int CharacteristicValueId { get; set; }
        public CharacteristicValue CharacteristicValue { get; set; }


    }
}
