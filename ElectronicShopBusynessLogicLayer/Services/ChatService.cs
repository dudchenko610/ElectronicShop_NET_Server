using ElectronicShopDataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ElectronicShopDataAccessLayer.Repositories;
using ElectronicShopDataAccessLayer.ViewModels.Chatting;
using Shared.ViewModels.Chatting;
using Shared.SignalR;
using Shared.Exceptions;
using Shared.Constants;

namespace ElectronicShopBusinessLogicLayer.Services
{
    public class ChatService
    {

        private MessageRepository _messageRepository;
        private UserService _userService;

        public ChatService(MessageRepository messageRepository, UserService userService)
        {
            _messageRepository = messageRepository;
            _userService = userService;
        }


        public async Task<List<ContactInfo>> GetMyContactsAsync()
        {
            User meUser = await _userService.GetMyUserAsync();
            return await _messageRepository.GetContactsAsync(meUser);
        }

        public async Task<ContactInfo> GetMyContactAsync(User oppositeUser)
        {
            User meUser = await _userService.GetMyUserAsync();
            return await _messageRepository.GetMyContactAsync(meUser, oppositeUser);
        }

        public async Task<GetMessagesResponse> GetMessagesAsync(GetMessagesRequest getMessagesInfo)
        {
            User myUser = await _userService.GetMyUserAsync();
            string listName = _messageRepository.GetChatName(myUser.Id, getMessagesInfo.ContactInfo.User.Id);

            GetMessagesResponse getMessagesResponse = await _messageRepository.GetMessagesAsync(listName, getMessagesInfo.MessageId);

            return getMessagesResponse;
        }

        public async Task<MessageSentFeedBack> SendMessageAsync(Message message, SendFeedBackDelegate feedBackDelegate)
        {
            User myUser = await _userService.GetMyUserAsync();

            if (message.AuthorUserId != myUser.Id)
            {
                throw new ServerException(Constants.Errors.Chatting.YOU_ARE_NOT_AUTHOR);
            }

            if (message.AuthorUserId == message.ReceiverUserId)
            {
                throw new ServerException(Constants.Errors.Chatting.YOU_ARE_CANNOT_BE_RECEIVER_OF_YOUR_MESSAGES);
            }

            await _messageRepository.CreateChatIfNotCreatedAsync(myUser, new User { Id = message.ReceiverUserId });

            string listName = _messageRepository.GetChatName(message.AuthorUserId, message.ReceiverUserId);

            // INSERT MESSAGE INTO DATABASE
            message.DateTime = new ElectronicShopDataAccessLayer.Models.DateTime();

            message.States.Add(new MessageState(message.AuthorUserId, true)); // My Id
            message.States.Add(new MessageState(message.ReceiverUserId, false));

            await _messageRepository.InsertMessageAsync(listName, message);

            // CALCULATE UNREAD AMOUNT FOR OPPOSITE USER
            int unreadCount = await _messageRepository.GetUnreadMessagesCountForUserAsync(listName, message.ReceiverUserId);

            // NOTIFY OPPOSITE USER
            object data = new
            {
                Message = message,
                UnreadCount = unreadCount
            };

            await feedBackDelegate(data, message.ReceiverUserName);

            MessageSentFeedBack model = new MessageSentFeedBack
            {
                DateTime = message.DateTime,
                MessageId = message.Id
            };

            return model;
        }

        public async Task<ReadMessagesData> ReadMessagesAsync(ReadMessagesData readMessagesData, SendFeedBackDelegate feedBackToOpposite)
        {


            User meUser = await _userService.GetMyUserAsync();
            string listName = _messageRepository.GetChatName(meUser.Id, readMessagesData.AuthorUserId);

            // READ MESSAGES WITH IDS
            _messageRepository.ReadMessages(listName, meUser.Id, readMessagesData.MessagesIds);

            // CALCULATE UNREAD AMOUNT
            int unreadAmount = await _messageRepository.GetUnreadMessagesCountForUserAsync(listName, meUser.Id);

            // NOTIFY OPPOSITE USER THAT I READ THAT MESSAGES
            ReadMessagesData data = new ReadMessagesData
            {
                AuthorUserId = readMessagesData.AuthorUserId,
                AuthorUserName = readMessagesData.AuthorUserName,

                OppositeUserId = meUser.Id,
                OppositeUserName = meUser.UserName,

                MessagesIds = readMessagesData.MessagesIds,
                UnreadMessagesAmount = unreadAmount
            };

            await feedBackToOpposite(data, readMessagesData.AuthorUserName);

            // NOTIFY ME THAT I READ THAT MESSAGES (update list with users)
            return data;
        }

       

    }
}
