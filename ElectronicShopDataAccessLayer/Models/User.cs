using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ElectronicShopDataAccessLayer.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public string Age { get; set; }

        public string AvatarFileName { get; set; }
        public string RefreshToken { get; set; }

        public List<ProductComment> ProductComments { get; set; }
        public List<Chat> Chats1 { get; set; }
        public List<Chat> Chats2 { get; set; }
    }
}
