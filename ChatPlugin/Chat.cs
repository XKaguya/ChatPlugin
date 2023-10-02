using System;
using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using Exiled.API.Enums;
using System.Text.RegularExpressions;
using PlayerRoles;
using System.Linq;

namespace ChatPlugin.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Say : ICommand
    {
        public string Command => "say";
        public string Description => "Say something to somebody !";

        public string[] Aliases => Array.Empty<string>();

        string Range;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender is PlayerCommandSender))
            {
                response = "Error. Only players can use this command.";
                return false;
            }

            Player player = Player.Get(sender);
            string playerName = player.Nickname;

            if (player.Role.Type == RoleTypeId.Spectator)
            {
                Range = "¹Û²ìÕß";

                string content = string.Join(" ", arguments);

                string message = $"[{Range}][{playerName}]: {content}";

                Log.Info(message);

                var ShowToPlayers = Player.List.Where(player_1 => player_1.Role.Side == player.Role.Side).ToList();
                foreach (var player_1 in ShowToPlayers)
                {
                    player.Broadcast(5, message);
                }

                response = message;
                return false;
            }
            else if (player.Role.Type == RoleTypeId.None)
            {
                Range = "´óÌü";

                string content = string.Join(" ", arguments);

                string message = $"[{Range}][{playerName}]: {content}";

                Log.Info(message);

                var ShowToPlayers = Player.List.Where(player_1 => player_1.Role.Side == player.Role.Side).ToList();
                foreach (var player_1 in ShowToPlayers)
                {
                    player.Broadcast(5, message);
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

                string content = string.Join(" ", arguments);

                string message = $"[{Range}][{playerName}][{roomName}]: {content}";

                Log.Info(message);

                var ShowToPlayers = Player.List.Where(player_1 => player_1.Role.Side == player.Role.Side).ToList();
                foreach (var player_1 in ShowToPlayers)
                {
                    player.ShowHint(message, 5);
                }

                response = message;
                return false;
            }
        }
    }
}
