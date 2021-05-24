using ElectronicShopDataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicShopDataAccessLayer.ViewModels.Chatting
{
    public class GetMessagesResponse
    {
        public List<Message> Messages { get; set; }
        public bool HasMore { get; set; }
    }
}
