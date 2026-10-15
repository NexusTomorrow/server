using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

[WireMessage(GameMessageOpcode.ServerInstanceResetResult, Direction.ServerToClient)]
public sealed partial class ServerInstanceResetResult
{
    [UInt(15)] public uint Unused { get; set; } // Probably WorldId but not used by client
    [UInt(1)] public bool Success { get; set; }
}
