using _project.Scripts.LeoECS.Components;
using _project.Scripts.LeoECS.Components.Despawn;
using _project.Scripts.LeoECS.Components.Destroy;
using _project.Scripts.LeoECS.Components.Events;
using Leopotam.EcsLite;

namespace _project.Scripts.LeoECS.Mono
{
    public class BulletEcsMonoTrigger : EcsMonoTrigger
    {
        private EcsPool<TeamComponent> _teamPool;
        private EcsPool<DespawnEvent> _despawnPool;
        private EcsPool<HealthComponent> _healthPool;

        protected override void OnInit()
        {
            base.OnInit();
            _teamPool = _world.GetPool<TeamComponent>();
            _despawnPool = _world.GetPool<DespawnEvent>();
            _healthPool = _world.GetPool<HealthComponent>();
        }

        protected override void OnTriggerEnter2DAction(EcsMonoObject secondMonoObject)
        {
            if (!PackedEntity.Unpack(_world, out var firstEntity) ||
                !secondMonoObject.PackedEntity.Unpack(_world, out var secondEntity)) return;

            if (!_teamPool.Has(secondEntity)) return;
            if (_teamPool.Get(firstEntity).Team == _teamPool.Get(secondEntity).Team) return;
            if (!_healthPool.Has(secondEntity)) return;
            if (_despawnPool.Has(firstEntity)) return;

            _despawnPool.Add(firstEntity).GameObject = gameObject;
        }
    }
}