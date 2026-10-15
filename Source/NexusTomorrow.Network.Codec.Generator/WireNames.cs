namespace NexusTomorrow.Network.Codec.Generator;

/// <summary>
/// The metadata names of the declaration attributes and runtime types the generator reads and emits.
/// </summary>
internal static class WireNames
{
    public const string Codec = "NexusTomorrow.Network.Codec";
    public const string WireStruct = Codec + ".WireStructAttribute";
    public const string WireMessage = "NexusTomorrow.Network.Message.WireMessageAttribute";
    public const string UInt = Codec + ".UIntAttribute";
    public const string SInt = Codec + ".SIntAttribute";
    public const string Float32 = Codec + ".Float32Attribute";
    public const string Float64 = Codec + ".Float64Attribute";
    public const string PackedFloat = Codec + ".PackedFloatAttribute";
    public const string WideString = Codec + ".WideStringAttribute";
    public const string NarrowString = Codec + ".NarrowStringAttribute";
    public const string TerminatedWideString = Codec + ".TerminatedWideStringAttribute";
    public const string Array = Codec + ".ArrayAttribute";
    public const string Count = Codec + ".CountAttribute";
    public const string FixedArray = Codec + ".FixedArrayAttribute";
    public const string Union = Codec + ".UnionAttribute";
    public const string UnionCase = Codec + ".UnionCaseAttribute";
    public const string Blob = Codec + ".BlobAttribute";
    public const string Pad = Codec + ".PadAttribute";
    public const string PadType = Codec + ".Pad";
    public const string NotOnWire = Codec + ".NotOnWireAttribute";
    public const string WireLayoutData = Codec + ".WireLayoutDataAttribute";
    public const string IWireStruct = Codec + ".IWireStruct";

    public const string GlobalCodec = "global::" + Codec;
    public const string Vector2 = "global::System.Numerics.Vector2";
    public const string Vector3 = "global::System.Numerics.Vector3";
}
