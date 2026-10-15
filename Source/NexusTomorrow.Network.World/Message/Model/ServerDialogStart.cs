using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerDialogStart, Direction.ServerToClient)]
public sealed partial class ServerDialogStart
{
    [UInt(32)] public uint DialogUnitId { get; set; }
    [UInt(1)] public bool Unused { get; set; }
}
