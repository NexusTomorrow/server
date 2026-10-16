using System.Reflection;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.ProtocolReference.Tests.Message.Model.Sample;

namespace NexusTomorrow.ProtocolReference.Tests;

public sealed class LayoutWalkTests
{
    [Fact]
    public void TheFixtureTreeIsTheGeneratorsTree()
    {
        LayoutWalkResult walk = new LayoutWalk([typeof(ServerFF21).Assembly]).Walk(typeof(ServerFF21));

        Assert.Equal("4 [0:32 w] 3 {2:1=(),7=(16)}", WireTree.Format(walk.Nodes));
    }

    [Fact]
    public void EveryNodeOfTheFixtureHasItsRow()
    {
        LayoutWalkResult walk = new LayoutWalk([typeof(ServerFF21).Assembly]).Walk(typeof(ServerFF21));

        string[] expected =
        [
            "`Unknown0 count`|4|unsigned",
            "`Unknown0`||array, counted by `Unknown0 count`",
            "`Unknown0[].Unknown0`|32|unsigned",
            "`Unknown0[].Unknown1`|varies|wide string",
            "`Unknown1`|3|signed; selects `Unknown2`",
            "`Unknown2`||union, selected by `Unknown1`",
            "`Unknown2(1)`||when `Unknown1` is 1; nothing more",
            "`Unknown2(-1)`||when `Unknown1` is -1",
            "`Unknown2(-1).Unknown0`|16|unsigned",
        ];
        Assert.Equal(expected, walk.Rows.Select(row => $"`{row.Path}`|{row.Bits}|{row.Encoding}"));
    }

    [Fact]
    public void EveryDeclaredMessageWalksToItsGeneratedTree()
    {
        Assembly[] models =
        [
            typeof(global::NexusTomorrow.Network.WireMessages).Assembly,
            typeof(global::NexusTomorrow.Network.Auth.WireMessages).Assembly,
            typeof(global::NexusTomorrow.Network.World.WireMessages).Assembly,
        ];
        var walk = new LayoutWalk(models);
        var problems = new List<string>();
        int checkedCount = 0;
        foreach (Type type in models.SelectMany(assembly => assembly.GetTypes()).Where(type => type.GetCustomAttribute<WireMessageAttribute>() is not null))
        {
            string expected = type.GetCustomAttribute<WireLayoutDataAttribute>()!.Tree;
            string walked;
            try
            {
                walked = WireTree.Format(walk.Walk(type).Nodes);
            }
            catch (InvalidOperationException exception)
            {
                walked = $"(the walk failed: {exception.Message})";
            }

            if (walked != expected)
                problems.Add($"{type.Name}: the walk gives {walked}, the generator {expected}");
            checkedCount++;
        }

        TestContext.Current.TestOutputHelper?.WriteLine($"{checkedCount} declared messages walked; {problems.Count} differ.");
        Assert.True(problems.Count == 0, string.Join(Environment.NewLine, problems));
        Assert.True(checkedCount > 1000, $"only {checkedCount} declared messages were found");
    }
}
