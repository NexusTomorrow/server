using NexusTomorrow.Game.Static.Costume;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

[WireMessage(GameMessageOpcode.ClientCostumeSave, Direction.ClientToServer)]
public sealed partial class ClientCostumeSave
{
    [WireStruct]
    public sealed partial class CostumeItem
    {
        [UInt(18)] public uint Item2Id { get; private set; }
        [FixedArray(3), UInt(32)] public uint[] DyeColorRampIds { get; } = new uint[3];
    }

    [SInt(32)] public int Index { get; private set; }
    [UInt(2)] public CostumeType Type { get; private set; }
    [UInt(64)] public ulong MannequinDecorId { get; private set; }
    [FixedArray(Costume.MaxCostumeItems)] public CostumeItem[] Items { get; } = new CostumeItem[Costume.MaxCostumeItems]; // in order of CostumeItemSlot enum
    [UInt(32)] public uint VisibilityMask { get; private set; }
    [UInt(1)] public bool UserServiceToken { get; private set; }
}
