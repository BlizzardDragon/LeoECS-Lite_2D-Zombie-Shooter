using _project.Scripts.LeoECS.Components.Despawn;
using EasyPoolKit;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace _project.Scripts.LeoECS.Systems.Despawn
{
    public class DespawnSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<DespawnEvent>> _despawnFilter;

        private readonly EcsWorldInject _world;

        public void Run(IEcsSystems systems)
        {
            var entities = _despawnFilter.Value.GetRawEntities();
            var count = _despawnFilter.Value.GetEntitiesCount();

            for (int i = 0; i < count; i++)
            {
                var entity = entities[i];

                var gameObject = _despawnFilter.Pools.Inc1.Get(entity).GameObject;

                if (gameObject != null)
                {
                    SimpleGOPoolKit.Instance.Despawn(gameObject);
                }

                _world.Value.DelEntity(entity);
            }
        }
    }
}