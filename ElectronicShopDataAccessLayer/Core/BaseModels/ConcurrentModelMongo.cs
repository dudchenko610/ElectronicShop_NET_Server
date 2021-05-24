using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ElectronicShopDataAccessLayer.Core.BaseModels
{
    public class ConcurrentModelMongo
    {
        [NotMapped]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [NotMapped]
        public string AccessId { get; set; } = "";
    }
}
