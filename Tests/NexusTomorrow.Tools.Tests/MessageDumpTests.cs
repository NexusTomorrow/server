using System.Buffers;
using System.Globalization;
using System.Reflection;
using System.Text.Json.Nodes;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Message.Model.Chat;
using NexusTomorrow.Network.World.Message.Model.Contract;
using NexusTomorrow.Network.World.Message.Model.Entity;
using NexusTomorrow.Network.World.Message.Model.Movement;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.Network.World.Message.Model.Spell;
using NexusTomorrow.Network.World.Message.Model.Unsettled;
using NexusTomorrow.Transport;

namespace NexusTomorrow.Tools.Tests;

public sealed class MessageDumpTests
{
    public static TheoryData<Type, string> Messages => new()
    {
        { typeof(Kicked), """{"Reason": 7}""" },
        { typeof(GearScoreChanged), """{"GearScore": 412.5}""" },
        { typeof(Server0931), """{"Unknown0": 7, "Unknown1": [1.5, -2.25]}""" },
        { typeof(Server0252), """{"Unknown0": [1, 2, 3]}""" },
        { typeof(Server0775), """{"Unknown0": "0A0B0C"}""" },
        { typeof(CombatLog), """{"Type": 0, "Payload": {"$case": 0, "Amount": 9, "Action": {"CasterId": 1, "TargetId": 2, "SpellId": 3, "CombatResult": 4}}}""" },
        {
            typeof(RealmRedirect),
            """
            {"Unknown0": 1, "SessionKey": {"Data1": -5, "Data2": 2, "Data3": 3, "Data4": [1, 2, 3, 4, 5, 6, 7, 8]},
             "Gateway": {"Address": 16777343, "Port": 24000}, "Unknown3": true, "RealmName": "Nexus", "RealmFlags": 0, "PvpType": 2,
             "NoteTextId": 12345}
            """
        },
    };

    [Theory]
    [MemberData(nameof(Messages))]
    public void ADumpedMessageBindsBackToTheSameBytes(Type type, string fields)
    {
        IWireStruct message = Bound(type, (JsonObject)JsonNode.Parse(fields)!);

        IWireStruct again = Bound(type, MessageDump.Of(message));

        Assert.Equal(Serialize(message), Serialize(again));
    }

    [Fact]
    public void AReceiptDumpsItsTicket()
    {
        IWireStruct receipt = Bound(typeof(MovementControlReceipt), new JsonObject { ["Ticket"] = 77 });

        Assert.Equal("""{"Ticket":77}""", MessageDump.Of(receipt).ToJsonString());
    }

    [Fact]
    public void AGetOnlyListBindsBackFromItsDump()
    {
        var message = new ChatSend();
        message.Links.Add(new ChatLink { Start = 1, End = 4 });
        message.Links.Add(new ChatLink { Start = 6, End = 9 });

        IWireStruct again = Bound(typeof(ChatSend), MessageDump.Of(message));

        Assert.Equal(Serialize(message), Serialize(again));
        Assert.Equal(2, ((ChatSend)again).Links.Count);
    }

    [Fact]
    public void AFloatThatIsNotABinaryFractionBindsBackToTheSameFloat()
    {
        IWireStruct message = Bound(typeof(GearScoreChanged), new JsonObject { ["GearScore"] = 0.1 });

        Assert.Equal(0.1f, ((GearScoreChanged)Bound(typeof(GearScoreChanged), MessageDump.Of(message))).GearScore);
    }

    [Fact]
    public void AFloatThatIsNotFiniteIsWrittenAsItsName()
    {
        var message = new GearScoreChanged { GearScore = float.NaN };

        Assert.Equal("""{"GearScore":"NaN"}""", MessageDump.Of(message).ToJsonString());
    }

