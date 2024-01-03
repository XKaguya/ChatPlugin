namespace ChatPlugin
{
    public class EventHandlers
    {
        public EventHandlers()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
        }

        ~EventHandlers()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
        }

        public void OnWaitingForPlayers()
        {
            API.ChatHistory.Clear();
        }
    }
}