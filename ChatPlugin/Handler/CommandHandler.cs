using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ChatPlugin;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using PlayerRoles;
using RemoteAdmin;

[CommandHandler(typeof(ClientCommandHandler))]
public class SayCommand : ICommand
{
    public string Command { get; set; } = "say";
    public string Description { get; set; } = "Say something to somebody!";
    public string[] Aliases { get; set; } = Array.Empty<string>();

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (ChatPlugin.ChatPlugin.Instance.Config.LegacyChatMode)
        {
            response = "";

            Player player = Player.Get(sender);

            string[] argsArray = arguments.ToArray();
            if (sender != null)
            {
                player = Player.Get(sender);
            }

            string fullCommand = string.Join(" ", argsArray);

            var regex = new Regex(@"\[(.*?)\]");
            var match = regex.Match(fullCommand);

            if (!match.Success)
            {
                response = "错误。参数1格式不正确。";
                return false;
            }

            int numericParameter = 0;

            if (argsArray.Length > 1 &&
                int.TryParse(argsArray[argsArray.Length - 1], out int parsedNumericParameter))
            {
                numericParameter = parsedNumericParameter;
            }

            int broadcastType = API.GetBroadcastType(player, argsArray);
            
            string finalMessage = API.BuildFinalMessage(player, match.Groups[1].Value.Trim(), broadcastType);
            
            string range = API.GetRange(player);

            API.StoreMessage(range, finalMessage);

            if (broadcastType == 1)
            {
                var showToPlayers = new List<Player>();
                
                if (player.IsScp || player.IsTutorial)
                {
                    showToPlayers = Player.List.Where(p => p.IsScp || p.IsTutorial).ToList();
                }
                else
                {
                    showToPlayers = Player.List.Where(p => p.Role.Side == player.Role.Side).ToList();
                }
                foreach (var p in showToPlayers)
                {
                    if (p.IsScp || p.Role.Side == Side.Flamingo || p.Role.Side == Side.Tutorial)
                    {
                        if (ChatPlugin.ChatPlugin.Instance.Config.ScpChat)
                        {
                            p.Broadcast(5, finalMessage, Broadcast.BroadcastFlags.Normal, true);
                        }
                        else
                        {
                            p.ShowHint(finalMessage, 5);
                        }
                    }
                    else if (p.IsHuman)
                    {
                        if (ChatPlugin.ChatPlugin.Instance.Config.HumanChat)
                        {
                            p.Broadcast(5, finalMessage, Broadcast.BroadcastFlags.Normal, true);
                        }
                        else
                        {
                            p.ShowHint(finalMessage, 5);
                        }
                    }
                }
            }
            else if (broadcastType == 2)
            {
                var showToPlayers_Human = Player.List.Where(p => p.IsHuman).ToList();
                foreach (var p in showToPlayers_Human)
                {
                    if (ChatPlugin.ChatPlugin.Instance.Config.HumanChat)
                    {
                        p.Broadcast(5, finalMessage, Broadcast.BroadcastFlags.Normal, true);
                    }
                    else
                    {
                        p.ShowHint(finalMessage, 5);
                    }
                }
            }
            else if (broadcastType == 3)
            {
                var showToPlayers_All = Player.List.ToList();
                foreach (var p in showToPlayers_All)
                {
                    if (p.IsAlive)
                    {
                        if (p.IsScp || p.Role.Side == Side.Flamingo || p.Role.Side == Side.Tutorial)
                        {
                            if (ChatPlugin.ChatPlugin.Instance.Config.ScpChat)
                            {
                                p.Broadcast(5, finalMessage, Broadcast.BroadcastFlags.Normal, true);
                            }
                            else
                            {
                                p.ShowHint(finalMessage, 5);
                            }
                        }
                        else if (p.IsHuman)
                        {
                            if (ChatPlugin.ChatPlugin.Instance.Config.HumanChat)
                            {
                                p.Broadcast(5, finalMessage, Broadcast.BroadcastFlags.Normal, true);
                            }
                            else
                            {
                                p.ShowHint(finalMessage, 5);
                            }
                        }
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
            else if (broadcastType == 5)
            {
                var showToPlayers = Player.List.Where(p => p.Role.Side == player.Role.Side).ToList();
                foreach (var p in showToPlayers)
                {
                    if (ChatPlugin.ChatPlugin.Instance.Config.ScpChat)
                    {
                        p.Broadcast(5, finalMessage, Broadcast.BroadcastFlags.Normal, true);
                    }
                    else
                    {
                        p.ShowHint(finalMessage, 5);
                    }
                }
            }

            response = finalMessage;
            return true;
        }
        else
        {
            response = "";

            Player player = Player.Get(sender);

            int broadcastType = API.GetBroadcastType(player);
            
            string range = API.GetRange(player);
        
            string msg = API.GetMessage(arguments);

            API.StoreMessage(range, msg);
            
            string finalMessage = API.BuildFinalMessage(player, msg, broadcastType);

            var showToPlayers = new List<Player>();
                
            if (player.IsScp || player.IsTutorial)
            {
                showToPlayers = Player.List.Where(p => p.IsScp || p.IsTutorial).ToList();
            }
            else
            {
                showToPlayers = Player.List.Where(p => p.Role.Side == player.Role.Side).ToList();
            }
            foreach (var p in showToPlayers)
            {
                if (p.IsScp || p.Role.Side == Side.Flamingo || p.Role.Side == Side.Tutorial)
                {
                    if (ChatPlugin.ChatPlugin.Instance.Config.ScpChat)
                    {
                        p.Broadcast(5, finalMessage, Broadcast.BroadcastFlags.Normal, true);
                    }
                    else
                    {
                        p.ShowHint(finalMessage, 5);
                    }
                }
                else if (p.IsHuman)
                {
                    if (ChatPlugin.ChatPlugin.Instance.Config.HumanChat)
                    {
                        p.Broadcast(5, finalMessage, Broadcast.BroadcastFlags.Normal, true);
                    }
                    else
                    {
                        p.ShowHint(finalMessage, 5);
                    }
                }
                else
                {
                    p.Broadcast(5, finalMessage, Broadcast.BroadcastFlags.Normal, true);
                }
            }
            
            return true;
        }

        return false;
    }
}

[CommandHandler(typeof(ClientCommandHandler))]
public class GlobalSayCommand : ICommand
{
    public string Command { get; set; } = "gsay";
    public string Description { get; set; } = "Say something to somebody!";
    public string[] Aliases { get; set; } = Array.Empty<string>();

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (ChatPlugin.ChatPlugin.Instance.Config.LegacyChatMode)
        {
            response = "Plguin is currently running at Legacy mode. Global Say command is not allowed.";
            return false;
        }

        response = "";

        Player player = Player.Get(sender);

        int broadcastType = API.GetBroadcastType(player);
        
        string msg = API.GetMessage(arguments);

        API.StoreMessage("Global", msg);
        
        string finalMessage = API.BuildFinalMessage(player, msg, broadcastType);
        
        var showToPlayers = Player.List.ToList();
        foreach (var p in showToPlayers)
        {
            if (p.IsAlive)
            {
                if (p.IsScp || p.Role.Side == Side.Flamingo || p.Role.Side == Side.Tutorial)
                {
                    if (ChatPlugin.ChatPlugin.Instance.Config.ScpChat)
                    {
                        p.Broadcast(5, finalMessage, Broadcast.BroadcastFlags.Normal, true);
                    }
                    else
                    {
                        p.ShowHint(finalMessage, 5);
                    }
                }
                else if (p.IsHuman)
                {
                    if (ChatPlugin.ChatPlugin.Instance.Config.HumanChat)
                    {
                        p.Broadcast(5, finalMessage, Broadcast.BroadcastFlags.Normal, true);
                    }
                    else
                    {
                        p.ShowHint(finalMessage, 5);
                    }
                }
                else
                {
                    p.Broadcast(5, finalMessage, Broadcast.BroadcastFlags.Normal, true);
                }
            }
            else
            {
                p.Broadcast(5, finalMessage, Broadcast.BroadcastFlags.Normal, true);
            }
        }
        
        return true;
    }
}

[CommandHandler(typeof(ClientCommandHandler))]
public class ServerBroadcastCommand : ICommand
{
    public string Command { get; set; } = "bc";
    public string Description { get; set; } = "Say something with broadcast in server wide.";
    public string[] Aliases { get; set; } = Array.Empty<string>();

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        response = "";

