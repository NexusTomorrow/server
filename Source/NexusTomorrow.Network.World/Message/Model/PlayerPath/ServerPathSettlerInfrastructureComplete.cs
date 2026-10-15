using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

// Triggers SettlerInfrastructureComplete lua event
[WireMessage(GameMessageOpcode.ServerPathSettlerInfrastructureComplete, Direction.ServerToClient)]
public sealed partial class ServerPathSettlerInfrastructureComplete
{
    [UInt(32)] public uint Unused { get; set; }
}
