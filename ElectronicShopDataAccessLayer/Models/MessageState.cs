using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicShopDataAccessLayer.Models
{
    public class MessageState
    {

        public string UserId { get; set; }
        public bool Read { get; set; }

        public MessageState(string UserId, bool Read)
        {
            this.UserId = UserId;
            this.Read = Read;
        }

    }
}