    [Fact]
    public void EveryDeclaredMessagesDefaultDumpBindsBackToTheSameBytes()
    {
        var problems = new List<string>();
        int dumped = 0;
        var unwritable = new List<string>();
        for (int opcode = 0; opcode <= ushort.MaxValue; opcode++)
        {
            IWireStruct?[] messages =
            [
                Network.World.WireMessages.CreateClientToServer((ushort)opcode)
                    ?? Network.Auth.WireMessages.CreateClientToServer((ushort)opcode)
                    ?? Network.WireMessages.CreateClientToServer((ushort)opcode),
                OracleVerbs.CreateServerMessage((ushort)opcode),
            ];
            foreach (IWireStruct? message in messages)
            {
                if (message is null)
                    continue;

                // A default that leaves a member null cannot be written at all, and one whose union holds an arm its selector does
                // not name (a union with no case-0 arm) is no message a client sends; neither has bytes to compare. Both are judged
                // from the message, so a dump that wrongly writes null fails below rather than hide here.
                if (HoldsNullMember(message) || !UnionsMatchTheirSelectors(message))
                {
                    unwritable.Add(message.GetType().Name);
                    continue;
                }

                JsonObject dump = MessageDump.Of(message);

                try
                {
                    if (!Serialize(message).AsSpan().SequenceEqual(Serialize(Bound(message.GetType(), dump))))
                        problems.Add($"{message.GetType().Name}: binds back to other bytes");
                }
                catch (Exception exception) when (exception is ArgumentException or InvalidOperationException)
                {
                    problems.Add($"{message.GetType().Name}: {exception.Message}");
                }

                dumped++;
            }
        }

        TestContext.Current.TestOutputHelper?.WriteLine(
            $"{dumped} declared messages bound back; {unwritable.Count} have a default no client sends: {string.Join(", ", unwritable.Order())}");
        Assert.True(dumped > 200, $"only {dumped} messages were dumped");
        Assert.True(problems.Count == 0, string.Join(Environment.NewLine, problems));

        // Pinned, so a message whose default stops being one a client sends is seen rather than skipped.
        Assert.Equal(["Client0255", "Client033F", "Client050D", "Client0510", "GuildBankTabNames"], unwritable.Order());
    }

    private static bool HoldsNullMember(object value)
    {
        foreach (MemberInfo member in MessageBinder.WireMembers(value.GetType()))
        {
            if (member.GetCustomAttribute<PadAttribute>() is not null)
                continue;

            object? held = MessageBinder.Read(member, value);
            if (held is null)
                return true;
            if (held is System.Collections.IEnumerable items and not string and not byte[])
            {
                if (items.Cast<object?>().Any(item => item is null || (MessageBinder.IsWireType(item.GetType()) && HoldsNullMember(item))))
                    return true;
            }
            else if ((MessageBinder.IsWireType(held.GetType()) || member.GetCustomAttribute<UnionAttribute>() is not null) && HoldsNullMember(held))
            {
                return true;
            }
        }

        return false;
    }

    private static bool UnionsMatchTheirSelectors(object value)
    {
        IReadOnlyList<MemberInfo> members = MessageBinder.WireMembers(value.GetType());
        foreach (MemberInfo member in members)
        {
            object? held = MessageBinder.Read(member, value);
            if (member.GetCustomAttribute<UnionAttribute>() is { } union && held is not null)
            {
                object? selector = MessageBinder.Read(members.Single(candidate => candidate.Name == union.Selector), value);
                decimal armCase = Convert.ToDecimal(held.GetType().GetCustomAttribute<UnionCaseAttribute>()!.Value, CultureInfo.InvariantCulture);
                if (Convert.ToDecimal(selector, CultureInfo.InvariantCulture) != armCase || !UnionsMatchTheirSelectors(held))
                    return false;
            }
            else if (held is not null && MessageBinder.IsWireType(held.GetType()) && !UnionsMatchTheirSelectors(held))
            {
                return false;
            }
        }

        return true;
    }

    private static IWireStruct Bound(Type type, JsonObject fields)
    {
        var message = (IWireStruct)Activator.CreateInstance(type, nonPublic: true)!;
        MessageBinder.Bind(message, fields);
        return message;
    }

    private static byte[] Serialize(IWireStruct message)
    {
        byte[] rented = FrameWriter.RentBody(message, out int length);
        byte[] body = rented.AsSpan(0, length).ToArray();
        ArrayPool<byte>.Shared.Return(rented, clearArray: true);
        return body;
    }
}
