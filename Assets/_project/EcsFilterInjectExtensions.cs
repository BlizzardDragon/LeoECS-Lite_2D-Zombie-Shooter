using Leopotam.EcsLite.Di;
using UnityEngine;

namespace LeoECS.Extensions
{
    public static class EcsFilterInjectExtensions
    {
        public static bool TryGetSingleEntity<T>(this EcsFilterInject<T> filterInject, out int entity)
            where T : struct, IEcsInclude
        {
            var filter = filterInject.Value;
            var count = filter.GetEntitiesCount();

            if (count != 1)
            {
                Debug.LogError($"Expected exactly 1 entity in {typeof(T).Name}, but got {count}");
                entity = -1;
                return false;
            }

            entity = filter.GetRawEntities()[0];
            return true;
        }
    }
}