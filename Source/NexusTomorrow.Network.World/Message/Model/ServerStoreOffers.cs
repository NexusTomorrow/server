using NexusTomorrow.Game.Static.Storefront;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerStoreOffers, Direction.ServerToClient)]
public sealed partial class ServerStoreOffers
{
    [WireStruct]
    public sealed partial class OfferGroup
    {
        [WireStruct]
        public sealed partial class Offer
        {
            [WireStruct]
            public sealed partial class OfferCurrencyData
            {
                [UInt(5)] public byte CurrencyId { get; set; } // 5
                [Float32] public float Price { get; set; }
                [UInt(2)] public DiscountType DiscountType { get; set; } // 2
                [Float32] public float DiscountValue { get; set; }
                [UInt(64)] public long DiscountTimeRemaining { get; set; }
                [UInt(64)] public long TimeSinceExpiry { get; set; }
            }

            [WireStruct]
            public sealed partial class OfferItemData
            {
                /// <summary>
                /// The item data <see cref="Type"/> selects.
                /// </summary>
                public interface IPayload : IWireStruct
                {
                }

                [WireStruct]
                [UnionCase(0)]
                public sealed partial class AccountItem : IPayload
                {
                    [UInt(15)] public ushort AccountItemId { get; set; }
                    [UInt(32)] public uint Amount { get; set; }
                }

                [WireStruct]
                [UnionCase(1)]
                public sealed partial class UnknownPayload1 : IPayload
                {
                    [UInt(32)] public uint Type1Unknown0 { get; set; }
                    [UInt(32)] public uint Type1Unknown1 { get; set; }
                }

                [WireStruct]
                [UnionCase(2)]
                public sealed partial class UnknownPayload2 : IPayload
                {
                    [UInt(32)] public uint Type2Unknown0 { get; set; }
                }

                [UInt(32)] public uint Type { get; set; } // Should be 0
                [Union(nameof(Type))] public IPayload Payload { get; set; } = new AccountItem();
            }

            [UInt(32)] public uint Id { get; set; }
            [WideString] public string Name { get; set; }
            [WideString] public string Description { get; set; }
            [Float32] public float PricePremium { get; set; }
            [Float32] public float PriceAlternative { get; set; }
            [UInt(32)] public DisplayFlag DisplayFlags { get; set; }
            [UInt(64)] public long Unknown6 { get; set; }
            [UInt(8)] public byte Unknown7 { get; set; }
            [Array(CountBits = 32)] public List<OfferCurrencyData> CurrencyData { get; set; } = [];
            [Array(CountBits = 32)] public List<OfferItemData> ItemData { get; set; } = [];
        }

        [UInt(32)] public uint Id { get; set; }
        [UInt(32)] public DisplayFlag DisplayFlags { get; set; }
        [WideString] public string Name { get; set; }
        [WideString] public string Description { get; set; }
        [UInt(14)] public ushort DisplayInfoOverride { get; set; }
        [Array(CountBits = 32)] public List<Offer> Offers { get; set; } = [];

        // The categories go on the wire as two columns, every Id and then every Index, sharing one count.
        [UInt(32), Count] public uint CategoryCount { get; set; }
        [Array(CountFrom = nameof(CategoryCount)), UInt(32)] public List<uint> CategoryIds { get; set; } = [];
        [Array(CountFrom = nameof(CategoryCount)), UInt(32)] public List<uint> CategoryIndexes { get; set; } = [];
    }

    [Array(CountBits = 32)] public List<OfferGroup> OfferGroups { get; set; } = [];
}
