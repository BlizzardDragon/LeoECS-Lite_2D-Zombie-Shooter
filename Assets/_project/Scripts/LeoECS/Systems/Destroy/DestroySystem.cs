using _project.Scripts.LeoECS.Components.Destroy;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace _project.Scripts.LeoECS.Systems.Destroy
{
    public class DestroySystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<DestroyEvent>> _destroyFilter;

        private readonly EcsWorldInject _world;

        public void Run(IEcsSystems systems)
        {
            var entities = _destroyFilter.Value.GetRawEntities();
            var count = _destroyFilter.Value.GetEntitiesCount();

            for (int i = 0; i < count; i++)
            {
                var entity = entities[i];

                var gameObject = _destroyFilter.Pools.Inc1.Get(entity).GameObject;

                if (gameObject != null)
                {
                    Object.Destroy(gameObject);
                }
                
                _world.Value.DelEntity(entity);
            }
        }
    }
}