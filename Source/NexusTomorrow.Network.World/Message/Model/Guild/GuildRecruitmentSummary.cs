using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// A recruiting guild's summary record.
/// </summary>
[WireStruct]
public sealed partial class GuildRecruitmentSummary
{
    [UInt(16)] public ushort Unknown0 { get; set; }

    [UInt(16)] public ushort Unknown1 { get; set; }

    [UInt(3)] public byte Unknown2 { get; set; }
}
