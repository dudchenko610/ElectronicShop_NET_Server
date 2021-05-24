using ElectronicShopDataAccessLayer.Core.BaseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicShopDataAccessLayer.Models
{
    public class OrderAuth : Entity
    {

        public int UserId { get; set; }
        public User User { get; set; }

        public List<OrderAuthItem> OrderItems { get; set; }

    }
}
