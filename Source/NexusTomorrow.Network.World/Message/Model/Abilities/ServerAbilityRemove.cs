using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

// Removes SpellbookItem from the AbilityBook
[WireMessage(GameMessageOpcode.ServerAbilityRemove, Direction.ServerToClient)]
public sealed partial class ServerAbilityRemove
{
    public ItemLocation Location { get; set; } = new();
}
