using ElectronicShopDataAccessLayer.Core.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ElectronicShopDataAccessLayer.Models
{
    public class Characteristic : Entity
    {
        public string Name { get; set; }


        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public List<CharacteristicValue> CharacteristicValues { get; set; } = new List<CharacteristicValue>();
    }
}
