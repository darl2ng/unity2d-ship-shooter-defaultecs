using System;

using DefaultEcs;

namespace Factory
{
    /// <summary>
    /// Entity factory to facilitate creation of entities with a preset of compponents.
    /// TODO: implement pooling and archetype concepts
    /// </summary>
    public sealed class EntityFactory : IEntityFactory
    {
        private readonly World _world;
        private readonly Action<Entity> _finalizer;

        public EntityFactory(World world, Action<Entity> finalizer)
        {
            _world = world;
            _finalizer = finalizer;
        }

        public Entity Create()
        {
            Entity entity = _world.CreateEntity();
            _finalizer?.Invoke(entity);
            return entity;
        }

        public Entity Create<T>() => Create<T>(default);

        public Entity Create<T>(in T component)
        {
            Entity entity = _world.CreateEntity();
            entity.Set<T>(component);
            _finalizer?.Invoke(entity);
            return entity;
        }

        public Entity Create<T1, T2>() => Create<T1, T2>(default, default);

        public Entity Create<T1, T2>(in T1 component1, in T2 component2)
        {
            Entity entity = _world.CreateEntity();
            entity.Set<T1>(component1);
            entity.Set<T2>(component2);
            _finalizer?.Invoke(entity);
            return entity;
        }

        public Entity Create<T1, T2, T3>() => Create<T1, T2, T3>(default, default, default);

        public Entity Create<T1, T2, T3>(in T1 component1, in T2 component2, in T3 component3)
        {
            Entity entity = _world.CreateEntity();
            entity.Set<T1>(component1);
            entity.Set<T2>(component2);
            entity.Set<T3>(component3);
            _finalizer?.Invoke(entity);
            return entity;
        }

        public Entity Create<T1, T2, T3, T4>() => Create<T1, T2, T3, T4>(default, default, default, default);

        public Entity Create<T1, T2, T3, T4>(in T1 component1, in T2 component2, in T3 component3, in T4 component4)
        {
            Entity entity = _world.CreateEntity();
            entity.Set<T1>(component1);
            entity.Set<T2>(component2);
            entity.Set<T3>(component3);
            entity.Set<T4>(component4);
            _finalizer?.Invoke(entity);
            return entity;
        }

        public Entity Create<T1, T2, T3, T4, T5>() => Create<T1, T2, T3, T4, T5>(default, default, default, default, default);

        public Entity Create<T1, T2, T3, T4, T5>(in T1 component1, in T2 component2, in T3 component3, in T4 component4, in T5 component5)
        {
            Entity entity = _world.CreateEntity();
            entity.Set<T1>(component1);
            entity.Set<T2>(component2);
            entity.Set<T3>(component3);
            entity.Set<T4>(component4);
            entity.Set<T5>(component5);
            _finalizer?.Invoke(entity);
            return entity;
        }
        public Entity Create<T1, T2, T3, T4, T5, T6>() => Create<T1, T2, T3, T4, T5, T6>(default, default, default, default, default, default);
        public Entity Create<T1, T2, T3, T4, T5, T6>(in T1 component1, in T2 component2, in T3 component3, in T4 component4, in T5 component5, in T6 component6)
        {
            Entity entity = _world.CreateEntity();
            entity.Set<T1>(component1);
            entity.Set<T2>(component2);
            entity.Set<T3>(component3);
            entity.Set<T4>(component4);
            entity.Set<T5>(component5);
            entity.Set<T6>(component6);
            _finalizer?.Invoke(entity);
            return entity;
        }
        public Entity Create<T1, T2, T3, T4, T5, T6, T7>() => Create<T1, T2, T3, T4, T5, T6, T7>(default, default, default, default, default, default, default);
        public Entity Create<T1, T2, T3, T4, T5, T6, T7>(in T1 component1, in T2 component2, in T3 component3, in T4 component4, in T5 component5, in T6 component6, in T7 component7)
        {
            Entity entity = _world.CreateEntity();
            entity.Set<T1>(component1);
            entity.Set<T2>(component2);
            entity.Set<T3>(component3);
            entity.Set<T4>(component4);
            entity.Set<T5>(component5);
            entity.Set<T6>(component6);
            entity.Set<T7>(component7);
            _finalizer?.Invoke(entity);
            return entity;
        }
        public Entity Create<T1, T2, T3, T4, T5, T6, T7, T8>() => Create<T1, T2, T3, T4, T5, T6, T7, T8>(default, default, default, default, default, default, default, default);
        public Entity Create<T1, T2, T3, T4, T5, T6, T7, T8>(in T1 component1, in T2 component2, in T3 component3, in T4 component4, in T5 component5, in T6 component6, in T7 component7, in T8 component8)
        {
            Entity entity = _world.CreateEntity();
            entity.Set<T1>(component1);
            entity.Set<T2>(component2);
            entity.Set<T3>(component3);
            entity.Set<T4>(component4);
            entity.Set<T5>(component5);
            entity.Set<T6>(component6);
            entity.Set<T7>(component7);
            entity.Set<T8>(component8);
            _finalizer?.Invoke(entity);
            return entity;
        }
        public Entity Create<T1, T2, T3, T4, T5, T6, T7, T8, T9>() => Create<T1, T2, T3, T4, T5, T6, T7, T8, T9>(default, default, default, default, default, default, default, default, default);
        public Entity Create<T1, T2, T3, T4, T5, T6, T7, T8, T9>(in T1 component1, in T2 component2, in T3 component3, in T4 component4, in T5 component5, in T6 component6, in T7 component7, in T8 component8, in T9 component9)
        {
            Entity entity = _world.CreateEntity();
            entity.Set<T1>(component1);
            entity.Set<T2>(component2);
            entity.Set<T3>(component3);
            entity.Set<T4>(component4);
            entity.Set<T5>(component5);
            entity.Set<T6>(component6);
            entity.Set<T7>(component7);
            entity.Set<T8>(component8);
            entity.Set<T9>(component9);
            _finalizer?.Invoke(entity);
            return entity;
        }
        public Entity Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>() => Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(default, default, default, default, default, default, default, default, default, default);
        public Entity Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(in T1 component1, in T2 component2, in T3 component3, in T4 component4, in T5 component5, in T6 component6, in T7 component7, in T8 component8, in T9 component9, in T10 component10)
        {
            Entity entity = _world.CreateEntity();
            entity.Set<T1>(component1);
            entity.Set<T2>(component2);
            entity.Set<T3>(component3);
            entity.Set<T4>(component4);
            entity.Set<T5>(component5);
            entity.Set<T6>(component6);
            entity.Set<T7>(component7);
            entity.Set<T8>(component8);
            entity.Set<T9>(component9);
            entity.Set<T10>(component10);
            _finalizer?.Invoke(entity);
            return entity;
        }
    }
}
