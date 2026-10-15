using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[Message(GameMessageOpcode.ServerAbilityPoints)]
[WireMessage(GameMessageOpcode.ServerAbilityPoints, Direction.ServerToClient)]
public sealed partial class ServerAbilityPoints : IWritable
{
    [UInt(32)] public uint AbilityPoints { get; set; }
    [UInt(32)] public uint TotalAbilityPoints { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(AbilityPoints);
        writer.Write(TotalAbilityPoints);
    }
}
