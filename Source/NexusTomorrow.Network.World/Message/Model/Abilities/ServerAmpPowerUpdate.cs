using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[Message(GameMessageOpcode.ServerAmpPowerUpdate)]
[WireMessage(GameMessageOpcode.ServerAmpPowerUpdate, Direction.ServerToClient)]
public sealed partial class ServerAmpPowerUpdate : IWritable
{
    [UInt(16)] public ushort BonusPower { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(BonusPower);
    }
}
