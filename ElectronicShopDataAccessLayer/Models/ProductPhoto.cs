using ElectronicShopDataAccessLayer.Core.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ElectronicShopDataAccessLayer.Models
{
    public class ProductPhoto : Entity
    {
        public string FileName { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        [NotMapped]
        public bool IsMain { get; set; }

    }
}
