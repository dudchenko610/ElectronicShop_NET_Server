using ElectronicShopDataAccessLayer.Core.BaseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicShopDataAccessLayer.Models
{
    public class OrderAuthItem : Entity
    {
        public int OrderAuthId { get; set; }
        public OrderAuth OrderAuth { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Count { get; set; }

    }
}
