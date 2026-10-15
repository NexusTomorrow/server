using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[Message(GameMessageOpcode.ClientRequestActionSetChanges)]
[WireMessage(GameMessageOpcode.ClientRequestActionSetChanges, Direction.ClientToServer)]
public sealed partial class ClientRequestActionSetChanges : IReadable
{
    [WireStruct]
    public sealed partial class ActionTier : IReadable
    {
        [UInt(18)] public uint Action { get; private set; }
        [UInt(8)] public byte Tier { get; private set; }

        public void Read(GamePacketReader reader)
        {
            Action = reader.ReadUInt(18u);
            Tier = reader.ReadByte();
        }
    }

    [Array(CountBits = 4), UInt(32)] public List<uint> Actions { get; } = [];
    [UInt(3)] public byte ActionSetIndex { get; private set; }
    [Array(CountBits = 5)] public List<ActionTier> ActionTiers { get; } = [];
    [Array(CountBits = 7), UInt(16)] public List<ushort> Amps { get; } = [];

    public void Read(GamePacketReader reader)
    {
        uint count = reader.ReadByte(4u);
        for (uint i = 0u; i < count; i++)
            Actions.Add(reader.ReadUInt());

        ActionSetIndex = reader.ReadByte(3u);

        count = reader.ReadByte(5u);
        for (uint i = 0u; i < count; i++)
        {
            var actionTier = new ActionTier();
            actionTier.Read(reader);
            ActionTiers.Add(actionTier);
        }

        count = reader.ReadByte(7u);
        for (uint i = 0u; i < count; i++)
            Amps.Add(reader.ReadUShort());
    }
}
