using BeeFriends.Context;
using BeeFriends.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace BeeFriends.Hubs
{
    public class ChatHub : Hub
    {
        private readonly string _botUser;
        private readonly AppDBContext _context;

        public ChatHub(AppDBContext context)
        {
            _context = context;
            _botUser = "MyChat Bot";
        }

        public async void SaveMessageUser(UserConnection userConnection, string message)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.name == userConnection.Room);
            var newMessage = new Message
            {
                UserMessage = message,
                RoomId = room.Id,
                User = userConnection.User,
            };
            await _context.Messages.AddAsync(newMessage);
            await _context.SaveChangesAsync();
        }

        public async void SaveMessageBot(UserConnection userConnection, string message)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.name == userConnection.Room);
            if (room == null) { return; }
            var newMessage = new Message
            {
                UserMessage = message,
                RoomId = room?.Id,
                User = _botUser,
            };
            await _context.Messages.AddAsync(newMessage);
            await _context.SaveChangesAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var userConnection = _context.UserConnections.FirstOrDefault(x => x.Id == Context.ConnectionId);

            if (userConnection != null)
            {
                _context.UserConnections.Remove(userConnection);
                Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", _botUser, $"{userConnection.User} has left");
                SendUsersConnected(userConnection.Room);
                SaveMessageBot(userConnection, $"{userConnection.User} has left");
            }

            return base.OnDisconnectedAsync(exception);
        }

        public async Task JoinRoom(UserConnection userConnection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.Room);
            userConnection.Id = Context.ConnectionId;
            _context.UserConnections.Add(userConnection);

            await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", _botUser, $"{userConnection.User} has joined {userConnection.Room}");

            SaveMessageBot(userConnection, $"{userConnection.User} has joined {userConnection.Room}");

            await SendUsersConnected(userConnection.Room);
        }

        public async Task SendMessage(string message)
        {
            var userConnection = _context.UserConnections.FirstOrDefault(x => x.Id == Context.ConnectionId);

            if (userConnection != null)
            {
                await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", userConnection.User, message);
                SaveMessageUser(userConnection, message);
            }
        }

        public async Task IsTypingMessage()
        {
            var userConnection = _context.UserConnections.FirstOrDefault(x => x.Id == Context.ConnectionId);

            if (userConnection != null)
            {
                await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", userConnection.User, userConnection.User + " is typing");
            }
        }

        public async Task UserStoppedTyping()
        {
            var userConnection = _context.UserConnections.FirstOrDefault(x => x.Id == Context.ConnectionId);

            if (userConnection != null)
            {
                await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", userConnection.User, "");

            }
        }

        public Task SendUsersConnected(string room)
        {
            var users = _context.UserConnections.Where(c => c.Room == room).Select(c => c.User).ToList();

            return Clients.Group(room).SendAsync("UsersInRoom", users);
        }

    }
}
