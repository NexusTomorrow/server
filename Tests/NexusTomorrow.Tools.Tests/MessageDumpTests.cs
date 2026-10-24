using System.Buffers;
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
        int unwritable = 0;
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

                // A default that leaves a member null cannot be written at all, so it has no bytes to compare.
                JsonObject dump = MessageDump.Of(message);
                if (HoldsNull(dump))
                {
                    unwritable++;
                    continue;
                }

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

        TestContext.Current.TestOutputHelper?.WriteLine($"{dumped} declared messages bound back; {unwritable} have a default that cannot be written.");
        Assert.True(dumped > 200, $"only {dumped} messages were dumped");
        Assert.True(problems.Count == 0, string.Join(Environment.NewLine, problems));
    }

    private static bool HoldsNull(JsonNode? node) => node switch
    {
        null => true,
        JsonObject values => values.Any(pair => HoldsNull(pair.Value)),
        JsonArray items => items.Any(HoldsNull),
        _ => false,
    };

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
