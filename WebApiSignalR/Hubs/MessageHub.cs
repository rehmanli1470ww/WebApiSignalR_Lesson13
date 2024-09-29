using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.AspNetCore.SignalR;
using WebApiSignalR.Helpers;

namespace WebApiSignalR.Hubs
{
    public class MessageHub : Hub
    {
        public static int ChevRoomUserCount { get; set; } = 0;
        public static int BmwRoomUserCount { get; set; } = 0;
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("ReceiveConnectInfo", "User Connected");
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        { 
            await Clients.Others.SendAsync("DisconnectInfo", "User disconnected");
        }

        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message + "'s Offer is ", FileHelper.Read());
        }

        public async Task JoinRoom(string room, string user)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, room);
            await Clients.OthersInGroup(room).SendAsync("ReceiveJoinInfo",room, user);
        }

        public async Task RoomCount(string room)
        {
            //await Groups.AddToGroupAsync(Context.ConnectionId, room);
            if (room == "chevrolet")
            {
                if(ChevRoomUserCount <= 3)
                {
                    ++ChevRoomUserCount;
                
                }
                await Clients.Group(room).SendAsync("ReceiveCount", room, ChevRoomUserCount);
            }
            if (room == "bmw")
            {
                if(BmwRoomUserCount <= 3) 
                {
                    ++BmwRoomUserCount;
                }
                await Clients.Group(room).SendAsync("ReceiveCount", room, BmwRoomUserCount);
            }
        }
        public async Task RoomCount2(string room)
        {
            //await Groups.AddToGroupAsync(Context.ConnectionId, room);
            if (room == "chevrolet")
            {
                if (ChevRoomUserCount == 4)
                {
                    --ChevRoomUserCount;
                }
                --ChevRoomUserCount;
                await Clients.OthersInGroup(room).SendAsync("ReceiveCount2", room, ChevRoomUserCount);
            }
            if (room == "bmw")
            {
                if (BmwRoomUserCount == 4)
                {
                    --BmwRoomUserCount;
                }
                --BmwRoomUserCount;
                await Clients.OthersInGroup(room).SendAsync("ReceiveCount2", room, BmwRoomUserCount);
            }
        }

        public async Task ReturnCount(string room)
        {
            //await Groups.AddToGroupAsync(Context.ConnectionId, room);
            if(room == "chevrolet") 
            {
                await Clients.Group(room).SendAsync("ReceiveAllCount", room, ChevRoomUserCount);
            }
            if(room == "bmw") 
            {
                await Clients.Group(room).SendAsync("ReceiveAllCount", room, BmwRoomUserCount);

            }

        }

        public async Task LeaveRoom(string room, string user)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, room);
            await Clients.OthersInGroup(room).SendAsync("ReceiveLeaveInfo", user + "leaved room");

        }

        public async Task SendMessageRoom(string room, string user)
        {
            await Clients.OthersInGroup(room).SendAsync("ReceiveInfoRoom", user, FileHelper.Read(room));
        }

        public async Task SendWinnerMessageRoom(string room, string message)
        {
            await Clients.Group(room).SendAsync("ReceiveWinInfoRoom", message, FileHelper.Read(room));
        }
    }
}
