using System.Collections.Frozen;
using System.Reflection;
using NexusTomorrow.Game.Static.Entity.Movement.Command;

namespace NexusTomorrow.Network.World.Entity;

/// <summary>
/// Creates the <see cref="IEntityCommandModel"/> for an <see cref="EntityCommand"/>, from every type in this assembly carrying <see cref="EntityCommandAttribute"/>.
/// </summary>
public static class EntityCommandFactory
{
    private static readonly FrozenDictionary<EntityCommand, Func<IEntityCommandModel>> factories = BuildFactories();

    private static FrozenDictionary<EntityCommand, Func<IEntityCommandModel>> BuildFactories()
    {
        var builder = new Dictionary<EntityCommand, Func<IEntityCommandModel>>();
        foreach (Type type in typeof(EntityCommandFactory).Assembly.GetTypes())
        {
            EntityCommandAttribute attribute = type.GetCustomAttribute<EntityCommandAttribute>();
            if (attribute == null || type.GetConstructor(Type.EmptyTypes) == null)
                continue;

            builder.Add(attribute.Command, () => (IEntityCommandModel)Activator.CreateInstance(type));
        }

        return builder.ToFrozenDictionary();
    }

    /// <summary>
    /// Return a new <see cref="IEntityCommandModel"/> for the supplied <see cref="EntityCommand"/>, or null when no model declares it.
    /// </summary>
    public static IEntityCommandModel NewEntityCommand(EntityCommand command)
    {
        return factories.TryGetValue(command, out Func<IEntityCommandModel> factory) ? factory() : null;
    }
}
