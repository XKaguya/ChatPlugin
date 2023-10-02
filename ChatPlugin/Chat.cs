using System;
using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using PlayerRoles;
using System.Linq;
using System.Collections.Generic;

namespace ChatPlugin.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]


    public class Say : ICommand
    {
        public string Command => "say";
        public string Description => "Say something to somebody !";

        public string[] Aliases => Array.Empty<string>();

        string Range;

        int broadcastType;
        // 1 Side Only
        // 2 All Human
        // 3 All

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender is PlayerCommandSender))
            {
                response = "Error. Only players can use this command.";
                return false;
            }

            if (arguments.Count < 2)
            {
                response = "Error. Not enough arguments.";
                return false;
            }

            string[] argsArray = arguments.ToArray();
            int.TryParse(argsArray[1], out broadcastType);

            if (!int.TryParse(argsArray[1], out broadcastType))
            {
                response = "Error. The second argument must be an integer.";
                return false;
            }

            Player player = Player.Get(sender);
            string playerName = player.Nickname;

            if (player.Role.Type == RoleTypeId.Spectator)
            {
                Range = "¹Û²ìÕß";

                string message = $"[{Range}][{playerName}]: {argsArray[0]}";

                Log.Info(message);

                var showToPlayers = Player.List.Where(p => p.Role.Side == player.Role.Side).ToList();
                foreach (var p in showToPlayers)
                {
                    Log.Debug(p);
                    p.Broadcast(5, message, Broadcast.BroadcastFlags.Normal, true);
                }

                response = message;
                return false;
            }
            else if (player.Role.Type == RoleTypeId.None)
            {
                Range = "´óÌü";

                string message = $"[{Range}][{playerName}]: {argsArray[0]}";

                Log.Info(message);

                var showToPlayers = Player.List.Where(p => p.Role.Side == player.Role.Side).ToList();
                foreach (var p in showToPlayers)
                {
                    Log.Debug(p);
                    p.Broadcast(5, message, Broadcast.BroadcastFlags.Normal, true);
                }

                response = message;
                return false;
            }
            else
            {
                Range = player.Role.Side.ToString();

                if (Range == "ChaosInsurgency")
                {
                    Range = "»ìãç·ÖÁÑÕß";
                }
                else if (Range == "Mtf")
                {
                    Range = "¾ÅÎ²ºü";
                }
                else if (Range == "Scp")
                {
                    Range = "SCP";
                }

                Room room = player.CurrentRoom;
                string roomName = room.RoomName.ToString();


                string message = $"[{Range}][{playerName}][{roomName}]: {argsArray[0]}";

                if (broadcastType == 1)
                {
                    Log.Info(message);

                    var showToPlayers = Player.List.Where(p => p.Role.Side == player.Role.Side).ToList();
                    foreach (var p in showToPlayers)
                    {
                        Log.Debug(p);
                        p.ShowHint(message, 5);
                    }
                }

                else if (broadcastType == 2)
                {
                    var showToPlayers_Human = Player.List.Where(p => p.IsHuman == player.IsHuman).ToList();
                    foreach (var p in showToPlayers_Human)
                    {
                        Log.Debug(p);
                        p.ShowHint(message, 5);
                    }
                }

                else if (broadcastType == 3)
                {
                    var showToPlayers_Alive = Player.List.Where(p => p.IsAlive).ToList();
                    foreach (var p in showToPlayers_Alive)
                    {
                        Log.Debug(p);
                        p.ShowHint(message, 5);
                    }

                    var showToPlayers_Dead = Player.List.Where(p => p.IsDead).ToList();
                    foreach (var p in showToPlayers_Dead)
                    {
                        Log.Debug(p);
                        p.ShowHint(message, 5);
                    }
                }


                response = message;
                return false;
            }
        }
    }
}
