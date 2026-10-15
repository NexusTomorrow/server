using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

// Seems to fire when buffs are applied from NPC-created Telegraphs, like speed boosts in Northern Wilds
[Message(GameMessageOpcode.ServerSpellExecute)]
[WireMessage(GameMessageOpcode.ServerSpellExecute, Direction.ServerToClient)]
public sealed partial class ServerSpellExecute : IWritable
{
    [UInt(32)] public uint CastingId { get; set; }
    public TargetInfo TargetInfo { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(CastingId);
        TargetInfo.Write(writer);
    }
}
