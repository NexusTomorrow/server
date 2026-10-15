using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[WireMessage(GameMessageOpcode.ClientRequestActionSetChanges, Direction.ClientToServer)]
public sealed partial class ClientRequestActionSetChanges
{
    [WireStruct]
    public sealed partial class ActionTier
    {
        [UInt(18)] public uint Action { get; private set; }
        [UInt(8)] public byte Tier { get; private set; }
    }

    [Array(CountBits = 4), UInt(32)] public List<uint> Actions { get; } = [];
    [UInt(3)] public byte ActionSetIndex { get; private set; }
    [Array(CountBits = 5)] public List<ActionTier> ActionTiers { get; } = [];
    [Array(CountBits = 7), UInt(16)] public List<ushort> Amps { get; } = [];
}
