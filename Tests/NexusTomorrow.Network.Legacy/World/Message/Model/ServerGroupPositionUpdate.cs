using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerGroupPositionUpdate)]
public class ServerGroupPositionUpdate : IWritable
{
    public class UnknownStruct0
    {
        public Identity Identity { get; set; }
        public Position Position { get; set; }
        public uint WorldZoneId { get; set; }
        public uint Flags { get; set; } = 0; // bInCombatPvp = 1, bIInCombatPve = 2, InCombat = 3
    }

    public ulong GroupId { get; set; }
    public uint WorldId { get; set; }
    public List<UnknownStruct0> Updates { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(GroupId);
        writer.Write(WorldId, 15);

        writer.Write((uint)Updates.Count);
        Updates.ForEach(update => update.Identity.Write(writer));
        Updates.ForEach(update => update.Position.Write(writer));
        Updates.ForEach(update => writer.Write(update.WorldZoneId));
        Updates.ForEach(update => writer.Write(update.Flags));
    }
}
