using ElectronicShopDataAccessLayer.Core.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ElectronicShopDataAccessLayer.Models
{
    public class CharacteristicValue : Entity
    {
        public string Name { get; set; }

        public int CharacteristicId { get; set; }
        public Characteristic Characteristic { get; set; }

        public List<N_N_Product_CharacteristicValue> N_N_Product_Characteristics { get; set; }


    }
}
