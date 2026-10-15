using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

// Triggers SettlerInfrastructureStarted lua event
[WireMessage(GameMessageOpcode.ServerPathSettlerInfrastructureStarted, Direction.ServerToClient)]
public sealed partial class ServerPathSettlerInfrastructureStarted
{
    [UInt(32)] public uint Unused { get; set; }
}
