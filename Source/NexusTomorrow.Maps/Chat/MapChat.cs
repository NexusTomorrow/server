using NexusTomorrow.Maps.Visibility;
using NexusTomorrow.Network.World.Message.Model.Chat;
using NexusTomorrow.Owners;
using NexusTomorrow.Owners.World;
using NexusTomorrow.Persistence.Realm;

namespace NexusTomorrow.Maps.Chat;

// The map's half of /say: a line the realm passed on is accepted to its speaker and sent, serialised once, to every other player in
// the world within MapSettings.SayRange of the speaker, three-dimensional, as the grid holds them since the last flush. The two chat
// messages are kept and refilled for each line; each is serialised before it is sent, so no queued send holds them (Send<T> queues
// the message itself, ClientConnection.Send).
internal sealed class MapChat
{
    // The client's built-in Say channel (ChatChannelKey.Type); its id stays 0.
    private const ushort SayChannelType = 4;

    private readonly AreaOfInterest _visibility;
    private readonly IMessageSerializer _serializer;
    private readonly int _sayRange;
    private readonly List<Player> _listeners = [];
    private readonly ChatSendAccepted _accepted;
    private readonly ChatMessageReceived _heard = new();

    public MapChat(MapSettings settings, AreaOfInterest visibility, IMessageSerializer serializer)
    {
        _visibility = visibility;
        _serializer = serializer;
        _sayRange = settings.SayRange;
        _accepted = new ChatSendAccepted { RealmName = settings.RealmName };
        _heard.Channel.Type = SayChannelType;
        _heard.SenderId.RealmId = settings.RealmId;
    }

    // A line whose speaker is not yet in the world, or is not the character the realm named, is ignored.
    public void Say(Player speaker, LocalChat line)
    {
        CharacterRecord character = speaker.Character;
        if (!speaker.InWorld || character.Id != line.CharacterId)
            return;

        _accepted.ChatId = line.ChatId;
        _accepted.SenderName = character.Name;
        _accepted.UnitId = speaker.UnitId;
        SerializedMessage accepted = _serializer.Serialize(_accepted);
        try
        {
            speaker.Session.Send(accepted);
        }
        finally
        {
            accepted.Release();
        }

        _visibility.CollectWithin(speaker.Location.Position, _sayRange, _listeners);
        try
        {
            if (!HasListener(speaker))
                return;

            _heard.SenderId.Id = character.Id;
            _heard.SenderName = character.Name;
            _heard.Text = line.Text;
            _heard.UnitId = speaker.UnitId;
            SerializedMessage heard = _serializer.Serialize(_heard);
            try
            {
                foreach (Player listener in _listeners)
                {
                    if (!ReferenceEquals(listener, speaker))
                        listener.Session.Send(heard);
                }
            }
            finally
            {
                heard.Release();
            }
        }
        finally
        {
            _listeners.Clear();
        }
    }

    // The players within range include the speaker, once the grid holds it.
    private bool HasListener(Player speaker)
    {
        foreach (Player listener in _listeners)
        {
            if (!ReferenceEquals(listener, speaker))
                return true;
        }

        return false;
    }
}
