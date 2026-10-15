using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Utility;

[WireMessage(GameMessageOpcode.ServerRandomRollResponse, Direction.ServerToClient)]
public sealed partial class ServerRandomRollResponse
{
    public Identity RollerIdentity { get; set; }
    [UInt(32)] public uint MinRandom { get; set; }
    [UInt(32)] public uint MaxRandom { get; set; }
    [UInt(32)] public int RandomRollResult { get; set; }
}
