using _project.Scripts.LeoECS.Components;
using _project.Scripts.LeoECS.Components.Events;
using Leopotam.EcsLite;

namespace _project.Scripts.LeoECS.Mono
{
    public class BulletEcsMonoTrigger : EcsMonoTrigger
    {
        private EcsPool<TeamComponent> _teamPool;
        private EcsPool<DestroyEvent> _destroyPool;
        private EcsPool<HealthComponent> _healthPool;

        protected override void OnInit()
        {
            base.OnInit();
            _teamPool = _world.GetPool<TeamComponent>();
            _destroyPool = _world.GetPool<DestroyEvent>();
            _healthPool = _world.GetPool<HealthComponent>();
        }

        protected override void OnTriggerEnter2DAction(EcsMonoObject secondMonoObject)
        {
            if (!PackedEntity.Unpack(_world, out var firstEntity) ||
                !secondMonoObject.PackedEntity.Unpack(_world, out var secondEntity)) return;

            if (!_teamPool.Has(secondEntity)) return;
            if (_teamPool.Get(firstEntity).Team == _teamPool.Get(secondEntity).Team) return;
            if (!_healthPool.Has(secondEntity)) return;
            if (_destroyPool.Has(firstEntity)) return;

            _destroyPool.Add(firstEntity).GameObject = gameObject;
        }
    }
}