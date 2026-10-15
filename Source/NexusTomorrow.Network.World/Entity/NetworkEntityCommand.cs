using NexusTomorrow.Game.Static.Entity.Movement.Command;

namespace NexusTomorrow.Network.World.Entity;

public class NetworkEntityCommand : INetworkEntityCommand
{
    public EntityCommand Command { get; set; }
    public IEntityCommandModel Model { get; set; }
}