        if (Player.Get(sender) != null)
        {
            response = "You are not allowed to use this command. This is server only.";
            return false;
        }

        string message = API.GetMessage(arguments);
        
        var showToPlayers = Player.List.ToList();
        string msg = $"[全体][服务器]: {message}";
        foreach (var p in showToPlayers)
        {
            p.Broadcast(5, msg, Broadcast.BroadcastFlags.Normal, true);
        }

        API.StoreMessage("Server", msg);
        
        response = msg;

        return true;
    }
}

[CommandHandler(typeof(ClientCommandHandler))]
public class HistoryCommand : ICommand
{
    public string Command { get; set; } = "his";
    public string Description { get; set; } = "Show the chat history.";
    public string[] Aliases { get; set; } = Array.Empty<string>();

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        response = "";

        if (Player.Get(sender) != null)
        {
            string range = API.GetRange(Player.Get(sender));
            
            int line;
                
            if (arguments.Array.Length >= 2)
            {
                line = int.Parse(arguments.Array[1]);
            }
            else
            {
                line = 5;
            }
            
            string history = API.GetHistory(range, line, false);

            response = history;
            
            return true;
        }
        else
        {
            string history = API.GetHistory("Server", 0, true);
        
            response = history;
            
            return true;
        }
        
        return true;
    }
}