using NexusTomorrow.Game.Static.Setting;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

[Message(GameMessageOpcode.ClientSetInstanceSettings)]
[WireMessage(GameMessageOpcode.ClientSetInstanceSettings, Direction.ClientToServer)]
public sealed partial class ClientSetInstanceSettings : IReadable
{
    [UInt(32)] public uint InstancePortalUnitId { get; private set; }
    [UInt(2)] public WorldDifficulty Difficulty { get; private set; }
    [UInt(8)] public byte PrimeLevel { get; private set; }
    [UInt(11)] public ushort Rally { get; private set; } // Was probably intended for more flags but is only used for one setting.

    public void Read(GamePacketReader reader)
    {
        InstancePortalUnitId = reader.ReadUInt();
        Difficulty = reader.ReadEnum<WorldDifficulty>(2u);
        PrimeLevel = reader.ReadByte();
        Rally = reader.ReadUShort(11u);
    }
}
