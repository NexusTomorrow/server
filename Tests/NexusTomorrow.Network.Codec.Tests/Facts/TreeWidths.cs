using System.Collections.Immutable;

namespace NexusTomorrow.Network.Codec.Tests.Facts;

/// <summary>
/// The empty path a layout tree gives: every string empty, every array empty, each union at its arm for selector 0.
/// </summary>
internal static class TreeWidths
{
    /// <summary>
    /// The widths in wire order, or null when a union on the path has no arm 0 or the path meets a recursive node. The reader view reads an
    /// empty terminated string as its count alone, as the client's readers do.
    /// </summary>
    public static List<int>? EmptyPath(ImmutableArray<WireNode> nodes, bool readerView)
    {
        var widths = new List<int>();
        return Append(nodes, readerView, widths) ? widths : null;
    }

    private static bool Append(ImmutableArray<WireNode> nodes, bool readerView, List<int> widths)
    {
        foreach (WireNode node in nodes)
        {
            switch (node.Kind)
            {
                case WireNodeKind.Field:
                    widths.Add(node.Bits);
                    break;
                case WireNodeKind.WideString or WireNodeKind.NarrowString:
                    widths.Add(1);
                    widths.Add(7);
                    break;
                case WireNodeKind.TerminatedWideString:
                    widths.Add(16);
                    if (!readerView)
                        widths.Add(16);
                    break;
                case WireNodeKind.Array:
                    break;
                case WireNodeKind.Union:
                    {
                        WireArm? zero = node.Arms.FirstOrDefault(arm => arm.Selector == 0);
                        if (zero is null || !Append(zero.Nodes, readerView, widths))
                            return false;
                        break;
                    }

                // Only a message holds an aligned blob, never nested (NTC009, NTC011, NTC012), so the bits before it are the whole message's.
                case WireNodeKind.Blob:
                    {
                        int misalignment = widths.Sum() % 8;
                        if (node.Aligned && misalignment != 0)
                            widths.Add(8 - misalignment);
                        widths.Add(node.Bits);
                        break;
                    }

                default:
                    return false;
            }
        }

        return true;
    }
}
