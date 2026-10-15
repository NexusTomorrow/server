using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

/// <summary>
/// This is sent to the Player being updated. Forces a refresh on the health bar of that user. UnitId always matches player ID of the client.
/// </summary>
[Message(GameMessageOpcode.ServerPlayerHealthUpdate)]
[WireMessage(GameMessageOpcode.ServerPlayerHealthUpdate, Direction.ServerToClient)]
public sealed partial class ServerPlayerHealthUpdate : IWritable
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(32)] public uint Health { get; set; }
    [UInt(18)] public UpdateHealthMask Mask { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(Health);
        writer.Write(Mask, 18u);
    }
}
