using BeeFriends.Context;
using BeeFriends.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BeeFriends.Hubs
{
    public class LobbyHub : Hub
    {
        private readonly AppDBContext _context;

        public LobbyHub(AppDBContext context)
        {
            _context = context;
        }

        public async Task JoinLobby(UserConnection userConnection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.Room);
            userConnection.Id = Context.ConnectionId;
            _context.UserConnections.Add(userConnection);

            var rooms = _context.Rooms.ToList();
            Clients.Group(userConnection.Room).SendAsync("ActiveRooms", rooms);
        }

        public async Task NewRoomCreated()
        {
            var rooms = _context.Rooms.ToList();
            Clients.Group("Lobby").SendAsync("ActiveRooms", rooms);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var userConnection = _context.UserConnections.FirstOrDefault(x => x.Id == Context.ConnectionId);

            if (userConnection != null)
            {
                _context.UserConnections.Remove(userConnection);
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}
