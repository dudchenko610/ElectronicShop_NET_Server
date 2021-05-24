using ElectronicShopDataAccessLayer.Contexts;
using ElectronicShopDataAccessLayer.Core;
using ElectronicShopDataAccessLayer.Models;
using ElectronicShopDataAccessLayer.ViewModels;
using ElectronicShopDataAccessLayer.ViewModels.Chatting;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShopDataAccessLayer.Repositories
{
    public class MessageRepository
    {

        private DbContextMongo _dbContextMongo;
        private DbContextSql _dbContextMSSQL;

        public MessageRepository(DbContextMongo dbContextMongo, DbContextSql dbContextMSSQL)
        {
            _dbContextMongo = dbContextMongo;
            _dbContextMSSQL = dbContextMSSQL;
        }

        public string GetChatName(string userId1, string userId2)
        {

            string result = "";
            for (int i = 0; i < userId1.Length; i++)
            {
                if (userId1[i] > userId2[i])
                {
                    result += userId1 + "." + userId2;
                    break;
                }
                else if (userId1[i] < userId2[i])
                {
                    result += userId2 + "." + userId1;
                    break;
                }
            }

            return "chat." + result;
        }

        public async Task<GetMessagesResponse> GetMessagesAsync(string listName, string lastMessageId)
        {
            int messagesCount = 10;

            IMongoCollection<Message> messagesCollection = _dbContextMongo.Database.GetCollection<Message>(listName);

            BsonDocument filter;
            List<Message> messages;

            if (string.IsNullOrEmpty(lastMessageId))
            {
                filter = new BsonDocument();
            }
            else
            {
                filter = new BsonDocument("_id", new BsonDocument("$lt", new ObjectId(lastMessageId)));
            }

            messages = await messagesCollection
               .Find(filter)
               .Sort(new SortDefinitionBuilder<Message>().Descending("$natural"))
               .Limit(10)
               .ToListAsync();

            bool hasMore = true;


            if (messages.Count < messagesCount)
            {
                hasMore = false;
            }
            else 
            {


                string lastInGotMessages = messages[messagesCount - 1].Id;


                filter = new BsonDocument("_id", new BsonDocument("$lt", new ObjectId(lastInGotMessages)));

                long count = await messagesCollection
                    .Find(filter)
                    .Sort(new SortDefinitionBuilder<Message>().Descending("$natural"))
                    .Limit(1)
                    .CountDocumentsAsync();

                if (count == 0)
                {
                    hasMore = false;
                }
            }

            GetMessagesResponse getMessagesResponse = new GetMessagesResponse
            {
                Messages = messages,
                HasMore = hasMore
            };

            return getMessagesResponse;
        }

        public async Task<Message> InsertMessageAsync(string listName, Message message)
        {
            IMongoCollection<Message> messages
                = _dbContextMongo.Database.GetCollection<Message>(listName);

            await messages.InsertOneAsync(message);

            return message;
        }

        public async Task<int> GetUnreadMessagesCountForUserAsync(string listName, string userId)
        {
            IMongoCollection<Message> messages
                = _dbContextMongo.Database.GetCollection<Message>(listName);

            var f = Builders<Message>.Filter.ElemMatch(
                    message => message.States,
                    message_state => message_state.UserId == userId && message_state.Read == false);

            long unreadMessagesAmount = await messages.CountDocumentsAsync(f);

            return (int) unreadMessagesAmount;
        }

        public async Task<Message> GetLastMessageAsync(string listName)
        {
            IMongoCollection<Message> messages
                    = _dbContextMongo.Database.GetCollection<Message>(listName);

            List<Message> msgs = await messages
                .Find(new BsonDocument())
                .Sort(new SortDefinitionBuilder<Message>().Descending("$natural"))
                .Limit(1)
                .ToListAsync();

            if (msgs.Count == 0)
            {
                return null;
            }

            return msgs[0];
        }

        public void ReadMessages(string listName, string meUserId, List<string> undreadMessagesIds)
        {
            IMongoCollection<Message> messages
                = _dbContextMongo.Database.GetCollection<Message>(listName);

            for (int i = 0; i < undreadMessagesIds.Count; i++)
            {
                messages.FindOneAndUpdate(
                    x => x.Id == undreadMessagesIds[i] && x.States.Any(c => c.UserId == meUserId && c.Read == false), // find this match
                    Builders<Message>.Update.Set(c => c.States[-1].Read, true));               // -1 means update first matching array element
            }

        }

        public async Task CreateChatIfNotCreatedAsync(User user1, User user2)
        {
            Chat chat = await _dbContextMSSQL.Chats
                .FirstOrDefaultAsync(c => (c.UserId1 == user1.Id && c.UserId2 == user2.Id) || (c.UserId2 == user1.Id && c.UserId1 == user2.Id) );

            if (chat == null)
            {
                Chat cht = new Chat
                {
                    UserId1 = user1.Id,
                    UserId2 = user2.Id
                };

                // relative to meUser in Chats1 User1 always is id of meUser

                await _dbContextMSSQL.Chats.AddAsync(cht);
                await _dbContextMSSQL.SaveChangesAsync();
            }

        }

        public async Task<List<ContactInfo>> GetContactsAsync(User meUser)
        {
            User meUserWithChats = await _dbContextMSSQL.Users
                .Include(u => u.Chats1)
                    .ThenInclude(ch => ch.User2)
                .Include(u => u.Chats2)
                    .ThenInclude(ch => ch.User1)
                .FirstOrDefaultAsync(u => u.Id == meUser.Id);

            List<User> contactUsers = new List<User>();

            contactUsers.AddRange(meUserWithChats.Chats1.Select(ch => ch.User2)); // I started chat
            contactUsers.AddRange(meUserWithChats.Chats2.Select(ch => ch.User1)); // Opposite user started chat


            List<ContactInfo> contactInfos = new List<ContactInfo>();
            // Get unread messages per user

            System.DateTime dt1970 = new System.DateTime(1970, 1, 1);

            string listName = "";
            foreach (User oppositeUser in contactUsers)
            {
                listName = GetChatName(meUser.Id, oppositeUser.Id);

                oppositeUser.Chats1 = null;
                oppositeUser.Chats2 = null;

                Message lastMsg = await GetLastMessageAsync(listName);
                System.DateTime dt = lastMsg.DateTime.GetSystemDateTime();

                TimeSpan span = dt - dt1970;

                contactInfos.Add(
                    new ContactInfo
                    {
                        User = oppositeUser,
                        UnreadCount = await GetUnreadMessagesCountForUserAsync(listName, meUser.Id), // INEFFECTIVE
                        MillisecondsSince1970 = span.TotalMilliseconds
                    }
                );
            }

            return contactInfos;
        }

        public async Task<ContactInfo> GetMyContactAsync(User meUser, User oppositeUser)
        {

            Chat chat = await _dbContextMSSQL.Chats
                .FirstOrDefaultAsync(c => (c.UserId1 == meUser.Id && c.UserId2 == oppositeUser.Id)
                    || (c.UserId2 == meUser.Id && c.UserId1 == oppositeUser.Id));

            if (chat == null) 
            {
                throw new Exception("Chat does not exists");
            }

            User opUser = await _dbContextMSSQL.Users.FirstOrDefaultAsync(u => u.Id == oppositeUser.Id);

            opUser.ProductComments = null;
            opUser.Chats1 = null;
            opUser.Chats2 = null;


            string listName = GetChatName(meUser.Id, oppositeUser.Id);

            System.DateTime dt1970 = new System.DateTime(1970, 1, 1);
            Message lastMsg = await GetLastMessageAsync(listName);
            System.DateTime dt = lastMsg.DateTime.GetSystemDateTime();

            TimeSpan span = dt - dt1970;


            ContactInfo contactInfo = new ContactInfo
            {
                User = opUser,
                UnreadCount = await GetUnreadMessagesCountForUserAsync(listName, meUser.Id), // INEFFECTIVE
                MillisecondsSince1970 = span.TotalMilliseconds
            };

            return contactInfo;
        }
    }
}
