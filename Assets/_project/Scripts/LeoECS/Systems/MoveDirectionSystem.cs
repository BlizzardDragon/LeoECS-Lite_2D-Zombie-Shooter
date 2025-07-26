using _project.Scripts.LeoECS.Components;
using _project.Scripts.LeoECS.Components.Tags;
using _project.Scripts.LeoECS.Services;
using LeoECS.Extensions;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace _project.Scripts.LeoECS.Systems
{
    public class MoveDirectionSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PlayerTag, MoveDirectionComponent, TransformComponent>> _playerFilter;

        private readonly EcsCustomInject<ITimeService> _timeService;

        public void Run(IEcsSystems systems)
        {
            if (!_playerFilter.TryGetSingleEntity(out var entity)) return;

            ref var transformComponent = ref _playerFilter.Pools.Inc3.Get(entity);
            ref var moveDirectionComponent = ref _playerFilter.Pools.Inc2.Get(entity);

            var directionX =
                moveDirectionComponent.MoveDirection * moveDirectionComponent.MoveSpeed * _timeService.Value.DeltaTime;

            transformComponent.Transform.Translate(new Vector3(directionX, 0, 0));
        }
    }
}