using System;
using Exiled.API.Features;
using Exiled.CustomRoles.Events;

namespace ChatPlugin
{
    public class ChatPlugin : Plugin<Config>
    {
        public override string Author => "RedLeaves";
        public override string Name => "ChatPlugin";

        public override Version Version { get; } = new(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new(8, 0, 1);

        public static ChatPlugin Instance;

        public override void OnEnabled()
        {
            Instance = this;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Instance = null;

            base.OnDisabled();
        }
    }
}