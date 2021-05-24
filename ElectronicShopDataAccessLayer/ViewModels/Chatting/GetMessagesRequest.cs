using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicShopDataAccessLayer.ViewModels.Chatting
{
    public class GetMessagesRequest
    {
        public ContactInfo ContactInfo { get; set; }
        public string MessageId { get; set; }
    }
}
