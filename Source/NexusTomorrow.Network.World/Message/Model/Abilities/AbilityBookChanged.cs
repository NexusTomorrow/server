using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[WireMessage(GameMessageOpcode.Server01A0, Direction.ServerToClient)]
public sealed partial class AbilityBookChanged
{
    [WireStruct]
    public sealed partial class AbilityBookEntry
    {
        [UInt(18)] public uint Unknown0 { get; set; }
        [UInt(4)] public byte Unknown1 { get; set; }
        [UInt(3)] public byte Unknown2 { get; set; }
    }

    [Array(CountBits = 32)] public List<AbilityBookEntry> Unknown0 { get; set; } = [];
}
