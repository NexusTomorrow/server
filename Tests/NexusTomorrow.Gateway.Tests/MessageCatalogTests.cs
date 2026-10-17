using NexusTomorrow.Gateway.Catalog;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Gateway.Tests;

public sealed class MessageCatalogTests
{
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
    public void OnlyMessagesReadToFieldsAreSendable()
    {
        Assert.True(MessageCatalog.Default.IsSendable(typeof(Network.World.Message.Model.Pregame.CharacterList)));
        Assert.False(MessageCatalog.Default.IsSendable(typeof(Network.World.Message.Model.Pregame.Server0015)));
    }

    [Fact]
    public void EveryPregameRowNamesAClientMessageReadToFields()
    {
        foreach (OpcodeRow row in OpcodeTable.Pregame.Values)
        {
            IWireMessage? message = MessageCatalog.Default.CreateInbound(row.Opcode, out InboundRefusal refusal);
            Assert.True(message is not null, $"0x{row.Opcode:X4}: {refusal}");
            Assert.True(MessageCatalog.Default.RungOf(message.GetType()) >= Rung.Fields);
        }
    }
}
