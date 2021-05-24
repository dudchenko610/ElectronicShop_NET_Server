using ElectronicShopDataAccessLayer.Core.BaseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicShopDataAccessLayer.Models
{
    public class OrderNotauth : Entity
    {

        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }

        public List<OrderNotauthItem> OrderItems { get; set; }

    }
}
