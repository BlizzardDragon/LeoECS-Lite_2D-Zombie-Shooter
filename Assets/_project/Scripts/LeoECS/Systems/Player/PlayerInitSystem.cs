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

        private readonly EcsSharedInject<SharedData> _shared;

        public void Init(IEcsSystems systems)
        {
            _playerFilter.TryGetSingleEntity(out var entity);

            _moveDirectionPool.Value.Add(entity).MoveSpeed = _shared.Value.PlayerConfig.MoveSpeed;
        }
    }
}