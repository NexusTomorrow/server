using NexusTomorrow.Gateway.Catalog;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Gateway.Tests;

public sealed class MessageCatalogTests
{
    private static readonly MessageCatalog s_gate = MessageCatalog.Build([(WireMessages.Layouts, WireMessages.CreateClientToServer, WireMessages.CreateServerToClient)]);

    [Fact]
    public void EachOpcodeHasOneDeclarationPerDirectionAcrossTheAssemblies()
    {
        WireLayout[] layouts =
        [
            .. global::NexusTomorrow.Network.WireMessages.Layouts,
            .. global::NexusTomorrow.Network.Auth.WireMessages.Layouts,
            .. global::NexusTomorrow.Network.World.WireMessages.Layouts,
        ];

        IEnumerable<IGrouping<ushort, WireLayout>> clientToServer = layouts.Where(l => l.Direction.HasFlag(Direction.ClientToServer)).GroupBy(l => l.Opcode).Where(g => g.Count() > 1);
        IEnumerable<IGrouping<ushort, WireLayout>> serverToClient = layouts.Where(l => l.Direction.HasFlag(Direction.ServerToClient)).GroupBy(l => l.Opcode).Where(g => g.Count() > 1);
        Assert.Empty(clientToServer.Select(g => $"0x{g.Key:X4} client to server"));
        Assert.Empty(serverToClient.Select(g => $"0x{g.Key:X4} server to client"));
    }

    [Fact]
    public void AMessageReadToFieldsIsCreated()
    {
        IWireMessage? message = MessageCatalog.Default.CreateInbound(0x07E0, out InboundRefusal refusal);
        Assert.IsType<Network.World.Message.Model.Pregame.CharacterListRequest>(message);
        Assert.Equal(InboundRefusal.None, refusal);
    }

    [Fact]
    public void AnUnverifiedMessageIsRefusedAsBelowFields()
    {
        // 0x0356 is a declared client message and has no rung.
        Assert.Null(MessageCatalog.Default.CreateInbound(0x0356, out InboundRefusal refusal));
        Assert.Equal(InboundRefusal.BelowFields, refusal);
    }

    [Fact]
    public void AnUndeclaredOpcodeIsRefusedAsUndeclared()
    {
        Assert.Null(MessageCatalog.Default.CreateInbound(0xFFFE, out InboundRefusal refusal));
        Assert.Equal(InboundRefusal.Undeclared, refusal);
    }

    [Fact]
    public void AMessageReadToFieldsMaySendWhateverItSets()
    {
        bool allowed = MessageCatalog.Default.MaySend(new Network.World.Message.Model.Pregame.CharacterRenameResult { Result = 18 }, out string? refusal);

        Assert.True(allowed);
        Assert.Null(refusal);
    }

    [Fact]
    public void AMessageBelowPurposeMayNotSend()
    {
        bool unverified = MessageCatalog.Default.MaySend(new Network.World.Message.Model.Unsettled.Server00B7(), out string? refusal);
        bool unread = s_gate.MaySend(new GateUnreadMessage { Known = 1 }, out _);

        Assert.False(unverified);
        Assert.Equal("Server00B7 is not read to Purpose", refusal);
        Assert.False(unread);
    }

    [Fact]
    public void ADormantMessageMayNotSendWhateverItsRungAndTheRefusalSaysItIsDormant()
    {
        bool allowed = s_gate.MaySend(new GateDormantMessage { Known = 1 }, out string? refusal);

        Assert.False(allowed);
        Assert.Equal("GateDormantMessage is dormant", refusal);
    }

    [Fact]
    public void AMessageHoldingADormantMessageAsAMemberMaySend()
    {
        bool allowed = s_gate.MaySend(new GateHolderMessage { Count = 1, Record = new GateDormantMessage { Known = 2 } }, out string? refusal);

        Assert.True(allowed);
        Assert.Null(refusal);
    }

    [Fact]
    public void APurposeMessageWithOnlyVerifiedMembersSetMaySend()
    {
        Assert.True(s_gate.MaySend(new GatePurposeMessage { Known = 5 }, out string? refusal));
        Assert.Null(refusal);
    }

    [Fact]
    public void APurposeMessageWithAnUnverifiedMemberSetMayNotSendAndTheRefusalNamesIt()
    {
        bool allowed = s_gate.MaySend(new GatePurposeMessage { Known = 5, Unread = 1 }, out string? refusal);

        Assert.False(allowed);
        Assert.Equal("GatePurposeMessage.Unread is set, but its meaning is not verified", refusal);
    }

    [Fact]
    public void TheSmallestPlayerStateMaySend()
    {
        Assert.True(MessageCatalog.Default.MaySend(new Network.World.Message.Model.WorldEntry.PlayerState(), out _));
    }

    [Fact]
    public void ASendTheGateAllowsAllocatesNothing()
    {
        var message = new GatePurposeMessage { Known = 5 };
        for (int i = 0; i < 10; i++)
            s_gate.MaySend(message, out _);

        long before = GC.GetAllocatedBytesForCurrentThread();
        for (int i = 0; i < 1000; i++)
            s_gate.MaySend(message, out _);
        long allocated = GC.GetAllocatedBytesForCurrentThread() - before;

        Assert.Equal(0L, allocated);
    }

    [Fact]
    public void EveryRowNamesAClientMessageTheServerMayRead()
    {
        foreach (OpcodeRow row in OpcodeTable.All.Values)
        {
            IWireMessage? message = MessageCatalog.Default.CreateInbound(row.Opcode, row.ReadsVerifiedMembers, out InboundRefusal refusal);
            Assert.True(message is not null, $"0x{row.Opcode:X4}: {refusal}");
            Rung floor = row.ReadsVerifiedMembers ? Rung.Purpose : Rung.Fields;
            Assert.True(MessageCatalog.Default.RungOf(message.GetType()) >= floor, $"0x{row.Opcode:X4} is read below {floor}");
        }
    }

    [Fact]
    public void AMessageAtPurposeIsRefusedUnlessItsRowReadsOnlyVerifiedMembers()
    {
        Assert.Null(MessageCatalog.Default.CreateInbound(0x0637, admitPurpose: false, out InboundRefusal withoutRefusal));
        Assert.Equal(InboundRefusal.BelowFields, withoutRefusal);

        Assert.IsType<Network.World.Message.Model.Movement.ClientMovement>(MessageCatalog.Default.CreateInbound(0x0637, admitPurpose: true, out _));

        Assert.Null(MessageCatalog.Default.CreateInbound(0x0356, admitPurpose: true, out InboundRefusal unverifiedRefusal));
        Assert.Equal(InboundRefusal.BelowPurpose, unverifiedRefusal);
    }
}
