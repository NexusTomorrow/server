using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

// Removes SpellbookItem from the AbilityBook
[Message(GameMessageOpcode.ServerAbilityRemove)]
public class ServerAbilityRemove : IWritable
{
    public ItemLocation Location { get; set; } = new();

    public void Write(GamePacketWriter writer)
    {
        Location.Write(writer);
    }
}
