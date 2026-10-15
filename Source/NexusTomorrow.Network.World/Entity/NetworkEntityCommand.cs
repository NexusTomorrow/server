using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Entity.Command;

namespace NexusTomorrow.Network.World.Entity;

/// <summary>
/// One element of an entity command list: the 5-bit command, then that command's payload.
/// </summary>
[WireStruct]
public sealed partial class NetworkEntityCommand : INetworkEntityCommand
{
    [UInt(5)] public EntityCommand Command { get; set; }
    [Union(nameof(Command))] public IEntityCommandModel Model { get; set; } = new SetTimeCommand();
}
