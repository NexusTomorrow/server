using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientGroupKick)]
[WireMessage(GameMessageOpcode.ClientGroupKick, Direction.ClientToServer)]
public sealed partial class ClientGroupKick : IReadable
{
    [UInt(64)] public ulong GroupId { get; set; }
    public Identity TargetedPlayer { get; set; } = new();

    public void Read(GamePacketReader reader)
    {
        GroupId = reader.ReadULong();
        TargetedPlayer.Read(reader);
    }
}
