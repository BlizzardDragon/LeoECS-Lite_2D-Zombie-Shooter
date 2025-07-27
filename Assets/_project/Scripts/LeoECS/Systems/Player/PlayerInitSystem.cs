using _project.Scripts.LeoECS.Components;
using _project.Scripts.LeoECS.Components.Tags;
using LeoECS.Extensions;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace _project.Scripts.LeoECS.Systems.Player
{
    public class PlayerInitSystem : IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<PlayerTag>> _playerFilter;

        private readonly EcsPoolInject<MoveDirectionComponent> _moveDirectionPool;
        private readonly EcsPoolInject<EcsMonoObjectComponent> _ecsMonoObjectPool;
        private readonly EcsPoolInject<TeamComponent> _teamPool;

        private readonly EcsSharedInject<SharedData> _shared;

        public void Init(IEcsSystems systems)
        {
            _playerFilter.TryGetSingleEntity(out var entity);

            _ecsMonoObjectPool.Value.Get(entity).EcsMonoObject.Init(entity, systems.GetWorld());
            _teamPool.Value.Add(entity).Team = TeamType.PlayerTeam;
            
            ref var moveDirectionComponent = ref _moveDirectionPool.Value.Add(entity);
            moveDirectionComponent.MoveSpeed = _shared.Value.PlayerConfig.MoveSpeed;
            moveDirectionComponent.LastMoveDirection = 1;
        }
    }
}