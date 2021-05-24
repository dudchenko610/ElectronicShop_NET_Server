using ElectronicShop.Core;
using ElectronicShop.Core.SignalR;
using ElectronicShopBusinessLogicLayer.Services;
using ElectronicShopDataAccessLayer.Models;
using ElectronicShopDataAccessLayer.ViewModels.Chatting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Driver;
using Shared.ViewModels.Chatting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicShop.Hubs
{
    public class ChatHub : SignalRHub
    {
        private ChatService _chatService;

        public ChatHub(ChatService chatService)
        {
            this._chatService = chatService;
        }


        [Authorize]
        public async Task<MessageSentFeedBack> SendMessage(Message incomingMessage)
        {

            return await _chatService.SendMessageAsync(incomingMessage,
                async (data, userName) => 
                {
                    await SendToUser(data, userName, "ReceiveMessage");
                }
            );


        }

        [Authorize]
        public async Task<ReadMessagesData> ReadMessages(ReadMessagesData content)
        {
            return await _chatService.ReadMessagesAsync(content,
                async (data, oppositeUserName) =>
                {
                    await SendToUser(data, oppositeUserName, "OppositeUserReadMessages");
                }
            );
        }



    }
}
