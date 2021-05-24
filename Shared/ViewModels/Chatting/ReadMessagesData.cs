using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ViewModels.Chatting
{
    public class ReadMessagesData
    {
        public List<string> MessagesIds { get; set; }
        public string AuthorUserId { get; set; }
        public string AuthorUserName { get; set; }


        public string OppositeUserId { get; set; }
        public string OppositeUserName { get; set; }


        public int UnreadMessagesAmount { get; set; }
    }
}
