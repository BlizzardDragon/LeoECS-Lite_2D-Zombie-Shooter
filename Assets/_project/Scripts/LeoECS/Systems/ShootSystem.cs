using _project.Scripts.LeoECS.Components;
using _project.Scripts.LeoECS.Components.Events;
using _project.Scripts.LeoECS.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace _project.Scripts.LeoECS.Systems
{
    public class ShootSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<ShootAnimationEvent, AmmoComponent, ReloadComponent>> _shootAnimationPool;

        private readonly EcsPoolInject<SpawnBulletEvent> _spawnBulletPool;
        private readonly EcsPoolInject<AmmoChangedEvent> _ammoChangedPool;

        private readonly EcsCustomInject<ITimeService> _timeService;

        public void Run(IEcsSystems systems)
        {
            var entities = _shootAnimationPool.Value.GetRawEntities();
            var count = _shootAnimationPool.Value.GetEntitiesCount();

            for (int i = 0; i < count; i++)
            {
                var entity = entities[i];

                var currentTime = _timeService.Value.CurrentTime;
                ref var reloadComponent = ref _shootAnimationPool.Pools.Inc3.Get(entity);

                if (reloadComponent.LastReloadTime + reloadComponent.ReloadDuration <= currentTime)
                {
                    reloadComponent.LastReloadTime = currentTime;

                    ref var ammoComponent = ref _shootAnimationPool.Pools.Inc2.Get(entity);

                    if (ammoComponent.AmmoCount > 0)
                    {
                        ammoComponent.AmmoCount--;

                        var shootPoint = _shootAnimationPool.Pools.Inc1.Get(entity).ShootPointSource;
                        _spawnBulletPool.Value.Add(entity).ShootPointSource = shootPoint;
                        _ammoChangedPool.Value.Add(entity);
                    }
                }
            }
        }
    }
}