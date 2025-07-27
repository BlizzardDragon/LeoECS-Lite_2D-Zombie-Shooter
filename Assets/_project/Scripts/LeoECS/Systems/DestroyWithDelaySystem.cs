using _project.Scripts.LeoECS.Components;
using _project.Scripts.LeoECS.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace _project.Scripts.LeoECS.Systems
{
    public class DestroyWithDelaySystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<DestroyWithDelayComponent>> _destroyWithDelayFilter;

        private readonly EcsCustomInject<ITimeService> _timeService;
        private readonly EcsWorldInject _world;
        
        public void Run(IEcsSystems systems)
        {
            var entities = _destroyWithDelayFilter.Value.GetRawEntities();
            var count = _destroyWithDelayFilter.Value.GetEntitiesCount();

            for (int i = 0; i < count; i++)
            {
                var entity = entities[i];

                ref var destroyWithDelayComponent = ref _destroyWithDelayFilter.Pools.Inc1.Get(entity);
                var destroyTime = destroyWithDelayComponent.StartTime + destroyWithDelayComponent.Delay;
                
                if (_timeService.Value.CurrentTime >= destroyTime)
                {
                    var gameObject = destroyWithDelayComponent.GameObject;

                    if (gameObject != null)
                    {
                        Object.Destroy(gameObject);
                    }
                    
                    _world.Value.DelEntity(entity);
                }
            }
        }
    }
}