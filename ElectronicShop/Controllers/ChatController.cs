using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicShopBusinessLogicLayer.Services;
using ElectronicShopDataAccessLayer.Models;
using ElectronicShopDataAccessLayer.ViewModels;
using ElectronicShopDataAccessLayer.ViewModels.Chatting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Shared.Constants;

namespace ElectronicShop.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ChatController : Controller
    {

        private ChatService _chatService;

        public ChatController(ChatService chatService)
        {
            _chatService = chatService;
        }


        [Authorize]
        [Route(Constants.Routes.Chat.GET_MESSAGES)]
        public async Task<IActionResult> GetMessages(GetMessagesRequest getMessagesInfo)
        {
            GetMessagesResponse res = await _chatService.GetMessagesAsync(getMessagesInfo);
            return Ok(res);
        }

        [Authorize]
        [Route(Constants.Routes.Chat.GET_MY_CONTACTS)]
        public async Task<IActionResult> GetMyContacts()
        {
            List<ContactInfo> contactInfo = await _chatService.GetMyContactsAsync();
            return Ok(contactInfo);
        }

        [Authorize]
        [Route(Constants.Routes.Chat.GET_MY_CONTACT)]
        public async Task<IActionResult> GetMyContact(User user)
        {
            ContactInfo ci = await _chatService.GetMyContactAsync(user);
            return Ok(ci);
        }

    }
}