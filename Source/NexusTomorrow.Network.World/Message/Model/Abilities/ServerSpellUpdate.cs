using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[Message(GameMessageOpcode.ServerSpellUpdate)]
[WireMessage(GameMessageOpcode.ServerSpellUpdate, Direction.ServerToClient)]
public sealed partial class ServerSpellUpdate : IWritable
{
    [UInt(18)] public uint Spell4BaseId { get; set; }
    [UInt(4)] public byte TierIndex { get; set; }
    [UInt(3)] public byte SpecIndex { get; set; }
    [UInt(1)] public bool Activated { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Spell4BaseId, 18u);
        writer.Write(TierIndex, 4u);
        writer.Write(SpecIndex, 3u);
        writer.Write(Activated);
    }
}
