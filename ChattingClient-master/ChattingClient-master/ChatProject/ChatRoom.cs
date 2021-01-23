using ChatProject.Models;
using ChatProject.Repositories;
using ChatProject.Repositories.Implementation;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProject
{
    public class ChatRoom
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public ChatRoom(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        private readonly ConcurrentDictionary<string, IServerStreamWriter<Message>> users = new ConcurrentDictionary<string, IServerStreamWriter<Message>>();
        private readonly ConcurrentDictionary<string, int> userRoomDictionary = new ConcurrentDictionary<string, int>();

        public async Task JoinAsync(Message message, IServerStreamWriter<Message> response)
        {
            users.TryAdd(message.User, response);
            userRoomDictionary.TryAdd(message.User, message.Room);
            await LoadChatHistoryForUser(message.Room, message.User, response);
        }

        public void Remove(string name)
        {
            users.TryRemove(name, out _);
            userRoomDictionary.TryRemove(name, out _);
        }

        public async Task BroadcastMessageAsync(Message message) => await BroadcastMessage(message);

        private async Task BroadcastMessage(Message message)
        {
            foreach (var user in users.Where(x => x.Key != message.User))
            {
                if (userRoomDictionary.GetValueOrDefault(user.Key).Equals(message.Room))
                {
                    var item = await SendMessageToSubscriber(user, message);
                    if (item != null)
                    {
                        Remove(item?.Key);
                    }
                }
            }
        }

        private async Task LoadChatHistoryForUser(int roomId, string username, IServerStreamWriter<Message> user)
        {

            using var scope = _scopeFactory.CreateScope();
            var _historyRepository = scope.ServiceProvider.GetRequiredService<IHistoryRepository>();
            var history = _historyRepository.GetHistoriesByRoomId(roomId);
            foreach (var message in history)
            {
                Message msg = new Message()
                {
                    User = message.Username,
                    Text = message.Text,
                    Room = message.RoomId
                };
                var pair = new KeyValuePair<string, IServerStreamWriter<Message>>(username, user);
                await SendMessageToSubscriber(pair, msg);
            }
        }

        private async Task<KeyValuePair<string, IServerStreamWriter<Message>>?> SendMessageToSubscriber(KeyValuePair<string, IServerStreamWriter<Message>> user, Message message)
        {
            try
            {
                await user.Value.WriteAsync(message);

                var msg = new Models.Message()
                {
                    Username = message.User,
                    Text = message.Text,
                    RoomId = message.Room
                };
                using var scope = _scopeFactory.CreateScope();
                var _messageRepository = scope.ServiceProvider.GetRequiredService<IMessageRepository>();
                int msgId = _messageRepository.NewMessage(msg).Result;

                var history = new History
                {
                    MessageId = msgId,
                    RoomId = message.Room
                };

                var _historyRepository = scope.ServiceProvider.GetRequiredService<IHistoryRepository>();
                _historyRepository.NewHistoryAsync(history);

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return user;
            }
        }

    }
}
