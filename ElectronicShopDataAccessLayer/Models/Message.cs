using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicShopDataAccessLayer.Models
{
    public class Message
    {

        public Message()
        {
            States = new List<MessageState>();
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string AuthorUserId { get; set; }
        public string AuthorUserName { get; set; }

        public string Content { get; set; }


        public string ReceiverUserId { get; set; }
        public string ReceiverUserName { get; set; }

        public DateTime DateTime { get; set; }
        public List<MessageState> States { get; set; }

        public Message Copy()
        {
            Message msg = new Message();
            msg.Id = this.Id;
            msg.AuthorUserId = this.AuthorUserId;
            msg.AuthorUserName = this.AuthorUserName;
            msg.Content = this.Content;
            msg.ReceiverUserId = this.ReceiverUserId;
            msg.ReceiverUserName = this.ReceiverUserName;
            msg.DateTime = this.DateTime;
            msg.States = this.States;

            return msg;
        }
    }
}
