using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientGroupPromote)]
[WireMessage(GameMessageOpcode.ClientGroupPromote, Direction.ClientToServer)]
public sealed partial class ClientGroupPromote : IReadable
{
    [UInt(64)] public ulong GroupId { get; set; }

    public Identity TargetedPlayer { get; set; } = new();

    public void Read(GamePacketReader reader)
    {
        GroupId = reader.ReadULong();
        TargetedPlayer.Read(reader);
    }
}
