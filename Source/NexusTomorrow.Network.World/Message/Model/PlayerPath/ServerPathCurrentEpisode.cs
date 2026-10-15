using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ServerPathSetCurrentEpisode, Direction.ServerToClient)]
public sealed partial class ServerPathSetCurrentEpisode
{
    [UInt(15)] public ushort Unused { get; set; } // shows values in sniffs but client does not use
    [UInt(14)] public ushort PathEpisodeId { get; set; }
}
