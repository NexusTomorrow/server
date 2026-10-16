using System.Collections.Immutable;

namespace NexusTomorrow.Network.Codec.Tests;

public sealed class WireTreeTests
{
    [Theory]
    [InlineData("")]
    [InlineData("32 16 32 1")]
    [InlineData("w n t")]
    [InlineData("3 [0:18] 4 [2:32] [2:32] 8 8 8 2 [8:8 8]")]
    [InlineData("14 64 w 5 [3:4 16 16 {0:0=(1),2=(18)}] 16")]
    [InlineData("5 b32a")]
    [InlineData("8 {0:0=(),255=(? 7)}")]
    public void FormatGivesBackTheTextParseRead(string text)
    {
        Assert.Equal(text, WireTree.Format(WireTree.Parse(text)));
    }

    [Fact]
    public void ParseReadsEachNodeKind()
    {
        ImmutableArray<WireNode> nodes = WireTree.Parse("14 64 w 5 [3:4 16 16 {0:0=(1),2=(18)}] 16");

        Assert.Equal([WireNodeKind.Field, WireNodeKind.Field, WireNodeKind.WideString, WireNodeKind.Field, WireNodeKind.Array, WireNodeKind.Field], nodes.Select(node => node.Kind));
        Assert.Equal(3, nodes[4].Reference);
        WireNode union = nodes[4].Element[3];
        Assert.Equal(WireNodeKind.Union, union.Kind);
        Assert.Equal(0, union.Reference);
        Assert.Equal([0ul, 2ul], union.Arms.Select(arm => arm.Selector));
        Assert.Equal(18, union.Arms[1].Nodes[0].Bits);
    }

    // In order: a zero-width field, a field over 64 bits, a count index not before its array, a count index naming the array itself,
    // a count that is not a field, an unclosed array, arms out of order, a union with no arm, two spaces, an unknown token, a blob with no width.
    [Theory]
    [InlineData("0")]
    [InlineData("65")]
    [InlineData("[0:8]")]
    [InlineData("8 [1:8]")]
    [InlineData("w [0:8]")]
    [InlineData("8 [0:8")]
    [InlineData("8 {0:2=(),1=()}")]
    [InlineData("8 {0:}")]
    [InlineData("8  8")]
    [InlineData("8 x")]
    [InlineData("b")]
    public void ParseRejectsMalformedText(string text)
    {
        Assert.Throws<FormatException>(() => WireTree.Parse(text));
    }
}
