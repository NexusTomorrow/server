using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// Moves the player into a world: on character select, the answer to <see cref="CharacterSelect"/>; in the world, a change of world.
/// </summary>
/// <remarks>
/// On character select the client takes it only while a selection is pending, shows the world's loading screen and loads the world.
/// </remarks>
[WireMessage(GameMessageOpcode.Server00AD, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class EnterWorld
{
    /// <summary>
    /// The world, as an id in the client's World table.
    /// </summary>
    [UInt(15)] public ushort WorldId { get; set; }

    /// <summary>
    /// The point the client centres the world's loading on, until its camera takes over.
    /// </summary>
    [Float32] public Vector3 Position { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [Float32] public float Unknown2 { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [Float32] public float Unknown3 { get; set; }
}
