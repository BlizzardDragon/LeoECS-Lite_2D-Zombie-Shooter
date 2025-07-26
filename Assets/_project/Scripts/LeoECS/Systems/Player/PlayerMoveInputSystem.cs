using _project.Scripts.LeoECS.Components;
using LeoECS.Extensions;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace _project.Scripts.LeoECS.Systems.Player
{
    public class PlayerMoveInputSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PlayerTag, MoveDirectionComponent>> _playerFilter;

        public void Run(IEcsSystems systems)
        {
            _playerFilter.TryGetSingleEntity(out var entity);

            var direction = Input.GetAxisRaw("Horizontal");
            _playerFilter.Pools.Inc2.Get(entity).MoveDirection = (int) direction;
        }
    }
}