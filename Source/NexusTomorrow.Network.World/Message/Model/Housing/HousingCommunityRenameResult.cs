using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Server024D, Direction.ServerToClient)]
public sealed partial class HousingCommunityRenameResult
{
    public RealmScopedId Unknown0 { get; set; } = new();
    [UInt(7)] public byte Unknown1 { get; set; }
}
