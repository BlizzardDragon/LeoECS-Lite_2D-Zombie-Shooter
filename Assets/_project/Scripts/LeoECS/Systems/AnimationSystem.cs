using _project.Scripts.LeoECS.Components;
using _project.Scripts.LeoECS.Components.Tags;
using _project.Scripts.LeoECS.Services;
using LeoECS.Extensions;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace _project.Scripts.LeoECS.Systems
{
    public class AnimationSystem : IEcsRunSystem
    {
        private readonly int _animationHash = Animator.StringToHash("Animation");

        private const int IdleIndex = 0;
        private const int RunIndex = 1;
        private const int StayAndShootIndex = 2;
        private const int RunAndShootIndex = 3;

        private readonly EcsFilterInject<Inc<PlayerTag, AnimatorComponent, MoveDirectionComponent>> _playerFilter;

        private readonly EcsCustomInject<IInputService> _inputService;

        public void Run(IEcsSystems systems)
        {
            if (!_playerFilter.TryGetSingleEntity(out var playerEntity)) return;

            var animator = _playerFilter.Pools.Inc2.Get(playerEntity).Animator;
            var direction = _playerFilter.Pools.Inc3.Get(playerEntity).MoveDirection;
            int animationIndex;

            if (direction == 0)
            {
                animationIndex = _inputService.Value.IsHoldLMB ? StayAndShootIndex : IdleIndex;
            }
            else
            {
                animationIndex = _inputService.Value.IsHoldLMB ? RunAndShootIndex : RunIndex;
            }

            animator.SetInteger(_animationHash, animationIndex);
        }
    }
}