using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exiled.API.Enums;
using Exiled.API.Features;
using PlayerRoles;

namespace ChatPlugin
{
    public class API
    {
        public static List<KeyValuePair<string, string>> ChatHistory = new ();
            
        public static string GetRange(Player player, int broadcastType)
        {
            if (player.Role.Type == RoleTypeId.Spectator)
            {
                return "观察者";
            }
            else if (player.Role.Type == RoleTypeId.None)
            {
                return "大厅";
            }
            else if (player.Role.Type == RoleTypeId.Tutorial)
            {
                return "SCP";
            }
            else if (broadcastType == 2)
            {
                return "全体人类";
            }
            else if (broadcastType == 3)
            {
                return "全体";
            }
            else
            {
                switch (player.Role.Side)
                {
                    case Side.ChaosInsurgency:
                        return "混沌分裂者";
                    case Side.Mtf:
                        return "九尾狐";
                    case Side.Scp:
                        return "SCP";
                    case Side.Flamingo:
                        return "火烈鸟";
                    default:
                        return "其他";
                }
            }
        }
        
        public static string GetRange(Player player)
        {
            if (player.Role.Type == RoleTypeId.Spectator)
            {
                return "观察者";
            }
            else if (player.Role.Type == RoleTypeId.None)
            {
                return "大厅";
            }
            else if (player.Role.Type == RoleTypeId.Tutorial)
            {
                return "SCP";
            }
            else
            {
                switch (player.Role.Side)
                {
                    case Side.ChaosInsurgency:
                        return "混沌分裂者";
                    case Side.Mtf:
                        return "九尾狐";
                    case Side.Scp:
                        return "SCP";
                    case Side.Flamingo:
                        return "火烈鸟";
                    default:
                        return "其他";
                }
            }
        }
        
        public static string BuildFinalMessage(Player player, string message, int broadcastType)
        {
            string playerName = player.Nickname;
            string roomName = "";

            if (player.Role.Type != RoleTypeId.Spectator && player.Role.Type != RoleTypeId.None)
            {
                Room room = player.CurrentRoom;
                roomName = $"[{room.RoomName}]";
            }

            return $"[{GetRange(player, broadcastType)}][{playerName}]{(string.IsNullOrEmpty(roomName) ? "" : roomName)}: {message}";
        }
        
        public static int GetBroadcastType(Player player, string[] argsArray)
        {
            // Broadcast type:
            // 1 Faction Side
            // 2 All Human
            // 3 All Man
            // 4 Default Lobby and Spectator
            // 5 Flamingo
            
            int broadcastType;
            if (player.Role.Type == RoleTypeId.Spectator || player.Role.Type == RoleTypeId.None)
            {
                broadcastType = 4;
            }
            else if (argsArray.Length > 1 && int.TryParse(argsArray[argsArray.Length - 1], out int parsedBroadcastType))
            {
                if (parsedBroadcastType == 4 || parsedBroadcastType == 5)
                {
                    broadcastType = 1;
                }
                else
                {
                    broadcastType = parsedBroadcastType;
                }
            }
            else if (player.Role.Type == RoleTypeId.AlphaFlamingo || player.Role.Type == RoleTypeId.Flamingo || player.Role.Type == RoleTypeId.ZombieFlamingo)
            {
                broadcastType = 5;
            }
            else
            {
                broadcastType = 1;
            }
            return broadcastType;
        }
        
        public static int GetBroadcastType(Player player)
        {
            // Broadcast type:
            // 1 Faction Side
            // 2 All Human
            // 3 All Man
            // 4 Default Lobby and Spectator
            // 5 Flamingo

            int broadcastType;
            if (player.Role.Type == RoleTypeId.Spectator || player.Role.Type == RoleTypeId.None)
            {
                broadcastType = 4;
            }
            else if (player.Role.Type == RoleTypeId.AlphaFlamingo || player.Role.Type == RoleTypeId.Flamingo || player.Role.Type == RoleTypeId.ZombieFlamingo)
            {
                broadcastType = 5;
            }
            else
            {
                broadcastType = 1;
            }
            return broadcastType;
        }

        public static string GetMessage(ArraySegment<string> arguments)
        {
            StringBuilder stringBuilder = new StringBuilder();
        
            foreach (var argument in arguments)
            {
                if (argument != arguments.Array[0])
                {
                    if (argument != arguments.Array[1])
                    {
                        stringBuilder.Append(" ");
                    }
                    stringBuilder.Append(argument);
                }
                
            }
            string message = stringBuilder.ToString();

            return message;
        }

        public static bool StoreMessage(string faction, string str)
        {
            ChatHistory.Add(new KeyValuePair<string, string>(faction, str));
            return true;
        }

        public static string GetHistory(string faction, int line, bool IsOverride)
        {
            if (IsOverride)
            {
                var result = string.Join(Environment.NewLine, ChatHistory.Select(entry => $"{entry.Value}"));
                
                return Environment.NewLine + result;
            }
            else
            {
                var filteredMessages = ChatHistory.Where(entry => entry.Key == faction || entry.Key == "Server").Select(entry => entry.Value);
                var result = string.Join(Environment.NewLine, filteredMessages.Take(line));
                
                return Environment.NewLine + result;
            }
        }
    }
}
