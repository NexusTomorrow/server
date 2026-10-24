using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server076D, Direction.ServerToClient)]
public sealed partial class Server076D
{
    [WireStruct]
    public sealed partial class Server076DPart0
    {
        /// <summary>
        /// The guild, by its id within the realm.
        /// </summary>
        [UInt(64)] public ulong GuildId { get; set; }

        [UInt(1)] public bool Unknown1 { get; set; }
        [UInt(32), Count] public uint Unknown2 { get; set; }

        [Array(CountFrom = nameof(Unknown2)), WideString] public List<string> Unknown3 { get; set; } = [];

        [Array(CountFrom = nameof(Unknown2)), UInt(1)] public List<bool> Unknown4 { get; set; } = [];
    }

    [Array(CountBits = 32)] public List<Server076DPart0> Changes { get; set; } = [];
}
