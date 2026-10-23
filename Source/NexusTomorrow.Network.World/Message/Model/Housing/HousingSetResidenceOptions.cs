using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Client050E, Direction.ClientToServer)]
public sealed partial class HousingSetResidenceOptions
{
    public RealmScopedId Unknown0 { get; } = new();
    [UInt(32)] public uint Unknown1 { get; private set; }
    [UInt(3)] public byte Unknown2 { get; private set; }
    [UInt(3)] public byte Unknown3 { get; private set; }
}
