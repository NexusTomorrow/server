using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

[WireMessage(GameMessageOpcode.Client0255, Direction.ClientToServer)]
public sealed partial class CostumeSave
{
    [WireStruct]
    public sealed partial class CostumeSaveSlot
    {
        [UInt(18)] public uint Unknown0 { get; private set; }
        [FixedArray(3), UInt(32)] public uint[] Unknown1 { get; } = new uint[3];
    }

    [SInt(32)] public int Unknown0 { get; private set; }
    [UInt(2)] public byte Unknown1 { get; private set; }
    [UInt(64)] public ulong Unknown2 { get; private set; }
    [FixedArray(7)] public CostumeSaveSlot[] Unknown3 { get; } = new CostumeSaveSlot[7];
    [UInt(32)] public uint Unknown4 { get; private set; }
    [UInt(1)] public bool Unknown5 { get; private set; }
}
