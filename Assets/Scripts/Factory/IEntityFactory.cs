using DefaultEcs;

namespace Factory
{
    /// <summary>
    /// Interface to facilitate creation of entities with different sets of components
    /// TODO: add the archetype concept to be able to create entities with archetype instead
    /// </summary>
    public interface IEntityFactory
    {
        Entity Create();
        Entity Create<T>();
        Entity Create<T>(in T component);
        Entity Create<T1, T2>();
        Entity Create<T1, T2>(in T1 component1, in T2 component2);
        Entity Create<T1, T2, T3>();
        Entity Create<T1, T2, T3>(in T1 component1, in T2 component2, in T3 component3);
        Entity Create<T1, T2, T3, T4>();
        Entity Create<T1, T2, T3, T4>(in T1 component1, in T2 component2, in T3 component3, in T4 component4);
        Entity Create<T1, T2, T3, T4, T5>();
        Entity Create<T1, T2, T3, T4, T5>(in T1 component1, in T2 component2, in T3 component3, in T4 component4, in T5 component5);
        Entity Create<T1, T2, T3, T4, T5, T6>();
        Entity Create<T1, T2, T3, T4, T5, T6>(in T1 component1, in T2 component2, in T3 component3, in T4 component4, in T5 component5, in T6 component6);
        Entity Create<T1, T2, T3, T4, T5, T6, T7>();
        Entity Create<T1, T2, T3, T4, T5, T6, T7>(in T1 component1, in T2 component2, in T3 component3, in T4 component4, in T5 component5, in T6 component6, in T7 component7);
        Entity Create<T1, T2, T3, T4, T5, T6, T7, T8>();
        Entity Create<T1, T2, T3, T4, T5, T6, T7, T8>(in T1 component1, in T2 component2, in T3 component3, in T4 component4, in T5 component5, in T6 component6, in T7 component7, in T8 component8);
        Entity Create<T1, T2, T3, T4, T5, T6, T7, T8, T9>();
        Entity Create<T1, T2, T3, T4, T5, T6, T7, T8, T9>(in T1 component1, in T2 component2, in T3 component3, in T4 component4, in T5 component5, in T6 component6, in T7 component7, in T8 component8, in T9 component9);
        Entity Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>();
        Entity Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(in T1 component1, in T2 component2, in T3 component3, in T4 component4, in T5 component5, in T6 component6, in T7 component7, in T8 component8, in T9 component9, in T10 component10);
    }
}
