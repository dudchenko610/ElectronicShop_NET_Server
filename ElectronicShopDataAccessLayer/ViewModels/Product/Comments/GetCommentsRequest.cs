using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicShopDataAccessLayer.ViewModels.Product.Comments
{
    public class GetCommentsRequest
    {
        public GetCommentsRequest()
        {
            Size = 10;
        }
        public int Size { get; set; } = 10;
        public int ProductId { get; set; }
        public int LastCommentId { get; set; }
    }
}
