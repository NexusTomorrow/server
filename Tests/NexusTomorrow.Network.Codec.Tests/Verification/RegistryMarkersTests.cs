using System.Reflection;
using NexusTomorrow.Gateway.Catalog;
using NexusTomorrow.Network.Codec.Tests.Facts;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Codec.Tests.Verification;

public sealed class RegistryMarkersTests
{
    private static readonly Assembly[] s_models =
    [
        typeof(global::NexusTomorrow.Network.WireMessages).Assembly,
        typeof(global::NexusTomorrow.Network.Auth.WireMessages).Assembly,
        typeof(global::NexusTomorrow.Network.World.WireMessages).Assembly,
    ];

    private static WireLayout[] Layouts() =>
    [
        .. global::NexusTomorrow.Network.WireMessages.Layouts,
        .. global::NexusTomorrow.Network.Auth.WireMessages.Layouts,
        .. global::NexusTomorrow.Network.World.WireMessages.Layouts,
    ];

    private static List<(Type Type, WireMessageAttribute Message, string Key)> Messages() =>
    [
        .. s_models.SelectMany(assembly => assembly.GetTypes())
            .Select(type => (Type: type, Message: type.GetCustomAttribute<WireMessageAttribute>()))
            .Where(m => m.Message is not null)
            .Select(m => (Type: m.Type, Message: m.Message!, Key: $"0x{(ushort)m.Message!.Opcode:X4}"))
            .OrderBy(m => m.Key, StringComparer.Ordinal),
    ];

    private static bool Carries<TMarker>(Type type)
        where TMarker : Attribute => type.IsDefined(typeof(TMarker), inherit: false);

    // A confirmed layout is the one the client takes, so on every side the message travels the facts give that side's layout and it agrees
    // field for field, with no split of one field into several, and no pinned disagreement stands on the message.
    [Fact]
    public void EveryLayoutConfirmedMessageMatchesTheClientFactsAndNoPinNamesIt()
    {
        Dictionary<int, WireFactsRow> rows = WireFacts.Load(WireFacts.TablePath);
        Dictionary<(ushort Opcode, Direction Direction), WireLayout> layouts = Layouts().ToDictionary(layout => (layout.Opcode, layout.Direction));
        var confirmed = Messages().Where(m => Carries<LayoutConfirmedAttribute>(m.Type)).ToList();
        TestContext.Current.TestOutputHelper?.WriteLine($"{confirmed.Count} layout-confirmed messages: {string.Join(", ", confirmed.Select(m => m.Key))}.");

        var problems = new List<string>();
        foreach ((Type type, WireMessageAttribute message, string key) in confirmed)
        {
            WireLayout layout = layouts[((ushort)message.Opcode, message.Direction)];
            WireFactsRow? row = rows.GetValueOrDefault(layout.Opcode);
            var splits = new List<WireSplit>();
            problems.AddRange(WireFactsComparison.Compare(layout, row, splits).Select(problem => $"{type.Name}: {problem}"));
            problems.AddRange(splits.Select(split => $"{type.Name}: {split.Key}: the client takes {string.Join(' ', split.Client)} where the declaration takes {string.Join(' ', split.Declared)}"));
            if ((layout.Direction & Direction.ClientToServer) != 0 && row?.ClientToServer?.Kind == "unknown")
                problems.Add($"{type.Name}: {key} client to server: the client facts give no layout for this side");
            if ((layout.Direction & Direction.ServerToClient) != 0 && row?.ServerToClient?.Kind == "unknown")
                problems.Add($"{type.Name}: {key} server to client: the client facts give no layout for this side");
            problems.AddRange(RegistryDisagreements.Pinned.Keys
                .Where(pin => pin == key || pin.StartsWith(key + " ", StringComparison.Ordinal))
                .Order(StringComparer.Ordinal)
                .Select(pin => $"{type.Name}: a disagreement is pinned at {pin}"));
        }

        if (problems.Count > 0)
            Assert.Fail(string.Join(Environment.NewLine, problems));
    }

    // A dormant message never travels by itself, so no opcode table row admits it, and it counts apart from the messages still to be read.
    // Its rung is independent of the mark: the client may read it as a record inside other messages.
    [Fact]
    public void EveryDormantMessageIsUnadmittedAndOffTheUnverifiedList()
    {
        var dormant = Messages().Where(m => Carries<DormantAttribute>(m.Type)).ToList();
        TestContext.Current.TestOutputHelper?.WriteLine($"{dormant.Count} dormant messages: {string.Join(", ", dormant.Select(m => m.Key))}.");

        var problems = new List<string>();
        foreach ((Type type, WireMessageAttribute message, string key) in dormant)
        {
            if (OpcodeTable.All.ContainsKey((ushort)message.Opcode))
                problems.Add($"{key} {type.Name}: dormant, but the opcode table has a row for it");
            if (RegistryVerification.Unverified.Contains(key))
                problems.Add($"{key} {type.Name}: dormant, but RegistryVerification.Unverified lists it");
        }

        if (problems.Count > 0)
            Assert.Fail(string.Join(Environment.NewLine, problems));
    }

    // The client never handles or sends a dormant message, so it never takes that message's layout, which is what a confirmed layout says it does.
    [Fact]
    public void NoMessageIsBothLayoutConfirmedAndDormant()
    {
        string[] both = [.. Messages().Where(m => Carries<LayoutConfirmedAttribute>(m.Type) && Carries<DormantAttribute>(m.Type)).Select(m => $"{m.Key} {m.Type.Name}")];

        Assert.Empty(both);
    }
}
