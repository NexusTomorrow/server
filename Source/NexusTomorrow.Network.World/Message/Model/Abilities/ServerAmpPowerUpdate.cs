using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[WireMessage(GameMessageOpcode.ServerAmpPowerUpdate, Direction.ServerToClient)]
public sealed partial class ServerAmpPowerUpdate
{
    [UInt(16)] public ushort BonusPower { get; set; }
}
