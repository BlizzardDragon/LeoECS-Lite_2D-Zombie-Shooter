using _project.Scripts.LeoECS.Components;
using _project.Scripts.LeoECS.Components.Events;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace _project.Scripts.LeoECS.Systems
{
    public class AmmoSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<ShootAnimationEvent, AmmoComponent>> _shootAnimationPool;

        private readonly EcsPoolInject<SpawnBulletEvent> _spawnBulletPool;

        public void Run(IEcsSystems systems)
        {
            var entities = _shootAnimationPool.Value.GetRawEntities();
            var count = _shootAnimationPool.Value.GetEntitiesCount();

            for (int i = 0; i < count; i++)
            {
                var entity = entities[i];
                ref var ammoComponent = ref _shootAnimationPool.Pools.Inc2.Get(entity);

                if (ammoComponent.AmmoCount > 0)
                {
                    ammoComponent.AmmoCount--;

                    var shootPoint = _shootAnimationPool.Pools.Inc1.Get(entity).ShootPointSource;
                    _spawnBulletPool.Value.Add(entity).ShootPointSource = shootPoint;
                }
            }
        }
    }
}