using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.ServerHousingBasics, Direction.ServerToClient)]
public sealed partial class ServerHousingBasics
{
    [Flags]
    public enum ResidencePrivacyLevelFlags
    {
        Public = 0x00,
        Private = 0x01,
        NeighboursOnly = 0x02,
        RoommatesOnly = 0x04
    }

    [UInt(64)] public ulong ResidenceId { get; set; }
    [UInt(64)] public ulong NeighbourhoodId { get; set; }
    [UInt(32)] public ResidencePrivacyLevelFlags PrivacyLevel { get; set; }
}
