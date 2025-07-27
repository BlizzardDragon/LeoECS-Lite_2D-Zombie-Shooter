using _project.Scripts.LeoECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace _project.Scripts.LeoECS.Systems
{
    public class TargetFollowSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<TargetFollowComponent, TransformComponent, MoveDirectionComponent>>
            _followerFilter;

        public void Run(IEcsSystems systems)
        {
            var entities = _followerFilter.Value.GetRawEntities();
            var count = _followerFilter.Value.GetEntitiesCount();

            for (int i = 0; i < count; i++)
            {
                var entity = entities[i];

                var currentTransform = _followerFilter.Pools.Inc2.Get(entity).Transform;
                var targetTransform = _followerFilter.Pools.Inc1.Get(entity).Target;

                var direction = targetTransform.position.x < currentTransform.position.x ? -1 : 1;
                _followerFilter.Pools.Inc3.Get(entity).MoveDirection = direction;
            }
        }
    }
}