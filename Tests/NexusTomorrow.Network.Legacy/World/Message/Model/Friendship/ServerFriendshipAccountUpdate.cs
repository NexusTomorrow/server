using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ServerFriendshipAccountUpdate)]
public class ServerFriendshipAccountUpdate : IWritable
{
    public uint AccountId { get; set; }
    public AccountPresenceState State { get; set; }
    public List<CharacterData> Characters { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(AccountId);
        writer.Write(State, 3u);
        writer.Write(Characters.Count);

        foreach (CharacterData character in Characters)
            character.Write(writer);
    }
}
