using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    [Authorize]
    public class PresenceHub : Hub
    {
        private readonly PresenceTracker _tracker;

        public PresenceHub(PresenceTracker tracker)
        {
            _tracker = tracker;
        }

        public override async Task OnConnectedAsync()
        {
            var isOnline = await _tracker.UserConnected(
                Context.User.GetUsername(),
                Context.ConnectionId
            );
            if (isOnline)
                await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUsername()); //UserIsOnline here is used in presence service

            var currentUsers = await _tracker.GetOnlineUsers();
            await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers); //GetOnlineUsers here is the method in presence tracker
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var isOffline = await _tracker.UserDisconnected(
                Context.User.GetUsername(),
                Context.ConnectionId
            );
            if (isOffline)
                await Clients.Caller.SendAsync("UserIsOffline", Context.User.GetUsername()); //UserIsOffline here is used in presence service

            await base.OnDisconnectedAsync(exception);
        }
    }
}
