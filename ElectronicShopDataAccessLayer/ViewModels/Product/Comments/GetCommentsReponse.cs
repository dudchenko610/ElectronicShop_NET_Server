using ElectronicShopDataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicShopDataAccessLayer.ViewModels.Product.Comments
{
    public class GetCommentsResponse
    {
        public List<ProductComment> Comments { get; set; }
        public bool HasMore { get; set; }
    }
}
