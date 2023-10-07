using System;
using System.Text.RegularExpressions;
using Exiled.API.Features;
using RemoteAdmin;
using PlayerRoles;
using System.Linq;
using System.Collections.Generic;
using CommandSystem;
using Exiled.API.Enums;

namespace ChatPlugin.Commands
{
    public class MessageHelper
    {
        public static int GetBroadcastType(Player player, string[] argsArray)
        {
            int broadcastType;
            if (player.Role.Type == RoleTypeId.Spectator)
            {
                broadcastType = 4;
            }
            else if (argsArray.Length > 1 && int.TryParse(argsArray[argsArray.Length - 1], out int parsedBroadcastType))
            {
                broadcastType = parsedBroadcastType;
            }
            else
            {
                broadcastType = 1;
            }
            return broadcastType;
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
                    default:
                        return "其他";
                }
            }
        }
    }

    [CommandHandler(typeof(ClientCommandHandler))]
    public class SayCommand : ICommand
    {
        public string Command => "say";
        public string Description => "Say something to somebody!";
        public string[] Aliases => Array.Empty<string>();

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "";

            if (!(sender is PlayerCommandSender playerCommandSender))
            {
                response = "错误。只有玩家可以使用该命令。";
                return false;
            }

            string[] argsArray = arguments.ToArray();
            Player player = Player.Get(playerCommandSender);

            string fullCommand = string.Join(" ", argsArray);

            var regex = new Regex(@"\[(.*?)\]");
            var match = regex.Match(fullCommand);

            string messageContent = match.Groups[1].Value;

            if (!match.Success)
            {
                response = "错误。参数1格式不正确。";
                return false;
            }

            int numericParameter = 0; // 默认值

            if (argsArray.Length > 1 && int.TryParse(argsArray[argsArray.Length - 1], out int parsedNumericParameter))
            {
                numericParameter = parsedNumericParameter;
            }

            int broadcastType = MessageHelper.GetBroadcastType(player, argsArray);
            string finalMessage = MessageHelper.BuildFinalMessage(player, match.Groups[1].Value.Trim(), broadcastType);

            Log.Info(finalMessage);

            if (broadcastType == 1)
            {
                var showToPlayers = Player.List.Where(p => p.Role.Side == player.Role.Side).ToList();
                foreach (var p in showToPlayers)
                {
                    p.ShowHint(finalMessage, 5);
                }
            }
            else if (broadcastType == 2)
            {
                var showToPlayers_Human = Player.List.Where(p => p.IsHuman).ToList();
                foreach (var p in showToPlayers_Human)
                {
                    p.ShowHint(finalMessage, 5);
                }
            }
            else if (broadcastType == 3)
            {
                var showToPlayers_All = Player.List.ToList();
                foreach (var p in showToPlayers_All)
                {
                    if (p.IsAlive)
                    {
                        p.ShowHint(finalMessage, 5);
                    }
                    else
                    {
                        p.Broadcast(5, finalMessage, Broadcast.BroadcastFlags.Normal, true);
                    }
                }
            }
            else if (broadcastType == 4)
            {
                var showToPlayers = Player.List.Where(p => p.Role.Side == player.Role.Side).ToList();
                foreach (var p in showToPlayers)
                {
                    p.Broadcast(5, finalMessage, Broadcast.BroadcastFlags.Normal, true);
                }
            }

            response = finalMessage;
            return true;
        }
    }
}
