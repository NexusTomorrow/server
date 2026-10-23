using System.Text.Json.Nodes;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Message.Model.Contract;
using NexusTomorrow.Network.World.Message.Model.Entity;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.Network.World.Message.Model.Spell;
using NexusTomorrow.Network.World.Message.Model.Unsettled;

namespace NexusTomorrow.Tools.Tests;

public sealed class MessageBuildTests
{
    [Fact]
    public void AMessageBuildsFromEveryMemberAndReadsBackTheSame()
    {
        (IWireStruct? message, byte[] body, string? refusal) = Build<Kicked>("""{"Reason": 7}""");

        Assert.Null(refusal);
        Assert.Equal(7, Assert.IsType<Kicked>(message).Reason);
        Assert.Single(body);
    }

    [Fact]
    public void AMissingMemberIsRefusedByItsPath()
    {
        Assert.Equal("Reason: no value given", Build<Kicked>("{}").Refusal);
    }

    [Fact]
    public void AMemberTheMessageDoesNotHaveIsRefused()
    {
        Assert.Equal("Speed: Kicked has no such wire member", Build<Kicked>("""{"Reason": 1, "Speed": 2}""").Refusal);
    }

    [Fact]
    public void AValueOutsideItsMembersTypeIsRefused()
    {
        Assert.Equal("Reason: 300 does not fit a Byte", Build<Kicked>("""{"Reason": 300}""").Refusal);
    }

    [Fact]
    public void AValueWiderThanItsBitsIsRefused()
    {
        // Reason is 5 bits wide, so 32 cannot travel.
        (IWireStruct? message, _, string? refusal) = Build<Kicked>("""{"Reason": 32}""");

        Assert.Null(message);
        Assert.NotNull(refusal);
    }

    [Fact]
    public void NestedStructsFixedArraysStringsAndFlagsBind()
    {
        const string Fields = """
            {
              "Unknown0": 1,
              "SessionKey": {"Data1": -5, "Data2": 2, "Data3": 3, "Data4": [1, 2, 3, 4, 5, 6, 7, 8]},
              "Gateway": {"Address": 16777343, "Port": 24000},
              "Unknown3": true,
              "RealmName": "Nexus",
              "RealmFlags": 0,
              "PvpType": 2,
              "NoteTextId": 12345
            }
            """;

        (IWireStruct? message, _, string? refusal) = Build<RealmRedirect>(Fields);

        Assert.Null(refusal);
        RealmRedirect redirect = Assert.IsType<RealmRedirect>(message);
        Assert.Equal(-5, redirect.SessionKey.Data1);
        Assert.Equal([1, 2, 3, 4, 5, 6, 7, 8], redirect.SessionKey.Data4);
        Assert.Equal(24000, redirect.Gateway.Port);
        Assert.True(redirect.Unknown3);
        Assert.Equal("Nexus", redirect.RealmName);
        Assert.Equal(12345u, redirect.NoteTextId);
    }

    [Fact]
    public void AFixedArrayTakesExactlyItsLength()
    {
        Assert.Equal("Unknown0: exactly 3 elements", Build<Server0252>("""{"Unknown0": [1, 2]}""").Refusal);
        Assert.Equal([1u, 2u, 3u], Assert.IsType<Server0252>(Build<Server0252>("""{"Unknown0": [1, 2, 3]}""").Message).Unknown0);
    }

    [Fact]
    public void ACountedArrayOfFloatsBinds()
    {
        (IWireStruct? message, _, string? refusal) = Build<Server0931>("""{"Unknown0": 7, "Unknown1": [1.5, -2.25]}""");

        Assert.Null(refusal);
        Assert.Equal([1.5f, -2.25f], Assert.IsType<Server0931>(message).Unknown1);
    }

    [Fact]
    public void AByteBlockTakesHex()
    {
        Assert.Equal([0x0A, 0x0B, 0x0C], Assert.IsType<Server0775>(Build<Server0775>("""{"Unknown0": "0A0B0C"}""").Message).Unknown0);
        Assert.Equal("Unknown0: a hex string", Build<Server0775>("""{"Unknown0": "xyz"}""").Refusal);
    }

    [Fact]
    public void AUnionArmIsChosenByItsCaseAndMustMatchItsSelector()
    {
        const string Arm = """{"$case": 0, "Amount": 9, "Action": {"CasterId": 1, "TargetId": 2, "SpellId": 3, "CombatResult": 4}}""";

        (IWireStruct? message, _, string? refusal) = Build<CombatLog>($$"""{"Type": 0, "Payload": {{Arm}}}""");
        Assert.Null(refusal);
        Assert.Equal(9u, Assert.IsType<CombatLogAmount0>(Assert.IsType<CombatLog>(message).Payload).Amount);

        Assert.Equal("Payload: its case 0 is not Type's value 1", Build<CombatLog>($$"""{"Type": 1, "Payload": {{Arm}}}""").Refusal);
        Assert.Equal("Payload: ICombatLogPayload has no arm for case 63",
            Build<CombatLog>("""{"Type": 63, "Payload": {"$case": 63}}""").Refusal);
    }

    [Fact]
    public void AnOpcodeNoDeclarationNamesIsRefused()
    {
        Assert.Equal("no server message 0xFFFE", OracleVerbs.Build(0xFFFE, []).Refusal);
    }

    private static (IWireStruct? Message, byte[] Body, string? Refusal) Build<T>(string fields)
        where T : IWireMessage => OracleVerbs.Build(T.Opcode, (JsonObject)JsonNode.Parse(fields)!);
}
