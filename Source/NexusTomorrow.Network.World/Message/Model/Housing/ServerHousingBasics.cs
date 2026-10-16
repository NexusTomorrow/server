using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Server010E, Direction.ServerToClient)]
public sealed partial class Server010E
{
    [Flags]
    public enum ResidencePrivacyLevelFlags
    {
        Public = 0x00,
        Private = 0x01,
        NeighboursOnly = 0x02,
        RoommatesOnly = 0x04
    }

    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(64)] public ulong Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
}
