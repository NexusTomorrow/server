using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Entity;

/// <summary>
/// One entity command's payload, an arm of the union a 5-bit <see cref="Game.Static.Entity.Movement.Command.EntityCommand"/> selects.
/// </summary>
public interface IEntityCommandModel : IWireStruct
{
}
