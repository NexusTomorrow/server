using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.GenericUnlock;

[WireMessage(GameMessageOpcode.Server018A, Direction.ServerToClient)]
public sealed partial class Server018A
{
    [UInt(14)] public ushort Unknown0 { get; set; }
    [UInt(1)] public bool Unknown1 { get; set; }
    [UInt(1)] public bool Unknown2 { get; set; }
}
