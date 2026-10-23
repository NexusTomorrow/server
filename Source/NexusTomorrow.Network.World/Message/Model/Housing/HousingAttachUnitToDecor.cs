using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Server053A, Direction.ServerToClient)]
public sealed partial class HousingAttachUnitToDecor
{
    public RealmScopedId Unknown0 { get; set; } = new();
    [UInt(64)] public ulong Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
}
