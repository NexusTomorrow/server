using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[WireMessage(GameMessageOpcode.ServerTradeskillRelearnCooldown, Direction.ServerToClient)]
public sealed partial class ServerTradeskillRelearnCooldown
{
    [UInt(32)] public uint RelearnCooldown { get; set; } // Sent as an offset from the time now, to the finish time, in milliseconds.
}
