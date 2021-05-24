
namespace ElectronicShopDataAccessLayer.ViewModels.Chatting
{
    public class ContactInfo
    {
        public ElectronicShopDataAccessLayer.Models.User User { get; set; }
        public int UnreadCount { get; set; }
        public double MillisecondsSince1970 { get; set; }
    }
}
