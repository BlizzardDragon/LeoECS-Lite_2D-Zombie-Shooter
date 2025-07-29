using _project.Scripts.LeoECS.Components.Despawn;
using _project.Scripts.LeoECS.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace _project.Scripts.LeoECS.Systems.Despawn
{
    public class DespawnWithDelaySystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<DespawnWithDelayComponent>> _despawnWithDelayFilter;

        private readonly EcsPoolInject<DespawnEvent> _despawnPool;

        private readonly EcsCustomInject<ITimeService> _timeService;
        private readonly EcsWorldInject _world;

        public void Run(IEcsSystems systems)
        {
            var entities = _despawnWithDelayFilter.Value.GetRawEntities();
            var count = _despawnWithDelayFilter.Value.GetEntitiesCount();

            for (int i = 0; i < count; i++)
            {
                var entity = entities[i];

                ref var despawnWithDelayComponent = ref _despawnWithDelayFilter.Pools.Inc1.Get(entity);
                var despawnTime = despawnWithDelayComponent.StartTime + despawnWithDelayComponent.Delay;

                if (_timeService.Value.CurrentTime >= despawnTime)
                {
                    if (!_despawnPool.Value.Has(entity))
                    {
                        var gameObject = despawnWithDelayComponent.GameObject;
                        _despawnPool.Value.Add(entity).GameObject = gameObject;
                    }

                    _despawnWithDelayFilter.Pools.Inc1.Del(entity);
                }
            }
        }
    }
}