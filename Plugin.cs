using System;
using Exiled.API.Features;
using Exiled.CustomRoles.Events;

namespace ChatPlugin
{
    public class ChatPlugin : Plugin<Config>
    {
        public override string Author => "Akizuki Kaguya";
        public override string Name => "ChatPlugin";

        public override Version Version { get; } = new(1, 0, 3);
        public override Version RequiredExiledVersion { get; } = new(8, 0, 1);

        public static ChatPlugin Instance;

        public EventHandlers EventHandler { get; private set; }

        public override void OnEnabled()
        {
            Instance = this;
            
            Log.Info($"Legacy Mode: {Config.LegacyChatMode}");
            Log.Info($"Huamn Chat Mode: {Config.HumanChat}");
            Log.Info($"Scp Chat Mode: {Config.ScpChat}");
            
            EventHandler = new EventHandlers();
            
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Instance = null;
            EventHandler = null;

            base.OnDisabled();
        }
    }
}