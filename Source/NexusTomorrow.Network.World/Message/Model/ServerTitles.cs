using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerTitles, Direction.ServerToClient)]
public sealed partial class ServerTitles
{
    [WireStruct]
    public sealed partial class Title
    {
        [UInt(14)] public ushort TitleId { get; set; }
        [UInt(1)] public bool Revoked { get; set; }
        [UInt(1)] public bool InSchedule { get; set; }
        [UInt(32)] public uint TimeRemaining { get; set; }
    }

    [Array(CountBits = 32)] public List<Title> Titles { get; set; } = [];
}
