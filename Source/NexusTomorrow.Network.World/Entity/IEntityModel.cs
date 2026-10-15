using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Entity;

/// <summary>
/// An entity's type-specific create data, an arm of the union the 6-bit <see cref="Game.Static.Entity.EntityType"/> before it selects.
/// </summary>
public interface IEntityModel : IWireStruct
{
}
