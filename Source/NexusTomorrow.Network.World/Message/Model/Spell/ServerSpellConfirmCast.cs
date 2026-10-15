using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

// After a client initialises a spell cast, the server confirms the cast with this message.
// Not used when spell casts are initiated by other sources.

[Message(GameMessageOpcode.ServerSpellConfirmCast)]
[WireMessage(GameMessageOpcode.ServerSpellConfirmCast, Direction.ServerToClient)]
public sealed partial class ServerSpellConfirmCast : IWritable
{
    [UInt(32)] public uint ClientSpellCastUniqueId { get; set; }
    [UInt(32)] public uint CastingId { get; set; }
    [UInt(32)] public uint CasterId { get; set; }
    public Position Position { get; set; } = new Position();
    [Float32] public float Yaw { get; set; }

    [Array(CountBits = 8)] public List<InitialPosition> InitialPositionData { get; set; } = [];
    [Array(CountBits = 8)] public List<TelegraphPosition> TelegraphPositionData { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(ClientSpellCastUniqueId);
        writer.Write(CastingId);
        writer.Write(CasterId);
        Position.Write(writer);
        writer.Write(Yaw);

        writer.Write(InitialPositionData.Count, 8u);
        InitialPositionData.ForEach(u => u.Write(writer));

        writer.Write(TelegraphPositionData.Count, 8u);
        TelegraphPositionData.ForEach(u => u.Write(writer));
    }
}
