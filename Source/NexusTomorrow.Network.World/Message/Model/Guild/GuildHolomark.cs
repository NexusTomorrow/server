using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// A guild's holomark: its background, foreground and scan lines, each a part with three colors.
/// </summary>
[WireStruct]
public sealed partial class GuildHolomark
{
    /// <summary>
    /// One layer of a holomark: a part and its three colors.
    /// </summary>
    [WireStruct]
    public sealed partial class Part
    {
        /// <summary>
        /// The part, a GuildStandardPart table id.
        /// </summary>
        [UInt(14)] public ushort PartId { get; set; }

        /// <summary>
        /// The part's first color, by id.
        /// </summary>
        [UInt(14)] public ushort Color1 { get; set; }

        /// <summary>
        /// The part's second color, by id.
        /// </summary>
        [UInt(14)] public ushort Color2 { get; set; }

        /// <summary>
        /// The part's third color, by id.
        /// </summary>
        [UInt(14)] public ushort Color3 { get; set; }
    }

    /// <summary>
    /// The background layer.
    /// </summary>
    public Part Background { get; set; } = new();

    /// <summary>
    /// The foreground layer.
    /// </summary>
    public Part Foreground { get; set; } = new();

    /// <summary>
    /// The scan lines layer.
    /// </summary>
    public Part ScanLines { get; set; } = new();
}
