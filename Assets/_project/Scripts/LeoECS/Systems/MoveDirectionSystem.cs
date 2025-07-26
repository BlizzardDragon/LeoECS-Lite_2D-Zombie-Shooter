using _project.Scripts.LeoECS.Components;
using _project.Scripts.LeoECS.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace _project.Scripts.LeoECS.Systems
{
    public class MoveDirectionSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<MoveDirectionComponent, TransformComponent>> _moveFilter;

        private readonly EcsCustomInject<ITimeService> _timeService;

        public void Run(IEcsSystems systems)
        {
            var entities = _moveFilter.Value.GetRawEntities();
            var count = _moveFilter.Value.GetEntitiesCount();

            for (int i = 0; i < count; i++)
            {
                var entity = entities[i];
                ref var moveDirComponent = ref _moveFilter.Pools.Inc1.Get(entity);
                ref var transformComponent = ref _moveFilter.Pools.Inc2.Get(entity);

                var xDirection =
                    moveDirComponent.MoveDirection * moveDirComponent.MoveSpeed * _timeService.Value.DeltaTime;

                transformComponent.Transform.Translate(new Vector3(xDirection, 0, 0));
            }
        }
    }
}