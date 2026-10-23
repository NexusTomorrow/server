using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Server053B, Direction.ServerToClient)]
public sealed partial class HousingBuildState
{
    public RealmScopedId Unknown0 { get; set; } = new();
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(3)] public byte Unknown3 { get; set; }
}
