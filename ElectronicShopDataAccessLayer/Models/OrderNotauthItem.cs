using ElectronicShopDataAccessLayer.Core.BaseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicShopDataAccessLayer.Models
{
    public class OrderNotauthItem : Entity
    {

        public int OrderNotauthId { get; set; }
        public OrderNotauth OrderNotauth { get; set; }


        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Count { get; set; }
    }
}
