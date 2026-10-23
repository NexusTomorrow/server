using System.Buffers;
using System.Text.Json.Nodes;
using NexusTomorrow.Network.Codec;
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
    public void EveryClientMessageDumps()
    {
        int dumped = 0;
        for (int opcode = 0; opcode <= ushort.MaxValue; opcode++)
        {
            IWireMessage? message = Network.World.WireMessages.CreateClientToServer((ushort)opcode)
                ?? Network.Auth.WireMessages.CreateClientToServer((ushort)opcode)
                ?? Network.WireMessages.CreateClientToServer((ushort)opcode);
            if (message is null)
                continue;

            _ = MessageDump.Of(message);
            dumped++;
        }

        Assert.True(dumped > 100, $"only {dumped} client messages were dumped");
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
