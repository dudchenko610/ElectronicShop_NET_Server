using ElectronicShopDataAccessLayer.Core.BaseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicShopDataAccessLayer.Models
{
    public class Chat : Entity
    {

        public string UserId1 { get; set; }
        public User User1 { get; set; }

        public string UserId2 { get; set; }
        public User User2 { get; set; }
    }
}
