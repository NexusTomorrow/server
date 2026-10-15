using NexusTomorrow.Game.Static.Entity.Movement.Command;

namespace NexusTomorrow.Network.World.Entity;

public interface INetworkEntityCommand
{
    EntityCommand Command { get; set; }
    IEntityCommandModel Model { get; set; }
}
