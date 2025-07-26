using _project.Scripts.LeoECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace _project.Scripts.LeoECS.Systems
{
    public class MoveFlipAnimationSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<FlipAnimationComponent, MoveDirectionComponent>> _flipFilter;

        public void Run(IEcsSystems systems)
        {
            var entities = _flipFilter.Value.GetRawEntities();
            var count = _flipFilter.Value.GetEntitiesCount();

            for (int i = 0; i < count; i++)
            {
                var entity = entities[i];

                ref var moveDirectionComponent = ref _flipFilter.Pools.Inc2.Get(entity);

                var moveDirection = moveDirectionComponent.MoveDirection;
                if (moveDirection != 0 && Mathf.Abs(moveDirection) == 1)
                {
                    _flipFilter.Pools.Inc1.Get(entity).FlipSource.localScale = new Vector3(moveDirection, 1, 1);
                }
            }
        }
    }
}