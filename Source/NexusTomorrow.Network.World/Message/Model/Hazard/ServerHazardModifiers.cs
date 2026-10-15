using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Hazard;

[Message(GameMessageOpcode.ServerHazardModifiers)]
[WireMessage(GameMessageOpcode.ServerHazardModifiers, Direction.ServerToClient)]
public sealed partial class ServerHazardModifiers : IWritable
{
    [Array(CountBits = 32)] public List<HazardModifier> HazardIdModifiers { get; set; } = [];
    [Array(CountBits = 32)] public List<HazardModifier> HazardTypeModifiers { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(HazardIdModifiers.Count);
        HazardIdModifiers.ForEach(hazard => hazard.Write(writer));

        writer.Write(HazardTypeModifiers.Count);
        HazardTypeModifiers.ForEach(hazard => hazard.Write(writer));
    }
}
