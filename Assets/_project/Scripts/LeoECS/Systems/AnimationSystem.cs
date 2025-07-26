using _project.Scripts.LeoECS.Components;
using _project.Scripts.LeoECS.Components.Input;
using _project.Scripts.LeoECS.Components.Tags;
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

        private readonly EcsFilterInject<Inc<InputTag>> _inputFilter;
        private readonly EcsFilterInject<Inc<PlayerTag, AnimatorComponent, MoveDirectionComponent>> _playerFilter;

        private readonly EcsPoolInject<LmbHoldComponent> _lmbHoldPool;

        public void Run(IEcsSystems systems)
        {
            if (!_inputFilter.TryGetSingleEntity(out var inputEntity)) return;
            if (!_playerFilter.TryGetSingleEntity(out var playerEntity)) return;

            var animator = _playerFilter.Pools.Inc2.Get(playerEntity).Animator;
            var direction = _playerFilter.Pools.Inc3.Get(playerEntity).MoveDirection;
            int animationIndex;

            if (direction == 0)
            {
                animationIndex = _lmbHoldPool.Value.Has(inputEntity) ? StayAndShootIndex : IdleIndex;
            }
            else
            {
                animationIndex = _lmbHoldPool.Value.Has(inputEntity) ? RunAndShootIndex : RunIndex;
            }

            animator.SetInteger(_animationHash, animationIndex);
        }
    }
}