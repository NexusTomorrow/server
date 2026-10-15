using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientResurrectAccept)]
[WireMessage(GameMessageOpcode.ClientResurrectAccept, Direction.ClientToServer)]
public sealed partial class ClientResurrectAccept : IReadable
{
    [UInt(32)] public uint UnitId { get; private set; }
    [UInt(32)] public ResurrectionType RezType { get; private set; }

    public void Read(GamePacketReader reader)
    {
        UnitId = reader.ReadUInt();
        RezType = reader.ReadEnum<ResurrectionType>(32u);
    }
}
