using _project.Scripts.LeoECS.Components;
using _project.Scripts.LeoECS.Components.Tags;
using LeoECS.Extensions;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace _project.Scripts.LeoECS.Systems.Player
{
    public class PlayerMoveInputSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PlayerTag, MoveDirectionComponent>> _playerFilter;

        public void Run(IEcsSystems systems)
        {
            _playerFilter.TryGetSingleEntity(out var entity);

            var direction = UnityEngine.Input.GetAxisRaw("Horizontal");
            _playerFilter.Pools.Inc2.Get(entity).MoveDirection = (int) direction;
        }
    }
}