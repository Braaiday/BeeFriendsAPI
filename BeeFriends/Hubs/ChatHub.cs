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
        private readonly List<UserConnection> _connections;
        private readonly AppDBContext _context;

        public ChatHub(List<UserConnection> connections, AppDBContext context)
        {
            _context = context;
            _botUser = "MyChat Bot";
            _connections = connections;
        }

        public async void SaveMessage(UserConnection userConnection, string message)
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

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var userConnection = _connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);

            if (userConnection != null)
            {
                _connections.Remove(userConnection);
                Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", _botUser, $"{userConnection.User} has left");
                SendUsersConnected(userConnection.Room);
                SaveMessage(userConnection, $"{userConnection.User} has left");
            }

            return base.OnDisconnectedAsync(exception);
        }

        public async Task JoinRoom(UserConnection userConnection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.Room);
            userConnection.ConnectionId = Context.ConnectionId;
            _connections.Add(userConnection);

            await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", _botUser, $"{userConnection.User} has joined {userConnection.Room}");

            SaveMessage(userConnection, $"{userConnection.User} has joined {userConnection.Room}");

            await SendUsersConnected(userConnection.Room);
        }

        public async Task SendMessage(string message)
        {
            var userConnection = _connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);

            if (userConnection != null)
            {
                await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", userConnection.User, message);
                SaveMessage(userConnection, message);
            }
        }

        public async Task IsTypingMessage()
        {
            var userConnection = _connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);

            if (userConnection != null)
            {
                await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", userConnection.User, userConnection.User + " is typing");
            }
        }

        public async Task UserStoppedTyping()
        {
            var userConnection = _connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);

            if (userConnection != null)
            {
                await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", userConnection.User, "");

            }
        }

        public Task SendUsersConnected(string room)
        {
            var users = _connections.Where(c => c.Room == room).Select(c => c.User).ToList();

            return Clients.Group(room).SendAsync("UsersInRoom", users);
        }

    }
}
