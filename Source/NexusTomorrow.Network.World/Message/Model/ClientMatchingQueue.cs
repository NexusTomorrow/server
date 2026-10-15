using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientMatchingQueue)]
[WireMessage(GameMessageOpcode.ClientMatchingQueue, Direction.ClientToServer)]
public sealed partial class ClientMatchingQueue : IReadable
{
    public MatchingMap MapData { get; private set; }
    [UInt(32)] public Role Roles { get; private set; }
    [UInt(32)] public uint PrimeLevel { get; private set; }

    public void Read(GamePacketReader reader)
    {
        MapData = new MatchingMap();
        MapData.Read(reader);

        Roles = reader.ReadEnum<Role>(32u);
        PrimeLevel = reader.ReadUInt();
    }
}
