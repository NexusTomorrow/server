using NexusTomorrow.Game.Static.Group;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientGroupMarkUnit)]
[WireMessage(GameMessageOpcode.ClientGroupMarkUnit, Direction.ClientToServer)]
public sealed partial class ClientGroupMark : IReadable
{
    [UInt(32)] public GroupMarker Marker { get; set; }

    [UInt(32)] public uint UnitId { get; set; }

    public void Read(GamePacketReader reader)
    {
        Marker = reader.ReadEnum<GroupMarker>(32u);
        UnitId = reader.ReadUInt();
    }
}
