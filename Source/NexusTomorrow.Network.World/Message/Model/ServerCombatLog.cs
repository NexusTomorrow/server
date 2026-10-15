using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Combat;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerCombatLog)]
[WireMessage(GameMessageOpcode.ServerCombatLog, Direction.ServerToClient)]
public partial class ServerCombatLog : IWritable
{
    [UInt(6)] public CombatLogType Type { get; set; }
    [Union(nameof(Type))] public ICombatLog CombatLog { get; set; }

    public virtual void Write(GamePacketWriter writer)
    {
        writer.Write(CombatLog.Type, 6u);
        CombatLog.Write(writer);
    }
}
