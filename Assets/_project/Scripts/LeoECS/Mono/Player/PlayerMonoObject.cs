using _project.Scripts.LeoECS.Components.Events;
using Leopotam.EcsLite;
using UnityEngine;

namespace _project.Scripts.LeoECS.Mono.Player
{
    public class PlayerMonoObject : EcsMonoObject
    {
        [SerializeField] private Transform _shootPoint;

        private EcsPool<ShootAnimationEvent> _shootPool;

        protected override void OnInit()
        {
            base.OnInit();
            _shootPool = _world.GetPool<ShootAnimationEvent>();
        }

        public void CreateShootAnimationEvent()
        {
            PackedEntity.Unpack(_world, out var entity);
            _shootPool.Add(entity).ShootPoint = _shootPoint;
        }
    }
}