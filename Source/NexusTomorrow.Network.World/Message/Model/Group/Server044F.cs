using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Group;

[WireMessage(GameMessageOpcode.Server044F, Direction.ServerToClient)]
public sealed partial class Server044F
{
    [UInt(64)] public ulong Unknown0 { get; set; }

    public RealmScopedId Unknown1 { get; set; } = new();

    [WideString] public string Unknown2 { get; set; } = "";
}
